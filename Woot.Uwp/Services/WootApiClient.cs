using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Woot.Uwp.Models;
using Windows.Data.Json;

namespace Woot.Uwp.Services
{
    public sealed class WootApiClient
    {
        private const string BaseUrl = "https://developer.woot.com/feed/";
        private readonly HttpClient client = new HttpClient();

        public async Task<IList<WootDeal>> GetFeedAsync(string feedName, string apiKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Enter your Woot API key in Settings before loading deals.");

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + Uri.EscapeDataString(feedName));
            request.Headers.Add("x-api-key", apiKey.Trim());
            request.Headers.Add("Accept", "application/json");

            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException("Woot returned HTTP " + (int)response.StatusCode + ".");

                var json = await response.Content.ReadAsStringAsync();
                return ParseDeals(json);
            }
        }

        private static IList<WootDeal> ParseDeals(string json)
        {
            var deals = new List<WootDeal>();
            JsonValue root;
            if (!JsonValue.TryParse(json, out root))
                throw new FormatException("The Woot response was not valid JSON.");

            JsonArray items = null;
            if (root.ValueType == JsonValueType.Array)
                items = root.GetArray();
            else if (root.ValueType == JsonValueType.Object)
            {
                var obj = root.GetObject();
                foreach (var key in new[] { "Items", "items", "Offers", "offers" })
                {
                    IJsonValue value;
                    if (obj.TryGetValue(key, out value) && value.ValueType == JsonValueType.Array)
                    {
                        items = value.GetArray();
                        break;
                    }
                }
            }

            if (items == null)
                throw new FormatException("The Woot response did not contain a deal list.");

            foreach (var item in items)
            {
                if (item.ValueType != JsonValueType.Object)
                    continue;
                var obj = item.GetObject();
                deals.Add(new WootDeal
                {
                    Title = ReadString(obj, "Title", "title") ?? "Untitled deal",
                    Subtitle = ReadString(obj, "Subtitle", "subtitle"),
                    SalePrice = FormatPrice(obj, "Price", "price", "SalePrice", "salePrice"),
                    ListPrice = FormatPrice(obj, "ListPrice", "listPrice", "OriginalPrice", "originalPrice"),
                    ImageUrl = ReadString(obj, "Photo", "photo", "Image", "image", "ImageUrl", "imageUrl"),
                    OfferUrl = ReadString(obj, "Url", "url", "SaleUrl", "saleUrl"),
                    IsSoldOut = ReadBool(obj, "IsSoldOut", "isSoldOut", "SoldOut", "soldOut"),
                    IsFeatured = ReadBool(obj, "IsFeatured", "isFeatured", "Featured", "featured")
                });
            }
            return deals;
        }

        private static string ReadString(JsonObject obj, params string[] names)
        {
            foreach (var name in names)
            {
                IJsonValue value;
                if (obj.TryGetValue(name, out value) && value.ValueType == JsonValueType.String)
                    return value.GetString();
            }
            return null;
        }

        private static bool ReadBool(JsonObject obj, params string[] names)
        {
            foreach (var name in names)
            {
                IJsonValue value;
                if (obj.TryGetValue(name, out value) && value.ValueType == JsonValueType.Boolean)
                    return value.GetBoolean();
            }
            return false;
        }

        private static string FormatPrice(JsonObject obj, params string[] names)
        {
            foreach (var name in names)
            {
                IJsonValue value;
                if (!obj.TryGetValue(name, out value))
                    continue;
                if (value.ValueType == JsonValueType.Number)
                    return value.GetNumber().ToString("C");
                if (value.ValueType == JsonValueType.String)
                    return value.GetString();
            }
            return null;
        }
    }
}
