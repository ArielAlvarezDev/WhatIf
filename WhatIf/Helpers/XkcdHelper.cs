using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using WhatIf.Models;
using WhatIf.ViewModels;

namespace WhatIf.Helpers
{
    public class XkcdHelper
    {
        private JavaScriptSerializer _jsS;
        readonly string _urlHome = $"https://xkcd.com/info.0.json";
        HttpClient _client;


        public XkcdComicVM ComicObtener()
        {
            _jsS = new JavaScriptSerializer();
            var comic = new XkcdComic();
            var jsonresponse = string.Empty;

            using (_client = new HttpClient())

            {
                var httpResp = _client.GetAsync(_urlHome).Result;
                if (httpResp.IsSuccessStatusCode)
                    comic = _jsS.Deserialize<XkcdComic>(httpResp.Content.ReadAsStringAsync().Result);
            }

            SetLastComicId(comic.Num);

            var viewModel = new XkcdComicVM()
            {
                Comic = comic,
                IdComic = comic.Num,
                NextId = VerifyNext(comic.Num, "next"),
                PrevId = VerifyNext(comic.Num, "previous")
            };

            return viewModel;
        }

        public XkcdComicVM ComicObtenerPorId(int idComic)
        {           

            _jsS = new JavaScriptSerializer();
            var url = $"https://xkcd.com/" + idComic + "/info.0.json";
            var comic = new XkcdComic();
            var jsonresponse = string.Empty;

            using (_client = new HttpClient())
            {
                var httpResp = _client.GetAsync(url).Result;
                if (httpResp.IsSuccessStatusCode)
                    comic = _jsS.Deserialize<XkcdComic>(httpResp.Content.ReadAsStringAsync().Result);
                else
                {
                    // I'm creating an empty object so it will be redirected to the home controller if you
                    // type Comic/404, comic/0, or any other invalid number directly into de address bar, since we know not
                    // which direction you're moving towards.

                    var comicDelDia = this.ComicObtener();
                    return comicDelDia;
                }

            }

            var viewModel = new XkcdComicVM()
            {
                Comic = comic,
                IdComic = comic.Num,
                NextId = VerifyNext(comic.Num, "next"),
                PrevId = VerifyNext(comic.Num, "previous")
            };
            return viewModel;

        }

        public XkcdComicVM MyFavourite()
        {
            //Reminds me of Éricka =')
            _jsS = new JavaScriptSerializer();
            var comic = new XkcdComic();
            var url = $"https://xkcd.com/162/info.0.json";

            var jsonResponse = string.Empty;

            using (_client = new HttpClient())
            {
                jsonResponse = _client.GetStringAsync(url).Result;
                comic = _jsS.Deserialize<XkcdComic>(jsonResponse);
            }

            var viewModel = new XkcdComicVM()
            {
                Comic = comic,
                IdComic = comic.Num,
                NextId = VerifyNext(comic.Num, "next"),
                PrevId = VerifyNext(comic.Num, "previous")
            };
            return viewModel;

        }
        /// <summary>
        /// Method to verify if there is a previous or following comic
        /// </summary>
        /// <param name="currentId">The id of the current Comic</param>
        /// <param name="direction"> The direction of the clicked arrow: Next or Previous</param>
        /// <returns></returns>
        public int VerifyNext(int currentId, string direction)
        {
            var url = string.Empty;
            var exists = false;
            var comic = new XkcdComic();
            var returnId = -1;
            var httpResponse = new HttpResponseMessage();
            //we have to actually jump twice, if we dont get a successfull response on the second jump, then
            // it means we've reached the last comic at either direction.
            // this is coded for the 404 use case.

            for (int i = 0; i < 2; i++)
            {
                if (direction.Equals("next", System.StringComparison.InvariantCultureIgnoreCase))
                    currentId++;
                else
                    currentId--;

                using (_client = new HttpClient())
                {
                    url = $"https://xkcd.com/" + currentId + "/info.0.json";
                    httpResponse = _client.GetAsync(url).Result;
                    exists = httpResponse.IsSuccessStatusCode;
                    
                }
                // if it does exist, then we break the for loop. The next/previous button will show on the view.
                if (exists)
                { returnId = _jsS.Deserialize<XkcdComic>(httpResponse.Content.ReadAsStringAsync().Result).Num;
                    break;
                }
                    
            }
            return returnId;
        }

        /// <summary>
        /// Set the last comic id in a session variable
        /// </summary>
        /// <param name="id"></param>
        public void SetLastComicId(int id)
        {
            HttpContext.Current.Session.Add("LastId", id);
        }

        public static int GetLastComicId()
        {
            int lastId = 0;
            try
            {
                lastId = (int)HttpContext.Current.Session["LastId"];
            }
            catch (System.Exception)
            {

                lastId=0;
            }
       
            
            return lastId;
        }




    }
}