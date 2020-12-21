using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka
{
    class Transaction
    {
        public string guid;
        public string toID;
        public int value;
        public string ret;
        public int sum;
        public string done;

        public Transaction(string g, string to, int v, int s, string r)
        {
            guid = g;
            toID = to;
            value = v;
            ret = r;
            sum = s;
            done = "false";
        }
    }
}
