using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapiApp
{
    class SwapiEntityResults<T> : SwapiEntity where T : SwapiEntity
    {
        public string Previous
        {
            get;
            set;
        }

        public string Next
        {
            get;
            set;
        }

        public string PreviousPageNo
        {
            get;
            set;
        }

        public string NextPageNo
        {
            get;
            set;
        }

        public Int64 Count
        {
            get;
            set;
        }

        public List<T> Results
        {
            get;
            set;
        }
    }
}
