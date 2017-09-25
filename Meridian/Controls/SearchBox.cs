using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Meridian.Controls
{
    public class SearchBox : TextBox
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(0));

        /// <summary>
        /// Search command
        /// </summary>
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public SearchBox()
        {
            DefaultStyleKey = typeof(SearchBox);
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == VirtualKey.Enter && !string.IsNullOrWhiteSpace(Text))
            {
                SearchCommand?.Execute(Text);
            }
        }
    }
}