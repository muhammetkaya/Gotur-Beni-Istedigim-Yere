using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web2.Models.DataViewModel
{
    public class MekanModelList
    {
        public MekanBilgiView mekan1 { get; set; }
        public MekanBilgiView mekan2 { get; set; }
        public MekanBilgiView mekan3 { get; set; }
        public MekanBilgiView mekan4 { get; set; }
        public MekanBilgiView mekan5 { get; set; }

        public MekanModelList()
        {
            this.mekan1 = new MekanBilgiView();
            this.mekan2 = new MekanBilgiView();
            this.mekan3 = new MekanBilgiView();
            this.mekan4 = new MekanBilgiView();
            this.mekan5 = new MekanBilgiView();
        }
    }
}