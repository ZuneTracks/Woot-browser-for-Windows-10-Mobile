using System;
using Windows.UI.Xaml;

namespace Woot.Uwp.Models
{
    public sealed class WootDeal
    {
        public string Title { get; set; }
        public string OfferId { get; set; }
        public string Subtitle { get; set; }
        public string SalePrice { get; set; }
        public string ListPrice { get; set; }
        public string ImageUrl { get; set; }
        public string OfferUrl { get; set; }
        public bool IsSoldOut { get; set; }
        public bool IsFeatured { get; set; }
        public string StateText { get { return IsSoldOut ? "SOLD OUT" : "AVAILABLE"; } }
        public Visibility FeaturedVisibility { get { return IsFeatured ? Visibility.Visible : Visibility.Collapsed; } }
    }
}
