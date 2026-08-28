using Windows.Storage;

namespace Woot.Uwp.Services
{
    public static class WootApiKeyProvider
    {
        public static string Get()
        {
#if WOOT_LOCAL_BUILD
            if (!string.IsNullOrWhiteSpace(LocalBuildConfiguration.ApiKey))
                return LocalBuildConfiguration.ApiKey;
#endif
            return ApplicationData.Current.LocalSettings.Values["WootApiKey"] as string;
        }
    }
}
