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
            cadena += "<th>Descripcion</th>";
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
                cadena += "<td>" + obj.reserva + "</td>";
                cadena += "<td>" + obj.detalle_servicio1.nombre + "</td>";
                cadena += "<td>" + obj.descripcion + "</td>";

                if (obj.tecnico1 == null)
                {
                    cadena += "<td class='red-text'>Pendiente</td>";
                }
                else
                {
                    cadena += "<td>" + obj.tecnico1.persona1.nombres + "</td>";
                }

                if (obj.reserva1.persona1 == null)
                {
                    cadena += "<td class='red-text'>Pendiente</td>";
                }
                else
                {
                    cadena += "<td>" + obj.reserva1.persona1.nombres + " " + obj.reserva1.persona1.paterno + " " + obj.reserva1.persona1.materno + "</td>";
                }


                cadena += "<td>" + obj.precio + "</td>";
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
        public ActionResult Guardar(int id, int tecnico, bool estado)
        {
            detalle_reserva obj;
            string error = "";
            if (string.IsNullOrEmpty(error))
            {
                obj = BD.detalle_reserva.Single(o => o.id == id);
                obj.tecnico = tecnico;
                obj.estado = estado;
                BD.SaveChanges();
            }
            return Json(error, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            detalle_reserva obj = BD.detalle_reserva.Single(o => o.id == id);


            if (obj.tecnico1 == null)
            {
                var detalle_reserva = new
                {
                    reserva = obj.reserva,
                    detalle_servicio = obj.detalle_servicio1.nombre,
                    tecnico_id = "",
                    tecnico = "",
                    usuario = "",
                    precio = obj.precio,
                    descripcion = obj.descripcion,
                    estado = obj.estado
                };
                return Json(detalle_reserva, JsonRequestBehavior.AllowGet);
            }

            //Impletar cuando este el login
            //if (obj.tecnico1 != null && obj.reserva1.persona1 != null)
            //{
            //    var detalle_reserva = new
            //    {
            //        reserva = obj.reserva,
            //        detalle_servicio = obj.detalle_servicio1.nombre,
            //        tecnico_id = obj.tecnico,
            //        tecnico = obj.tecnico1.persona1.nombres + " " + obj.tecnico1.persona1.paterno + " " + obj.tecnico1.persona1.materno,
            //        usuario = obj.reserva1.persona1.nombres + " " + obj.reserva1.persona1.paterno + " " + obj.reserva1.persona1.materno,
            //        precio = obj.precio,
            //        descripcion = obj.descripcion,
            //        estado = obj.estado
            //    };
            //    return Json(detalle_reserva, JsonRequestBehavior.AllowGet);
            //}

            if (obj.tecnico1!=null)
            {
                var detalle_reserva = new
                {
                    reserva = obj.reserva,
                    detalle_servicio = obj.detalle_servicio1.nombre,
                    tecnico_id = obj.tecnico,
                    tecnico = obj.tecnico1.persona1.nombres + " " + obj.tecnico1.persona1.paterno + " " + obj.tecnico1.persona1.materno,
                    usuario = "",
                    precio = obj.precio,
                    descripcion = obj.descripcion,
                    estado = obj.estado
                };
                return Json(detalle_reserva, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);


        }
        public ActionResult Delete(int id)
        {
            try
            {
                detalle_reserva d_r = BD.detalle_reserva.Single(o => o.id == id);
                BD.detalle_reserva.Remove(d_r);
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
