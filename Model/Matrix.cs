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
using System.Collections;
using System.Collections.Generic;

namespace inline.Model
{
    public class Matrix<T> : IEnumerable
    {
        protected readonly T[,] _matrix;
        public Matrix(int rows, int cols)
        {
            _matrix = new T[rows, cols];
            Rows = rows;
            Columns = cols;

        }

        public Matrix(T[,] matrix)
        {
            _matrix = (T[,])matrix.Clone();
            Rows = matrix.GetLength(0);
            Columns = matrix.GetLength(1);
        }

        public int Rows { get; private set; }
        public int Columns { get; private set; }


        public T this[int row, int col]
        {
            get { return _matrix[row, col]; }
            set { _matrix[row, col] = value; }
        }      


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _matrix.GetEnumerator();
        }


        public Matrix<T> Clone()
        {
            return new Matrix<T>(_matrix);
        }
    }
}
