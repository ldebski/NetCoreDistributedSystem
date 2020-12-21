import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.Gson;
import com.google.gson.JsonParser;
import com.google.gson.JsonSyntaxException;

public class MyJsonParser {

    static class Transaction
    {
        public String guid;
        public String toID;
        public Long value;
        public String ret;
        public Long sum;
        public String done;

        public Transaction(String g, String to, Long v, Long s, String r)
        {
            guid = g;
            toID = to;
            value = v;
            ret = r;
            sum = s;
        }
    }

    private static final Gson g = new Gson();
    private static final JsonParser jsonParser = new JsonParser();

    public static String swapValue(String messageJson){
        try {
            Transaction t = g.fromJson(messageJson, Transaction.class);
            t.value = t.value * (-1);
            t.ret = "true"; // mark as returning
            ObjectMapper mapper = new ObjectMapper();
            return mapper.writeValueAsString(t);

        } catch (NullPointerException | JsonSyntaxException | JsonProcessingException e) {
            System.out.println("message error");
            return "";
        }
    }

    public static String getToID(String messageJson){
        try {
            return jsonParser.parse(messageJson)
                    .getAsJsonObject()
                    .get("toID")
                    .getAsString();
        } catch (NullPointerException | JsonSyntaxException e) {
            System.out.println("Failed to extract value from message: " + messageJson);
            return "";
        }
    }

    public static boolean isDone(String messageJson){
        try {
            return jsonParser.parse(messageJson)
                    .getAsJsonObject()
                    .get("done")
                    .getAsString().equals("true");
        } catch (NullPointerException | JsonSyntaxException e) {
            System.out.println("Failed to extract value from message: " + messageJson);
            return false;
        }
    }

    public static boolean isRet(String messageJson){
        try {
            String s =  jsonParser.parse(messageJson)
                    .getAsJsonObject()
                    .get("ret")
                    .getAsString();
            return s.equals("true");
        } catch (NullPointerException | JsonSyntaxException e) {
            System.out.println("Failed to extract value from message: " + messageJson);
            return false;
        }
    }

    public static String extractValueFromMessages(String messageJson1, String messageJson2) {
        try {
            Transaction t = g.fromJson(messageJson2, Transaction.class);
            Long v1 = extractSumFromMessage(messageJson1);
            Long v2 = extractValueFromMessage(messageJson2);
            long sum = v1 + v2;
            if (sum >= 0 && t.toID != null) {
                t.sum = sum;
                t.done = "true";
            }
            else {
                t.sum = v1;
                t.done = "false";
            }
            ObjectMapper mapper = new ObjectMapper();
            return mapper.writeValueAsString(t);

        } catch (NullPointerException | JsonSyntaxException | JsonProcessingException e) {
            System.out.println("messages error");
            return messageJson2;
        }
    }

    public static Long extractValueFromMessage(String messageJson) {
        try {
            return jsonParser.parse(messageJson)
                    .getAsJsonObject()
                    .get("value")
                    .getAsLong();
        } catch (NullPointerException | JsonSyntaxException e) {
            System.out.println("Failed to extract value from message: " + messageJson);
            return 0L;
        }
    }

    public static Long extractSumFromMessage(String messageJson) {
        try {
            return jsonParser.parse(messageJson)
                    .getAsJsonObject()
                    .get("sum")
                    .getAsLong();
        } catch (NullPointerException | JsonSyntaxException e) {
            System.out.println("Failed to extract value from message: " + messageJson);
            return 0L;
        }
    }
}
