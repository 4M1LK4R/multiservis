using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class ReservaController : Controller
    {
        multiservisEntities BD = new multiservisEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Reserva()
        {
            return View();
        }
        public ActionResult ListarDetalleServicio()
        {
            string cadena = "";
            cadena += "<ul class='collapsible' data-collapsible='expandable'>";
            foreach (var servicio in BD.servicio.ToList().Where(o => o.estado))
            {
                cadena += "<li>";
                cadena += "<div class='collapsible-header'><i class='icon-briefcase'></i>"+servicio.nombre+"</div>";
                cadena += "<div class='collapsible-body grey lighten-3'>";
                foreach (var detalle_servicio in BD.detalle_servicio.ToList().Where(o => o.estado && o.servicio == servicio.id))
                {
                    cadena += "<p>";
                    cadena += "<input type='checkbox' id='"+detalle_servicio.id+ "' onclick='Agregar(" + detalle_servicio.id + ",\"" + detalle_servicio.precio + "\");'/>";
                    cadena += "<label for='" + detalle_servicio.id + "'>" + detalle_servicio.nombre + "</label>";
                    cadena += "<span>&nbsp;&nbsp;<b>Costo:</b>&nbsp;" + detalle_servicio.precio;
                    //cadena += "<b>&nbsp;&nbsp;Duración Hrs:</b>&nbsp;" + detalle_servicio.tiempo;
                    cadena += "</span>";
                    cadena += "</p>";
                    cadena += "<hr/>";
                }
                cadena += "</div>";
                cadena += "</li>";
            }
            cadena += "</ul>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
    }
}