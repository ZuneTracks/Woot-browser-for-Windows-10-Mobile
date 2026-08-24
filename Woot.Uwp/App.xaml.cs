using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace Woot.Uwp
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        public static void ApplyTheme()
        {
            var saved = ApplicationData.Current.LocalSettings.Values["WootDarkTheme"];
            var dark = saved == null || Convert.ToBoolean(saved);
            SetBrush("PhonePageBrush", dark ? "#181818" : "#F2F2F2");
            SetBrush("PhoneHeaderBrush", dark ? "#242424" : "#FFFFFF");
            SetBrush("PhoneCardBrush", dark ? "#292929" : "#FFFFFF");
            SetBrush("PhonePrimaryTextBrush", dark ? "#FFFFFF" : "#202020");
            SetBrush("PhoneMutedTextBrush", dark ? "#B8B8B8" : "#606060");
        }

        private static void SetBrush(string key, string hex)
        {
            var brush = Current.Resources[key] as SolidColorBrush;
            if (brush != null)
                brush.Color = Color.FromArgb(255,
                    Convert.ToByte(hex.Substring(1, 2), 16),
                    Convert.ToByte(hex.Substring(3, 2), 16),
                    Convert.ToByte(hex.Substring(5, 2), 16));
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            if (rootFrame == null)
            {
                rootFrame = new Windows.UI.Xaml.Controls.Frame();
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
                rootFrame.Navigate(typeof(MainPage), e.Arguments);

            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
        }
    }
}
