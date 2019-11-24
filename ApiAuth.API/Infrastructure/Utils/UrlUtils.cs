using Microsoft.AspNetCore.Http;

namespace ApiAuth.API.Infrastructure.Utils
{
    public static class UrlUtils
    {
        public static string GenerateUrl(string host, string path, IQueryCollection query, int? page)
        {
            var url = $"{host}{path}?currentPage={page}";
            foreach (var pair in query)
            {
                if (pair.Key == "currentPage")
                {
                    continue;
                }

                url += $"&{pair.Key}={pair.Value}";
            }

            return url;
        }
    }
}
