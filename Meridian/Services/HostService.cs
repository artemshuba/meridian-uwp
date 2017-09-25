using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Meridian.Services
{
    /// <summary>
    /// Service for updating actual client id/secret/useragent for vk
    /// </summary>
    public static class HostService
    {
        public static async Task Update()
        {
            try
            {
                Logger.Info("Getting host info from server.");

                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(1200);
                var hostString = await httpClient.GetStringAsync("http://meridianvk.com/host.js");

                Logger.Info("Got host info: " + hostString);

                var host = JObject.Parse(hostString);
                var clientId = host["clientId"].Value<string>();
                var clientSecret = host["clientSecret"].Value<string>();
                var userAgent = host["userAgent"].Value<string>();

                var p = host["params"];

                var vk = Ioc.Resolve<VkLib.Vk>();
                vk.AppId = clientId;
                vk.ClientSecret = clientSecret;
                vk.UserAgent = userAgent;
                vk.LoginParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(p.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
