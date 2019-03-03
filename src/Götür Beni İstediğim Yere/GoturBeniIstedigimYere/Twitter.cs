using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class Twitter
    {
        private MvcAuthorizer auth { get; set; }
        private TwitterContext twitterCtx;
        public Twitter(MvcAuthorizer Authorizer)
        {
            this.auth = Authorizer;
            twitterCtx = new TwitterContext(auth);
        }
        #region PUBLIC
        public User getUserInformation(string Username)
        {
            try
            {
                var userResponse = (from user in twitterCtx.User
                                    where user.Type == UserType.Lookup &&
                                          user.ScreenNameList == Username
                                    select user).ToList().First();
                return userResponse;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        public List<Status> getAllPosts(string Username)
        {
            try
            {
                List<Status> statusList = new List<Status>();

                var twitterCtx = new TwitterContext(auth);
                ulong maxID;
                const int Count = 200;

                ulong sinceID = 1;

                var userStatusResponse =
                    (from tweet in twitterCtx.Status
                     where tweet.Type == StatusType.User &&
                       tweet.ScreenName == Username &&
                       tweet.SinceID == sinceID &&
                       tweet.Count == Count
                     select tweet)
                    .ToList();

                statusList.AddRange(userStatusResponse);

                // first tweet processed on current query
                maxID = userStatusResponse.Min(
                    status => ulong.Parse(status.StatusID.ToString())) - 1;

                do
                {
                    // now add sinceID and maxID
                    userStatusResponse =
                        (from tweet in twitterCtx.Status
                         where tweet.Type == StatusType.User &&
                               tweet.ScreenName == Username &&
                               tweet.Count == Count &&
                               tweet.SinceID == sinceID &&
                               tweet.MaxID == maxID
                         select tweet)
                        .ToList();

                    if (userStatusResponse.Count > 0)
                    {
                        // first tweet processed on current query
                        maxID = userStatusResponse.Min(
                            status => ulong.Parse(status.StatusID.ToString())) - 1;

                        statusList.AddRange(userStatusResponse);
                    }
                }
                while (userStatusResponse.Count != 0 && statusList.Count < 100);

                return statusList;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public List<Status> getPosts(string Username, ulong SinceID)
        {
            try
            {
                List<Status> statusList = new List<Status>();

                var twitterCtx = new TwitterContext(auth);
                ulong maxID = 0;
                const int Count = 200;

                var userStatusResponse =
                    (from tweet in twitterCtx.Status
                     where tweet.Type == StatusType.Home &&
                       tweet.ScreenName == Username &&
                       //tweet.SinceID == SinceID &&
                       tweet.MaxID == maxID &&
                       tweet.Count == Count &&
                       tweet.RetweetedStatus.Source == null
                     select tweet)
                    .ToList();

                statusList.AddRange(userStatusResponse);

                // first tweet processed on current query
                maxID = userStatusResponse.Min(
                    status => ulong.Parse(status.StatusID.ToString())) - 1;

                do
                {
                    // now add sinceID and maxID
                    userStatusResponse =
                        (from tweet in twitterCtx.Status
                         where tweet.Type == StatusType.Home &&
                               tweet.ScreenName == Username &&
                               tweet.Count == Count &&
                               //tweet.SinceID == SinceID &&
                               tweet.MaxID == maxID &&
                               tweet.RetweetedStatus.Source == null
                         select tweet)
                        .ToList();

                    if (userStatusResponse.Count > 0)
                    {
                        // first tweet processed on current query
                        maxID = userStatusResponse.Min(
                            status => ulong.Parse(status.StatusID.ToString())) - 1;

                        statusList.AddRange(userStatusResponse);
                    }
                }
                while (userStatusResponse.Count != 0 && statusList.Count < 100);

                return statusList;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public Status getPost(string Username, string TweetID)
        {
            try
            {
                var twitterCtx = new TwitterContext(auth);

                var userStatusResponse =
                    (from tweet in twitterCtx.Status
                     where tweet.Type == StatusType.User &&
                       tweet.ScreenName == Username &&
                       tweet.StatusID == Convert.ToDouble(TweetID)
                     select tweet).FirstOrDefault();


                return userStatusResponse;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
