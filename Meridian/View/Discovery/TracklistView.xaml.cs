using Meridian.ViewModel.Discovery;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Meridian.View.Discovery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TracklistView : Page
    {
        public TracklistViewModel ViewModel => DataContext as TracklistViewModel;

        public TracklistView()
        {
            this.InitializeComponent();
        }
    }
}