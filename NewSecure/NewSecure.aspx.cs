using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToTwitter;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using NewSecure;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;

namespace NewSecure
{
    public partial class Test1 : System.Web.UI.Page
    {
        public static SignInAuthorizer auth;
        DictionaryDB dict;
        List<Status> NetworkList = new List<Status>();
        List<Status> MobileList = new List<Status>();
        List<Status> DesktopList = new List<Status>();
        List<Status> OthersList = new List<Status>();
        string activate = "deactivated";

        #region Home Categories (integars)
        int networkCategory = 0;
        int desktopCategory = 0;
        int mobileCategory = 0;
        int otherCategory = 0;
        #endregion

        #region Network Categories (integars)
        int emailCategory = 0;
        int OnlineCategory = 0;
        int SocialCategory = 0;
        int WebsiteCategory = 0;
        int ServerCategory = 0;
        int netotherCategory = 0;
        #endregion

        #region Mobile Categories (integars)
        int telecomCategory = 0;
        int MHCategory = 0;
        int MSCategory = 0;
        int PlatformCategory = 0;
        int MobOtherCategory = 0;
        #endregion

        #region Desktop Categories (integars)
        int DHCategory = 0;
        int DSCategory = 0;
        int OperatingCategory = 0;
        int DeskOtherCategory = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                auth = new SignInAuthorizer
                {
                    Credentials = new SessionStateCredentials
                    {
                        ConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"],
                        ConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"]
                    },
                    PerformRedirect =
                        twitterUrl => Response.Redirect(twitterUrl, false)
                };

                if (!auth.IsAuthorized)
                {
                    auth.BeginAuthorization(Request.Url);
                }
                auth.CompleteAuthorization(Request.Url);

