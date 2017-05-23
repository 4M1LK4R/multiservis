using System.Web;
using System.Web.Optimization;

namespace multiservis
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //Css Estilos
            bundles.Add(new StyleBundle("~/Estilos").Include(
                "~/Resources/plugins/materialize/css/materialize.css",
                "~/Resources/plugins/css/style.css",
                "~/Resources/plugins/fontello/css/fontello.css",
                "~/Resources/plugins/datatable/css/jquery.dataTables.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/area").Include(
                "~/Resources/plugins/js/area.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/servicio").Include(
    "~/Resources/plugins/js/servicio.js"
    ));
        }
    }
}
