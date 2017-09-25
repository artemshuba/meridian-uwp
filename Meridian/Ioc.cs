using GalaSoft.MvvmLight.Ioc;
using LastFmLib;
using Meridian.Services.VK;
using System.IO;
using VkLib;
using Windows.Storage;
using DeezerLib;
using Meridian.Services;
using Microsoft.Groove.Api.Client;
using Meridian.Services.Discovery;

namespace Meridian
{
    public class Ioc
    {
        private static readonly string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Meridian.db");

        public static void Setup()
        {
            //using official VK app for Windows appId and secret
            SimpleIoc.Default.Register(() => new Vk(appId: "3502561", clientSecret: "omvP3y2MZmpREFZJDNHd", apiVersion: "5.60", userAgent: "com.vk.wp_app/4111 (WindowsPhone_10.0.14393.0, Microsoft__RM-1090_1010)"));
            SimpleIoc.Default.Register(() => new LastFm(apiKey: "a012acc1e5f8a61bc7e58238ce3021d8", apiSecret: "86776d4f43a72633fb37fb28713a7798"));
            SimpleIoc.Default.Register(() => new Deezer(appId: "229622", secretKey: "da90d8606bf99c8e1b403a80e03aefa3"));
            SimpleIoc.Default.Register(() => GrooveClientFactory.CreateGrooveClient("00000000480DD98A", "czFUCiXbMxQ7M+z5ycKFPWs2W1S7TxFh"));
            //SimpleIoc.Default.Register(() => new SQLiteAsyncConnection(DbPath));

            SimpleIoc.Default.Register<VkTracksService>();
            SimpleIoc.Default.Register<VkUserService>();
            SimpleIoc.Default.Register<CacheService>();
            SimpleIoc.Default.Register<ImageService>();
            SimpleIoc.Default.Register<DiscoveryService>();
            SimpleIoc.Default.Register<MusicResolveService>();
            SimpleIoc.Default.Register<ScrobblingService>();
        }

        public static T Resolve<T>()
        {
            return SimpleIoc.Default.GetInstance<T>();
        }
    }
}