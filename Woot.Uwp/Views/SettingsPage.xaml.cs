using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Woot.Uwp.Views
{
    public sealed partial class SettingsPage : Page
    {
        private const string ApiKeySetting = "WootApiKey";

        public SettingsPage()
        {
            InitializeComponent();
            var value = ApplicationData.Current.LocalSettings.Values[ApiKeySetting] as string;
            if (!string.IsNullOrEmpty(value))
            {
                ApiKeyBox.Password = value;
                StatusText.Text = "An API key is saved locally.";
            }
            else
                StatusText.Text = "No API key saved.";
            var theme = ApplicationData.Current.LocalSettings.Values["WootDarkTheme"];
            DarkThemeSwitch.IsOn = theme == null || (theme is bool && (bool)theme);
            RootGrid.RequestedTheme = DarkThemeSwitch.IsOn ? ElementTheme.Dark : ElementTheme.Light;
            App.ApplyTheme();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var key = ApiKeyBox.Password == null ? string.Empty : ApiKeyBox.Password.Trim();
            if (string.IsNullOrEmpty(key))
            {
                StatusText.Text = "Enter an API key, or use Clear to remove the saved key.";
                return;
            }
            ApplicationData.Current.LocalSettings.Values[ApiKeySetting] = key;
            StatusText.Text = "API key saved locally.";
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values.Remove(ApiKeySetting);
            ApiKeyBox.Password = string.Empty;
            StatusText.Text = "API key cleared.";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var flyout = new Flyout();
            var content = new StackPanel { Width = 260, Padding = new Windows.UI.Xaml.Thickness(8) };
            content.Children.Add(new TextBlock { Text = "Woot! UWP", FontSize = 24 });
            content.Children.Add(new TextBlock { Text = "Woot! UWP is a Universal Windows app for Windows 10 Mobile", TextWrapping = TextWrapping.Wrap, Margin = new Windows.UI.Xaml.Thickness(0, 10, 0, 0) });
            content.Children.Add(new TextBlock { Text = "Build 1.0.0.7", Margin = new Windows.UI.Xaml.Thickness(0, 10, 0, 0) });
            content.Children.Add(new TextBlock { Text = "Developed by ZuneTracks" });
            content.Children.Add(new HyperlinkButton
            {
                Content = "github.com/ZuneTracks",
                NavigateUri = new Uri("https://github.com/ZuneTracks"),
                Margin = new Windows.UI.Xaml.Thickness(0, 4, 0, 0)
            });
            flyout.Content = content;
            flyout.ShowAt(button);
        }

        private void DarkThemeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["WootDarkTheme"] = DarkThemeSwitch.IsOn;
            RootGrid.RequestedTheme = DarkThemeSwitch.IsOn ? ElementTheme.Dark : ElementTheme.Light;
            App.ApplyTheme();
        }
    }
}
