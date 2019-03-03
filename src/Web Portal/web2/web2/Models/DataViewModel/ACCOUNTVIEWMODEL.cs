using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web2.Models.DataViewModel
{
    public class ACCOUNTVİEWMODEL
    {

        public int USERID { get; set; }
        [Required(ErrorMessage ="Kullanıcı Adı Bos Gecilemez",AllowEmptyStrings =false)]
        public string USERNAME { get; set; }
        [Required(ErrorMessage ="Sifre Bos Gecilemez",AllowEmptyStrings =false)]
        public string PASSWORD { get; set; }
        public string ROLE { get; set; }

    }
}

