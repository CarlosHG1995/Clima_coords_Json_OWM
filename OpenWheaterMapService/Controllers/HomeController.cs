using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OpenWheaterMapService.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async Task <ActionResult> Index(string lat, string lon)
        {
            double latitud = double.Parse(lat.Substring(0,5));
            double longitud = double.Parse(lon.Substring(0,5));
        

            var elTiempo = await Models.OpenWheaterMapProxy.RecuperaTiempo(latitud, longitud);

            ViewBag.Name = elTiempo.Name;
            ViewBag.Temp = elTiempo.Main.Temp.ToString() + "°C";
            ViewBag.Description = elTiempo.Weather[0].Description;
           // ViewBag.Icono = "Assests/Weather/" + elTiempo.Weather[0].Icon+".png";


            return View();
        }
    }
}