//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoturBeniIstedigimYere.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLoginInfo
    {
        public int USERLOGINID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string COUNTRY { get; set; }
        public string CITY { get; set; }
        public Nullable<int> USERID { get; set; }
        public Nullable<bool> ISACTIVE { get; set; }
        public Nullable<int> ROLE { get; set; }
    }
}
