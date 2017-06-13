using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class AdmReservaController : Controller
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
            cadena += "<th>COD Reserva</th>";
            cadena += "<th>Detalle de Servicio</th>";
            cadena += "<th>Tecnico</th>";
            cadena += "<th>Usuario</th>";
            cadena += "<th>Precio</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.detalle_reserva.ToList())
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.reserva+ "</td>";
                cadena += "<td>" + obj.detalle_servicio1.nombre + "</td>";
                cadena += "<td>" + obj.tecnico1.persona1.nombres + "</td>";
                cadena += "<td>" + obj.reserva1.persona1.nombres + "</td>";
                cadena += "<td>" + obj.precio+ "</td>";
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
                cadena += "<a class='waves-effect waves-light btn btn-floating red'><i class='icon-trash' onclick='ModalConfirmar(" + obj.id + ",\"" + obj.id + "\");'></i></a>";
                cadena += "</td>";
                cadena += "</tr>";
            }
            cadena += "</tbody>";
            cadena += "</table>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(int id, int tecnico, int tipo_servicio, string fecha, string especialidad, string nivel, bool estado)
        {
            reserva obj;
            string error = "";
            try
            {
                DateTime d = DateTime.Parse(fecha).Date;
            }
            catch
            {
                error = "Debe seleccionar una fecha valida!";
            }
            if (string.IsNullOrEmpty(error))
            {
                if (id == 0)
                {
                    obj = new reserva();
                    obj.tecnico = tecnico;
                    obj.tipo_servicio = tipo_servicio;
                    obj.fecha = DateTime.Parse(fecha).Date;
                    obj.especialidad = especialidad;
                    obj.nivel = nivel;
                    obj.estado = estado;
                    BD.reserva.Add(obj);
                    BD.SaveChanges();
                }
                else
                {
                    obj = BD.reserva.Single(o => o.id == id);
                    obj.tecnico = tecnico;
                    obj.tipo_servicio = tipo_servicio;
                    obj.fecha = DateTime.Parse(fecha).Date;
                    obj.especialidad = especialidad;
                    obj.nivel = nivel;
                    obj.estado = estado;
                    BD.SaveChanges();
                }
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            reserva obj = BD.reserva.Single(o => o.id == id);
            var reserva = new
            {
                tecnico = obj.tecnico,
                tipo_servicio = obj.tipo_servicio,
                fecha = obj.fecha.ToShortDateString(),
                especialidad = obj.especialidad,
                nivel = obj.nivel,
                estado = obj.estado
            };
            return Json(reserva, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                reserva t = BD.reserva.Single(o => o.id == id);
                BD.reserva.Remove(t);
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
