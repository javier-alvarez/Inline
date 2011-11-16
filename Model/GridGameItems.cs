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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;


namespace inline.Model
{
    public class GridGameItems:Matrix<GameItem>, IEnumerable<GameItem>
    {
        public GridGameItems(int rows, int cols):base(rows,cols)
        {
            
        }

        public GridGameItems(GameItem[,] matrix)
            : base(matrix)
        {

        }      

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Number of items cleared.</returns>
        public int ClearConsecutiveItems()
        {
            List<GameItem> pointsToEmpty = GetPointsToEmpty();

            pointsToEmpty.ForEach(x => x.State = State.Empty);
            return pointsToEmpty.Count;
        }

        public int CalculatePoints()
        {
            return GetPointsToEmpty().Count;
        }

        private List<GameItem> GetPointsToEmpty()
        {
            List<GameItem> pointsToEmpty = new List<GameItem>();
            for (int i = 0; i < Rows; i++)
            {
                pointsToEmpty.AddRange(FilterConsecutiveItems(RowItems(i).ToList()));
            }
            for (int j = 0; j < Columns; j++)
            {
                pointsToEmpty.AddRange(FilterConsecutiveItems(ColumnItems(j).ToList()));
            }
            pointsToEmpty.AddRange(FilterConsecutiveItems(RightDiagonalItems().ToList()));
            pointsToEmpty.AddRange(FilterConsecutiveItems(LeftDiagonalItems().ToList()));
            return pointsToEmpty;
        }
     

       
        IEnumerable<GameItem> FilterConsecutiveItems(IList<GameItem> input)
        {
            List<GameItem> consecutiveItems = new List<GameItem>();
            consecutiveItems.Add(input[0]);
            for (int i = 1; i < input.Count; i++)
            {
                if (consecutiveItems[0].State == input[i].State)
                {
                    consecutiveItems.Add(input[i]);
                }
                else
                {
                    if ( consecutiveItems.Count > 3 && consecutiveItems[0].State != State.Empty )
                    {
                        foreach ( var item in consecutiveItems )
                            yield return item;
                    }
                    consecutiveItems.Clear();
                    consecutiveItems.Add(input[i]);
                }
            }

            //final tail
            if (consecutiveItems.Count > 3 && consecutiveItems[0].State != State.Empty)
            {
                foreach (var item in consecutiveItems)
                    yield return item;
            }
        }

        public IEnumerable<GameItem> RowItems(int row)
        {
            for (int i = 0; i < Columns; i++)
            {
                yield return _matrix[row, i];
            }
        }

        public IEnumerable<GameItem> ColumnItems(int column)
        {
            for (int i = 0; i < Rows; i++)
            {
                yield return _matrix[i, column];
            }
        }

        public IEnumerable<GameItem> RightDiagonalItems()
        {

            //First half
            for (int i = 0; i < Columns-3; i++)
            {
                int x = 0;
                int y = i;
                while (y < Columns && x < Rows)
                {
                    yield return _matrix[x, y];
                    x++;
                    y++;
                }
                yield return new GameItem((int)State.Empty);
            }

            //Second half skipping main diagonal
            for (int i = 1; i < Rows-3; i++)
            {
                int x = i;
                int y = 0;
                while (y < Columns && x < Rows)
                {
                    yield return _matrix[x, y];
                    x++;
                    y++;
                }
                yield return new GameItem((int)State.Empty);
            }
        }

        public IEnumerable<GameItem> LeftDiagonalItems()
        {
            //First half
            for (int i = 3; i < Columns; i++)
            {
                int x = 0;
                int y = i;
                while (y >=0 && x < Rows)
                {
                    yield return _matrix[x, y];
                    x++;
                    y--;
                }
                yield return new GameItem((int)State.Empty);
            }

            //Second half skipping main diagonal
            for (int i = 1; i < Rows-3; i++)
            {
                int x = i;
                int y = Columns-1;
                while (y >= 0 && x < Rows)
                {
                    yield return _matrix[x, y];
                    x++;
                    y--;
                }
                yield return new GameItem((int)State.Empty);
            }
        }



        public IEnumerator<GameItem> GetEnumerator()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    yield return this[y, x];
                }
            }
        }
    }
}
