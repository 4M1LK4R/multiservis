using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class UsuarioController : Controller
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
            cadena += "<th>Nombre de Usuario</th>";
            cadena += "<th>Nombres</th>";
            cadena += "<th>Paterno</th>";
            cadena += "<th>Materno</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.usuario.ToList())
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.nombre_usuario + "</td>";
                cadena += "<td>" + obj.persona1.nombres+ "</td>";
                cadena += "<td>" + obj.persona1.paterno + "</td>";
                cadena += "<td>" + obj.persona1.materno + "</td>";
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
                cadena += "<a class='waves-effect waves-light btn btn-floating red'><i class='icon-trash' onclick='ModalConfirmar(" + obj.id + ",\"" + obj.persona1.nombres + "\");'></i></a>";
                cadena += "</td>";
                cadena += "</tr>";
            }
            cadena += "</tbody>";
            cadena += "</table>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(int id, string nombres, string parterno, string materno, string correo, string nacionalidad, string ci, string telefono, string direccion, string nombre_usuario, string password_usuario, bool estado)
        {
            persona obj_p;
            usuario obj_u;
            string error = "";
            if (string.IsNullOrEmpty(nombres))
                error = "El campo nombres esta vacio";
            if (string.IsNullOrEmpty(nacionalidad))
                error = "El campo nacionalidad esta vacio";
            if (string.IsNullOrEmpty(nombre_usuario))
                error = "El campo nombre de usuario esta vacio";
            if (string.IsNullOrEmpty(password_usuario))
                error = "El campo paswword esta vacio";
            if (BD.usuario.ToList().Exists(o => o.nombre_usuario == nombre_usuario)&&id==0)
                error = "Ya existe un usuario con ese nombre!";

            if (string.IsNullOrEmpty(error))
            {
                if (id == 0)
                {
                    obj_p = new persona();
                    obj_p.nombres = nombres;
                    obj_p.paterno = parterno;
                    obj_p.materno = materno;
                    obj_p.correo = correo;
                    obj_p.nacionalidad = nacionalidad;
                    obj_p.ci = int.Parse(ci);
                    obj_p.telefono = telefono;
                    obj_p.direccion = direccion;
                    BD.persona.Add(obj_p);
                    BD.SaveChanges();

                    obj_u = new usuario();
                    obj_u.persona = obj_p.id;
                    obj_u.nombre_usuario = nombre_usuario;
                    obj_u.pasword_usuario = password_usuario;
                    obj_u.estado = estado;
                    BD.usuario.Add(obj_u);
                    BD.SaveChanges();
                }
                else
                {
                    obj_u = BD.usuario.Single(o => o.id == id);
                    obj_u.nombre_usuario = nombre_usuario;
                    obj_u.pasword_usuario = password_usuario;
                    obj_u.estado = estado;

                    obj_p = BD.persona.Single(o => o.id == obj_u.persona);
                    obj_p.nombres = nombres;
                    obj_p.paterno = parterno;
                    obj_p.materno = materno;
                    obj_p.correo = correo;
                    obj_p.nacionalidad = nacionalidad;
                    obj_p.ci = int.Parse(ci);
                    obj_p.telefono = telefono;
                    obj_p.direccion = direccion;

                    BD.SaveChanges();
                }
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            usuario obj = BD.usuario.Single(o => o.id == id);
            var usuario = new
            {
                nombres = obj.persona1.nombres,
                paterno = obj.persona1.paterno,
                materno = obj.persona1.materno,
                correo = obj.persona1.correo,
                nacionalidad = obj.persona1.nacionalidad,
                ci = obj.persona1.ci,
                telefono = obj.persona1.telefono,
                direccion = obj.persona1.direccion,

                id_persona = obj.persona,
                nombre_usuario = obj.nombre_usuario,
                estado = obj.estado
            };
            return Json(usuario, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                usuario u = BD.usuario.Single(o => o.id == id);
                BD.usuario.Remove(u);
                BD.SaveChanges();
                persona p = BD.persona.Single(o => o.id == id);
                BD.persona.Remove(p);
                BD.SaveChanges();
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListarSelectUsuarios()
        {
            string cadena = "<select id='selectUsuario'>";
            cadena += "<option value='' disabled selected>(Seleccionar)</option>";
            foreach (var item in BD.usuario.ToList())
            {
                cadena += "<option value=" + item.id + ">" + item.nombre_usuario+ "</option>";
            }
            cadena += "</select>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
    }
}