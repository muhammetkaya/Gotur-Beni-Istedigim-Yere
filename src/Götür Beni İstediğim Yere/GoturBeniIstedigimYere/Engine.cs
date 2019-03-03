using GoturBeniIstedigimYere.DataAccessLayer;
using GoturBeniIstedigimYere.EntityFramework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class Engine
    {
        public double PopularityThreshold { get; set; }
        public double CategoryQualityThreshold { get; set; }
        private string clientID = ConfigurationManager.AppSettings["4SquareClientID"];
        private string clientSecret = ConfigurationManager.AppSettings["4SquareClientSecret"];
        private List<Place> places;
        private List<PlaceCategories> placeCategories;
        private List<Categorie> CATEGORIES;
        private List<Checkin> UserCheckins;
        private List<CheckinOperations.CheckinUser> checkinUser;

        /// <summary>
        /// Thresholds
        /// </summary>
        /// <param name="Popularity"></param>
        /// <param name="CategoryQuality"></param>
        public Engine(double Popularity, double CategoryQuality)
        {
            PopularityThreshold = Popularity;
            CategoryQualityThreshold = CategoryQuality;

            places = CheckinOperations.getPlaces().ToList();
            placeCategories = CheckinOperations.getPlaceCategories();
            CATEGORIES = CheckinOperations.getCategorie();
            UserCheckins = CheckinOperations.listCheckIns();
            checkinUser = CheckinOperations.listCheckInsReturnedUser();
        }

        public double UserCategoryPoint(double CheckinCount, int CategoryID, int PlaceID)
        {
            double uCategoryPoint = 0;
            //double CategoryPoint = getCategoryPoint(CategoryID);
            //double VenueQuality = getVenueQuality(PlaceID);
            double VenueQuality = 1;
            //double UserCP = CheckinCount * (1 + HobbyPoint);
            double UserCP = CheckinCount;
            uCategoryPoint = UserCP * VenueQuality;
            return uCategoryPoint;
        }
        public double CMPoint(double CategoryPoint, double VenueQuality)
        {
            double CMpoint = (CategoryPoint) * (1 + VenueQuality);
            return CMpoint;
        }
        public double getCategoryPoint(int CategoryID)
        {
            double returnedConsumerRatio = 0;
            double popularity = 0;
            double categoryQuality = 0;
            double CategoryPoint = 0;
            double TotalCheckinCount = 0;
            double NetCheckinCount = 0;

            Dictionary<string, int> CategorieTotal = new Dictionary<string, int>();
            Dictionary<string, int> CategorieDistinct = new Dictionary<string, int>();

            CategorieTotal = CategorieCounts(UserCheckins);

            var checkins = UserCheckins.Select(u => u.PLACEID).Distinct();
            CategorieDistinct = CategorieCounts(checkins);

            var users = checkinUser.FindAll(u => u.placeCategory.CATEGORIEID == CategoryID).Select(u => u.user.USERID).Distinct();

            var cat = CATEGORIES.Find(c => c.CATEGORIEID == CategoryID);

            TotalCheckinCount = CategorieTotal[cat.NAME];
            NetCheckinCount = CategorieDistinct[cat.NAME];

            returnedConsumerRatio = getReturnedConsumerRatio(users.Count(), TotalCheckinCount);
            categoryQuality = 0;
            popularity = 0;

            if (NetCheckinCount != 0)
                popularity = TotalCheckinCount / NetCheckinCount;
            if (returnedConsumerRatio != 0)
                categoryQuality = NetCheckinCount * returnedConsumerRatio;

            popularity = popularity * PopularityThreshold;
            categoryQuality = categoryQuality * CategoryQualityThreshold;

            CategoryPoint = popularity * (1 + categoryQuality);

            return CategoryPoint;
        }
        public double getVenueQuality(int PlaceID)
        {
            var venue = places.Find(p => p.PLACEID == PlaceID);
            Swarm sw = new Swarm(clientID, clientSecret);
            var newVenue = sw.VenueDetails(venue.SWARMID);

            double rating = 1;
            if (newVenue.rating >= 0)
                rating = newVenue.rating;
            long checkinCount = newVenue.stats.checkinsCount;
            long userCount = newVenue.stats.usersCount;


            double PopulerUserCheckinPoint = getPopulerUserCheckinPoint(PlaceID);
            double VenueQuality = getDefaultVenueQuality(checkinCount, userCount);
            if (PopulerUserCheckinPoint > 0)
                VenueQuality = VenueQuality * (1 + PopulerUserCheckinPoint);
            VenueQuality *= rating;
            return VenueQuality;

        }
        public double getVenueQuality(FourSquare.SharpSquare.Entities.Venue v)
        {
            var venue = places.Find(p => p.SWARMID == v.id);
            var newVenue = v;

            double rating = 1;
            if (newVenue.rating > 0)
                rating = newVenue.rating;
            long checkinCount = newVenue.stats.checkinsCount;
            long userCount = newVenue.stats.usersCount;
            double PopulerUserCheckinPoint = 0;
            if (venue != null)
                PopulerUserCheckinPoint = getPopulerUserCheckinPoint(venue.PLACEID);
            double VenueQuality = getDefaultVenueQuality(checkinCount, userCount);
            if (PopulerUserCheckinPoint > 0)
                VenueQuality = VenueQuality * (1 + PopulerUserCheckinPoint);
            VenueQuality *= rating;
            return VenueQuality;

        }

        public Dictionary<string, double> CategoryNormalize(Dictionary<string, double> catAndValues)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            return result;
        }

        private Dictionary<string, int> CategorieCounts(List<Checkin> checkins)
        {
            Dictionary<string, int> result = getDefaultData();
            ConcurrentDictionary<string, int> res = new ConcurrentDictionary<string, int>();
            foreach (var item in CATEGORIES)
                res.AddOrUpdate(item.NAME, 0, (key, oldvalue) => oldvalue + 1);
            Parallel.ForEach(checkins, (check) =>
            {
                //var Place = places.Find(p => p.PLACEID == check.PLACEID.Value);
                //var PlaceCategories = placeCategories.FindAll(c => c.PLACEID == Place.PLACEID);
                var Place = (from p in places
                             where p.PLACEID == check.PLACEID.Value
                             select p).FirstOrDefault();

                var PlaceCategories = (from c in placeCategories
                                       where c.PLACEID == Place.PLACEID
                                       select c).ToList();

                foreach (var pCat in PlaceCategories)
                {

                    //var cat = CheckinOperations.getCategorie().Find(c => c.CATEGORIEID == pCat.CATEGORIEID);

                    var cat = (from c in CATEGORIES
                               where c.CATEGORIEID == pCat.CATEGORIEID
                               select c).FirstOrDefault();
                    res.AddOrUpdate(cat.NAME, 1, (key, oldvalue) => oldvalue + 1);
                }
            });

            result = res.ToDictionary(x => x.Key, x => x.Value);

            return result;
        }

        private Dictionary<string, int> CategorieCounts(IEnumerable<int?> checkins)
        {
            Dictionary<string, int> result = getDefaultData();

            ConcurrentDictionary<string, int> res = new ConcurrentDictionary<string, int>();
            foreach (var item in CATEGORIES)
                res.AddOrUpdate(item.NAME, 0, (key, oldvalue) => oldvalue + 1);

            Parallel.ForEach(checkins, (check) =>
            {
                //var Place = places.Find(p => p.PLACEID == check.Value);
                //var PlaceCategories = placeCategories.FindAll(c => c.PLACEID == Place.PLACEID);

                var Place = (from p in places
                             where p.PLACEID == check.Value
                             select p).FirstOrDefault();

                var PlaceCategories = (from c in placeCategories
                                       where c.PLACEID == Place.PLACEID
                                       select c).ToList();

                foreach (var pCat in PlaceCategories)
                {
                    //var cat = CheckinOperations.getCategorie().Find(c => c.CATEGORIEID == pCat.CATEGORIEID);
                    var cat = (from c in CATEGORIES
                               where c.CATEGORIEID == pCat.CATEGORIEID
                               select c).FirstOrDefault();
                    res.AddOrUpdate(cat.NAME, 1, (key, oldvalue) => oldvalue + 1);
                }
            });

            result = res.ToDictionary(x => x.Key, x => x.Value);
            //foreach (var checkin in checkins)
            //{
            //    var Place = places.Find(p => p.PLACEID == checkin.Value);
            //    var PlaceCategories = placeCategories.FindAll(c => c.PLACEID == Place.PLACEID);
            //    foreach (var pCat in PlaceCategories)
            //    {
            //        var cat = CheckinOperations.getCategorie().Find(c => c.CATEGORIEID == pCat.CATEGORIEID);
            //        result[cat.NAME] += 1;
            //    }
            //}
            return result;
        }

        private double getDefaultVenueQuality(double CheckinCount, double UserCount)
        {
            double VenueQuality = 0;
            double returnedConsumerRatio = getReturnedConsumerRatio(UserCount, CheckinCount);
            if (returnedConsumerRatio == 0)
                VenueQuality = CheckinCount / UserCount;
            else
                VenueQuality = returnedConsumerRatio * UserCount;
            return VenueQuality;
        }
        private double getPopulerUserCheckinPoint(int PlaceID)
        {
            double VenueQuality = 0;
            var venue = places.Find(p => p.PLACEID == PlaceID);
            double checkinCount = UserCheckins.FindAll(c => c.PLACEID == PlaceID).Count;
            double userCount = checkinUser.FindAll(u => u.placeCategory.PLACEID == PlaceID).Select(u => u.user.USERID).Distinct().Count();
            if (userCount == 0) return VenueQuality;

            VenueQuality = getDefaultVenueQuality(checkinCount, userCount);
            return VenueQuality;
        }
        private double getReturnedConsumerRatio(double UserCount, double TotalCheckinCount)
        {
            double returnedConsumerCount = 0;
            double returnedConsumerRatio = 0;
            if (TotalCheckinCount == 0) return 0;
            returnedConsumerCount = (UserCount / TotalCheckinCount) * Math.Abs(TotalCheckinCount - UserCount);
            returnedConsumerRatio = returnedConsumerCount / UserCount;
            return returnedConsumerRatio;
        }
        private Dictionary<string, int> getDefaultData()
        {
            Dictionary<string, int> data = new Dictionary<string, int>();

            var categories = CheckinOperations.getCategorie();
            foreach (var cat in categories)
                data.Add(cat.NAME, 0);
            return data;
        }
    }
}
