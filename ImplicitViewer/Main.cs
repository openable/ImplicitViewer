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
        public string path;
        public string file;
        public string[] fList;
        public string fullPath;

        public Task1 task1;
        public Task2 task2;
        public int current;

        private int mx;     // modify x coordinate
        private int my;     // modify y coordinate
        private int mwl;    // modify width left coordinate
        private int mhu;    // modify height up coordinate
        private int mwr;    // modify width right coordinate
        private int mhd;    // modify height down coordinate

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
                    textBox1.Text = file = openPanel.SafeFileName;
                    textBox1.Tag = fullPath = openPanel.FileName;
                    path = fullPath.Substring(0, (fullPath.Length - file.Length));
                    fList = null;
                }
                else
                {
                    textBox1.Text = "0 / " + openPanel.FileNames.Length;
                    path = openPanel.FileName.Substring(0, (openPanel.FileName.Length - openPanel.SafeFileName.Length));
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
                this.current = Setting.taskList.Count - 1;
                showTask();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int? cnt = Setting.taskList?.Count;
            if (cnt == null)
            {
                MessageBox.Show("업데이트 할 내역이 없습니다.", "오류", MessageBoxButtons.OK);
                return;
            }

            string newPath = path + "NewGaze\\";
            DirectoryInfo di = new DirectoryInfo(newPath);
            if (di.Exists == false)
                di.Create();

            Setting.csvFile = new StringBuilder();
            Setting.rawFile = new StringBuilder();

            Setting.csvFile.AppendLine("피험자 ID,문항번호,제시자극,응답시간,선택단어,단어,Eye Track time (ms)");
            for (int i = 0; i < Setting.taskList.Count; i++)
            {
                Item item = (Item)Setting.taskList[i];

                for (int j = 0; j < item.choice.Length; j++)
                {
                    Setting.csvFile.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                        Setting.ID, item.pNum, item.stimulus.Replace("\r\n", " ").Replace(",", " +"),
                        item.rTime, item.dWord,
                        item.choice[j].Replace("\r\n", " ").Replace(",", " +"), item.gTime[j]));
                }
            }

            Setting.writer = new StreamWriter(newPath + file.Substring(0, (file.Length - 7)) + "data.csv", true, Setting.encode);
            Setting.writer.Write(Setting.csvFile);

            Setting.writer.Close();

            MessageBox.Show("업데이트 파일을 생성하였습니다.", "확인", MessageBoxButtons.OK);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Tag.Equals("") && fList == null)
            {
                MessageBox.Show("데이터 파일을 선택해 주세요.", "오류", MessageBoxButtons.OK);
                return;
            }

            if (textBox2.Text.Equals("X") || textBox2.Text.Equals(""))
                textBox2.Text = "0";
            if (textBox3.Text.Equals("Y") || textBox3.Text.Equals(""))
                textBox3.Text = "0";
            if (textBox4.Text.Equals("WL") || textBox4.Text.Equals(""))
                textBox4.Text = "0";
            if (textBox5.Text.Equals("HU") || textBox5.Text.Equals(""))
                textBox5.Text = "0";
            if (textBox6.Text.Equals("WR") || textBox6.Text.Equals(""))
                textBox6.Text = "0";
            if (textBox7.Text.Equals("HD") || textBox7.Text.Equals(""))
                textBox7.Text = "0";

            mx = Convert.ToInt32(textBox2.Text);
            my = Convert.ToInt32(textBox3.Text);
            mwl = Convert.ToInt32(textBox4.Text);
            mhu = Convert.ToInt32(textBox5.Text);
            mwr = Convert.ToInt32(textBox6.Text);
            mhd = Convert.ToInt32(textBox7.Text);

            bulkReader();

            mx = 0;
            my = 0;
            mwl = 0;
            mhu = 0;
            mwr = 0;
            mhd = 0;

            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";

            MessageBox.Show("일괄 업데이트를 수정하였습니다.", "확인", MessageBoxButtons.OK);
        }

        private void bulkReader()
        {
            if (fList == null)
                fList = new string[1] { fullPath };

            Setting.encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
            for (int f = 0; f < fList.Length; f++)
            {
                Setting.path = fList[f];
                Setting.reader = new StreamReader(Setting.path, Setting.encode);

                string[] w;
                string line = Setting.reader.ReadLine();
                if (line.IndexOf("[실험 시작]") == -1)
                {
                    MessageBox.Show("옳지 않은 데이터 파일 입니다.", "오류", MessageBoxButtons.OK);
                    Setting.reader.Close();
                    continue;
                }
                else
                {
                    w = line.Split('\t');

                    Setting.ID = w[1];
                    Setting.SCREEN_WIDTH = Int32.Parse(w[3]);
                    Setting.SCREEN_HEIGHT = Int32.Parse(w[4]);

                    Setting.reader.Close();
                }

                if (f == 0)
                    setCoordinate();

                Setting.taskList = new ArrayList();
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

                bulkUpdate();
//                bulkWriter();
                textBox1.Text = (f + 1) + " / " + fList.Length;
            }
        }

        private void bulkUpdate()
        {
            for (int t = 0; t < Setting.taskList.Count; t++)
            {
                Item item = (Item)Setting.taskList[t];

                Word stimulus;
                Word[] words = new Word[item.choice.Length];

                // Stimulus 초기화
                if (item.stimulus.Contains("s.png"))
                {
                    stimulus = new Word(item.stimulus, true, true);
                    stimulus.Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.stimulus);
                    stimulus.SetBounds((int)(Setting.SCREEN_WIDTH / 2 - 105), (int)Setting.margin.Y, 210, 280);

                    if (item.sizeSet)
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

                    if (item.sizeSet)
                    {
                        item.rStimuls.w = (int)(300 + (2 * Setting.xBuffer));
                        item.rStimuls.h = (int)(400 + (2 * Setting.yBuffer));
                    }
                }
                else
                {
                    stimulus = new Word(item.stimulus, false, true);
                    stimulus.SetBounds((int)Setting.cStimulus.X, (int)Setting.cStimulus.Y, (int)Setting.sStimulus.X, (int)Setting.sStimulus.Y);

                    if (item.sizeSet)
                    {
                        item.rStimuls.w = (int)(Setting.sStimulus.X + (2 * Setting.xBuffer));
                        item.rStimuls.h = (int)(Setting.sStimulus.Y + (2 * Setting.yBuffer));
                    }
                }

                // choice word 초기화
                if (item.choice.Length == 15)
                {
                    for (int i = 0; i < item.choice.Length; i++)
                    {
                        words[i] = new Word(item.choice[i], false, false);
                        words[i].SetBounds((int)Setting.cWord[i].X, (int)Setting.cWord[i].Y, (int)Setting.sWord.X, (int)Setting.sWord.Y);
                        if (item.sizeSet)
                        {
                            item.rChoice[i].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                            item.rChoice[i].h = (int)(Setting.sWord.Y + (2 * Setting.yBuffer));
                        }
                    }

                }
                else if (item.choice.Length == 2)
                {
                    if (item.choice[0].Contains("s.png"))
                    {
                        words[0] = new Word(item.choice[0], true, false);
                        words[0].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[0]);
                        words[0].SetBounds((int)(Setting.margin.X + Setting.xInterval * 5),
                                            (int)Setting.cWord[0].Y,
                                            210, 280);

                        if (item.sizeSet)
                        {
                            item.rChoice[0].w = (int)(210 + (2 * Setting.xBuffer));
                            item.rChoice[0].h = (int)(280 + (2 * Setting.yBuffer));
                        }
                    }
                    else if (item.choice[0].Contains(".png"))
                    {
                        words[0] = new Word(item.choice[0], true, false);
                        words[0].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[0]);
                        words[0].SetBounds((int)(Setting.margin.X + Setting.xInterval * 5),
                                            (int)Setting.cWord[0].Y,
                                            300, 400);

                        if (item.sizeSet)
                        {
                            item.rChoice[0].w = (int)(300 + (2 * Setting.xBuffer));
                            item.rChoice[0].h = (int)(400 + (2 * Setting.yBuffer));
                        }
                    }
                    else
                    {
                        words[0] = new Word(item.choice[0], false, false);
                        words[0].SetBounds((int)(Setting.cWord[5].X + Setting.xInterval * 4),
                                            (int)Setting.cWord[5].Y,
                                            (int)Setting.sWord.X,
                                            (int)(Setting.sWord.Y * 1.5));

                        if (item.sizeSet)
                        {
                            item.rChoice[0].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                            item.rChoice[0].h = (int)((Setting.sWord.Y * 1.5) + (2 * Setting.yBuffer));
                        }
                    }

                    if (item.choice[1].Contains("s.png"))
                    {
                        words[1] = new Word(item.choice[1], true, false);
                        words[1].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[1]);
                        words[1].SetBounds((int)(Setting.SCREEN_WIDTH - (Setting.margin.X + Setting.xInterval * 5 + 210)),
                                            (int)Setting.cWord[4].Y,
                                            210, 280);

                        if (item.sizeSet)
                        {
                            item.rChoice[1].w = (int)(210 + (2 * Setting.xBuffer));
                            item.rChoice[1].h = (int)(280 + (2 * Setting.yBuffer));
                        }
                    }
                    else if (item.choice[1].Contains(".png"))
                    {
                        words[1] = new Word(item.choice[1], true, false);
                        words[1].Image = Image.FromFile(Application.StartupPath + "\\model\\" + item.choice[1]);
                        words[1].SetBounds((int)(Setting.SCREEN_WIDTH - (Setting.margin.X + Setting.xInterval * 5 + 300)),
                                            (int)Setting.cWord[4].Y,
                                            300, 400);
                        if (item.sizeSet)
                        {
                            item.rChoice[1].w = (int)(300 + (2 * Setting.xBuffer));
                            item.rChoice[1].h = (int)(400 + (2 * Setting.yBuffer));
                        }
                    }
                    else
                    {
                        words[1] = new Word(item.choice[1], false, false);
                        words[1].SetBounds((int)(Setting.cWord[9].X - Setting.xInterval * 4),
                                            (int)Setting.cWord[9].Y,
                                            (int)Setting.sWord.X,
                                            (int)(Setting.sWord.Y * 1.5));
                        if (item.sizeSet)
                        {
                            item.rChoice[1].w = (int)(Setting.sWord.X + (2 * Setting.xBuffer));
                            item.rChoice[1].h = (int)((Setting.sWord.Y * 1.5) + (2 * Setting.yBuffer));
                        }
                    }
                }

                // stimulus 범위 좌표 일괄 반영
                stimulus.nB.x = item.rStimuls.x + mx - mwl;
                stimulus.nB.y = item.rStimuls.y + my - mhu;
                stimulus.nB.w = mwl + item.rStimuls.w + mwr;
                stimulus.nB.h = mhu + item.rStimuls.h + mhd;

                // choice word 범위 좌표 일괄 반영
                for (int i = 0; i < item.choice.Length; i++)
                {
                    words[i].nB.x = item.rChoice[i].x + mx - mwl;
                    words[i].nB.y = item.rChoice[i].y + my - mhu;
                    words[i].nB.w = mwl + item.rChoice[i].w + mwr;
                    words[i].nB.h = mhu + item.rChoice[i].h + mhd;
                }

                // simulus와 choice word에 속하는지 여부 체크하고 gazeTime 누적
                foreach (Cdot c in item.cList)
                {
                    if (stimulus.newHit(c.time, c.x, c.y))
                    {
                        continue;
                    }

                    foreach (Word w in words)
                    {
                        if (w.newHit(c.time, c.x, c.y))
                        {
                            break;
                        }
                    }
                }

                // stimulus 일괄 변경된 범위 좌표와 응시시간 item 객체에 반영
                item.rStimuls.x = stimulus.nB.x;
                item.rStimuls.y = stimulus.nB.y;
                item.rStimuls.w = stimulus.nB.w;
                item.rStimuls.h = stimulus.nB.h;
                item.sTime = (int)stimulus.gazeTime;

                // choice word 일괄 변경된 범위 좌표와 응시시간 item 객체에 반영
                for (int i = 0; i < item.choice.Length; i++)
                {
                    item.rChoice[i].x = words[i].nB.x;
                    item.rChoice[i].y = words[i].nB.y;
                    item.rChoice[i].w = words[i].nB.w;
                    item.rChoice[i].h = words[i].nB.h;
                    item.gTime[i] = (int)words[i].gazeTime;
                }

                item.sizeSet = false;
            }
        }

        private void bulkWriter(string file)
        {
            string newPath = path + "NewGaze\\";
            DirectoryInfo di = new DirectoryInfo(newPath);
            if (di.Exists == false)
                di.Create();

            Setting.csvFile = new StringBuilder();
            Setting.rawFile = new StringBuilder();

            Setting.csvFile.AppendLine("피험자 ID,문항번호,제시자극,응답시간,선택단어,단어,Eye Track time (ms)");
            for (int i = 0; i < Setting.taskList.Count; i++)
            {
                Item item = (Item)Setting.taskList[i];

                for (int j = 0; j < item.choice.Length; j++)
                {
                    Setting.csvFile.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                        Setting.ID, item.pNum, item.stimulus.Replace("\r\n", " ").Replace(",", " +"),
                        item.rTime, item.dWord,
                        item.choice[j].Replace("\r\n", " ").Replace(",", " +"), item.gTime[j]));
                }
            }

            Setting.writer = new StreamWriter(newPath + file.Substring(0, (file.Length - 7)) + "data.csv", true, Setting.encode);
            Setting.writer.Write(Setting.csvFile);

            Setting.writer.Close();
        }
    }
}
