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

        private int mx;     // modify x coordinate
        private int my;     // modify y coordinate
        private int mwl;    // modify width left coordinate
        private int mhu;    // modify height up coordinate
        private int mwr;    // modify width right coordinate
        private int mhd;    // modify height down coordinate

        public Task1(int num)
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.SetBounds(0, 0, Setting.SCREEN_WIDTH, Setting.SCREEN_HEIGHT);

            current = num + 1;
            Item task = (Item)Setting.taskList[num];
            taskNum.Text = "문항:\t" + task.pNum;
            
            initStimulus(task);

            mx = 0;
            my = 0;
            mwl = 0;
            mhu = 0;
            mwr = 0;
            mhd = 0;
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
            stimulus.nB.x = item.rStimuls.x;
            stimulus.nB.y = item.rStimuls.y;
            stimulus.nB.w = item.rStimuls.w;
            stimulus.nB.h = item.rStimuls.h;
            stimulus.SendToBack();
            this.Controls.Add(stimulus);

            for (int i = 0; i < 15; i++)
            {
                words[i] = new Word(item.choice[i], false, false);
                words[i].SetBounds((int)Setting.cWord[i].X, (int)Setting.cWord[i].Y, (int)Setting.sWord.X, (int)Setting.sWord.Y);
                if (sizeSet)
                {
                    item.rChoice[i].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                    item.rChoice[i].h = (int)(Setting.sWord.Y + (2 * Setting.yBuffer));
                }

                words[i].Parent = this;
                words[i].nB.x = item.rChoice[i].x;
                words[i].nB.y = item.rChoice[i].y;
                words[i].nB.w = item.rChoice[i].w;
                words[i].nB.h = item.rChoice[i].h;
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
                bool check;
                foreach (Cdot c in item.cList)
                {
                    check = true;

                    if (c.y < Setting.margin.Y)
                        continue;

                    if (stimulus.newHit(0, c.x, c.y))
                    {
                        gr.FillRectangle(brr, (c.x - 2), (c.y - 2 - Setting.margin.Y), 5, 5);
                        map.BringToFront();
                        continue;
                    }

                    foreach (Word w in words)
                    {
                        if (w.newHit(0, c.x, c.y))
                        {
                            gr.FillRectangle(brr, (c.x - 2), (c.y - 2 - Setting.margin.Y), 5, 5);
                            map.BringToFront();
                            check = false;
                            break;
                        }
                    }

                    if (check)
                    {
                        gr.FillRectangle(brb, (c.x - 2), (c.y - 2 - Setting.margin.Y), 5, 5);
                        map.BringToFront();
                    }
                }

                Pen myPen = new Pen(Color.Red);
                gr.DrawRectangle(myPen,
                                new Rectangle(item.rStimuls.x + mx - mwl, 
                                            item.rStimuls.y - Setting.margin.Y + my - mhu,
                                            mwl + item.rStimuls.w + mwr,
                                            mhu + item.rStimuls.h + mhd));

                stimulus.SendToBack();
                for (int i = 0; i < 15; i++)
                {
                    gr.DrawRectangle(myPen,
                                new Rectangle(item.rChoice[i].x + mx - mwl,
                                            item.rChoice[i].y - Setting.margin.Y + my - mhu,
                                            mwl + item.rChoice[i].w + mwr,
                                            mhu + item.rChoice[i].h + mhd));
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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자,백스페이스,마이너스,소숫점 만 입력받는다.
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8 && e.KeyChar != 45 && e.KeyChar != 46) //8:백스페이스,45:마이너스,46:소수점
                e.Handled = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
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
            if (textBox3.Text.Equals("WL"))
                textBox3.Text = "";
        }

        private void textBox4_MouseEnter(object sender, EventArgs e)
        {
            if (textBox4.Text.Equals("HU"))
                textBox4.Text = "";
        }

        private void textBox5_MouseEnter(object sender, EventArgs e)
        {
            if (textBox5.Text.Equals("WR"))
                textBox5.Text = "";
        }

        private void textBox6_MouseEnter(object sender, EventArgs e)
        {
            if (textBox6.Text.Equals("HD"))
                textBox6.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("X")|| textBox2.Text.Equals("Y") ||
                textBox3.Text.Equals("WL") || textBox4.Text.Equals("HU") ||
                textBox5.Text.Equals("WR") || textBox6.Text.Equals("HD"))
            {
                MessageBox.Show("숫자만 입력하세요.", "오류", MessageBoxButtons.OK);
                return;
            }

            if (textBox1.Text.Equals(""))
                textBox1.Text = "0";
            if (textBox2.Text.Equals(""))
                textBox2.Text = "0";
            if (textBox3.Text.Equals(""))
                textBox3.Text = "0";
            if (textBox4.Text.Equals(""))
                textBox4.Text = "0";
            if (textBox5.Text.Equals(""))
                textBox5.Text = "0";
            if (textBox6.Text.Equals(""))
                textBox6.Text = "0";

            mx = Convert.ToInt32(textBox1.Text);
            my = Convert.ToInt32(textBox2.Text);
            mwl = Convert.ToInt32(textBox3.Text);
            mhu = Convert.ToInt32(textBox4.Text);
            mwr = Convert.ToInt32(textBox5.Text);
            mhd = Convert.ToInt32(textBox6.Text);


            if (mapShow)
            {
                //map panel이 없는 상태, 바로 새로 그림
                button3_Click(sender, e);
            }
            else
            {
                //map panel이 있는 상태, 지우고 다시 그림
                this.Controls.Remove(map);
                mapShow = true;
                button3_Click(sender, e);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Item item = (Item)Setting.taskList[(current - 1)];

            stimulus.nB.x = item.rStimuls.x + mx - mwl;
            stimulus.nB.y = item.rStimuls.y + my - mhu;
            stimulus.nB.w = mwl + item.rStimuls.w + mwr;
            stimulus.nB.h = mhu + item.rStimuls.h + mhd;

            for (int i = 0; i < 15; i++)
            {
                words[i].nB.x = item.rChoice[i].x + mx - mwl;
                words[i].nB.y = item.rChoice[i].y + my - mhu;
                words[i].nB.w = mwl + item.rChoice[i].w + mwr;
                words[i].nB.h = mhu + item.rChoice[i].h + mhd;
            }

        }
    }
}