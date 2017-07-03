using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;

namespace multiservis.Controllers
{
    public class FichaTecnicaController : Controller
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
            cadena += "<th>COD-Reserva</th>";
            cadena += "<th>COD-Detalle Reserva</th>";
            cadena += "<th>Detalle de Servicio</th>";            
            cadena += "<th>Ficha Técnica</th>";
            cadena += "<th>Unidades de Materiales</th>";
            cadena += "<th>Unidades de Herramientas</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.detalle_reserva.ToList().Where(o=>o.estado))
            {
                cadena += "<tr>";
                cadena += "<td>" + obj.reserva + "</td>";
                cadena += "<td>" + obj.id + "</td>";
                cadena += "<td>" + obj.detalle_servicio1.nombre + "</td>";

                if (obj.ficha_tecnica.Count == 0)
                {
                    cadena += "<td class='red-text'>Sin Asignar</td>";
                }
                else
                {
                    cadena += "<td class='green-text'>Asignada</td>";
                }
                //Unidades de Materiales
                cadena += "<td>";
                bool flag1 = false;
                foreach (var item in BD.detalle_ficha_material.ToList().Where(o => o.ficha_tecnica1.detalle_reserva == obj.id))
                {
                    if (flag1)
                        cadena += " - ";
                    cadena += item.unidad_material1.material1.nombre;
                    flag1 = true;
                }
                cadena += "</td>";
                //Unidades de Herramientas
                cadena += "<td>";
                bool flag2 = false;
                foreach (var item in BD.detalle_ficha_herramienta.ToList().Where(o => o.ficha_tecnica1.detalle_reserva == obj.id))
                {
                    if (flag2)
                        cadena += " - ";
                    cadena += item.unidad_herramienta1.herramienta1.nombre;
                    flag2 = true;
                }
                cadena += "</td>";
                cadena += "<td>";

                if (BD.ficha_tecnica.ToList().Exists(o => o.detalle_reserva == obj.id))
                {
                    cadena += "<a class='waves-effect waves-light btn btn-floating orange'><i class='icon-box' onclick='Editar(" + BD.ficha_tecnica.Single(o => o.detalle_reserva == obj.id).id + ");'></i></a>&nbsp;";
                }
                else
                {
                    cadena += "<a class='waves-effect waves-light btn btn-floating orange'><i class='icon-box' onclick='Nuevo("+obj.id+");'></i></a>&nbsp;";
                }
                cadena += "</td>";
                cadena += "</tr>";
            }
            cadena += "</tbody>";
            cadena += "</table>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(int id, int detalle_reserva, string usuario_almacen, int nro_ficha, string descripcion, string herramientas, string materiales)
        {
            ficha_tecnica obj;
            string error = "";
            if (string.IsNullOrEmpty(error))
            {
                if (id == 0)
                {
                    obj = new ficha_tecnica();
                    obj.detalle_reserva = detalle_reserva;
                    //obj.usuario_almacen = int.Parse(usuario_almacen);
                    obj.nro_ficha = nro_ficha;
                    obj.descripcion_problema = descripcion;
                    obj.hora = DateTime.Now.Hour;
                    BD.ficha_tecnica.Add(obj);
                    BD.SaveChanges();

                    RegistrarDetalleFichaMateriales(obj.id, materiales);
                    RegistrarDetalleFichaHerramientas(obj.id, herramientas);
                }
                else
                {
                    obj = BD.ficha_tecnica.Single(o => o.id == id);
                    obj.detalle_reserva = detalle_reserva;
                    //obj.usuario_almacen = int.Parse(usuario_almacen);
                    obj.nro_ficha = nro_ficha;
                    obj.descripcion_problema = descripcion;
                    obj.hora = DateTime.Now.Hour;
                    BD.SaveChanges();

                    foreach (var item in BD.detalle_ficha_material.Where(o => o.ficha_tecnica == id))
                    {
                        BD.detalle_ficha_material.Remove(item);
                    }
                    foreach (var item in BD.detalle_ficha_herramienta.Where(o => o.ficha_tecnica == id))
                    {
                        BD.detalle_ficha_herramienta.Remove(item);
                    }

                    RegistrarDetalleFichaMateriales(obj.id, materiales);
                    RegistrarDetalleFichaHerramientas(obj.id, herramientas);

                }
            }
            return Json(error, JsonRequestBehavior.AllowGet);
        }
        void RegistrarDetalleFichaMateriales(int id, string materiales)
        {
            detalle_ficha_material obj;
            string[] split = materiales.Split(new Char[] { ',' });
            for (int i = 0; i < split.Length; i++)
            {
                obj = new detalle_ficha_material();
                obj.ficha_tecnica = id;
                obj.unidad_material = int.Parse(split[i]);
                obj.estado = true;
                BD.detalle_ficha_material.Add(obj);
                BD.SaveChanges();
            }
        }
        void RegistrarDetalleFichaHerramientas(int id, string herramientas)
        {
            detalle_ficha_herramienta obj;
            string[] split = herramientas.Split(new Char[] { ',' });
            for (int i = 0; i < split.Length; i++)
            {
                obj = new detalle_ficha_herramienta();
                obj.ficha_tecnica = id;
                obj.unidad_herramienta = int.Parse(split[i]);
                obj.estado = true;
                BD.detalle_ficha_herramienta.Add(obj);
                BD.SaveChanges();
            }
        }
        public ActionResult Get(int id)
        {
            ficha_tecnica obj = BD.ficha_tecnica.Single(o => o.id == id);
            bool flag1 = false;
            string materiales = "";
            bool flag2 = false;
            string herramientas = "";

            foreach (var item in BD.detalle_ficha_material.ToList().Where(o => o.ficha_tecnica == obj.id))
            {
                if (flag1)
                materiales += ",";
                materiales += item.unidad_material.ToString();
                flag1 = true;
            }

            foreach (var item in BD.detalle_ficha_herramienta.ToList().Where(o => o.ficha_tecnica == obj.id))
            {
                if (flag2)
                    herramientas += ",";
                herramientas += item.unidad_herramienta.ToString();
                flag2 = true;
            }
            var ficha_tecnica = new
            {
                detalle_reserva = obj.detalle_reserva,
                usuario_almacen = obj.usuario_almacen,
                nro_ficha = obj.nro_ficha,
                descripcion = obj.descripcion_problema,
                hora = obj.hora,
                materiales = materiales,
                herramientas = herramientas
            };
            return Json(ficha_tecnica, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ficha_tecnica f_t = BD.ficha_tecnica.Single(o => o.id == id);
                BD.ficha_tecnica.Remove(f_t);
                BD.SaveChanges();
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListarDetalleReserva()
        {
            string cadena = "<select id='selectDetalleReserva'>";
            cadena += "<option value='' disabled selected>(Seleccionar)</option>";
            foreach (var item in BD.detalle_reserva.ToList().Where(o => o.estado))
            {
                cadena += "<option value=" + item.id + ">" + item.id + "</option>";
            }
            cadena += "</select>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListarUnidadMaterial()
        {
            string cadena = "<select id='selectUnidadMaterial' multiple>";
            cadena += "<option value='' disabled selected>(Seleccionar)</option>";
            foreach (var item in BD.unidad_material.ToList().Where(o => o.estado))
            {
                cadena += "<option value=" + item.id + ">" + item.material1.nombre + "</option>";
            }
            cadena += "</select>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListarUnidadHerramienta()
        {
            string cadena = "<select id='selectUnidadHerramienta' multiple>";
            cadena += "<option value='' disabled selected>(Seleccionar)</option>";
            foreach (var item in BD.unidad_herramienta.ToList().Where(o => o.estado))
            {
                cadena += "<option value=" + item.id + ">" + item.herramienta1.nombre + "</option>";
            }
            cadena += "</select>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }
    }
}