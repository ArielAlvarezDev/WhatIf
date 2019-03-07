
using System.Web.Mvc;
using System.Web.Routing;

namespace WhatIf
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // In order to avoid the question marks and to resemble webApi-like requests
            // we need to add a new route for the comic action method.

            routes.MapRoute(
                name: "HomeComic",
                url: "Comic/{IdComic}",
                defaults: new { controller = "Home", action = "Comic", IdComic=UrlParameter.Optional}
            );
           
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
