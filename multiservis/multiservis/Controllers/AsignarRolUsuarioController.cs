using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class AsignarRolUsuarioController : Controller
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
            cadena += "<th>Nombre Usuario</th>";
            cadena += "<th>Rol</th>";
            cadena += "<th>Fecha Asignado</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.asignar_rol_usuario.ToList())
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.usuario1.nombre_usuario + "</td>";
                cadena += "<td>" + obj.rol1.nombre + "</td>";
                cadena += "<td>" + obj.fecha_asigna.ToShortDateString() + "</td>";
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
        public ActionResult Guardar(int id, int usuario, int rol, string fecha, bool estado)
        {
            asignar_rol_usuario obj;
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
                    obj = new asignar_rol_usuario();
                    obj.usuario = usuario;
                    obj.rol = rol;
                    obj.fecha_asigna = DateTime.Parse(fecha).Date;
                    obj.estado = estado;
                    BD.asignar_rol_usuario.Add(obj);
                    BD.SaveChanges();
                }
                else
                {
                    obj = BD.asignar_rol_usuario.Single(o => o.id == id);
                    obj.usuario = usuario;
                    obj.rol = rol;
                    obj.fecha_asigna = DateTime.Parse(fecha).Date;
                    obj.estado = estado;
                    BD.SaveChanges();
                }
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            asignar_rol_usuario obj = BD.asignar_rol_usuario.Single(o => o.id == id);
            var asignar_rol_usuario = new
            {
                usuario = obj.usuario,
                rol = obj.rol,
                fecha = obj.fecha_asigna.ToShortDateString(),
                estado = obj.estado
            };
            return Json(asignar_rol_usuario, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                asignar_rol_usuario obj = BD.asignar_rol_usuario.Single(o => o.id == id);
                BD.asignar_rol_usuario.Remove(obj);
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