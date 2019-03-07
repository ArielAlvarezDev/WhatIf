using System.Web.Mvc;
using WhatIf.Helpers;

namespace WhatIf.Controllers
{
    public class ComicController : Controller
    {
        public ActionResult Index()
        {

            return RedirectToAction("Index","Home");
        }


        public ActionResult Comic(object IdComic)
        {
            int.TryParse(IdComic.ToString(), out int idComic);
            try
            {
                if (idComic == 0)
                    idComic = XkcdHelper.GetLastComicId();
            }
            catch (System.Exception)
            {

                return RedirectToAction("Index","Home");
            }
            

            var helper = new XkcdHelper();
            var comic = helper.ComicObtenerPorId(idComic);
            return View("Index",comic);
        }
        
        
    }
        
}