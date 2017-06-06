using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class UnidadMaterialController : Controller
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
            cadena += "<th>material</th>";
            cadena += "<th>Fecha de Ingreso</th>";
            cadena += "<th>Precio de Compra</th>";
            cadena += "<th>Precio de Venta</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.unidad_material.ToList())
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.material1.nombre + "</td>";
                cadena += "<td>" + obj.fecha_ingreso.Date.ToShortDateString() + "</td>";
                cadena += "<td>" + obj.precio_compra + "</td>";
                cadena += "<td>" + obj.precio_venta + "</td>";
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
                cadena += "<a class='waves-effect waves-light btn btn-floating red'><i class='icon-trash' onclick='ModalConfirmar(" + obj.id + ",\"" + obj.material1.nombre + "\");'></i></a>";
                cadena += "</td>";
                cadena += "</tr>";
            }
            cadena += "</tbody>";
            cadena += "</table>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(int id, int material, string fecha_ingreso, string precio_compra, string precio_venta, bool estado)
        {
            unidad_material obj;
            string error = "";
            try
            {
                DateTime d = DateTime.Parse(fecha_ingreso).Date;
            }
            catch
            {
                error = "Debe seleccionar una fecha valida!";
            }

            if (string.IsNullOrEmpty(error))
            {
                if (id == 0)
                {
                    obj = new unidad_material();
                    obj.material = material;
                    obj.fecha_ingreso = DateTime.Parse(fecha_ingreso).Date;
                    obj.precio_compra = Convert.ToDecimal(precio_compra);
                    obj.precio_venta = Convert.ToDecimal(precio_venta);
                    obj.estado = estado;
                    BD.unidad_material.Add(obj);
                    BD.SaveChanges();
                }
                else
                {
                    obj = BD.unidad_material.Single(o => o.id == id);
                    obj.material = material;
                    obj.fecha_ingreso = DateTime.Parse(fecha_ingreso).Date;
                    obj.precio_compra = decimal.Parse(precio_compra);
                    obj.precio_venta = decimal.Parse(precio_venta);
                    obj.estado = estado;
                    BD.SaveChanges();
                }
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            unidad_material obj = BD.unidad_material.Single(o => o.id == id);
            var unidad_material = new
            {
                material = obj.material,
                fecha_ingreso = obj.fecha_ingreso.ToShortDateString(),
                precio_compra = obj.precio_compra,
                precio_venta = obj.precio_venta,
                estado = obj.estado
            };
            return Json(unidad_material, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                unidad_material obj = BD.unidad_material.Single(o => o.id == id);
                BD.unidad_material.Remove(obj);
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
