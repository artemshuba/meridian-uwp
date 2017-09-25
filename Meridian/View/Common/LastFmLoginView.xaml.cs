using Meridian.ViewModel.Common;
using Windows.System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Meridian.View.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LastFmLoginView : Page
    {
        public LastFmLoginViewModel ViewModel => (LastFmLoginViewModel)DataContext;

        public LastFmLoginView()
        {
            this.InitializeComponent();
        }

        private void LoginBox_OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
                return;

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            }
            else
            {
                ViewModel.LoginCommand.Execute(null);
            }
        }

        private void PasswordBox_OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
                return;

            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
                {
                    LoginTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                }
                else
                {
                    ViewModel.LoginCommand.Execute(null);
                }
            }
        }
    }
}