using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OopRestaurant.Startup))]
namespace OopRestaurant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
