using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using inline.Model;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace inline
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly NextItemGetter mNextItemGetter = new NextItemGetter();
        public IList<GameItem> NextItems { get; set; }
        public GridGameItems Items { get { return _items; } }
        public ViewModel()
        {
            NextItems = new ObservableCollection<GameItem>();
            
            _items = new GridGameItems(7, 7);
            for (int i = 0; i < _items.Rows; i++)
            {
                for (int j = 0; j < _items.Columns; j++)
                {
                    _items[i, j] = new GameItem(i,j, "");
                }
            }
            Result = string.Format("Score: {0}", 0);

        }
        int total = 0;

        public int Total { get { return total; } }

        internal void ShowPoints(int incomingPoints)
        {
            if(incomingPoints>=4)
                Result = string.Format("Score: {0} + {1} {2}", total.ToString(), CalculateBonus(incomingPoints),PointsToCheerUp(incomingPoints));
        }

        private int CalculateBonus(int incomingPoints)
        {
            if(incomingPoints>=4)
                return incomingPoints + (int)Math.Pow((incomingPoints - 4), 2);
            return 0;
        }

        private string PointsToCheerUp(int incomingPoints)
        {
            switch (incomingPoints)
            {
                case 4:return "Well done!";
                case 5: return "Cool!";
                case 6: return "Awesome!";
                default:
                    if (incomingPoints >= 7)
                        return "Legendary!";
                    else
                        return string.Empty;
            }
        }

        public void AddPoints(int value)
        {
            total += CalculateBonus(value);

            Result = string.Format("Score: {0}", total.ToString());
        }

        public GameItem SelectedItem { get; set; }

        private string _result;
        public event PropertyChangedEventHandler PropertyChanged;
        private GridGameItems _items;
        public string Result
        {
            get { return _result; }
            set { _result = value; this.RaisePropertyChanged(PropertyChanged, "Result"); }
        }

        public bool PlaceNextItems()
        {
            var emptyCells = _items.Where(x => x.State == State.Empty).ToArray();
            if (emptyCells.Length < 3)
            {
                return false;
            }
            Random rnd = new Random();
            foreach (var item in NextItems)
            {
                //could be already populated by the previous iteration
                while (true)
                {
                    int nextPosition = rnd.Next(emptyCells.Length);
                    if (emptyCells[nextPosition].State == State.Empty)
                    {
                        emptyCells[nextPosition].State = item.State;
                        emptyCells[nextPosition].Content = item.Content;
                        break;
                    }
                }
            }
            NextItems.Clear();
            NextItems.Add(mNextItemGetter.GetNext());
            NextItems.Add(mNextItemGetter.GetNext());
            NextItems.Add(mNextItemGetter.GetNext());
            return true;
        }

        internal void MoveSelectedGameItemTo(GameItem gameItem)
        {
            gameItem.State = SelectedItem.State;
            gameItem.Content = SelectedItem.Content;
            SelectedItem.State = State.Empty;
        }

        internal void ChangeVisualStyleOfEmptyCells(GameItem gameItem)
        {
            foreach (var item in AccesibleCellsFrom(gameItem))
            {
                item.State = State.Empty;
            }
        }

        public List<GameItem> AccesibleCellsFrom(GameItem gameItem)
        {
            List<GameItem> result = new List<GameItem>();
            GetAccesibleCellsFrom(result, gameItem.X, gameItem.Y);
            return result;
        }

        private void GetAccesibleCellsFrom(List<GameItem> accesibleItems, int i, int j)
        {
            if (i + 1 < Items.Rows && Items[i + 1, j].State == State.Empty && !accesibleItems.Contains(Items[i + 1, j]))
            {
                accesibleItems.Add(Items[i + 1, j]);
                GetAccesibleCellsFrom(accesibleItems, i+1, j);
            }
            if (i - 1 >= 0 && Items[i - 1, j].State == State.Empty && !accesibleItems.Contains(Items[i - 1, j]))
            {
                accesibleItems.Add(Items[i - 1, j]);
                GetAccesibleCellsFrom(accesibleItems, i - 1, j);
            }

            if (j - 1 >= 0 && Items[i, j - 1].State == State.Empty && !accesibleItems.Contains(Items[i, j - 1]))
            {
                accesibleItems.Add(Items[i, j - 1]);
                GetAccesibleCellsFrom(accesibleItems, i, j - 1);
            }
            if (j + 1 < Items.Columns && Items[i, j + 1].State == State.Empty && !accesibleItems.Contains(Items[i, j + 1]))
            {
                accesibleItems.Add(Items[i, j + 1]);
                GetAccesibleCellsFrom(accesibleItems, i, j + 1);
            }


        }

        internal void ChangeStyleOfEmptyCellsToNormal()
        {
            foreach (var item in Items)
                if (item.State == State.Empty)
                    item.State = State.Empty;
        }



        internal void Reset()
        {
            foreach (var item in Items)
            {
                item.State = State.Empty;
            }
            NextItems.Clear();
            NextItems.Add(mNextItemGetter.GetNext());
            NextItems.Add(mNextItemGetter.GetNext());
            NextItems.Add(mNextItemGetter.GetNext());
        }
    }
}
