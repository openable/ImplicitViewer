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
        //private Panel map;
        private bool mapShow = true;
        private bool sizeSet = true;        // 제시자극, 선택순서 너비, 높이 고정 명시 변수

        public Task1(int num)
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetBounds(0, 0, Setting.SCREEN_WIDTH, Setting.SCREEN_HEIGHT);

            current = num + 1;
            Item task = (Item)Setting.taskList[num];
            taskNum.Text = "문항:\t" + task.pNum;
            
            initStimulus(task);
        }

        private void initStimulus(Item item)
        {
            //item.shuffle();
            
            if (item.stimulus.Contains("s.png"))
            {
                stimulus = new Word(item.stimulus, true, true);
                stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 105), (int)Setting.margin.Y, 210, 280);

                if (sizeSet)
                {
                    item.rStimuls.w = (int)(210 + (2 * Setting.xBuffer));
                    item.rStimuls.h = (int)(280 + (2 * Setting.yBuffer));
                }
            }
            else if (item.stimulus.Contains(".png"))
            {
                stimulus = new Word(item.stimulus, true, true);
                stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 150), (int)Setting.margin.Y, 300, 400);

                if (sizeSet)
                {
                    item.rStimuls.w = (int)(300 + (2 * Setting.xBuffer));
                    item.rStimuls.h = (int)(400 + (2 * Setting.yBuffer));
                }
            }
            else
            {
                stimulus = new Word(item.stimulus, false, true);
                stimulus.SetBounds((int)Setting.cStimulus.X, (int)Setting.cStimulus.Y, (int)Setting.sStimulus.X, (int)Setting.sStimulus.Y);

                if (sizeSet)
                {
                    item.rStimuls.w = (int)(Setting.sStimulus.X + (2 * Setting.xBuffer));
                    item.rStimuls.h = (int)(Setting.sStimulus.Y + (2 * Setting.yBuffer));
                }
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

                if (sizeSet)
                {
                    item.rChoice[i].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                    item.rChoice[i].h = (int)(Setting.sWord.Y + (2 * Setting.yBuffer));
                }
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
                //map = new Panel();

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

                Pen myPen = new Pen(Color.Red);
                gr.DrawRectangle(myPen, new Rectangle(item.rStimuls.x, item.rStimuls.y - Setting.margin.Y, item.rStimuls.w, item.rStimuls.h));

                stimulus.SendToBack();
                for (int i = 0; i < 15; i++)
                {
                    gr.DrawRectangle(myPen, new Rectangle(item.rChoice[i].x, item.rChoice[i].y - Setting.margin.Y, item.rChoice[i].w, item.rChoice[i].h));
                    words[i].SendToBack();
                }

                brr.Dispose();
                brb.Dispose();
                myPen.Dispose();
                gr.Dispose();

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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("X"))
                textBox1.Text = "";
        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals("Y"))
                textBox2.Text = "";
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals("W"))
                textBox3.Text = "";
        }

        private void textBox4_MouseEnter(object sender, EventArgs e)
        {
            if (textBox4.Text.Equals("H"))
                textBox4.Text = "";
        }
    }
}