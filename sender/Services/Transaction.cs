namespace kafka
{
    internal class Transaction
    {
        public string done;
        public string fromID;
        public string ret;
        public int sum;
        public string toID;
        public int value;

        public Transaction(string from, string to, int v, int s, string r)
        {
            fromID = from;
            toID = to;
            value = v;
            ret = r;
            sum = s;
            done = "false";
        }
    }
}