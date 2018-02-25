﻿using System.Threading.Tasks;

namespace Sweesh.Core.Abstract
{
    using Models;

    public interface IConfigAdapter : IBaseAdapter<Config>
    {
        Task Update(Config config);
        Task AddConfigItem(ConfigItem item, string configId);
        Task<Config> GetByAppId(Config config);
        Task<Config> GetByAppName(Config config);
        Task<Config> GetByAppName(string appname, string userid);
    }
}
