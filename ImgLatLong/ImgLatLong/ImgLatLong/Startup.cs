using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImgLatLong.Startup))]
namespace ImgLatLong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
