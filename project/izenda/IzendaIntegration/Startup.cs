using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IzendaIntegration.Startup))]
namespace IzendaIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
