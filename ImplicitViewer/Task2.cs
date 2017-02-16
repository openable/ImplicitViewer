﻿using System;
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

namespace ImplicitViewer
{
    public partial class Task2 : Form
    {
        private int current;
        private Word stimulus;
        private Word[] choice = new Word[2];
        private Map map;
        //private Panel map;
        private bool mapShow = true;

        public Task2(int num)
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
            //if (!Setting.pictureType) item.reverse();

            if (item.stimulus.Contains("s.png"))
            {
                stimulus = new Word(item.stimulus, true, true);
                stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 105), (int)Setting.margin.Y, 210, 280);

                item.rStimuls.w = (int)(210 + (2 * Setting.xBuffer));
                item.rStimuls.h = (int)(280 + (2 * Setting.yBuffer));
            }
            else if (item.stimulus.Contains(".png"))
            {
                stimulus = new Word(item.stimulus, true, true);
                stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 150), (int)Setting.margin.Y, 300, 400);

                item.rStimuls.w = (int)(300 + (2 * Setting.xBuffer));
                item.rStimuls.h = (int)(400 + (2 * Setting.yBuffer));
            }
            else
            {
                stimulus = new Word(item.stimulus, false, true);
                stimulus.SetBounds((int)Setting.cStimulus.X, (int)Setting.cStimulus.Y, (int)Setting.sStimulus.X, (int)Setting.sStimulus.Y);

                item.rStimuls.w = (int)(Setting.sStimulus.X + (2 * Setting.xBuffer));
                item.rStimuls.h = (int)(Setting.sStimulus.Y + (2 * Setting.yBuffer));
            }
            stimulus.Parent = this;
            stimulus.SendToBack();
            this.Controls.Add(stimulus);

            if (item.choice[0].Contains("s.png"))
            {
                choice[0] = new Word(item.choice[0], true, false);
                choice[0].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[0]);
                choice[0].SetBounds((int)(Setting.margin.X + Setting.xInterval * 5),
                                    (int)Setting.cWord[0].Y,
                                    210, 280);

                item.rChoice[0].w = (int)(210 + (2 * Setting.xBuffer));
                item.rChoice[0].h = (int)(280 + (2 * Setting.yBuffer));
            }
            else if (item.choice[0].Contains(".png"))
            {
                choice[0] = new Word(item.choice[0], true, false);
                choice[0].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[0]);
                choice[0].SetBounds((int)(Setting.margin.X + Setting.xInterval * 5),
                                    (int)Setting.cWord[0].Y,
                                    300, 400);

                item.rChoice[0].w = (int)(300 + (2 * Setting.xBuffer));
                item.rChoice[0].h = (int)(400 + (2 * Setting.yBuffer));
            }
            else
            {
                choice[0] = new Word(item.choice[0], false, false);
                choice[0].SetBounds((int)(Setting.cWord[5].X + Setting.xInterval * 4),
                                    (int)Setting.cWord[5].Y,
                                    (int)Setting.sWord.X,
                                    (int)(Setting.sWord.Y * 1.5));

                item.rChoice[0].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                item.rChoice[0].h = (int)((Setting.sWord.Y * 1.5) + (2 * Setting.yBuffer));
            }
            choice[0].Parent = this;
            this.Controls.Add(choice[0]);

            if (item.choice[1].Contains("s.png"))
            {
                choice[1] = new Word(item.choice[1], true, false);
                choice[1].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[1]);
                choice[1].SetBounds((int)(Setting.SCREEN_WIDTH - (Setting.margin.X + Setting.xInterval * 5 + 210)),
                                    (int)Setting.cWord[4].Y,
                                    210, 280);

                item.rChoice[1].w = (int)(210 + (2 * Setting.xBuffer));
                item.rChoice[1].h = (int)(280 + (2 * Setting.yBuffer));
            }
            else if (item.choice[1].Contains(".png"))
            {
                choice[1] = new Word(item.choice[1], true, false);
                choice[1].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[1]);
                choice[1].SetBounds((int)(Setting.SCREEN_WIDTH - (Setting.margin.X + Setting.xInterval * 5 + 300)),
                                    (int)Setting.cWord[4].Y,
                                    300, 400);

                item.rChoice[1].w = (int)(300 + (2 * Setting.xBuffer));
                item.rChoice[1].h = (int)(400 + (2 * Setting.yBuffer));
            }
            else
            {
                choice[1] = new Word(item.choice[1], false, false);
                choice[1].SetBounds((int)(Setting.cWord[9].X - Setting.xInterval * 4),
                                    (int)Setting.cWord[9].Y,
                                    (int)Setting.sWord.X,
                                    (int)(Setting.sWord.Y * 1.5));

                item.rChoice[1].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                item.rChoice[1].h = (int)((Setting.sWord.Y * 1.5) + (2 * Setting.yBuffer));
            }
            choice[1].Parent = this;
            this.Controls.Add(choice[1]);
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
                map.BackColor = Color.Transparent;
                map.Parent = this;
                map.SetBounds(0, Setting.margin.Y, Setting.SCREEN_WIDTH, (Setting.SCREEN_HEIGHT - Setting.margin.Y));
                this.Controls.Add(map);
                map.BringToFront();


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
                for (int i = 0; i < 2; i++)
                {
                    gr.DrawRectangle(myPen, new Rectangle(item.rChoice[i].x, item.rChoice[i].y - Setting.margin.Y, item.rChoice[i].w, item.rChoice[i].h));
                    choice[i].SendToBack();
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
    }
}