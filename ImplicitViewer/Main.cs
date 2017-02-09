using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ImplicitViewer.Model;
using System.Collections;

namespace ImplicitViewer
{
    public partial class Main : Form
    {
        public Task1 task1;
        public Task2 task2;
        public int current;

        public Main()
        {
            InitializeComponent();
            Setting.main = this;
            Setting.fontSize = 20;
            textBox1.Tag = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openPanel.Filter = "Raw Data (*raw.txt)|*raw.txt|All files (*.*)|*.*";
            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openPanel.SafeFileName;
                textBox1.Tag = openPanel.FileName;
            }
            else
            {
                textBox1.Text = "None";
                textBox1.Tag = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Tag.Equals(""))
                MessageBox.Show("데이터 파일을 선택해 주세요.", "오류", MessageBoxButtons.OK);
            else
                initReader();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void initReader()
        {
            string line;
            Setting.encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            Setting.path = textBox1.Tag.ToString();
            Setting.reader = new StreamReader(Setting.path, Setting.encode);

            line = Setting.reader.ReadLine();
            if (line.IndexOf("[실험 시작]") == -1)
            {
                MessageBox.Show("옳지 않은 데이터 파일 입니다.", "오류", MessageBoxButtons.OK);
                Setting.reader.Close();
                return;
            }
            else
            {
                string[] w = line.Split('\t');

                Setting.ID = w[1];
                Setting.SCREEN_WIDTH = Int32.Parse(w[3]);
                Setting.SCREEN_HEIGHT = Int32.Parse(w[4]);

                Setting.reader.Close();
            }

            setCoordinate();
            setTask();
        }

