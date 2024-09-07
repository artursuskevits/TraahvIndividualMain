using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TraahvIndividual.Startup))]
namespace TraahvIndividual
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
