using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class CheckInResult
    {
        public class Meta
        {
            public int code { get; set; }
            public string requestId { get; set; }
        }

        public class Entity
        {
            public List<int> indices { get; set; }
            public string type { get; set; }
            public string id { get; set; }
        }

        public class Photo
        {
            public string prefix { get; set; }
            public string suffix { get; set; }
        }

        public class With
        {
            public string id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string gender { get; set; }
            public Photo photo { get; set; }
            public bool isAnonymous { get; set; }
        }

        public class Photo2
        {
            public string prefix { get; set; }
            public string suffix { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string gender { get; set; }
            public Photo2 photo { get; set; }
            public bool isAnonymous { get; set; }
        }

        public class Contact
        {
        }

        public class Location
        {
            public string address { get; set; }
            public string crossStreet { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public string cc { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public List<string> formattedAddress { get; set; }
        }

        public class Icon
        {
            public string prefix { get; set; }
            public string suffix { get; set; }
        }

        public class Category
        {
            public string id { get; set; }
            public string name { get; set; }
            public string pluralName { get; set; }
            public string shortName { get; set; }
            public Icon icon { get; set; }
            public bool primary { get; set; }
        }

        public class Stats
        {
            public int checkinsCount { get; set; }
            public int usersCount { get; set; }
            public int tipCount { get; set; }
        }

        public class BeenHere
        {
            public int unconfirmedCount { get; set; }
            public bool marked { get; set; }
            public int lastCheckinExpiredAt { get; set; }
        }

        public class Venue
        {
            public string id { get; set; }
            public string name { get; set; }
            public Contact contact { get; set; }
            public Location location { get; set; }
            public List<Category> categories { get; set; }
            public bool verified { get; set; }
            public Stats stats { get; set; }
            public bool allowMenuUrlEdit { get; set; }
            public BeenHere beenHere { get; set; }
        }

        public class Source
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Photos
        {
            public int count { get; set; }
            public List<object> items { get; set; }
        }

        public class Photo3
        {
            public string prefix { get; set; }
            public string suffix { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string gender { get; set; }
            public Photo3 photo { get; set; }
            public bool isAnonymous { get; set; }
        }

        public class Group
        {
            public string type { get; set; }
            public int count { get; set; }
            public List<Item> items { get; set; }
        }

        public class Likes
        {
            public int count { get; set; }
            public List<Group> groups { get; set; }
            public string summary { get; set; }
        }

        public class Score2
        {
            public string icon { get; set; }
            public string message { get; set; }
            public int points { get; set; }
        }

        public class Score
        {
            public int total { get; set; }
            public List<Score2> scores { get; set; }
        }

        public class Checkin
        {
            public string id { get; set; }
            public int createdAt { get; set; }
            public string type { get; set; }
            public List<Entity> entities { get; set; }
            public string shout { get; set; }
            public int timeZoneOffset { get; set; }
            public List<With> with { get; set; }
            public User user { get; set; }
            public Venue venue { get; set; }
            public Source source { get; set; }
            public Photos photos { get; set; }
            public Likes likes { get; set; }
            public bool isMayor { get; set; }
            public Score score { get; set; }
            public string reasonCannotSeeComments { get; set; }
            public string reasonCannotAddComments { get; set; }
        }

        public class Response
        {
            public Checkin checkin { get; set; }
            public string signature { get; set; }
        }

        public class RootObject
        {
            public Meta meta { get; set; }
            public Response response { get; set; }
        }
    }
}
