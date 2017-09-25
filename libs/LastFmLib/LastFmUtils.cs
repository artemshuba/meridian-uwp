using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
namespace LastFmLib
{
    internal static class LastFmUtils
    {
        public static string BuildSig(string secretKey, string method, IDictionary<string, string> parameters)
        {
            parameters.Add("method", method);
            var temp = parameters.OrderBy(x => x.Key);
            var s = new StringBuilder();
            foreach (var p in temp)
            {
                s.Append(p.Key);
                s.Append(p.Value);
            }

            s.Append(secretKey);
            return Md5(s.ToString());
        }

        public static string Md5(string input)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }
    }
}
