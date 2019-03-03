using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere.EntityFramework.DataViewModel
{
    public class PlaceViewModel
    {
        public int PLACEID { get; set; }
        public string SWARMID { get; set; }
        public string NAME { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string ADDRESS { get; set; }
        public string LAT { get; set; }
        public string LNG { get; set; }
        public Nullable<int> CHECKINSCOUNT { get; set; }
        public Nullable<int> TIPCOUNT { get; set; }
        public Nullable<int> USERSCOUNT { get; set; }
    }
}
