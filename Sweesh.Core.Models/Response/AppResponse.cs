using System;
namespace Sweesh.Core.Models.Response
{
    public class AppResponse
    {
        public ConfigItem Config { get; set; } = null;
        public File App { get; set; } = null;

        public AppResponse() {}

        public AppResponse(ConfigItem config, File app)
        {
            this.Config = config;
            this.App = app;
        }
    }
}
