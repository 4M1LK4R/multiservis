using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using multiservis.Models;
using System.IO;

namespace multiservis.Controllers
{
    public class TecnicoController : Controller
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
            cadena += "<th>Fotografía</th>";
            cadena += "<th>Nombres</th>";
            cadena += "<th>Paterno</th>";
            cadena += "<th>Materno</th>";
            cadena += "<th>Fecha Inscripcion</th>";
            cadena += "<th>Estado</th>";
            cadena += "<th>Opciones</th>";
            cadena += "</tr>";
            cadena += "</thead>";
            cadena += "<tbody>";
            foreach (var obj in BD.tecnico.ToList())
            {
                cadena += "<tr>";
                cadena += "<td><img class='responsive-img' src='" + obj.ruta_imagen+ "'></td>";
                cadena += "<td>" + obj.persona1.nombres + "</td>";
                cadena += "<td>" + obj.persona1.paterno + "</td>";
                cadena += "<td>" + obj.persona1.materno + "</td>";
                cadena += "<td>" + obj.fecha_inscripcion.ToShortDateString() + "</td>";
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
        public ActionResult Guardar(int id, string nombres, string parterno, string materno, string correo, string nacionalidad, string ci, string telefono, string direccion, string nro_seguro, string salario, string fecha_inscripcion, bool estado)
        {
            persona obj_p;
            tecnico obj_t;
            string error = "";
            if (string.IsNullOrEmpty(nombres))
                error = "El campo nombres esta vacio";
            if (string.IsNullOrEmpty(nacionalidad))
                error = "El campo nacionalidad esta vacio";
            if (string.IsNullOrEmpty(salario))
                error = "El campo salario esta vacio";

            try
            {
                DateTime d = DateTime.Parse(fecha_inscripcion).Date;
            }
            catch
            {
                error = "Debe seleccionar una fecha valida!";
            }



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

                    obj_t = new tecnico();
                    obj_t.persona = obj_p.id;
                    obj_t.nro_seguro = int.Parse(nro_seguro);
                    obj_t.salario = Convert.ToDecimal(salario);
                    obj_t.fecha_inscripcion = DateTime.Parse(fecha_inscripcion).Date;
                    obj_t.estado = estado;
                    BD.tecnico.Add(obj_t);
                    BD.SaveChanges();
                    var resp1 = new { check = true, msg = error , id = obj_t.id};
                    return Json(resp1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    obj_t = BD.tecnico.Single(o => o.id == id);
                    obj_t.nro_seguro = int.Parse(nro_seguro);
                    obj_t.salario = Convert.ToDecimal(salario);
                    //obj_t.fecha_inscripcion = DateTime.Parse(fecha_inscripcion).Date;
                    obj_t.estado = estado;

                    obj_p = BD.persona.Single(o => o.id == obj_t.persona);
                    obj_p.nombres = nombres;
                    obj_p.paterno = parterno;
                    obj_p.materno = materno;
                    obj_p.correo = correo;
                    obj_p.nacionalidad = nacionalidad;
                    obj_p.ci = int.Parse(ci);
                    obj_p.telefono = telefono;
                    obj_p.direccion = direccion;

                    BD.SaveChanges();
                    var resp2 = new { check = true, msg = error, id = obj_t.id};
                    return Json(resp2, JsonRequestBehavior.AllowGet);

                }
            }
            var resp = new { check = false, msg = error };
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int id)
        {
            tecnico obj = BD.tecnico.Single(o => o.id == id);
            var tecnico = new
            {
                ruta_img = obj.ruta_imagen,
                nombres = obj.persona1.nombres,
                paterno = obj.persona1.paterno,
                materno = obj.persona1.materno,
                correo = obj.persona1.correo,
                nacionalidad = obj.persona1.nacionalidad,
                ci = obj.persona1.ci,
                telefono = obj.persona1.telefono,
                direccion = obj.persona1.direccion,

                id_persona = obj.persona,
                nro_seguro = obj.nro_seguro,
                salario = obj.salario.ToString(),
                fecha_inscripcion = obj.fecha_inscripcion.ToShortDateString(),
                estado = obj.estado
            };
            return Json(tecnico, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                tecnico t = BD.tecnico.Single(o => o.id == id);
                BD.tecnico.Remove(t);
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
        public ActionResult ListarSelectTecnicos()
        {
            string cadena = "<select id='selectTecnico'>";
            cadena += "<option value='' disabled selected>(Seleccionar)</option>";
            foreach (var item in BD.tecnico.ToList())
            {
                cadena += "<option value=" + item.id + ">" + item.persona1.nombres + " | C.I.:" + item.persona1.ci + "</option>";
            }
            cadena += "</select>";
            return Json(cadena, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SubirImagen(int id_img)
        {
            tecnico tec = BD.tecnico.Single(o => o.id == id_img);

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                var fileName = Path.GetFileName(file.FileName);

                fileName = id_img+"_"+tec.persona1.nombres + "_" + tec.persona1.paterno + fileName.Substring(fileName.LastIndexOf("."));

                var path = Path.Combine(Server.MapPath("~/Resources/images/tecnicos/"), fileName);

                tec.ruta_imagen = "../Resources/images/tecnicos/"+ fileName;

                BD.SaveChanges();
                file.SaveAs(path);
            }

        }

    }
}