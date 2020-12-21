namespace kafka
{
    internal class Transaction
    {
        public string done;
        public string guid;
        public string ret;
        public int sum;
        public string toID;
        public int value;

        public Transaction(string g, string to, int v, int s)
        {
            guid = g;
            toID = to;
            value = v;
            sum = s;
            ret = "false";
            done = "true";
        }
    }
}