namespace KafkaConsumer
{
    internal class Transaction
    {
        public Transaction(string g, string to, int v, int s)
        {
            guid = g;
            toID = to;
            value = v;
            sum = s;
            ret = "false";
            done = "true";
        }

        public string guid { get; set; }
        public string toID { get; set; }
        public int value { get; set; }
        public string ret { get; set; }
        public int sum { get; set; }
        public string done { get; set; }
    }
}