using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitViewer.Model
{
    class Cdot
    {
        public double time;
        public int x;
        public int y;
        public string word;

        public Cdot()
        {
            time = 0;
            x = -1;
            y = -1;
            word = "";
        }

        public Cdot(int x, int y)
        {
            time = 0;
            this.x = x;
            this.y = y;
        }

        public Cdot(double t, int x, int y)
        {
            time = t;
            this.x = x;
            this.y = y;
        }

        public Cdot(int x, int y, string w)
        {
            time = 0;
            this.x = x;
            this.y = y;
            this.word = w;
        }

        public Cdot(double t, int x, int y, string w)
        {
            time = t;
            this.x = x;
            this.y = y;
            this.word = w;
        }
    }
}
