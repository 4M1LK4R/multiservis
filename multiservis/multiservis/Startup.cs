using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(multiservis.Startup))]
namespace multiservis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
