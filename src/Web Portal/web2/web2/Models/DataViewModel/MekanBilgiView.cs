using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web2.Models.DataViewModel
{
    public class MekanBilgiView
    {
        public string LAT { get; set; }
        public string LNG { get; set; }
        public string mekanAd { get; set; }
        public string mekanIl { get; set; }
        public string mekanIlce { get; set; }
        public string mekanKategori { get; set; }
    }
}