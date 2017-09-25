using Microsoft.Groove.Api.Client;
using Microsoft.Groove.Api.DataContract;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Meridian.Services.Images
{
    public class GrooveImageResolver : IImageResolver
    {
        private readonly IGrooveClient _groove;

        public GrooveImageResolver()
        {
            _groove = Ioc.Resolve<IGrooveClient>();
        }

        public async Task<Uri> GetAlbumCover(string artist, string title)
        {
            var result = await _groove.SearchAsync(MediaNamespace.music, artist + " - " + title, filter: SearchFilter.Tracks, maxItems: 1);

            var grooveArtist = result?.Artists?.Items.FirstOrDefault();

            if (grooveArtist != null)
            {
                var imageUrl = grooveArtist.GetImageUrl();

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    //check if image exists
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(new Uri(imageUrl), HttpCompletionOption.ResponseHeadersRead);
                    if (response.IsSuccessStatusCode)
                        return new Uri(imageUrl);
                }
            }

            return null;
        }

        public async Task<Uri> GetArtistImageUri(string artist, bool big = true)
        {
            var result = await _groove.SearchAsync(MediaNamespace.music, artist, filter: SearchFilter.Artists, maxItems: 1);

            var grooveArtist = result?.Artists?.Items.FirstOrDefault();

            if (grooveArtist != null)
            {
                var imageUrl = grooveArtist.GetImageUrl();

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    //check if image exists
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(new Uri(imageUrl), HttpCompletionOption.ResponseHeadersRead);
                    if (response.IsSuccessStatusCode)
                        return new Uri(imageUrl);
                }
            }

            return null;
        }
    }
}
