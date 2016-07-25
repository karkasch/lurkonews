using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lurkonews.Startup))]
namespace Lurkonews
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
