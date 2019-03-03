using GoturBeniIstedigimYere.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class PostAnalyzer
    {
        private const string instagram_Suffix = "instagram.com";
        private const string foursquare_Suffix = "foursquare.com";
        private const string twitter_Suffix = "twitter.com";

        private bool isCheckin(string text, string source)
        {
            bool checkin = false;

            PostType pType = getPostType(source);
            switch (pType)
            {
                case PostType.Foursquare:
                    checkin = true;
                    break;
                case PostType.Instagram:
                    checkin = false;
                    break;
                case PostType.Twitter:
                    checkin = false;
                    break;
                case PostType.Other:
                    checkin = false;
                    break;
                default:
                    break;
            }
            return checkin;
        }

        private PostType getPostType(string Source)
        {
            PostType type = PostType.Other;
            if (Source.Contains(instagram_Suffix))
                type = PostType.Instagram;
            else if (Source.Contains(foursquare_Suffix))
                type = PostType.Foursquare;
            else if (Source.Contains(twitter_Suffix))
                type = PostType.Twitter;
            return type;
        }

        public List<Post> getCheckins(List<Post> Posts)
        {
            List<Post> checkins = new List<Post>();
            foreach (var post in Posts)
                if (isCheckin(post.TEXT, post.SOURCE))
                    checkins.Add(post);
            return checkins;
        }

        public string getUrl(string tweet)
        {
            int shortlinkStartIndex = tweet.IndexOf("http");
            string url = "";
            for (int i = shortlinkStartIndex; i < tweet.Length; i++)
            {
                if (tweet[i] == ' ')
                    break;
                url += tweet[i];
            }
            return url;
        }
        public string GetCheckinID(string url)
        {
            url = UrlLengthen(url);
            string[] parts = url.Split('/');

            return parts[parts.Length - 1];
        }

        private string UrlLengthen(string url)
        {
            string newUrl = url;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = false;

            var response = (HttpWebResponse)request.GetResponse();

            if ((int)response.StatusCode == 301 || (int)response.StatusCode == 302)
            {
                newUrl = response.Headers["Location"];
            }
            return newUrl;
        }
    }
}
