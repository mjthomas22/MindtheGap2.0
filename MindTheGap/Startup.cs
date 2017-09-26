using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MindTheGap.Startup))]
namespace MindTheGap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
