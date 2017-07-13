using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class CuentaController : Controller
    {
        // GET: Cuenta
        public ActionResult Index()
        {
            return View();
        }
        multiservisEntities BD = new multiservisEntities();
        public ActionResult Login(string nik, string pass)
        {
            usuario obj = BD.usuario.SingleOrDefault(o => o.nombre_usuario == nik & o.pasword_usuario == pass);
            if (obj != null)
            {
                FormsAuthentication.SetAuthCookie(obj.nombre_usuario, false);
                foreach (var usuario in obj.asignar_rol_usuario)
                {
                    switch (usuario.rol)
                    {
                        case 1:
                            return RedirectToAction("Index", "Area");
                            break;
                        case 2:
                            return RedirectToAction("Index", "Reserva");
                            break;
                        default:
                            return RedirectToAction("Index", "Cuenta");
                            break;
                    }
                }
                return RedirectToAction("Index", "Cuenta");
            }
            else
            {
                return RedirectToAction("Index", "Cuenta");
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Cuenta");
        }
        public ActionResult CrearCuenta( string nombres, string parterno, string materno, string correo, string nacionalidad, string ci, string telefono, string direccion, string nombre_usuario, string password_usuario)
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
            if (BD.usuario.ToList().Exists(o => o.nombre_usuario == nombre_usuario))
                error = "Ya existe un usuario con ese nombre!";

            if (string.IsNullOrEmpty(nombres))
                error = "El campo nombre esta vacio";

            if (BD.usuario.ToList().Exists(o => o.nombre_usuario == correo))
                error = "Ya existe una cuenta asiciada a ese correo!";


            if (string.IsNullOrEmpty(error))
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
                obj_u.estado = true;
                BD.usuario.Add(obj_u);
                BD.SaveChanges();

                asignar_rol_usuario a_r_u;
                a_r_u = new asignar_rol_usuario();
                a_r_u.usuario = obj_u.id;
                a_r_u.rol = 2;
                a_r_u.fecha_asigna = DateTime.Now.Date;
                a_r_u.estado = true;
                BD.asignar_rol_usuario.Add(a_r_u);
                BD.SaveChanges();


            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }

    }
}