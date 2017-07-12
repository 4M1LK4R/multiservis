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
                return RedirectToAction("Index", "Area");
            }
            else
            {
                return RedirectToAction("Index", "Cuenta");
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult CrearCuenta(string nombre, string apellido, string correo, string pass)
        {
            usuario obj;
            persona obj_p;
            string error = "";
            if (string.IsNullOrEmpty(nombre))
                error = "El campo nombre esta vacio";

            if (BD.usuario.ToList().Exists(o => o.nombre_usuario == correo))
                error = "Ya existe una cuenta asiciada a ese correo!";


            if (string.IsNullOrEmpty(error))
            {
                obj_p = new persona();
                //obj_p.nombre = nombre;
                //obj_p.apellido = apellido;
                //obj_p.estado = true;
                //BD.persona.Add(obj_p);

                //obj = new usuario();
                //obj.correo = correo;
                //obj.username = correo;
                //obj.pass = pass;
                //obj.rol = 2;
                //obj.estado = true;
                //BD.usuario.Add(obj);
                BD.SaveChanges();
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
    }
}