using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimitingPro
{
    public class GlobalContext
    {
        public static int RequestQty { get; set; }
        public static int RequestQtyP { get; set; }
    }
}
