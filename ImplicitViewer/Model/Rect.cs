using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitViewer.Model
{
    class Rect
    {
        public int x;   //x좌표
        public int y;   //y좌표
        public int w;   //너비 Width
        public int h;   //높이 Height

        public Rect()
        {
            x = -1;
            y = -1;
            w = -1;
            h = -1;
        }

        public Rect(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }
    }
}
