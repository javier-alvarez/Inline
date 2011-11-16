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
using System.ComponentModel;

namespace inline.Model
{
    public enum State
    {
        Empty, Some1, Some2, Some3,Some4,Some5,Some6
    }
    public class GameItem:INotifyPropertyChanged,IEquatable<GameItem>
    {
        private string mContent;
        public int X { get; set; }
        public int Y { get; set; }
        public string Content 
        { 
            get
            {
                return mContent;
            }
            set
            {
                mContent = value;
                this.RaisePropertyChanged(PropertyChanged, "Content");
            }
        }

        public GameItem(int value)
        {
            _state = (State) value;
        }

        public GameItem(int x,int y, string content)
        {
            Content = content;
            X = x;
            Y = y;
        }

        private State _state=0;

        public State State
        {
            get
            { 
                return _state; }
            set
            {
                _state = value;
                if (_state == State.Empty)
                {
                    Content = "";
                }
                this.RaisePropertyChanged(PropertyChanged, "State");
            }
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals(GameItem other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
