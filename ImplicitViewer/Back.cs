using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ImplicitViewer.Model;

namespace ImplicitViewer
{
    public partial class Back : Form
    {
        private int current;

        public Back()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.content.Text = "";
            this.msg.Text = "";
            this.startBtn.Hide();
        }

        public Back(int num)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            current = num + 1;

            initScreen((Item)Setting.taskList[num]);

            Setting.rawFile.WriteLine("==========================================");
            Setting.rawFile.WriteLine("안내화면: " + current);
        }

        private void initScreen(Item item)
        {
            this.content.AutoSize = false;
            this.content.Text = item.content;
            this.content.SetBounds((int)(Setting.margin.X * 3),
                                    (int)(Setting.margin.Y * 10),
                                    (int)(Setting.SCREEN_WIDTH - (Setting.margin.X * 6)),
                                    (int)(Setting.SCREEN_HEIGHT / 3.0));
            this.content.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            this.msg.AutoSize = false;
            this.msg.Text = item.msg;
            this.msg.SetBounds((int)(Setting.margin.X * 3),
                                    (int)(Setting.SCREEN_HEIGHT / 10 * 8 - 100),
                                    (int)(Setting.SCREEN_WIDTH - (Setting.margin.X * 6)),
                                    (int)(Setting.SCREEN_HEIGHT / 3.0));
            this.msg.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            if (msg.Text.Equals(""))
                this.startBtn.Text = "종료";

            this.startBtn.Location = new System.Drawing.Point((int)(Setting.SCREEN_WIDTH / 2 - 100), (int)(Setting.SCREEN_HEIGHT / 10 * 8));
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("==========================================");
            Setting.dataFile.WriteLine("==========================================");
            Console.WriteLine("안내화면\t" + current);
            Setting.dataFile.WriteLine("안내화면\t" + current);
            // Console.WriteLine("==========================================");
            // Setting.dataFile.WriteLine("==========================================");

            this.Close();
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            // Setting.rawFile.WriteLine("==========================================");
        }
    }
}
