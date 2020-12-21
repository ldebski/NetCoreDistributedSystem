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

        public Transaction(string g)
        {
            guid = g;
            toID = null;
            value = 0;
            sum = 0;
            ret = "false";
            done = "true";
        }
    }
}