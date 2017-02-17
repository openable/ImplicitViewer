using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitViewer.Model
{
    class Word : System.Windows.Forms.Button
    {
        public double gazeTime;
        private bool sequential = false;
        private double starTime;
        public string value;
        public Rect nB;         // new boundary (x, y, width, height)

        public Word()
        {
            gazeTime = 0.0;
            starTime = 0.0;
            nB = new Rect();
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            UseVisualStyleBackColor = true;
        }

        public Word(string text, bool img, bool head)
        {
            gazeTime = 0.0;
            starTime = 0.0;
            nB = new Rect();
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            UseVisualStyleBackColor = true;
            if (!img) this.Text = text;
            this.value = text;
            this.FlatAppearance.BorderSize = 1;
            // 단어 테두리 색깔 수정 부분
            this.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            if (head)
            {
                this.ForeColor = System.Drawing.Color.Yellow;
                this.Font = new System.Drawing.Font("Malgun Gothic", (Setting.fontSize + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            else
            {
                this.ForeColor = System.Drawing.Color.White;
                this.Font = new System.Drawing.Font("Malgun Gothic", Setting.fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            }
            //this.BackColor = System.Drawing.Color.Black;
            //this.BackColor = System.Drawing.Color.FromArgb(50, 255, 0, 0);
            this.BackColor = System.Drawing.Color.Transparent;
        }

        public bool newHit(double time, double x, double y)
        {
            bool isHit = false;
            if (nB.x < x
                && x < (nB.x + nB.w)
                && nB.y < y
                && y < (nB.y + nB.h))
            {
                isHit = true;
                if (!sequential)
                {
                    sequential = true;
                    starTime = time;
                }
                else
                {
                    gazeTime = gazeTime + (time - starTime);
                    starTime = time;
                }
            }
            else
            {
                sequential = false;
            }

            return isHit;
        }
    }
}

