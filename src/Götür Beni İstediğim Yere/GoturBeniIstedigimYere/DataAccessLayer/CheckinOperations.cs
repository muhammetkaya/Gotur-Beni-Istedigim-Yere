using GoturBeniIstedigimYere.EntityFramework;
using GoturBeniIstedigimYere.EntityFramework.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere.DataAccessLayer
{
    public class CheckinOperations
    {
        #region PROPERTY
        #endregion
        #region FIELDS
        private static readonly object _lock = new object();
        #endregion

        #region USER
        public static void insertUser(User user)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.User.Add(user);
                entity.SaveChanges();
            }
        }
        public static List<User> getUsers()
        {
            List<User> users = new List<User>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                users = entity.User.ToList();
            }
            return users;
        }
        public static User getUser(string TweetID)
        {
            User user = new User();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                user = entity.User.ToList().Find(u => u.TWID == TweetID);
            }
            return user;
        }
        public static void deleteUser(User user)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.User.Remove(user);
                entity.SaveChanges();
            }

        }
        #endregion
        #region CATEGORY
        public static void insertCategorie(Categorie category)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Categorie.Add(category);
                entity.SaveChanges();
            }
        }
        public static Categorie getCategory(string Name)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                var result = entity.Categorie.ToList().Find(cat => cat.NAME == Name);
                return result;
            }
        }
        public static Categorie getCategory(int CategoryID)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                var result = entity.Categorie.ToList().Find(cat => cat.CATEGORIEID == CategoryID);
                return result;
            }
        }
        public static Categorie getCategoryBySwarmID(string SwarmID)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                var result = entity.Categorie.ToList().Find(cat => cat.SWARMID == SwarmID);
                return result;
            }
        }
        public static List<Categorie> getCategorie()
        {
            List<Categorie> Categories = new List<Categorie>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Categories = entity.Categorie.ToList().FindAll(cat => cat.PARENTCATEGORYID != null);
                Categories.Sort((x, y) => string.Compare(x.NAME, y.NAME));
            }
            return Categories;
        }
        public static void deleteCategorie(Categorie categorie)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Categorie.Remove(categorie);
                entity.SaveChanges();
            }

        }
        #endregion
        #region POSTS

        public static void insertPost(Post post)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                if (entity.Post.ToList().Find(p => p.TWEETID == post.TWEETID) == null)
                {
                    lock (_lock)
                    {
                        entity.Post.Add(post);
                        entity.SaveChanges();
                    }

                }

            }
        }
        public static List<Post> getPosts(int UserID)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                var Posts = entity.Post.ToList().FindAll(e => e.USERID == UserID);
                return Posts;
            }
        }

        #endregion
        #region CHECKIN
        public static void insertCheckIn(Checkin checkIn)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Checkin.Add(checkIn);
                entity.SaveChanges();
            }
        }
        public static List<Checkin> getCheckIns(int userID)
        {
            List<Checkin> checkins = new List<Checkin>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                checkins = (from checkin in entity.Checkin
                            join post in entity.Post on checkin.POSTID equals post.POSTID
                            where post.USERID == userID
                            select checkin).ToList();
            }
            return checkins;
        }
        public static void deleteCheckIn(Checkin checkIn)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Checkin.Remove(checkIn);
                entity.SaveChanges();
            }

        }
        public static List<Checkin> listCheckIns()
        {
            List<Checkin> checkins = new List<Checkin>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                checkins = (from checkin in entity.Checkin
                            join post in entity.Post on checkin.POSTID equals post.POSTID
                            select checkin).ToList();
            }
            return checkins;
        }
        public static List<User> listCheckInsReturnedUser(int CategoryID)
        {
            List<User> users = new List<User>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                users = (from checkin in entity.Checkin
                         join post in entity.Post on checkin.POSTID equals post.POSTID
                         join user in entity.User on post.USERID equals user.USERID
                         join pCat in entity.PlaceCategories on checkin.PLACEID equals pCat.PLACEID
                         where pCat.CATEGORIEID == CategoryID
                         select user).ToList();
            }
            return users;
        }
        public class CheckinUser
        {
            public Checkin checkin;
            public Post post;
            public User user;
            public PlaceCategories placeCategory;
            public CheckinUser()
            {

            }

        }
        public static List<CheckinUser> listCheckInsReturnedUser()
        {
            List<CheckinUser> checkinResult = new List<CheckinUser>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                checkinResult = (from checkin in entity.Checkin
                                 join post in entity.Post on checkin.POSTID equals post.POSTID
                                 join user in entity.User on post.USERID equals user.USERID
                                 join pCat in entity.PlaceCategories on checkin.PLACEID equals pCat.PLACEID
                                 select new CheckinUser() { checkin = checkin, placeCategory = pCat, post = post, user = user }).ToList();
            }
            return checkinResult;
        }
        #endregion

        #region PLACE
        public static void insertPlace(Place place)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                if (entity.Place.ToList().Find(p => p.SWARMID == place.SWARMID) == null)
                {
                    entity.Place.Add(place);
                    entity.SaveChanges();
                }
            }
        }
        public static Place getPlace(string SwarmID)
        {
            Place place = new Place();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                place = entity.Place.ToList().Find(p => p.SWARMID == SwarmID);
            }
            return place;
        }
        public static Place getPlace(int PlaceID)
        {
            Place place = new Place();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                place = entity.Place.ToList().Find(p => p.PLACEID == PlaceID);
            }
            return place;
        }
        public static List<Place> getPlaces()
        {
            List<Place> places = new List<Place>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                places = entity.Place.ToList();
            }
            return places;
        }
        public static void deletePlace(Place place)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Place.Remove(place);
                entity.SaveChanges();
            }

        }
        #endregion
        #region PLACECATEGORIES
        public static void insertPlaceCategories(PlaceCategories category)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.PlaceCategories.Add(category);
                entity.SaveChanges();
            }
        }
        public static List<PlaceCategories> getPlaceCategories(int placeID)
        {
            List<PlaceCategories> categories = new List<PlaceCategories>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                categories = entity.PlaceCategories.ToList().FindAll(cat => cat.PLACEID == placeID);
            }
            return categories;
        }
        public static List<PlaceCategories> getPlaceCategories()
        {
            List<PlaceCategories> categories = new List<PlaceCategories>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                categories = entity.PlaceCategories.ToList();
            }
            return categories;
        }
        public static void deletePlaceCategories(PlaceCategories category)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.PlaceCategories.Remove(category);
                entity.SaveChanges();
            }

        }
        #endregion

        #region TRIPADVISORDATASET
        public static void mekanEkle(TRIPADVISORPLACESVIEWMODEL tm)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                TRIPADVISORPLACES yeni = new TRIPADVISORPLACES()
                {
                    LOCATION = tm.LOCATION,
                    PLACENAME = tm.PLACENAME,
                    ID = tm.ID
                };
                db.TRIPADVISORPLACES.Add(yeni);
                db.SaveChanges();
            }
        }

        public static void placesEkle(PlaceModelList m)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                Place yeni = new Place()
                {
                    ADDRESS = m.PlaceM.ADDRESS,
                    CHECKINSCOUNT = m.PlaceM.CHECKINSCOUNT,
                    CITY = m.PlaceM.CITY,
                    COUNTRY = m.PlaceM.COUNTRY,
                    CREATEDDATE = System.DateTime.Now,
                    LAT = m.PlaceM.LAT,
                    LNG = m.PlaceM.LNG,
                    NAME = m.PlaceM.NAME,
                    PLACEID = m.PlaceM.PLACEID,
                    SWARMID = m.PlaceM.SWARMID,
                    TIPCOUNT = m.PlaceM.TIPCOUNT,
                    USERSCOUNT = m.PlaceM.USERSCOUNT
                };

                db.Place.Add(yeni);
                db.SaveChanges();

                var kayit = (from p in db.Place
                             where (p.SWARMID == yeni.SWARMID)
                             select p).SingleOrDefault();

                PlaceCategories yeniC = new PlaceCategories()
                {
                    CATEGORIEID = m.PlaceCateM.CATEGORIEID,
                    PLACEID = kayit.PLACEID
                };
                db.PlaceCategories.Add(yeniC);
                db.SaveChanges();
            }
        }

        public static TRIPADVISORPLACESVIEWMODEL mekanGetir(int id)
        {
            using (CheckinDatasetEntities4 db = new CheckinDatasetEntities4())
            {
                var mekanBilgi = (from p in db.TRIPADVISORPLACES
                                  where (p.ID == id)
                                  select p).SingleOrDefault();
                TRIPADVISORPLACESVIEWMODEL dataModel = new TRIPADVISORPLACESVIEWMODEL()
                {
                    LOCATION = mekanBilgi.LOCATION,
                    PLACENAME = mekanBilgi.PLACENAME,
                    ID = mekanBilgi.ID
                };

                return dataModel;
            }
        }

        #endregion

        #region LIKES
        public static void insertLikes(Likes like)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Likes.Add(like);
                entity.SaveChanges();
            }
        }
        public static Likes getLike(int LikeID)
        {
            Likes Like = new Likes();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Like = entity.Likes.ToList().Find(l => l.LIKEID == LikeID);
            }
            return Like;
        }
        public static void deleteLikes(Likes Likes)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.Likes.Remove(Likes);
                entity.SaveChanges();
            }

        }
        public static Likes searchLike(string Title)
        {
            Likes Like = new Likes();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Like = entity.Likes.ToList().Find(l => l.TITLE == Title);
            }
            return Like;
        }
        public static List<Likes> listLikes()
        {
            List<Likes> Likes = new List<Likes>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Likes = entity.Likes.ToList();
            }
            return Likes;
        }
        #endregion

        #region LIKECATEGORIES
        public static void insertLikeCategories(EntityFramework.LikeCategories likeCat)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.LikeCategories.Add(likeCat);
                entity.SaveChanges();
            }
        }
        public static LikeCategories getLikeCategories(int LikeCatID)
        {
            LikeCategories LikeCat = new LikeCategories();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                LikeCat = entity.LikeCategories.ToList().Find(l => l.LIKECATEGORYID == LikeCatID);
            }
            return LikeCat;
        }
        public static void deleteLikeCategories(LikeCategories LikeCat)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.LikeCategories.Remove(LikeCat);
                entity.SaveChanges();
            }

        }
        public static List<LikeCategories> searchLikeCategoriesByCategoryID(int CategoryID)
        {
            List<LikeCategories> Like = new List<LikeCategories>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Like = entity.LikeCategories.ToList().FindAll(l => l.CATEGORIEID == CategoryID);
            }
            return Like;
        }
        public static List<LikeCategories> searchLikeCategoriesByLikeID(int LikeID)
        {
            List<LikeCategories> Like = new List<LikeCategories>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Like = entity.LikeCategories.ToList().FindAll(l => l.LIKEID == LikeID);
            }
            return Like;
        }
        public static List<LikeCategories> listLikeCategories()
        {
            List<LikeCategories> Likes = new List<LikeCategories>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Likes = entity.LikeCategories.ToList();
            }
            return Likes;
        }
        #endregion

        #region USERLIKES
        public static void insertUserLikes(UserLikes userLike)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.UserLikes.Add(userLike);
                entity.SaveChanges();
            }
        }
        public static UserLikes getUserLikes(int userLikeID)
        {
            UserLikes uLike = new UserLikes();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                uLike = entity.UserLikes.ToList().Find(l => l.USERLIKEID == userLikeID);
            }
            return uLike;
        }
        public static void deleteUserLikes(UserLikes userLike)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.UserLikes.Remove(userLike);
                entity.SaveChanges();
            }

        }
        public static List<UserLikes> searchUserLikesByLikeID(int LikeID)
        {
            List<UserLikes> Likes = new List<UserLikes>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Likes = entity.UserLikes.ToList().FindAll(l => l.LIKEID == LikeID);
            }
            return Likes;
        }
        public static List<UserLikes> searchUserLikesByUserID(int UserLoginID)
        {
            List<UserLikes> Likes = new List<UserLikes>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Likes = entity.UserLikes.ToList().FindAll(l => l.USERLOGINID == UserLoginID);
            }
            return Likes;
        }
        public static List<UserLikes> listUserLikes()
        {
            List<UserLikes> Likes = new List<UserLikes>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Likes = entity.UserLikes.ToList();
            }
            return Likes;
        }

        #endregion

        #region CategoryProfile
        public static void insertCategoryProfile(CategoryProfile cProfile)
        {
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                entity.CategoryProfile.Add(cProfile);
                entity.SaveChanges();
            }
        }
        public static List<CategoryProfile> listCategoryProfile()
        {
            List<CategoryProfile> Likes = new List<CategoryProfile>();
            using (var entity = new EntityFramework.CheckinDatasetEntities4())
            {
                Likes = entity.CategoryProfile.ToList();
            }
            return Likes;
        }
        #endregion

        #region UserLoginInfo

        //public static void insertUSerLoginInfo(UserLoginInfo user)
        //{
        //    using (var entity = new EntityFramework.CheckinDatasetEntities4())
        //    {
        //        entity.UserLoginInfo.Add(user);
        //        entity.SaveChanges();
        //    }

        //}
        //public static void updateUSerLoginInfo(UserLoginInfo user)
        //{
        //    using (var entity = new EntityFramework.CheckinDatasetEntities4())
        //    {
        //        var u = entity.UserLoginInfo.Single(x => x.USERLOGINID == user.USERLOGINID);
        //        u.SWARMACCESSTOKEN = user.SWARMACCESSTOKEN;
        //        entity.SaveChanges();
        //    }

        //}
        #endregion
    }
}