        private void setCoordinate()
        {
            Setting.margin = new PointF(30, 30);
            Setting.sStimulus = new PointF((float)((Setting.SCREEN_WIDTH - Setting.margin.X * 2) / 4.0),
                (float)((Setting.SCREEN_HEIGHT - Setting.margin.Y * 2) / 8.0));
            Setting.cStimulus = new PointF((float)((Setting.SCREEN_WIDTH - Setting.sStimulus.X) / 2.0),
                (float)(Setting.margin.Y + (Setting.sStimulus.Y / 2)));

            Setting.aWord = new PointF((float)(Setting.SCREEN_WIDTH - Setting.margin.X * 2),
                (float)((Setting.SCREEN_HEIGHT / 10.0) * 5));
            Setting.xInterval = (float)((Setting.aWord.X / 5.0) / 10.0);
            Setting.yInterval = (float)((Setting.aWord.Y / 3.0) / 10.0);
            Setting.xBuffer = (float)(((Setting.aWord.X / 5.0) - Setting.xInterval) / 8.0);
            Setting.yBuffer = (float)(((Setting.aWord.Y / 3.0) - Setting.yInterval) / 8.0);

            Setting.sWord = new PointF((float)((Setting.aWord.X / 5.0) - (Setting.xInterval * 2) - (Setting.xBuffer * 2)),
                (float)((Setting.aWord.Y / 3.0) - (Setting.yInterval * 2) - (Setting.yBuffer * 2)));

            Setting.cWord[0] = new PointF((float)(Setting.margin.X + Setting.xInterval + Setting.xBuffer),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + Setting.yInterval + Setting.yBuffer);
            Setting.cWord[1] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 3 + Setting.sWord.X),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + Setting.yInterval + Setting.yBuffer);
            Setting.cWord[2] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 5 + Setting.sWord.X * 2),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + Setting.yInterval + Setting.yBuffer);
            Setting.cWord[3] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 7 + Setting.sWord.X * 3),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + Setting.yInterval + Setting.yBuffer);
            Setting.cWord[4] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 9 + Setting.sWord.X * 4),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + Setting.yInterval + Setting.yBuffer);

            Setting.cWord[5] = new PointF((float)(Setting.margin.X + Setting.xInterval + Setting.xBuffer),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 3 + Setting.sWord.Y);
            Setting.cWord[6] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 3 + Setting.sWord.X),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 3 + Setting.sWord.Y);
            Setting.cWord[7] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 5 + Setting.sWord.X * 2),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 3 + Setting.sWord.Y);
            Setting.cWord[8] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 7 + Setting.sWord.X * 3),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 3 + Setting.sWord.Y);
            Setting.cWord[9] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 9 + Setting.sWord.X * 4),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 3 + Setting.sWord.Y);

            Setting.cWord[10] = new PointF((float)(Setting.margin.X + Setting.xInterval + Setting.xBuffer),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 5 + Setting.sWord.Y * 2);
            Setting.cWord[11] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 3 + Setting.sWord.X),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 5 + Setting.sWord.Y * 2);
            Setting.cWord[12] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 5 + Setting.sWord.X * 2),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 5 + Setting.sWord.Y * 2);
            Setting.cWord[13] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 7 + Setting.sWord.X * 3),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 5 + Setting.sWord.Y * 2);
            Setting.cWord[14] = new PointF((float)(Setting.margin.X + (Setting.xInterval + Setting.xBuffer) * 9 + Setting.sWord.X * 4),
                (float)(Setting.margin.Y + ((Setting.SCREEN_HEIGHT / 5.0) * 2)) + (Setting.yInterval + Setting.yBuffer) * 5 + Setting.sWord.Y * 2);
        }

        private void setTask()
        {
            Setting.taskList = new ArrayList();
            string line;
            string[] w;
            Setting.reader = new StreamReader(Setting.path, Setting.encode);

            int type;
            string stimulus;
            string[] choice;


            while((line = Setting.reader.ReadLine()) != null)
            {
                if (line.IndexOf("문항번호:") != -1)
                {
                    line = Setting.reader.ReadLine();
                    w = line.Split('\t');
                    type = Int32.Parse(w[1]);

                    line = Setting.reader.ReadLine();
                    w = line.Split('\t');
                    stimulus = w[1];

                    line = Setting.reader.ReadLine();
                    if (type == 1)
                    {
                        string[] wo = new string[15];

                        for (int i = 0; i < 15; i++)
                        {
                            line = Setting.reader.ReadLine();
                            w = line.Split('\t');
                            wo[i] = w[0];
                        }

                        choice = wo;
                    }
                    else
                    {
                        string[] wo = new string[2];

                        for (int i = 0; i < 2; i++)
                        {
                            line = Setting.reader.ReadLine();
                            w = line.Split('\t');
                            wo[i] = w[0];
                        }

                        choice = wo;
                    }

                    Item item = new Item(type, stimulus);
                    item.choice = choice;
                    item.cList = new ArrayList();

                    if (type == 1)
                    {
                        for (int i = 0; i < (3 + 1 + 15 + 3 + 1); i++)
                            line = Setting.reader.ReadLine();
                    }
                    else
                    {
                        for (int i = 0; i < (3 + 1 + 2 + 3 + 1); i++)
                            line = Setting.reader.ReadLine();
                    }

                    while (!(line = Setting.reader.ReadLine()).Equals(""))
                    {
                        if (line.IndexOf("[") != -1)
                            continue;

                        w = line.Split('\t');
                        Cdot cdot = new Cdot(Int32.Parse(w[1]), Int32.Parse(w[2]));
                        if (w.Length > 3)
                            cdot.word = w[3];
                        item.cList.Add(cdot);
                    }

                    Setting.taskList.Add(item);
                }
            }

            Setting.reader.Close();

            current = 0;
            showTask();
        }

        public void showTask()
        {
            if ((0 <= current) && (current < Setting.taskList.Count))
            {
                Item item = (Item)Setting.taskList[current];

                if (item.type == 1)
                {
                    task1 = new Task1(current);
                    task1.Show();
                }
                else
                {
                    task2 = new Task2(current);
                    task2.Show();
                }
            }
            else if (current < 0)
            {
                MessageBox.Show("첫번째 화면 입니다.", "오류", MessageBoxButtons.OK);
                this.current = 0;
                showTask();
            }
            else
            {
                MessageBox.Show("마지막 화면 입니다.", "오류", MessageBoxButtons.OK);
                this.current = Setting.taskList.Count -1;
                showTask();
            }
        }
    }
}
