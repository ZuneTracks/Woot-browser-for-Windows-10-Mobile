using System;
using System.Collections.Generic;
using System.Linq;
using Woot.Uwp.Models;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Woot.Uwp.Services
{
    public static class WootTileService
    {
        private const string LastViewedTitle = "WootLastViewedTitle";
        private const string LastViewedPrice = "WootLastViewedPrice";

        public static void Update(WootDeal lastViewed, IEnumerable<WootDeal> featuredDeals)
        {
            var settings = ApplicationData.Current.LocalSettings.Values;
            if (lastViewed != null)
            {
                settings[LastViewedTitle] = lastViewed.Title ?? string.Empty;
                settings[LastViewedPrice] = lastViewed.SalePrice ?? string.Empty;
            }

            var title = lastViewed == null ? settings[LastViewedTitle] as string : lastViewed.Title;
            var price = lastViewed == null ? settings[LastViewedPrice] as string : lastViewed.SalePrice;
            var featured = featuredDeals == null ? new List<WootDeal>() : featuredDeals.Take(2).ToList();
            var document = new XmlDocument();
            document.LoadXml(BuildTileXml(title, price, featured));
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(document));
        }

        private static string BuildTileXml(string lastTitle, string lastPrice, IList<WootDeal> featured)
        {
            var featuredText = featured.Count == 0
                ? "Featured: refresh the app to load deals."
                : "Featured: " + (featured[0].Title ?? string.Empty);
            if (featured.Count > 1)
                featuredText += " | " + (featured[1].Title ?? string.Empty);
            var lastViewedText = string.IsNullOrWhiteSpace(lastTitle)
                ? "Last viewed: open a deal."
                : "Last viewed: " + lastTitle + (string.IsNullOrWhiteSpace(lastPrice) ? string.Empty : " - " + lastPrice);
            var lines = new List<string>
            {
                "WOOT! DEALS",
                lastViewedText,
                featuredText
            };

            return "<tile><visual branding='name'>" +
                "<binding template='TileSquare71x71Image'>" +
                "<image id='1' src='ms-appx:///Assets/Square44x44Logo.png' alt='Woot!' />" +
                "</binding>" +
                BuildBinding("TileSquare150x150Text04", lines) +
                BuildBinding("TileWide310x150Text03", lines) +
                "</visual></tile>";
        }

        private static string BuildBinding(string template, IList<string> lines)
        {
            return "<binding template='" + template + "'>" +
                string.Join(string.Empty, lines.Select((line, index) => "<text id='" + (index + 1) + "'>" + Escape(line) + "</text>")) +
                "</binding>";
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
