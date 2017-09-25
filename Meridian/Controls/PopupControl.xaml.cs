using Jupiter.Services.Navigation;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Meridian.Controls
{
    public interface IPopupContent
    {
        event EventHandler<object> CloseRequested;
    }

    public partial class PopupControl : UserControl
    {
        protected NavigationService _navigationService;
        private object _result;
        private object _parameter;

        // Using a DependencyProperty as the backing store for ContentType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTypeProperty =
            DependencyProperty.Register("ContentType", typeof(Type), typeof(PopupControl), new PropertyMetadata(null, (d, e) =>
            {
                var control = (PopupControl)d;
                var type = (Type)e.NewValue;
                control.PopupFrame.Navigate(type, control._parameter);
            }));

        public Type ContentType
        {
            get { return (Type)GetValue(ContentTypeProperty); }
            set { SetValue(ContentTypeProperty, value); }
        }

        public event EventHandler Closed;

        public PopupControl()
        {
            this.InitializeComponent();

            Initialize();
        }

        public static Task<object> Show<T>(object parameter = null)
        {
            var popup = new PopupControl();
            popup._parameter = parameter;
            popup.ContentType = typeof(T);
            popup.Show();

            var tcs = new TaskCompletionSource<object>();

            EventHandler handler = null;
            handler = (s, e) =>
            {
                popup.Closed -= handler;
                tcs.TrySetResult(popup._result);
            };
            popup.Closed += handler;

            return tcs.Task;
        }

        public void Show()
        {
            var showAnim = (Storyboard)Resources["ShowAnim"];
            showAnim.Begin();

            Shell.Current.AddPopup(this);
        }

        public void Close()
        {
            var closeAnim = (Storyboard)Resources["CloseAnim"];
            closeAnim.Begin();
        }

        protected virtual void Initialize()
        {
            _navigationService = new NavigationService(PopupFrame);
        }

        protected void PopupFrame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            var popupContent = PopupFrame.Content as IPopupContent;
            if (popupContent != null)
            {
                popupContent.CloseRequested -= PopupContent_CloseRequested;
                return;
            }

            popupContent = (PopupFrame.Content as Page)?.DataContext as IPopupContent;

            if (popupContent != null)
                popupContent.CloseRequested -= PopupContent_CloseRequested;
        }

        protected void PopupFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var popupContent = PopupFrame.Content as IPopupContent;
            if (popupContent != null)
            {
                popupContent.CloseRequested += PopupContent_CloseRequested;
                return;
            }

            popupContent = (PopupFrame.Content as Page)?.DataContext as IPopupContent;

            if (popupContent != null)
                popupContent.CloseRequested += PopupContent_CloseRequested;
        }

        private void PopupContent_CloseRequested(object sender, object result)
        {
            _result = result;
            Close();
        }

        protected void CloseAnim_Completed(object sender, object e)
        {
            Shell.Current.RemovePopup();

            Closed?.Invoke(this, EventArgs.Empty);
        }

        protected void BackgroundOverlay_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Close();
        }
    }
}