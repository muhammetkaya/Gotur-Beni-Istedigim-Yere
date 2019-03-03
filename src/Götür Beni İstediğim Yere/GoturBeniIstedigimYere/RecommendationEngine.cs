using GoturBeniIstedigimYere.DataAccessLayer;
using GoturBeniIstedigimYere.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class RecommendationEngine
    {
        private List<Categorie> categories;
        private List<Place> places;
        private List<PlaceCategories> pCategories;
        private AnalyzeEngine aEng;

        public RecommendationEngine()
        {
            categories = CheckinOperations.getCategorie();
            places = CheckinOperations.getPlaces();
            pCategories = CheckinOperations.getPlaceCategories();
            aEng = new AnalyzeEngine();
        }

        public List<PlaceAndImages> RecommendVenues(List<UserLikes> userLikes)
        {
            var catProfile = aEng.getCategoryProfile(userLikes);
            var RecommendeVenues = getRecommendVenue(catProfile);
            return getImages(RecommendeVenues);

        }
        public List<PlaceAndImages> RecommendVenues(List<UserLikes> userLikes, List<FourSquare.SharpSquare.Entities.VenueHistory> Checkins)
        {
            List<VenueCategories> venues = new List<VenueCategories>();

            Parallel.ForEach(Checkins, checkin =>
            {
                venues.Add(new VenueCategories(checkin.venue));
            });

            var catProfile = aEng.getCategoryProfile(venues, userLikes);
            var RecommendeVenues = getRecommendVenue(catProfile);
            return getImages(RecommendeVenues);
        }
        public List<PlaceAndImages> RecommendVenues(List<FourSquare.SharpSquare.Entities.VenueHistory> Checkins)
        {
            List<VenueCategories> venues = new List<VenueCategories>();

            Parallel.ForEach(Checkins, checkin =>
            {
                venues.Add(new VenueCategories(checkin.venue));
            });
            var catProfile = aEng.getCategoryProfile(venues);
            var RecommendeVenues = getRecommendVenue(catProfile);
            return getImages(RecommendeVenues);
        }

        private List<Place> SortByHobbies(Dictionary<string, double> categorizedHobbies, List<Place> result)
        {
            return result;
        }

        private Place getPlace(List<int> ids, Categorie c)
        {
            var placeCategory = pCategories.FindAll(p => (!ids.Contains(p.PLACEID.Value)) && p.CATEGORIEID == c.CATEGORIEID);
            var result = new List<Place>();
            foreach (var item in placeCategory)
            {
                var place = places.Find(p => p.PLACEID == item.PLACEID.Value);
                result.Add(place);
            }
            if (result.Count > 0)
            {
                int CheckinMax = result.Max(p => p.CHECKINSCOUNT).Value;
                int userMax = result.Max(p => p.USERSCOUNT).Value;
                return result.Find(p => p.CHECKINSCOUNT == CheckinMax || p.USERSCOUNT == userMax);
            }
            else
                return null;

        }

        public List<PlaceAndImages> DifferentCategoryRecommendVenues(List<UserLikes> userLikes)
        {
            var catProfile = aEng.getCategoryProfile(userLikes);
            var similarUsers = getSimilarUsers(catProfile);
            var result = getRecommendVenue(catProfile, similarUsers[0].fenomenProfile);
            return getImages(result);
        }

        public List<PlaceAndImages> DifferentCategoryRecommendVenues(List<UserLikes> userLikes, List<FourSquare.SharpSquare.Entities.VenueHistory> Checkins)
        {
            List<VenueCategories> venues = new List<VenueCategories>();

            Parallel.ForEach(Checkins, checkin =>
            {
                venues.Add(new VenueCategories(checkin.venue));
            });

            var catProfile = aEng.getCategoryProfile(venues, userLikes);
            var similarUsers = getSimilarUsers(catProfile);
            var result = getRecommendVenue(catProfile, similarUsers[0].fenomenProfile);
            return getImages(result);
        }

        public List<PlaceAndImages> DifferentCategoryRecommendVenues(List<FourSquare.SharpSquare.Entities.VenueHistory> Checkins)
        {
            List<VenueCategories> venues = new List<VenueCategories>();

            Parallel.ForEach(Checkins, checkin =>
            {
                venues.Add(new VenueCategories(checkin.venue));
            });
            var catProfile = aEng.getCategoryProfile(venues);
            var similarUsers = getSimilarUsers(catProfile);
            var result = getRecommendVenue(catProfile, similarUsers[0].fenomenProfile);
            return getImages(result);

        }

        public class FenomenResult
        {
            public Dictionary<string, double> fenomenProfile;
            public double SimilarityResult { get; set; }
            public FenomenResult()
            {
                fenomenProfile = new Dictionary<string, double>();
            }
        }
        private List<FenomenResult> getSimilarUsers(Dictionary<string, double> CategoryProfile)
        {
            //Fenomen Profilleri oluşturuluyor..
            var FenomenProfiles = CheckinOperations.listCategoryProfile();
            List<Dictionary<string, double>> fProfiles = new List<Dictionary<string, double>>();
            string[] splitChars1 = { "#" };
            string[] splitChars2 = { ":" };
            foreach (var profile in FenomenProfiles)
            {
                Dictionary<string, double> profil = new Dictionary<string, double>();
                var features = profile.FEATURES.Split(splitChars1, StringSplitOptions.RemoveEmptyEntries);
                foreach (var f in features)
                {
                    string[] feat = f.Split(splitChars2, StringSplitOptions.RemoveEmptyEntries);
                    string name = feat[0];
                    string value = feat[1];
                    profil.Add(name, Convert.ToDouble(value));
                }
                fProfiles.Add(profil);
            }
            var result = new List<FenomenResult>();
            var userVector = aEng.VectorToList(CategoryProfile);
            for (int i = 0; i < fProfiles.Count; i++)
            {
                var fenomenVector = aEng.VectorToList(fProfiles[i]);
                var r = CosineSimilarity.GetCosineSimilarity(userVector, fenomenVector);
                result.Add(new FenomenResult()
                {
                    fenomenProfile = fProfiles[i],
                    SimilarityResult = r
                });
            }

            result.Sort((emp1, emp2) => emp1.SimilarityResult.CompareTo(emp2.SimilarityResult));

            return result;

        }
        private List<Place> getRecommendVenue(Dictionary<string, double> CategoryProfile)
        {
            List<Place> result = new List<Place>();
            var sortedCategoryProfile = from pair in CategoryProfile
                                        orderby pair.Value descending
                                        select pair;
            List<KeyValuePair<string, double>> lstSortedCategoryProfile = sortedCategoryProfile.ToList();
            for (int i = 0; i < lstSortedCategoryProfile.Count; i++)
            {
                //Search 1
                string TripleCategories = string.Empty;
                string DoubleCategories = string.Empty;
                string SingleCategories = string.Empty;
                for (int j = i; j < lstSortedCategoryProfile.Count && j < i + 3; j++)
                    TripleCategories += lstSortedCategoryProfile[j].Key + ",";
                List<Place> tripleRecommendation = getVenues(TripleCategories);
                for (int j = i; j < lstSortedCategoryProfile.Count && j < i + 2; j++)
                    DoubleCategories += lstSortedCategoryProfile[j].Key + ",";
                List<Place> doubleRecommendation = getVenues(DoubleCategories);
                for (int j = i; j < lstSortedCategoryProfile.Count && j < i + 1; j++)
                    SingleCategories += lstSortedCategoryProfile[j].Key + ",";
                List<Place> singleRecommendation = getVenues(SingleCategories);

                if (tripleRecommendation.Count > 0)
                    result.Add(tripleRecommendation[0]);
                if (doubleRecommendation.Count > 0)
                    result.Add(doubleRecommendation[0]);
                else if (tripleRecommendation.Count > 1)
                    result.Add(tripleRecommendation[1]);
                if (tripleRecommendation.Count == 0 || doubleRecommendation.Count == 0)
                {
                    if (doubleRecommendation.Count > 1)
                        result.Add(doubleRecommendation[1]);
                    else if(singleRecommendation.Count > 0)
                        result.Add(singleRecommendation[0]);
                }


            }
            return result;
        }
        private List<Place> getRecommendVenue(Dictionary<string, double> UserProfile, Dictionary<string, double> FenomenProfil)
        {
            List<Place> result = new List<Place>();
            var sortedUserProfile = from pair in UserProfile
                                    orderby pair.Value descending
                                    select pair;
            foreach (var item in sortedUserProfile)
                if (item.Value != 0)
                    FenomenProfil.Remove(item.Key);
            return getRecommendVenue(FenomenProfil);
        }
        private List<Place> getVenues(string Categories)
        {
            string[] splitChar = { "," };
            string[] Cats = Categories.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            List<Place> venues = places.ToList();
            foreach (var item in Cats)
            {
                var cat = categories.Find(c => c.NAME == item);
                venues = venues.FindAll(v => pCategories.FindAll(pc => pc.PLACEID == v.PLACEID).Find(pc2 => pc2.CATEGORIEID == cat.CATEGORIEID) != null);
            }
            return venues;
        }

        private List<PlaceAndImages> getImages(List<Place> places)
        {
            List<PlaceAndImages> pAndImages = new List<PlaceAndImages>();
            foreach (var item in places)
            {
                var pc = pCategories.Find(p => p.PLACEID == item.PLACEID);
                var category = categories.Find(c => c.CATEGORIEID == pc.CATEGORIEID);
                var url = category.ICON.Replace("ss3.4sqi.net", "foursquare.com");
                url = url.Replace("categories_v2", "categories");
                url = url.Replace(".png", "64.png");
                pAndImages.Add(new PlaceAndImages()
                {
                    Place = item,
                    Icon = url
                });
            }
            return pAndImages;
        }
    }
}
