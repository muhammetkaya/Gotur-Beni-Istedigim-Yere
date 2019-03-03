using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web2.Models.DataViewModel;
using System.ComponentModel.DataAnnotations;
using GoturBeniIstedigimYere.EntityFramework;

namespace web2.BL
{
    public class BL
    {
        public static void besPuanEkle(int[] dizi, int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                if (dizi.Count() > 1)
                {
                    foreach (int deger in dizi)
                    {
                        UserLikes yeni = new UserLikes()
                        {
                            USERLOGINID = id,
                            LIKEID = deger,
                            VALUE = 5
                        };
                        db.UserLikes.Add(yeni);
                        db.SaveChanges();

                    }
                }

            }
        }
        public static void dortPuanEkle(int[] dizi, int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                if (dizi.Count() > 1)
                {
                    foreach (int deger in dizi)
                    {
                        UserLikes yeni = new UserLikes()
                        {
                            USERLOGINID = id,
                            LIKEID = deger,
                            VALUE = 4
                        };
                        db.UserLikes.Add(yeni);
                        db.SaveChanges();

                    }
                }

            }
        }
        public static void ucPuanEkle(int[] dizi, int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                if (dizi.Count() > 1)
                {
                    foreach (int deger in dizi)
                    {
                        UserLikes yeni = new UserLikes()
                        {
                            USERLOGINID = id,
                            LIKEID = deger,
                            VALUE = 3
                        };
                        db.UserLikes.Add(yeni);
                        db.SaveChanges();

                    }
                }

            }
        }
        public static void ikiPuanEkle(int[] dizi, int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                if(dizi.Count()>1)
                {
                    foreach (int deger in dizi)
                    {
                        UserLikes yeni = new UserLikes()
                        {
                            USERLOGINID = id,
                            LIKEID = deger,
                            VALUE = 2
                        };
                        db.UserLikes.Add(yeni);
                        db.SaveChanges();

                    }
                }

            }
        }
        public static void birPuanEkle(int[] dizi, int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                if(dizi.Count()>1)
                {
                    foreach (int deger in dizi)
                    {
                        UserLikes yeni = new UserLikes()
                        {
                            USERLOGINID = id,
                            LIKEID = deger,
                            VALUE = 1
                        };
                        db.UserLikes.Add(yeni);
                        db.SaveChanges();

                    }
                }

            }
        }
        public static void sifirPuanEkle(int[] dizi, int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                if(dizi.Count()>1)
                {
                    foreach (int deger in dizi)
                    {
                        UserLikes yeni = new UserLikes()
                        {
                            USERLOGINID = id,
                            LIKEID = deger,
                            VALUE = 0
                        };
                        db.UserLikes.Add(yeni);
                        db.SaveChanges();

                    }
                }

            }
        }

        public static void HobiEkle(HobiView h, string userName)
        {
            try
            {
                using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
                {
                    var UserLogID = (from p in db.UserLoginInfo
                                     where p.USERNAME == userName
                                     select p.USERLOGINID).FirstOrDefault();
                    besPuanEkle(h.besPuan, UserLogID);
                    dortPuanEkle(h.dortPuan, UserLogID);
                    ucPuanEkle(h.ucPuan, UserLogID);
                    ikiPuanEkle(h.ikiPuan, UserLogID);
                    birPuanEkle(h.birPuan, UserLogID);
                    sifirPuanEkle(h.sifirPuan, UserLogID);
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public static List<UserLikes> HobiListele(int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                List<UserLikes> liste = new List<UserLikes>();
                foreach(var kayit in db.UserLikes)
                {
                    if(kayit.USERLOGINID==id)
                    {
                        liste.Add(new UserLikes()
                        {
                            LIKEID = kayit.LIKEID,
                            USERLIKEID = kayit.USERLIKEID,
                            USERLOGINID = kayit.USERLOGINID,
                            VALUE = kayit.VALUE
                        });
                    }
                }
                return liste;
            }

        }


        public static void Kayit(UserLoginInfoViewModel s)
        {

            try
            {
                using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
                {
                    UserLoginInfo yeni = new UserLoginInfo()
                    {
                        CITY = s.CITY,
                        COUNTRY = s.COUNTRY,
                        EMAIL = s.EMAIL,
                        ISACTIVE = true,
                        NAME = s.NAME,
                        PASSWORD = s.PASSWORD,
                        ROLE = 0,
                        SURNAME = s.SURNAME,
                        USERNAME = s.USERNAME,
                        USERID = s.USERID
                    };
                    db.UserLoginInfo.Add(yeni);
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    }
}