using GoturBeniIstedigimYere.DataAccessLayer;
using GoturBeniIstedigimYere.EntityFramework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class AnalyzeEngine
    {
        double popularityThreshold = 0.6;
        double CategoryQualityThreshold = 0.4;
        ConcurrentDictionary<string, double> categoryPoints;
        List<PlaceCategories> pCategories;
        List<Categorie> Categorie;
        List<LikeCategories> likeCategories;
        public AnalyzeEngine()
        {
            categoryPoints = new ConcurrentDictionary<string, double>();
            _Load();
        }
        public AnalyzeEngine(bool tf)
        {
            categoryPoints = new ConcurrentDictionary<string, double>();
            this.Categorie = CheckinOperations.getCategorie();
            pCategories = CheckinOperations.getPlaceCategories();
            likeCategories = CheckinOperations.listLikeCategories();
        }

        public List<UserLikes> getUserHobbyList(DataTable dt)
        {
            List<UserLikes> uLikes = new List<UserLikes>();
            foreach (var item in dt.Rows)
            {
                DataRow dr = (DataRow)item;
                int LikeID = Convert.ToInt32(dr.ItemArray[0]);
                string Title = Convert.ToString(dr.ItemArray[1]);
                string Description = Convert.ToString(dr.ItemArray[2]);
                var ParentLikeID = dr.ItemArray[3];
                int Value = Convert.ToInt32(dr.ItemArray[4]);

                uLikes.Add(new UserLikes()
                {
                    LIKEID = LikeID,
                    USERLOGINID = 1,
                    VALUE = Value
                });
            }
            return uLikes;
        }

        public Dictionary<string, double> getUserHobbyCategories(List<UserLikes> likes)
        {
            Dictionary<string, double> categories = getDefaultData();
            int max = likes.Max(l => l.VALUE).Value;
            int min = likes.Min(l => l.VALUE).Value;
            foreach (var item in likes)
            {
                //double NormalizedValue = Normalize(item.VALUE.Value, max, min);
                var lCategories = likeCategories.FindAll(lc => lc.LIKEID == item.LIKEID.Value);
                foreach (var cat in lCategories)
                {
                    var category = Categorie.Find(ca => ca.CATEGORIEID == cat.CATEGORIEID.Value);
                    categories[category.NAME] += item.VALUE.Value;
                }
            }
            return categories;
        }

        public List<Checkin> getUserCheckin()
        {
            return new List<Checkin>();
        }

        public void _Load()
        {
            categoryPoints = new ConcurrentDictionary<string, double>();
            this.Categorie = CheckinOperations.getCategorie();

            Engine eng = new Engine(popularityThreshold, CategoryQualityThreshold);

            List<string> list_lines = new List<string>();
            Parallel.ForEach(Categorie, (cat) =>
            {
                var catPoint = eng.getCategoryPoint(cat.CATEGORIEID);
                categoryPoints.AddOrUpdate(cat.NAME, catPoint, (key, oldvalue) => oldvalue + catPoint);
            });
            pCategories = CheckinOperations.getPlaceCategories();
            likeCategories = CheckinOperations.listLikeCategories();
            //var catPoint = eng.getCategoryPoint(896);
            //categoryPoints.AddOrUpdate("Amphitheater", catPoint, (key, oldvalue) => oldvalue + catPoint);
        }

        public Dictionary<string, double> getCategoryProfile(List<VenueCategories> Checkins, List<UserLikes> Likes)
        {
            Engine eng = new Engine(popularityThreshold, CategoryQualityThreshold);

            Dictionary<string, double> userVector = getDefaultData();

            var CategorizedHobbies = getUserHobbyCategories(Likes);

            foreach (KeyValuePair<string, double> item in CategorizedHobbies)
            {
                double KategoriIlgiDerecesi = item.Value;
                var category = Categorie.Find(c => c.NAME == item.Key);
                var kategoriName = item.Key;
                var cats = Checkins.FindAll(c => c.Categories.Find(c2 => c2.name == item.Key) != null).ToList();
                double CMPoints = 0;
                var CategoryPoint = eng.getCategoryPoint(category.CATEGORIEID);
                if (cats.Count > 0)
                {
                    foreach (var c in cats)
                    {
                        var venueQuality = eng.getVenueQuality(c.Venue);
                        CMPoints += eng.CMPoint(CategoryPoint, venueQuality);
                    }

                    CMPoints = CMPoints / cats.Count;
                    userVector[item.Key] = KategoriIlgiDerecesi * CMPoints;//Profil Puanı

                }
                else
                {
                    CMPoints = eng.CMPoint(CategoryPoint, 0);
                    userVector[item.Key] = 1 * CMPoints;//Profil Puanı
                }
            }

            double max = userVector.Max(v => v.Value);
            double min = userVector.Min(v => v.Value);

            foreach (var c in CategorizedHobbies)
                userVector[c.Key] = Normalize(userVector[c.Key], max, min);

            return userVector;
        }

        public Dictionary<string, double> getCategoryProfile(List<VenueCategories> Checkins)
        {
            Engine eng = new Engine(popularityThreshold, CategoryQualityThreshold);

            Dictionary<string, double> userVector = getDefaultData();

            var CategorizedHobbies = getDefaultData();

            foreach (KeyValuePair<string, double> item in CategorizedHobbies)
            {
                double KategoriIlgiDerecesi = item.Value;
                var category = Categorie.Find(c => c.NAME == item.Key);
                var kategoriName = item.Key;
                var cats = Checkins.FindAll(c => c.Categories.Find(c2 => c2.name == item.Key) != null).ToList();
                double CMPoints = 0;
                var CategoryPoint = eng.getCategoryPoint(category.CATEGORIEID);
                if (cats.Count > 0)
                {
                    foreach (var c in cats)
                    {
                        var venueQuality = eng.getVenueQuality(c.Venue);
                        CMPoints += eng.CMPoint(CategoryPoint, venueQuality);
                    }

                    CMPoints = CMPoints / cats.Count;
                    userVector[item.Key] = 1 * CMPoints;

                }
                else
                {
                    CMPoints = eng.CMPoint(CategoryPoint, 0);
                    userVector[item.Key] = 0 * CMPoints;
                }
            }

            double max = userVector.Max(v => v.Value);
            double min = userVector.Min(v => v.Value);

            foreach (var c in CategorizedHobbies)
                userVector[c.Key] = Normalize(userVector[c.Key], max, min);

            return userVector;
        }

        public Dictionary<string, double> getCategoryProfile(List<UserLikes> Likes)
        {
            Engine eng = new Engine(popularityThreshold, CategoryQualityThreshold);

            Dictionary<string, double> userVector = getDefaultData();

            var CategorizedHobbies = getUserHobbyCategories(Likes);

            foreach (KeyValuePair<string, double> item in CategorizedHobbies)
            {
                double KategoriIlgiDerecesi = item.Value;
                var category = Categorie.Find(c => c.NAME == item.Key);
                var kategoriName = item.Key;
                double CMPoints = 0;
                var CategoryPoint = eng.getCategoryPoint(category.CATEGORIEID);

                CMPoints = eng.CMPoint(CategoryPoint, 0);
                userVector[item.Key] = KategoriIlgiDerecesi * CMPoints;

            }

            double max = userVector.Max(v => v.Value);
            double min = userVector.Min(v => v.Value);

            foreach (var c in CategorizedHobbies)
                userVector[c.Key] = Normalize(userVector[c.Key], max, min);

            return userVector;
        }

        public List<double> VectorToList(Dictionary<string, double> vector)
        {
            List<double> vList = new List<double>();
            foreach (KeyValuePair<string, double> item in vector)
                vList.Add(item.Value);
            return vList;
        }

        private Dictionary<string, double> getDefaultData()
        {
            Dictionary<string, double> data = new Dictionary<string, double>();


            foreach (var cat in Categorie)
                data.Add(cat.NAME, 0);
            return data;
        }
        public double Normalize(double value, double maxValue, double minValue)
        {
            return (value - minValue) / (maxValue - minValue);
        }

    }
}
