using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SD210_BugTracker.Startup))]
namespace SD210_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
