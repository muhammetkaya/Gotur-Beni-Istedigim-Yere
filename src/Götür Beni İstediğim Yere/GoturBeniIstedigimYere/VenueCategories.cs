using FourSquare.SharpSquare.Entities;
using GoturBeniIstedigimYere.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class VenueCategories
    {
        public Venue Venue { get; set; }
        public List<FourSquare.SharpSquare.Entities.Category> Categories { get; set; }
        private string clientID = ConfigurationManager.AppSettings["4SquareClientID"];
        private string clientSecret = ConfigurationManager.AppSettings["4SquareClientSecret"];
        private List<EntityFramework.Place> places = CheckinOperations.getPlaces();
        public VenueCategories(Venue venue)
        {
            this.Venue = venue;
            Categories = new List<FourSquare.SharpSquare.Entities.Category>();
            getCategories();
        }
        public VenueCategories(EntityFramework.Checkin checkin)
        {
            var venue = places.Find(p => checkin.PLACEID == p.PLACEID);
            Swarm sw = new Swarm(clientID, clientSecret);
            var newVenue = sw.VenueDetails(venue.SWARMID);
            this.Venue = newVenue;
            Categories = new List<FourSquare.SharpSquare.Entities.Category>();
            getCategories();
        }

        void getCategories()
        {
            category(Venue.categories);
        }
        void category(List<FourSquare.SharpSquare.Entities.Category> treeCategories)
        {
            treeCategories.ForEach(c =>
            {
                Categories.Add(c);
                if (c.categories != null)
                    category(c.categories);
            });
        }

    }
}
