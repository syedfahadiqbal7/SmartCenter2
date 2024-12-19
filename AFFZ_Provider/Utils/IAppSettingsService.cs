using AFFZ_Provider.Models;
using Microsoft.Extensions.Options;

namespace AFFZ_Provider.Utils
{
    public interface IAppSettingsService
    {
        string GetBaseIpAddress();
    }

    public class AppSettingsService : IAppSettingsService
    {
        private readonly AppSettings _settings;

        public AppSettingsService(IOptions<AppSettings> options)
        {
            _settings = options.Value;
        }

        public string GetBaseIpAddress()
        {
            return _settings.BaseIpAddress;
        }
    }
}
