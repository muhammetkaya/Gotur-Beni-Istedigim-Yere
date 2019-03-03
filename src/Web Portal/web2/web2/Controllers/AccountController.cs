using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web2.Models.DataViewModel;
using System.Web.Security;
using System.Configuration;
using FourSquare.SharpSquare.Core;
using FourSquare.SharpSquare.Entities;
using GoturBeniIstedigimYere.EntityFramework;
using GoturBeniIstedigimYere.DataAccessLayer;
using GoturBeniIstedigimYere;

namespace web2.Controllers
{

    public class AccountController : Controller
    {
        string clientID = ConfigurationManager.AppSettings["4SquareClientID"];
        string clientSecret = ConfigurationManager.AppSettings["4SquareClientSecret"];
        public ActionResult UserClicksAuthenticate()
        {

            var redirectUri = "http://" + Request.Url.Authority + this.Url.Action("AuthorizeCallback", new { userCode = "userCode" });
            //var redirectUri = "http://www.muhammetkaya.com/redirect_url" + this.Url.Action("AuthorizeCallback", new { userCode = "userCode" });
            var sharpSquare = new SharpSquare(clientID, clientSecret);
            var authUrl = sharpSquare.GetAuthenticateUrl(redirectUri);

            return new RedirectResult(authUrl, permanent: false);
        }

        List<VenueHistory> venues;

