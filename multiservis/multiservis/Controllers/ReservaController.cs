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
                cadena += "<div class='collapsible-header'><i class='icon-briefcase'></i>" + servicio.nombre + "</div>";
                cadena += "<div class='collapsible-body grey lighten-3'>";
                foreach (var detalle_servicio in BD.detalle_servicio.ToList().Where(o => o.estado && o.servicio == servicio.id))
                {
                    cadena += "";
                    cadena += "<p><input type='checkbox' id='" + detalle_servicio.id + "' onclick='Agregar(" + detalle_servicio.id + ",\"" + detalle_servicio.precio + "\");'/>";
                    cadena += "<label for='" + detalle_servicio.id + "'>" + detalle_servicio.nombre + "</label></p>";
                    cadena += "<p><span>&nbsp;&nbsp;<b>Costo:</b>&nbsp;" + detalle_servicio.precio + "</span></p>";
                    cadena += "<p><input data-length='10' placeholder='Descripcion del Problema' class='white' id='des_' type='text'></p>";                    
                    //cadena += "<b>&nbsp;&nbsp;Duración Hrs:</b>&nbsp;" + detalle_servicio.tiempo;
                    cadena += "<hr/>";
                }
                cadena += "</div>";
                cadena += "</li>";
            }
            cadena += "</ul>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(int id,string persona,string usuario,string monto_total,string detalles,bool estado)
        {
            reserva obj;
            string msg = "";
            if (string.IsNullOrEmpty(msg))
            {
                if (id == 0)
                {
                    obj = new reserva();

                    //obj.persona = int.Parse(persona);
                    //obj.usuario = id_usu(usuario);

                    obj.persona = null;
                    obj.usuario = null;

                    obj.monto_total = Convert.ToDecimal(monto_total);
                    obj.estado = estado;
                    BD.reserva.Add(obj);
                    BD.SaveChanges();
                    RegistrarDetalleReservaTema(obj, detalles);
                }
                else
                {
                    obj = BD.reserva.Single(o => o.id == id);
                    
                    //obj.persona = int.Parse(persona);
                    //obj.usuario = id_usu(usuario);

                    obj.persona = null;
                    obj.usuario = null;

                    obj.monto_total = Convert.ToDecimal(monto_total);
                    obj.estado = estado;
                    foreach (var item in BD.detalle_reserva.Where(o => o.reserva == id))
                    {
                        BD.detalle_reserva.Remove(item);
                    }
                    RegistrarDetalleReservaTema(obj, detalles);
                }
            }
            

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        void RegistrarDetalleReservaTema(reserva reserva, string detalles)
        {
            detalle_reserva obj;
            string[] split = detalles.Split(new Char[] { ',' });
            for (int i = 0; i < split.Length; i++)
            {
                obj = new detalle_reserva();
                obj.reserva = reserva.id;
                obj.detalle_servicio = int.Parse(split[i]);
                obj.precio = ((detalle_servicio)BD.detalle_servicio.Single(o => o.id == obj.detalle_servicio)).precio;
                obj.estado = true;
                BD.detalle_reserva.Add(obj);
                BD.SaveChanges();
            }
        }

        private int id_usu(string usuario)
        {
            int id = 0;
            if (usuario == "admin@admin")
            {
                id = 1;
            }
            else
            {
                id = BD.usuario.Single(o => o.nombre_usuario == usuario).id;
            }
            return id;
        }

    }
}