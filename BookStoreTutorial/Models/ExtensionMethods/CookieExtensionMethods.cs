using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.ExtensionMethods
{
    public static class CookieExtensionMethods
    {
        public static string GetString(this IRequestCookieCollection cookies, string key) => cookies[key];

        public static int? GetInt32(this IRequestCookieCollection cookies, string key) =>
            int.TryParse(cookies[key], out int i) ? i : (int?)null;

        public static T GetObject<T>(this IRequestCookieCollection cookies, string key)
        {
            var value = cookies[key];
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
