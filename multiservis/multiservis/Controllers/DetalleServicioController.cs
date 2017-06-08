using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class DetalleServicioController : Controller
    {
        multiservisEntities BD = new multiservisEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            string cadena = "";
            cadena = "<table id='data' class='display highlight' cellspacing='0' hidden>";
            cadena += "<thead class='red darken-3 white-text z-depth-3'>";
            cadena += "<tr>";
            cadena += "<th>Servicio</th>";
            cadena += "<th>Nombre Detalle</th>";
            cadena += "<th>Precio</th>";
            cadena += "<th>Tiempo (Hrs.)</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.detalle_servicio.ToList())
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.servicio1.nombre + "</td>";
                cadena += "<td>" + obj.nombre + "</td>";
                cadena += "<td>" + obj.precio + "</td>";
                cadena += "<td>" + obj.tiempo + "</td>";
                if (obj.estado)
                {
                    cadena += "<td>Activo</td>";
                }
                else
                {
                    cadena += "<td>Inactivo</td>";
                }
                cadena += "<td>";
                cadena += "<a class='waves-effect waves-light btn btn-floating blue'><i class='icon-pencil-1' onclick='Editar(" + obj.id + ");'></i></a>&nbsp;";
                cadena += "<a class='waves-effect waves-light btn btn-floating red'><i class='icon-trash' onclick='ModalConfirmar(" + obj.id + ",\"" + obj.nombre + "\");'></i></a>";
                cadena += "</td>";
                cadena += "</tr>";
            }
            cadena += "</tbody>";
            cadena += "</table>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(int id, int servicio, string nombre, string precio, string tiempo, bool estado)
        {
            detalle_servicio obj;
            string error = "";

            if (string.IsNullOrEmpty(error))
            {
                if (id == 0)
                {
                    obj = new detalle_servicio();
                    obj.servicio = servicio;
                    obj.nombre = nombre;
                    obj.precio = Convert.ToDecimal(precio);
                    obj.tiempo = tiempo;
                    obj.estado = estado;
                    BD.detalle_servicio.Add(obj);
                    BD.SaveChanges();
                }
                else
                {
                    obj = BD.detalle_servicio.Single(o => o.id == id);
                    obj.servicio = servicio;
                    obj.nombre = nombre;
                    obj.precio = Convert.ToDecimal(precio);
                    obj.tiempo = tiempo;
                    obj.estado = estado;
                    BD.SaveChanges();
                }
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            detalle_servicio obj = BD.detalle_servicio.Single(o => o.id == id);
            var detalle_servicio = new
            {
                servicio = obj.servicio,
                nombre = obj.nombre,
                precio = obj.precio,
                tiempo = obj.tiempo,
                estado = obj.estado
            };
            return Json(detalle_servicio, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                detalle_servicio obj = BD.detalle_servicio.Single(o => o.id == id);
                BD.detalle_servicio.Remove(obj);
                BD.SaveChanges();
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
