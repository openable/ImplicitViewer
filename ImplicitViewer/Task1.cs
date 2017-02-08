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

namespace ImplicitViewer
{
    public partial class Task1 : Form
    {
        private int current;
        private Word stimulus;
        private Word[] words = new Word[15];

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
            this.Controls.Add(stimulus);

            for (int i = 0; i < 15; i++)
            {
                words[i] = new Word(item.choice[i], false, false);
                words[i].SetBounds((int)Setting.cWord[i].X, (int)Setting.cWord[i].Y, (int)Setting.sWord.X, (int)Setting.sWord.Y);
                this.Controls.Add(words[i]);
            }
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            // Setting.rawFile.WriteLine("==========================================");
        }
    }
}