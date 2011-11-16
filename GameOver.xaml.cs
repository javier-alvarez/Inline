using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace inline
{
    public partial class GameOver : PhoneApplicationPage
    {
        public GameOver()
        {
            InitializeComponent();
           
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            GameOverText.Text = string.Format
            (@"Your final score is {0}! Despite your best refactoring efforts, the code ended up being a huge mess... Try again and see whether you can do better next time!", this.NavigationContext.QueryString["result"]);                                 
            base.OnNavigatedTo(e);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}