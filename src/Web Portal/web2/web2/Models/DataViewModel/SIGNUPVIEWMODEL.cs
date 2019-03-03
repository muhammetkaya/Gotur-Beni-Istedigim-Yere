using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web2.Models.DataViewModel
{
    public class SIGNUPVIEWMODEL
    {
        public int SIGNUPID { get; set; }

        [Required(ErrorMessage = "Şifre Boş Bırakılamaz", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "En Az 8 Karakter Olmalı")]
        public string PASSWORD { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("PASSWORD", ErrorMessage = "Şifreler Eşleşmiyor")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Remote("IsUserMailExists","Account",ErrorMessage ="Lütfen Başka Mail Deneyin")]
        [Required(ErrorMessage ="Email Boş Bırakılamaz")]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$",ErrorMessage = "Doğru EMail Girdiğinize Emin Olun")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı Boş Bırakılamaz", AllowEmptyStrings = false)]
        [Remote("IsUserExists", "Account", ErrorMessage = "Lütfen Başka Bir Kullanıcı Adı Deneyin")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Ad Boş Bırakılamaz", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad Boş Bırakılamaz", AllowEmptyStrings = false)]
        public string Surname { get; set; }
    }
}



