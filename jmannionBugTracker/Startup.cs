using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jmannionBugTracker.Startup))]
namespace jmannionBugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