                getUserDetails();

            }
        }
        private void getUserDetails()
        {
            if (auth.CompleteAuthorization(Request.Url) == true)
            {
                var twitterCtx = new LinqToTwitter.TwitterContext(auth);

                var user =
                (from tweet in twitterCtx.User
                 where tweet.Type == UserType.Show &&
                     tweet.ScreenName == auth.Credentials.ScreenName
                 select tweet).SingleOrDefault();

                username.InnerText = auth.Credentials.ScreenName;
                profilepic.Src = user.ProfileImageUrl;
            }
        }

        public void getSource()
        {
            List<ListItems> columnList = new List<ListItems>();
            if (auth.CompleteAuthorization(Request.Url) == true)
            {
                dict = new DictionaryDB();
                var twitterCtx = new LinqToTwitter.TwitterContext(auth);

                var tweets = from t in twitterCtx.Status
                             where t.Type == StatusType.User && t.ScreenName == DropDownList1.SelectedValue && t.Count == Int32.Parse(tweetnum.SelectedValue)
                             select t;

                foreach (var tweetStatus in tweets)
                {
                    var listItem = new ListItems();
                    List<UrlEntity> Urle = tweetStatus.Entities.UrlEntities;
                    foreach (var url in Urle)
                    {
                        if (!string.IsNullOrWhiteSpace(url.ExpandedUrl))
                            listItem.url = url.ExpandedUrl;
                    }
                    listItem.tweet = tweetStatus.Text;
                    listItem.date = tweetStatus.CreatedAt;
                    if (!string.IsNullOrEmpty(dict.getTweetCategory(tweetStatus.Text)))
                    {
                        listItem.category = dict.getTweetCategory(tweetStatus.Text);
                    }
                    else
                    {
                        listItem.category = "Others";
                    }
                    columnList.Add(listItem);
                }

                ListView1.DataSource = columnList.AsQueryable().ToArray();
                ListView1.DataBind();

                if (tweets != null)
                {
                    var dictionaryword = dict.getDictionaryWords();
                    foreach (var tweetStatus in tweets)
                    {
                        foreach (string word in dictionaryword)
                        {
                            if (tweetStatus.Text.Contains(word) == true)
                            {
                                string category = dict.getWordCategory(word);
                                break;
                            }
                        }
                    }



                    #region Pie Charts (Home, Network, Desktop, Mobile, Others)
                    foreach (var tweetStatus in tweets)
                    {
                        if (!string.IsNullOrEmpty(dict.getTweetCategory(tweetStatus.Text)))
                        {
                            if (dict.getTweetCategory(tweetStatus.Text) == "Network")
                            {
                                NetworkList.Add(tweetStatus);
                                networkCategory++;
                                #region Network Pie Chart
                                if (dict.getSecTweetCategory(tweetStatus.Text) == "Emails")
                                {
                                    emailCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Online Game")
                                {
                                    OnlineCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Social Media")
                                {
                                    SocialCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Server")
                                {
                                    ServerCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Website")
                                {
                                    WebsiteCategory++;
                                }

                                else
                                {
                                    netotherCategory++;
                                }
                                #endregion
                            }
                            else if (dict.getTweetCategory(tweetStatus.Text) == "Desktop")
                            {
                                desktopCategory++;
                                DesktopList.Add(tweetStatus);
                                #region Desktop Pie Chart
                                if (dict.getSecTweetCategory(tweetStatus.Text) == "Com Hardware")
                                {
                                    DHCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Com Software")
                                {
                                    DSCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Operating System")
                                {
                                    OperatingCategory++;
                                }
                                else
                                {
                                    DeskOtherCategory++;
                                }
                                #endregion
                            }
                            else if (dict.getTweetCategory(tweetStatus.Text) == "Mobile")
                            {
                                mobileCategory++;
                                MobileList.Add(tweetStatus);
                                #region Mobile Pie Chart
                                if (dict.getSecTweetCategory(tweetStatus.Text) == "Telecommunication")
                                {
                                    telecomCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Hardware")
                                {
                                    MHCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Software")
                                {
                                    MSCategory++;
                                }
                                else if (dict.getSecTweetCategory(tweetStatus.Text) == "Platform")
                                {
                                    PlatformCategory++;
                                }
                                else
                                {
                                    MobOtherCategory++;
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            otherCategory++;
                            OthersList.Add(tweetStatus);
                        }
                    }
                    #endregion

                    #region Home Categories (Segment)
                    if (networkCategory != 0)
                    {
                        DataPoint network = new DataPoint();
                        network.SetValueXY("Network", networkCategory);
                        Chart1.Series[0].Points.Add(network);
                        Chart1.Series["Series1"]["PieLabelStyle"] = "outside";
                        columnList.Clear();
                        foreach (var tweetStatus in NetworkList)
                        {
                            var listItem = new ListItems();
                            List<UrlEntity> Urle = tweetStatus.Entities.UrlEntities;
                            foreach (var url in Urle)
                            {
                                if (!string.IsNullOrWhiteSpace(url.ExpandedUrl))
                                    listItem.url = url.ExpandedUrl;
                            }
                            listItem.tweet = tweetStatus.Text;
                            listItem.date = tweetStatus.CreatedAt;
                            if (!string.IsNullOrEmpty(dict.getSecTweetCategory(tweetStatus.Text)))
                            {
                                listItem.category = dict.getSecTweetCategory(tweetStatus.Text);
                            }
                            else
                            {
                                listItem.category = "Others";
                            }
                            columnList.Add(listItem);
                        }
                        ListView2.DataSource = columnList.AsQueryable().ToArray();
                        ListView2.DataBind();
                    }
                    if (desktopCategory != 0)
                    {
                        DataPoint desktop = new DataPoint();
                        desktop.SetValueXY("Desktop", desktopCategory);
                        Chart1.Series[0].Points.Add(desktop);
                        Chart1.Series["Series1"]["PieLabelStyle"] = "outside";
                        columnList.Clear();
                        foreach (var tweetStatus in DesktopList)
                        {
                            var listItem = new ListItems();
                            List<UrlEntity> Urle = tweetStatus.Entities.UrlEntities;
                            foreach (var url in Urle)
                            {
                                if (!string.IsNullOrWhiteSpace(url.ExpandedUrl))
                                    listItem.url = url.ExpandedUrl;
                            }
                            listItem.tweet = tweetStatus.Text;
                            listItem.date = tweetStatus.CreatedAt;
                            if (!string.IsNullOrEmpty(dict.getSecTweetCategory(tweetStatus.Text)))
                            {
                                listItem.category = dict.getSecTweetCategory(tweetStatus.Text);
                            }
                            else
                            {
                                listItem.category = "Others";
                            }
                            columnList.Add(listItem);
                        }
                        ListView4.DataSource = columnList.AsQueryable().ToArray();
                        ListView4.DataBind();
                    }
                    if (mobileCategory != 0)
                    {
                        DataPoint mobile = new DataPoint();
                        mobile.SetValueXY("Mobile", mobileCategory);
                        Chart1.Series[0].Points.Add(mobile);
                        Chart1.Series["Series1"]["PieLabelStyle"] = "outside";
                        columnList.Clear();
                        foreach (var tweetStatus in MobileList)
                        {
                            var listItem = new ListItems();
                            List<UrlEntity> Urle = tweetStatus.Entities.UrlEntities;
                            foreach (var url in Urle)
                            {
                                if (!string.IsNullOrWhiteSpace(url.ExpandedUrl))
                                    listItem.url = url.ExpandedUrl;
                            }
                            listItem.tweet = tweetStatus.Text;
                            listItem.date = tweetStatus.CreatedAt;
                            if (!string.IsNullOrEmpty(dict.getSecTweetCategory(tweetStatus.Text)))
                            {
                                listItem.category = dict.getSecTweetCategory(tweetStatus.Text);
                            }
                            else
                            {
                                listItem.category = "Others";
                            }
                            columnList.Add(listItem);
                        }
                        ListView3.DataSource = columnList.AsQueryable().ToArray();
                        ListView3.DataBind();
                    }
                    if (otherCategory != 0)
                    {
                        DataPoint others = new DataPoint();
                        others.SetValueXY("Others", otherCategory);
                        Chart1.Series["Series1"]["PieLabelStyle"] = "outside";
                        Chart1.Series[0].Points.Add(others);
                        columnList.Clear();
                        foreach (var tweetStatus in OthersList)
                        {
                            var listItem = new ListItems();
                            List<UrlEntity> Urle = tweetStatus.Entities.UrlEntities;
                            foreach (var url in Urle)
                            {
                                if (!string.IsNullOrWhiteSpace(url.ExpandedUrl))
                                    listItem.url = url.ExpandedUrl;
                            }
                            listItem.tweet = tweetStatus.Text;
                            listItem.date = tweetStatus.CreatedAt;
                            if (!string.IsNullOrEmpty(dict.getSecTweetCategory(tweetStatus.Text)))
                            {
                                listItem.category = dict.getSecTweetCategory(tweetStatus.Text);
                            }
                            else
                            {
                                listItem.category = "Others";
                            }
                            columnList.Add(listItem);
                        }
                        ListView5.DataSource = columnList.AsQueryable().ToArray();
                        ListView5.DataBind();
                    }
                    #endregion

                    #region Network Categories (Segment)
                    if (emailCategory != 0)
                    {
                        DataPoint email = new DataPoint();
                        email.SetValueXY("Email", emailCategory);
                        Chart2.Series[0].Points.Add(email);
                        Chart2.Series["Series1"]["PieLabelStyle"] = "outside";
                    }
                    if (OnlineCategory != 0)
                    {
                        DataPoint onlinegame = new DataPoint();
                        onlinegame.SetValueXY("Online Game", OnlineCategory);
                        Chart2.Series[0].Points.Add(onlinegame);
                        Chart2.Series["Series1"]["PieLabelStyle"] = "outside";
                    }
                    if (SocialCategory != 0)
                    {
                        DataPoint social = new DataPoint();
                        social.SetValueXY("Social Media", SocialCategory);
                        Chart2.Series[0].Points.Add(social);
                        Chart2.Series["Series1"]["PieLabelStyle"] = "outside";
                    }
                    if (WebsiteCategory != 0)
                    {
                        DataPoint website = new DataPoint();
                        website.SetValueXY("Website", WebsiteCategory);
                        Chart2.Series[0].Points.Add(website);
                        Chart2.Series["Series1"]["PieLabelStyle"] = "outside";
                    }
                    if (ServerCategory != 0)
                    {
                        DataPoint server = new DataPoint();
                        server.SetValueXY("Server", ServerCategory);
                        Chart2.Series[0].Points.Add(server);
                        Chart2.Series["Series1"]["PieLabelStyle"] = "outside";
                    }
                    if (netotherCategory != 0)
                    {
                        DataPoint netother = new DataPoint();
                        netother.SetValueXY("Others", netotherCategory);
                        Chart2.Series[0].Points.Add(netother);
                        Chart2.Series["Series1"]["PieLabelStyle"] = "outside";
                    }
                    if (emailCategory == 0 && OnlineCategory == 0 && SocialCategory == 0 && WebsiteCategory == 0 && ServerCategory == 0 && netotherCategory == 0)
                    {
                        Chart2.Visible = false;
                    }
                    #endregion

                    #region Mobile Category (Segment)
                    if (telecomCategory != 0)
                    {
                        DataPoint telecom = new DataPoint();
                        telecom.SetValueXY("Telecom", telecomCategory);
                        Chart3.Series[0].Points.Add(telecom);
                        Chart3.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (MHCategory != 0)
                    {
                        DataPoint mobhardware = new DataPoint();
                        mobhardware.SetValueXY("Hardware", MHCategory);
                        Chart3.Series[0].Points.Add(mobhardware);
                        Chart3.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (MSCategory != 0)
                    {
                        DataPoint mobsoftware = new DataPoint();
                        mobsoftware.SetValueXY("Software", MSCategory);
                        Chart3.Series[0].Points.Add(mobsoftware);
                        Chart3.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (PlatformCategory != 0)
                    {
                        DataPoint platform = new DataPoint();
                        platform.SetValueXY("Platform", PlatformCategory);
                        Chart3.Series[0].Points.Add(platform);
                        Chart3.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (MobOtherCategory != 0)
                    {
                        DataPoint mobothers = new DataPoint();
                        mobothers.SetValueXY("Others", MobOtherCategory);
                        Chart3.Series[0].Points.Add(mobothers);
                        Chart3.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (telecomCategory == 0 && MHCategory == 0 && MSCategory == 0 && PlatformCategory == 0 && MobOtherCategory == 0)
                    {
                        Chart3.Visible = false;
                    }
                    #endregion

                    #region Desktop Categories (Segment)
                    if (DHCategory != 0)
                    {
                        DataPoint DeskHardware = new DataPoint();
                        DeskHardware.SetValueXY("Hardware", DHCategory);
                        Chart4.Series[0].Points.Add(DeskHardware);
                        Chart4.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (DSCategory != 0)
                    {
                        DataPoint DeskSoftware = new DataPoint();
                        DeskSoftware.SetValueXY("Software", DSCategory);
                        Chart4.Series[0].Points.Add(DeskSoftware);
                        Chart4.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (OperatingCategory != 0)
                    {
                        DataPoint OS = new DataPoint();
                        OS.SetValueXY("Operating System", OperatingCategory);
                        Chart4.Series[0].Points.Add(OS);
                        Chart4.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (DeskOtherCategory != 0)
                    {
                        DataPoint DeskOther = new DataPoint();
                        DeskOther.SetValueXY("Others", DeskOtherCategory);
                        Chart4.Series[0].Points.Add(DeskOther);
                        Chart4.Series["Series1"]["PieLabelStyle"] = "outside";
                    }

                    if (DHCategory == 0 && DSCategory == 0 && OperatingCategory == 0 && DeskOtherCategory == 0)
                    {
                        Chart4.Visible = false;
                    }
                    #endregion
                }

            }
            activate = "activated";

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activate = "deactivated";
            ListView1.Items.Clear();
            getSource();
        }

        public void displayMessage(string strMsg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "alert('" + strMsg + "');", true);
        }

        public void DataPager1_PreRender(object sender, EventArgs e)
        {
            if (activate != "activated")
            {
                getSource();
            }
        }
    }
}