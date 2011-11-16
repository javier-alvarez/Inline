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
using inline.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace inline
{
    enum GameState
    {
        AboutToChooseNextItem,
        AboutToMoveTheItem
    }

    public partial class MainPage : PhoneApplicationPage
    {
        private ViewModel _viewModel = new ViewModel();
        private GameState mState = GameState.AboutToChooseNextItem;
        // Constructor
        public MainPage()
        {
            this.DataContext = this._viewModel;

            InitializeComponent();

            for (int i = 0; i < 7; i++)
            {
                ContentGameGrid.RowDefinitions.Add(new RowDefinition());
                ContentGameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }


            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {

                    var button = new Button();
                    button.Style = (Style)Resources["TextButton"];
                    button.Name = i.ToString() + j.ToString();
                    button.SetBinding(Button.ContentProperty, new Binding("Content") { Source = _viewModel.Items[i, j] });
                    button.DataContext = _viewModel.Items[i, j];
     
             
                    ContentGameGrid.Children.Add(button);
                    button.Height = 80;
                    int x = i;
                    int y = j;
                    _viewModel.Items[x, y].PropertyChanged += (sender, e) =>
                    {
                        if (_viewModel.Items[x, y].State == inline.Model.State.Empty && mState == GameState.AboutToMoveTheItem)
                            VisualStateManager.GoToState(button, "ValidDestinationForSelected", true);
                        else 
                            VisualStateManager.GoToState(button, _viewModel.Items[x, y].State.ToString(), true);
                    };
                    button.Click += new RoutedEventHandler(button_Click);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    
                }
            }
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Reset();
            base.OnNavigatedTo(e);
        }

        //handle creation crap bug is still in wpf (disaster!)
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            PlaceNextItems();
            Popup codePopup = new Popup();
            TextBlock popupText = new TextBlock();
            popupText.Text = "Popup Text";
            codePopup.Child = popupText;
            Reset();
            
        }

        private void Reset()
        {
            _viewModel.Reset();
            mState = GameState.AboutToChooseNextItem;
            PlaceNextItems();
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            GameItem item = (GameItem)((Button)sender).DataContext;

            switch (mState)
            {
                case GameState.AboutToChooseNextItem:
                    if (item.State != Model.State.Empty)
                    {
                       
                        _viewModel.SelectedItem = item;
                        VisualStateManager.GoToState((Button)sender, string.Format("{0}Selected", item.State.ToString()), true);
                        mState = GameState.AboutToMoveTheItem;
                        _viewModel.ChangeVisualStyleOfEmptyCells(item);
                    }
                    break;
                case GameState.AboutToMoveTheItem:
                    mState = GameState.AboutToChooseNextItem;
                    if (item.State == Model.State.Empty&&_viewModel.AccesibleCellsFrom(_viewModel.SelectedItem).Contains(item))
                    {
                        _viewModel.MoveSelectedGameItemTo(item);
                        _viewModel.ChangeStyleOfEmptyCellsToNormal();
                        ClearItemsAndPlaceNext();                        
                    }
                    else
                    {
                        _viewModel.ChangeStyleOfEmptyCellsToNormal();
                        _viewModel.SelectedItem.State = _viewModel.SelectedItem.State;
                        goto case GameState.AboutToChooseNextItem;
                    }
                    break;
            }            
        }


        private void PlaceNextItems()
        {
            if (!_viewModel.PlaceNextItems())
            {
                GameOver();
                return;
            }
            VisualStateManager.GoToState(b1,_viewModel.NextItems[0].State.ToString(),true);
            VisualStateManager.GoToState(b2, _viewModel.NextItems[1].State.ToString(), true);
            VisualStateManager.GoToState(b3, _viewModel.NextItems[2].State.ToString(), true);
            RemoveItems();
        }

        private void GameOver()
        {         
            NavigationService.Navigate(new Uri("/GameOver.xaml?result="+_viewModel.Total, UriKind.Relative));
        }

       
        private void RemoveItems()
        {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    Thread.Sleep(700);
                    this.Dispatcher.BeginInvoke(() =>
                        {
                            _viewModel.AddPoints (_viewModel.Items.ClearConsecutiveItems());
                        });
                });
        }

        private void ClearItemsAndPlaceNext()
        {
            _viewModel.ShowPoints(_viewModel.Items.CalculatePoints());
            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.Sleep(800);
                this.Dispatcher.BeginInvoke(() =>
                {
                    int itemsRemoved = _viewModel.Items.ClearConsecutiveItems();
                    _viewModel.AddPoints(itemsRemoved);
                    if (itemsRemoved == 0)
                    {
                        PlaceNextItems();
                    }
                });
            });
        }
    }
}
