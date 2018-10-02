using System;
using ClashOfClans.Pages.Model;

namespace ClashOfClans.Pages
{
    public static class FixUrl
    {
        public static IconUrls Fix(IconUrls urls)
        {
            return new IconUrls
            {
                Small = Fix(urls.Small),
                Medium = Fix(urls.Medium),
                Large = Fix(urls.Large)
            };
        }

        private static string Fix(string url)
        {
            try
            {
                var uri = new Uri(url);
                var path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                return "/images/coc/" + path;
            }
            catch
            {
                return url;
            }
        }
    }
}