using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class TecnicoAreaController : Controller
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
            cadena += "<th>Tecnico</th>";
            cadena += "<th>Tipo de Servicio</th>";
            cadena += "<th>Fecha</th>";
            cadena += "<th>Especialidad</th>";
            cadena += "<th>Nivel</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.tecnico_area.ToList())
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.tecnico1.persona1.nombres + "</td>";
                cadena += "<td>" + obj.tipo_servicio1.nombre + "</td>";
                cadena += "<td>" + obj.fecha.ToShortDateString() + "</td>";
                cadena += "<td>" + obj.especialidad + "</td>";
                cadena += "<td>" + obj.nivel + "</td>";
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
                cadena += "<a class='waves-effect waves-light btn btn-floating red'><i class='icon-trash' onclick='ModalConfirmar(" + obj.id + ",\"" + obj.id+  "\");'></i></a>";
                cadena += "</td>";
                cadena += "</tr>";
            }
            cadena += "</tbody>";
            cadena += "</table>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(int id, int tecnico, int tipo_servicio, string fecha,string especialidad,string nivel, bool estado)
        {
            tecnico_area obj;
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
                    obj = new tecnico_area();
                    obj.tecnico = tecnico;
                    obj.tipo_servicio = tipo_servicio;
                    obj.fecha = DateTime.Parse(fecha).Date;
                    obj.especialidad = especialidad;
                    obj.nivel = nivel;
                    obj.estado = estado;
                    BD.tecnico_area.Add(obj);
                    BD.SaveChanges();
                }
                else
                {
                    obj = BD.tecnico_area.Single(o => o.id == id);
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
            tecnico_area obj = BD.tecnico_area.Single(o => o.id == id);
            var tecnico_area = new
            {
                tecnico = obj.tecnico,
                tipo_servicio = obj.tipo_servicio,
                fecha = obj.fecha.ToShortDateString(),
                especialidad = obj.especialidad,
                nivel = obj.nivel,
                estado = obj.estado
            };
            return Json(tecnico_area, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                tecnico_area t = BD.tecnico_area.Single(o => o.id == id);
                BD.tecnico_area.Remove(t);
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