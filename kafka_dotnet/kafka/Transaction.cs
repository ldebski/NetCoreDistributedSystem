using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka
{
    class Transaction
    {
        public string fromID;
        public string toID;
        public int value;

        public Transaction(string from, string to, int v)
        {
            fromID = from;
            toID = to;
            value = v;
        }
    }
}
