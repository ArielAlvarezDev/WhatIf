
using System.Web.Mvc;
using WhatIf.Helpers;

namespace WhatIf.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var helper = new XkcdHelper();
            var comic = helper.ComicObtener();


            return View(comic);
        }        

        [Route("Comic/IdComic")]
        public ActionResult Comic(object IdComic)
        {

            // In the event IdComic is null, or not valid, It should redirect to home page (I guess).
            int.TryParse(IdComic.ToString(), out int idComic);
            try
            {
                if (idComic == 0)
                    idComic = XkcdHelper.GetLastComicId();
            }
            catch (System.Exception)
            {

                return RedirectToAction("Index", "Home");
            }      
            var helper = new XkcdHelper();
            var comic = helper.ComicObtenerPorId(idComic);



            return View("Index", comic);
        }

       
    }
}