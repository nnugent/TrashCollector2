using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrashCollector2.Startup))]
namespace TrashCollector2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
