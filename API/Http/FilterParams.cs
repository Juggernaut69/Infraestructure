using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Http
{
    public class FilterParams
    {
        public int ClientID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrderID { get; set; }
    }
}