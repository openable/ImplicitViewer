using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using ImplicitViewer.Model;
using System.Runtime.InteropServices;

namespace ImplicitViewer
{
    public partial class Task1 : Form
    {
        private int current;
        private Word stimulus;
        private Word[] words = new Word[15];
        private Map map;
        private bool mapShow = true;

        public Task1(int num)
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetBounds(0, 0, Setting.SCREEN_WIDTH, Setting.SCREEN_HEIGHT);

            current = num + 1;
            taskNum.Text = "화면:\t" + (current);
            Item task = (Item)Setting.taskList[num];

            initStimulus(task);
        }

        private void initStimulus(Item item)
        {
            item.shuffle();

            if (item.stimulus.Contains("s.png"))
            {
                stimulus = new Word(item.stimulus, true, true);
                stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 105), (int)Setting.margin.Y, 210, 280);
            }
            else if (item.stimulus.Contains(".png"))
            {
                stimulus = new Word(item.stimulus, true, true);
                stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 150), (int)Setting.margin.Y, 300, 400);
            }
            else
            {
                stimulus = new Word(item.stimulus, false, true);
                stimulus.SetBounds((int)Setting.cStimulus.X, (int)Setting.cStimulus.Y, (int)Setting.sStimulus.X, (int)Setting.sStimulus.Y);
            }
            stimulus.Parent = this;
            stimulus.SendToBack();
            this.Controls.Add(stimulus);

            for (int i = 0; i < 15; i++)
            {
                words[i] = new Word(item.choice[i], false, false);
                words[i].SetBounds((int)Setting.cWord[i].X, (int)Setting.cWord[i].Y, (int)Setting.sWord.X, (int)Setting.sWord.Y);
                words[i].Parent = this;
                this.Controls.Add(words[i]);
            }
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            // Setting.rawFile.WriteLine("==========================================");
        }

        private void keyControl(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp)
            {
                this.Close();
                Setting.main.current--;
                Setting.main.showTask();
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                this.Close();
                Setting.main.current++;
                Setting.main.showTask();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Setting.main.current--;
            Setting.main.showTask();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Setting.main.current++;
            Setting.main.showTask();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mapShow)
            {
                Item item = (Item)Setting.taskList[(current - 1)];
                map = new Map();

                //pan.BackColor = Color.FromArgb(90, 255, 255, 255);
                //map.BackColor = Color.Transparent;
                map.Parent = this;
                map.SetBounds(0, Setting.margin.Y, Setting.SCREEN_WIDTH, (Setting.SCREEN_HEIGHT - Setting.margin.Y));
                map.BringToFront();
                this.Controls.Add(map);

                
                Graphics gr = map.CreateGraphics();

                //Graphics gr = this.CreateGraphics();

                Brush brr = new SolidBrush(Color.Red);
                Brush brb = new SolidBrush(Color.SkyBlue);
                foreach (Cdot c in item.cList)
                {
                    if (c.y < Setting.margin.Y)
                        continue;
                    if (c.word == null)
                        gr.FillRectangle(brb, (c.x - 2), (c.y - 2 - Setting.margin.Y), 5, 5);
                    else
                        gr.FillRectangle(brr, (c.x - 2), (c.y - 2 - Setting.margin.Y), 5, 5);
                    map.BringToFront();
                }

                gr.Dispose();

                stimulus.SendToBack();
                foreach (Word w in words)
                {
                    w.SendToBack();
                }
                

                mapShow = false;
            }
            else
            {
                this.Controls.Remove(map);
                mapShow = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}