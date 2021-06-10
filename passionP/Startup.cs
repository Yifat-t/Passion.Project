using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(passionP.Startup))]
namespace passionP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
