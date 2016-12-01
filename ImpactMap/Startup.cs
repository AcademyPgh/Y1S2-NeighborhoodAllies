using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImpactMap.Startup))]
namespace ImpactMap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
