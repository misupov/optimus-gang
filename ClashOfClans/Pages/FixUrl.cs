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
                Small = urls.Small != null ? Fix(urls.Small) : null,
                Medium = urls.Medium != null ? Fix(urls.Medium) : null,
                Large = urls.Large != null ? Fix(urls.Large) : null
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