        public ActionResult AuthorizeCallback(string code, string userCode)
        {
            var redirectUri = "http://" + Request.Url.Authority + this.Url.Action("AuthorizeCallback", new { userCode = userCode });

            var sharpSquare = new SharpSquare(clientID, clientSecret);

            var accessToken = sharpSquare.GetAccessToken(redirectUri, code);

            // need this in order to make calls to API
            // it's redundant because token is already set in GetAccessToken() call but it helps to understand the workflow better. 
            UserLoginInfo u1 = new UserLoginInfo();
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {

                var UserLogID = (from p in db.UserLoginInfo
                                 where p.USERNAME == User.Identity.Name
                                 select p.USERLOGINID).FirstOrDefault();
                string token = accessToken;
                db.Token.Add(new Token()
                {
                    TOKENACID = token,
                    USERLOGINID = UserLogID
                });
                db.SaveChanges();
            }
            //CheckinOperations.insertUSerLoginInfo(u1);

            return RedirectToAction("Index", "Home");
        }


        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = "0")]
        public ActionResult HobiEkle()
        {
            return View();
        }
        [HttpPost]
        public JsonResult HobiEkle(HobiView h)
        {
            BL.BL.HobiEkle(h, User.Identity.Name);
            return Json(true);
        }

        public ActionResult mekanKesfet()
        {
            List<UserLikes> userLikes;
            List<FourSquare.SharpSquare.Entities.VenueHistory> Checkins;
            string token;
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                var UserLogID = (from p in db.UserLoginInfo
                                 where p.USERNAME == User.Identity.Name
                                 select p.USERLOGINID).FirstOrDefault();
                userLikes = web2.BL.BL.HobiListele(UserLogID);
                token = (from p in db.Token
                         where p.USERLOGINID == UserLogID
                         select p.TOKENACID).FirstOrDefault();

            }

            var sharpSquare = new FourSquare.SharpSquare.Core.SharpSquare(clientID, clientSecret);
            sharpSquare.SetAccessToken(token);
            venues = sharpSquare.GetUserVenueHistory();
            Checkins = venues;


            GoturBeniIstedigimYere.RecommendationEngine rEng = new GoturBeniIstedigimYere.RecommendationEngine();


            List<PlaceAndImages> result = rEng.DifferentCategoryRecommendVenues(userLikes, Checkins);

            for (int i = 0; i < 5; i++)
            {
                result[i].Place.LAT = result[i].Place.LAT.Replace(",", ".");
                result[i].Place.LNG = result[i].Place.LNG.Replace(",", ".");
            }

            MekanModelList a = new MekanModelList();
            a.mekan1.LAT = result[0].Place.LAT;
            a.mekan1.LNG = result[0].Place.LNG;
            a.mekan1.mekanAd = result[0].Place.NAME;
            a.mekan1.mekanIl = result[0].Place.CITY;
            a.mekan1.mekanIlce = result[0].Place.ADDRESS;

            a.mekan2.LAT = result[1].Place.LAT;
            a.mekan2.LNG = result[1].Place.LNG;
            a.mekan2.mekanAd = result[1].Place.NAME;
            a.mekan2.mekanIl = result[1].Place.CITY;
            a.mekan2.mekanIlce = result[1].Place.ADDRESS;

            a.mekan3.LAT = result[2].Place.LAT;
            a.mekan3.LNG = result[2].Place.LNG;
            a.mekan3.mekanAd = result[2].Place.NAME;
            a.mekan3.mekanIl = result[2].Place.CITY;
            a.mekan3.mekanIlce = result[2].Place.ADDRESS;

            a.mekan4.LAT = result[3].Place.LAT;
            a.mekan4.LNG = result[3].Place.LNG;
            a.mekan4.mekanAd = result[3].Place.NAME;
            a.mekan4.mekanIl = result[3].Place.CITY;
            a.mekan4.mekanIlce = result[3].Place.ADDRESS;

            a.mekan5.LAT = result[4].Place.LAT;
            a.mekan5.LNG = result[4].Place.LNG;
            a.mekan5.mekanAd = result[4].Place.NAME;
            a.mekan5.mekanIl = result[4].Place.CITY;
            a.mekan5.mekanIlce = result[4].Place.ADDRESS;

            return View("AnalizSonuc", a);

        }

        public ActionResult mekanOneriAl()
        {
            List<UserLikes> userLikes;
            List<FourSquare.SharpSquare.Entities.VenueHistory> Checkins;
            string token;
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                var UserLogID = (from p in db.UserLoginInfo
                                 where p.USERNAME == User.Identity.Name
                                 select p.USERLOGINID).FirstOrDefault();
                userLikes = web2.BL.BL.HobiListele(UserLogID);
                token = (from p in db.Token
                         where p.USERLOGINID == UserLogID
                         select p.TOKENACID).FirstOrDefault();

            }

            var sharpSquare = new FourSquare.SharpSquare.Core.SharpSquare(clientID, clientSecret);
            sharpSquare.SetAccessToken(token);
            venues = sharpSquare.GetUserVenueHistory();
            Checkins = venues;


            GoturBeniIstedigimYere.RecommendationEngine rEng = new GoturBeniIstedigimYere.RecommendationEngine();


            List<PlaceAndImages> result = rEng.RecommendVenues(userLikes, Checkins);

            for (int i = 0; i < 5; i++)
            {
                result[i].Place.LAT = result[i].Place.LAT.Replace(",", ".");
                result[i].Place.LNG = result[i].Place.LNG.Replace(",", ".");
            }

            MekanModelList a = new MekanModelList();
            a.mekan1.LAT = result[0].Place.LAT;
            a.mekan1.LNG = result[0].Place.LNG;
            a.mekan1.mekanAd = result[0].Place.NAME;
            a.mekan1.mekanIl = result[0].Place.CITY;
            a.mekan1.mekanIlce = result[0].Place.ADDRESS;

            a.mekan2.LAT = result[1].Place.LAT;
            a.mekan2.LNG = result[1].Place.LNG;
            a.mekan2.mekanAd = result[1].Place.NAME;
            a.mekan2.mekanIl = result[1].Place.CITY;
            a.mekan2.mekanIlce = result[1].Place.ADDRESS;

            a.mekan3.LAT = result[2].Place.LAT;
            a.mekan3.LNG = result[2].Place.LNG;
            a.mekan3.mekanAd = result[2].Place.NAME;
            a.mekan3.mekanIl = result[2].Place.CITY;
            a.mekan3.mekanIlce = result[2].Place.ADDRESS;

            a.mekan4.LAT = result[3].Place.LAT;
            a.mekan4.LNG = result[3].Place.LNG;
            a.mekan4.mekanAd = result[3].Place.NAME;
            a.mekan4.mekanIl = result[3].Place.CITY;
            a.mekan4.mekanIlce = result[3].Place.ADDRESS;

            a.mekan5.LAT = result[4].Place.LAT;
            a.mekan5.LNG = result[4].Place.LNG;
            a.mekan5.mekanAd = result[4].Place.NAME;
            a.mekan5.mekanIl = result[4].Place.CITY;
            a.mekan5.mekanIlce = result[4].Place.ADDRESS;

            return View("AnalizSonuc",a);

        }

        [Authorize(Roles = "0")]
        public ActionResult AnalizSonuc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ACCOUNTVİEWMODEL a)
        {
            using (CheckinDatasetEntities4 DB = new CheckinDatasetEntities4())
            {
                var count = DB.UserLoginInfo.Where(x => x.USERNAME == a.USERNAME && x.PASSWORD == a.PASSWORD).Count();
                if (count == 0)
                {
                    ViewBag.Msg = "Invalid User";
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(a.USERNAME, false);
                    return RedirectToAction("AccountHome", "Account");
                }
            } 
            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(UserLoginInfoViewModel s)
        {
            if (ModelState.IsValid)
            {
                    BL.BL.Kayit(s);
                    s = null;
                    ViewBag.Message = "Kayıt Basarılı!";
            }


            return RedirectToAction("Login", "Account");
        }

        public ActionResult KayitDeneme()
        {
            return View();
        }

        public JsonResult IsUserExists(string UserName)
        {
            using (CheckinDatasetEntities4 DB = new CheckinDatasetEntities4())
            {
                return Json(!DB.UserLoginInfo.Any(x => x.USERNAME == UserName), JsonRequestBehavior.AllowGet);
            }
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        }

        public JsonResult IsUserMailExists(string eMail)
        {
            using (CheckinDatasetEntities4 DB = new CheckinDatasetEntities4())
            {
                //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
                return Json(!DB.UserLoginInfo.Any(x => x.EMAIL == eMail), JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "0")]
        public ActionResult AccountHome()
        {
            return View();
        }
    }
}