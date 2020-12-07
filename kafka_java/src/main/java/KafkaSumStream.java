import com.google.gson.JsonParser;
import com.google.gson.JsonSyntaxException;
import org.apache.kafka.common.serialization.Serdes;
import org.apache.kafka.common.utils.Bytes;
import org.apache.kafka.streams.KafkaStreams;
import org.apache.kafka.streams.KeyValue;
import org.apache.kafka.streams.StreamsBuilder;
import org.apache.kafka.streams.StreamsConfig;
import org.apache.kafka.streams.kstream.*;
import org.apache.kafka.streams.state.KeyValueStore;

import java.util.Properties;

public class KafkaSumStream {
    public static void main(String[] args){
        // create properties
        Properties properties = new Properties();
        properties.setProperty(StreamsConfig.BOOTSTRAP_SERVERS_CONFIG, "localhost:9092");
        properties.setProperty(StreamsConfig.APPLICATION_ID_CONFIG, "demo-kafka-stream");
        properties.setProperty(StreamsConfig.DEFAULT_KEY_SERDE_CLASS_CONFIG, Serdes.StringSerde.class.getName());
        properties.setProperty(StreamsConfig.DEFAULT_VALUE_SERDE_CLASS_CONFIG, Serdes.StringSerde.class.getName());
        properties.put(StreamsConfig.CACHE_MAX_BYTES_BUFFERING_CONFIG, 0);


        // create topology
        StreamsBuilder streamsBuilder = new StreamsBuilder();

        streamsBuilder.stream("my_topic")
                .groupByKey()
                .reduce((v1, v2) ->  // agg_value, newValue
                extractValueFromMessage(v1.toString()) + extractValueFromMessage(v2.toString()) >= 0 ?
                extractValueFromMessages(v1.toString(), v2.toString()) : v1)
                .toStream()
                .to("out_topic");

        KafkaStreams kafkaStreams = new KafkaStreams(streamsBuilder.build(), properties);

        kafkaStreams.start();
    }


    private static JsonParser jsonParser = new JsonParser();

    private static String extractValueFromMessages(String messageJson1, String messageJson2){
        // gson library
        try {
            long i = jsonParser.parse(messageJson1)
                    .getAsJsonObject()
                    .get("value")
                    .getAsLong() +
                    jsonParser.parse(messageJson2)
                            .getAsJsonObject()
                            .get("value")
                            .getAsLong();;
            if (jsonParser.parse(messageJson2)
                    .getAsJsonObject()
                    .get("value")
                    .getAsLong() == 0)
                i = 0;
            return "{\"fromID\":\"1\",\"toID\":\"2\",\"value\":"+i+"}";
        }
        catch (NullPointerException | JsonSyntaxException e){
            System.out.println("messages error");
            return messageJson2;
        }
    }

    private static Long extractValueFromMessage(String messageJson){
        // gson library
        try {
            return jsonParser.parse(messageJson)
                    .getAsJsonObject()
                    .get("value")
                    .getAsLong();
        }
        catch (NullPointerException | JsonSyntaxException e){
            System.out.println("Failed to extract value from message: " + messageJson);
            return 0L;
        }
    }

}
