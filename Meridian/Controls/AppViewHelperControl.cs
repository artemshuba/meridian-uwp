using Windows.UI.Xaml;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation.Metadata;

namespace Meridian.Controls
{
    public class AppViewHelperControl : Control
    {
        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppViewForegroundProperty =
            DependencyProperty.Register("AppViewForeground", typeof(Brush), typeof(AppViewHelperControl), new PropertyMetadata(null, (d, e) =>
            {
                var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                appView.TitleBar.ButtonForegroundColor = ((SolidColorBrush)e.NewValue).Color;

                if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName))
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    statusBar.ForegroundColor = ((SolidColorBrush)e.NewValue).Color;
                }
            }));

        /// <summary>
        /// Foreground brush
        /// </summary>
        public Brush AppViewForeground
        {
            get { return (Brush)GetValue(AppViewForegroundProperty); }
            set { SetValue(AppViewForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AppViewBackgroundProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppViewBackgroundProperty =
            DependencyProperty.Register("AppViewBackground", typeof(Brush), typeof(AppViewHelperControl), new PropertyMetadata(null, (d, e) =>
            {
                if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName))
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    statusBar.BackgroundColor = ((SolidColorBrush)e.NewValue).Color;
                    statusBar.BackgroundOpacity = 1;
                }
            }));

        /// <summary>
        /// Background brush
        /// </summary>
        public Brush AppViewBackground
        {
            get { return (Brush)GetValue(AppViewBackgroundProperty); }
            set { SetValue(AppViewBackgroundProperty, value); }
        }
    }
}