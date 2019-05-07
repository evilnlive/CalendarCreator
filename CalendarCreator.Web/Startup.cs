using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CalendarCreator.Web.Startup))]
namespace CalendarCreator.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
