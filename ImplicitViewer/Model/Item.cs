using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitViewer.Model
{
    class Item
    {
        public int type;        //문항유형
        public string stimulus; //자극단어
        public string[] choice; //선택단어 목록
        public ArrayList cList; //시선좌표 raw line data, 점 찍을 수 있게

        public int pNum;        //문항번호
        public int rTime;       //응답시간
        public int sTime;       //자극단어 응시시간
        public string dWord;    //선택단어
        public int[] gTime;     //선택단어 응시시간 저장용

        public Item()
        {
            type = 0;
            stimulus = "";
            choice = null;
        }

        public Item(int type, string stimulus)
        {
            this.type = type;
            this.stimulus = stimulus;
        }

        public void shuffle()
        {
            int r1;
            int r2;
            string temp;
            Random r = new Random();

            for (int i = 0; i < choice.Length; i++)
            {
                r1 = r.Next(0, choice.Length);
                r2 = r.Next(0, choice.Length);

                temp = choice[r1];
                choice[r1] = choice[r2];
                choice[r2] = temp;
            }
        }

        public void reverse()
        {
            string temp;

            for (int i = 0; i < choice.Length / 2; i++)
            {
                temp = choice[i];
                choice[i] = choice[choice.Length - i - 1];
                choice[choice.Length - i - 1] = temp;
            }
        }
    }
}
