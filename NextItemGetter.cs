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
using System.Collections.Generic;

namespace inline.Model
{
    public class NextItemGetter
    {
        private static IDictionary<State, string[]> mCaptions = new Dictionary<State, string[]>();

        static NextItemGetter()
        {
            mCaptions.Add(State.Some1, new[] { "for", "while", "switch", "{", "}","using","if","else","break;", "throw" });
            mCaptions.Add(State.Some2, new[] { "insert", "select", "update", "delete", "group\nby", "inner\njoin" });
            mCaptions.Add(State.Some3, new[] { "int\n10h", "int\n21h", "mov ax,\n13h", "rep\nstosw", "aaa", "pop\nes", "xor\nsi, si", "push\na000h", "rep\nmovsw", "ret", "shl di,\n4" });
            mCaptions.Add(State.Some4, new[] { "sum\n[1..10]", "tail", "head", "lcm\n12 8", "gcd\n21 14", "odd 5", "succ 20", "pred 21" });
            mCaptions.Add(State.Some5, new[] { "ldloc.0", "add", "stloc.0", "ldc.i4.2", "newobj", "ldarg.0", "ret", "newobj","rem" });
            mCaptions.Add(State.Some6, new[] { "puts", "-1.abs", "lambda", "Proc.new", "yield", "each", "collect", "rescue", "raise" });



        }
        private Random rnd = new Random();

        public GameItem GetNext()
        {
            var next = new GameItem(rnd.Next(6) + 1);
            next.Content = mCaptions[next.State][rnd.Next(mCaptions[next.State].Length)];
            return next;
        }
    }
}
