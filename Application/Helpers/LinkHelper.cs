using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Helpers
{
    public static class LinkHelper
    {
        public static bool IsLinkValid(string sourceLink)
        {
            string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            return Regex.IsMatch(sourceLink, pattern);
        }

        public static string GenerateBackHalf(string sourceLink)
        {
            //TODO: Временное решение для генерации ссылки. Поменять
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceLink + DateTime.Now.ToString()));
            var result = Convert.ToBase64String(hash);
            var r = result.Replace("/", string.Empty);
            return r;
        }
    }
}
