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
        private string[] fList;

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
            openPanel.Multiselect = true;

            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                if (openPanel.FileNames.Length == 1)
                {
                    textBox1.Text = openPanel.SafeFileName;
                    textBox1.Tag = openPanel.FileName;
                    fList = null;
                }
                else
                {
                    textBox1.Text = "0 / " + openPanel.FileNames.Length;
                    fList = openPanel.FileNames;
                }
                
            }
            else
            {
                textBox1.Text = "None";
                textBox1.Tag = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Tag.Equals("") && fList == null)
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
            if (fList == null)
                Setting.path = textBox1.Tag.ToString();
            else
                Setting.path = fList[0];
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
            Setting.margin = new Point(30, 30);
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
            int? cnt = Setting.taskList?.Count;
            if (cnt == null)
            {
                Setting.taskList = new ArrayList();
                string line;
                string[] w;
                Setting.reader = new StreamReader(Setting.path, Setting.encode);

                int type;
                string stimulus;
                Rect rStimuls;
                int nChoice;
                string[] choice;
                Rect[] rChoice;

                int pNum;        //문항번호
                int rTime;       //응답시간
                string dWord;    //선택단어
                int sTime;       //자극단어 응시시간
                int[] gTime;     //선택순서 응시시간 저장용

                while ((line = Setting.reader.ReadLine()) != null)
                {
                    if (line.IndexOf("문항번호:") != -1)
                    {
                        //문항번호 인식
                        w = line.Split('\t');
                        pNum = Int32.Parse(w[1]);

                        //문항유형 인식
                        line = Setting.reader.ReadLine();
                        w = line.Split('\t');
                        type = Int32.Parse(w[1]);

                        //제시자극 인식
                        line = Setting.reader.ReadLine();
                        w = line.Split('\t');
                        stimulus = w[1];

                        rStimuls = new Rect();
                        //제시자극이 2줄로 넘어오는 경우를 대비하기 위해 if문 예외처리
                        if (w.Length == 6)
                        {
                            rStimuls.x = (int)Convert.ToSingle(w[2]);
                            rStimuls.y = (int)Convert.ToSingle(w[3]);
                            // raw 데이터 출력에 이미 잘못되어서 Task 생성시 기본 환경 값을 토대로 바로 잡기로 함.
                            rStimuls.w = (int)Convert.ToSingle(w[4]);
                            rStimuls.h = (int)Convert.ToSingle(w[5]);
                        }

                        //제시자극 인식 할 때 줄바꿈 들어간 경우 예외처리
                        line = Setting.reader.ReadLine();
                        w = line.Split('\t');
                        if (!w[0].Equals("선택순서:"))
                        {
                            stimulus = stimulus + "\r\n" + w[0];
                            rStimuls.x = (int)Convert.ToSingle(w[1]);
                            rStimuls.y = (int)Convert.ToSingle(w[2]);
                            // raw 데이터 출력에 이미 잘못되어서 Task 생성시 기본 환경 값을 토대로 바로 잡기로 함.
                            rStimuls.w = (int)Convert.ToSingle(w[3]);
                            rStimuls.h = (int)Convert.ToSingle(w[4]);
                            line = Setting.reader.ReadLine();
                        }

                        //선택순서 인식
                        if (type == 1)
                            nChoice = 15;
                        else
                            nChoice = 2;

                        choice = new string[nChoice];
                        rChoice = new Rect[nChoice];
                        for (int i = 0; i < nChoice; i++)
                        {
                            line = Setting.reader.ReadLine();
                            w = line.Split('\t');
                            choice[i] = w[0];

                            rChoice[i] = new Rect();
                            if (w.Length < 6)
                            {
                                rChoice[i].x = (int)Convert.ToSingle(w[1]);
                                rChoice[i].y = (int)Convert.ToSingle(w[2]);
                                // raw 데이터 출력에 이미 잘못되어서 Task 생성시 기본 환경 값을 토대로 바로 잡기로 함.
                                rChoice[i].w = (int)Convert.ToSingle(w[3]);
                                rChoice[i].h = (int)Convert.ToSingle(w[4]);
                            }
                            else
                            {
                                rChoice[i].x = (int)Convert.ToSingle(w[2]);
                                rChoice[i].y = (int)Convert.ToSingle(w[3]);
                                // raw 데이터 출력에 이미 잘못되어서 Task 생성시 기본 환경 값을 토대로 바로 잡기로 함.
                                rChoice[i].w = (int)Convert.ToSingle(w[4]);
                                rChoice[i].h = (int)Convert.ToSingle(w[5]);
                            }
                        }

                        //문항유형과 제시자극, 선택순서 정보로 item 변수 생성
                        Item item = new Item(type, stimulus);
                        item.choice = choice;

                        //제시자극과 선택순서 좌표 정보 설정
                        item.rStimuls = rStimuls;
                        item.rChoice = rChoice;

                        //응답시간 인식
                        line = Setting.reader.ReadLine();
                        w = line.Split('\t');
                        rTime = Int32.Parse(w[1]);

                        //선택단어 인식
                        line = Setting.reader.ReadLine();
                        w = line.Split('\t');
                        dWord = w[1];

                        //응시시간 안내 줄 넘기기
                        line = Setting.reader.ReadLine();

                        //제시자극 응시시간 인식
                        line = Setting.reader.ReadLine();
                        w = line.Split('\t');
                        sTime = (int)Convert.ToSingle(w[1]);

                        //선택순서 응시시간 인식
                        if (type == 1)
                        {
                            gTime = new int[15];
                            for (int i = 0; i < 15; i++)
                            {
                                line = Setting.reader.ReadLine();
                                w = line.Split('\t');
                                gTime[i] = (int)Convert.ToSingle(w[1]);
                            }
                        }
                        else
                        {
                            gTime = new int[2];
                            for (int i = 0; i < 2; i++)
                            {
                                line = Setting.reader.ReadLine();
                                w = line.Split('\t');
                                gTime[i] = (int)Convert.ToSingle(w[1]);
                            }
                        }

                        //총 응시시간, 총 이탈시간, 총 이동비중 정보는 인식하지 않고 뛰어넘음
                        for (int i = 0; i < 3; i++)
                            line = Setting.reader.ReadLine();

                        //문항번호, 응답시간, 선택단어, 제시자극 응시시간, 선택순서 응시시간 정보 추가
                        item.pNum = pNum;
                        item.rTime = rTime;
                        item.dWord = dWord;
                        item.sTime = sTime;
                        item.gTime = gTime;

                        //시선응시 raw data line 인식
                        item.cList = new ArrayList();
                        while (!(line = Setting.reader.ReadLine()).Equals(""))
                        {
                            //시선이탈 정보 뛰어넘기 예외 처리
                            if (line.IndexOf("[") != -1)
                                continue;

                            w = line.Split('\t');
                            Cdot cdot = new Cdot(Convert.ToDouble(w[0]), Int32.Parse(w[1]), Int32.Parse(w[2]));
                            if (w.Length > 3)
                                cdot.word = w[3];
                            item.cList.Add(cdot);
                        }

                        Setting.taskList.Add(item);
                    }
                }

                Setting.reader.Close();

                if (fList != null)
                {
                    textBox1.Text = "1 / " + fList.Length;

                    for (int k = 1; k < fList.Length; k++)
                    {
                        int num = 0;
                        Setting.reader = new StreamReader(fList[k], Setting.encode);


                        while ((line = Setting.reader.ReadLine()) != null)
                        {
                            if (line.IndexOf("총 이동비중:") != -1)
                            {
                                Item item = (Item)Setting.taskList[num];

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

                                Setting.taskList[num] = item;
                                num++;
                            }
                        }


                        Setting.reader.Close();
                        textBox1.Text = (k + 1) + " / " + fList.Length;
                    }
                }

            }
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
