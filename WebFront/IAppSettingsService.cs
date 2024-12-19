using Microsoft.Extensions.Options;
using SCAPI.WebFront.Models;

namespace SCAPI.WebFront
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
            return _settings.APIURL;
        }
    }
}
