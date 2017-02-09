using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitViewer.Model
{
    class Cdot
    {
        public int x;
        public int y;
        public string word;

        public Cdot()
        {
            x = -1;
            y = -1;
            word = "";
        }

        public Cdot(int x, int y, string w)
        {
            this.x = x;
            this.y = y;
            this.word = w;
        }
    }
}
