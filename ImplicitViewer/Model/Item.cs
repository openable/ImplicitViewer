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
        public int type;
        public string stimulus;
        public string[] choice;
        public ArrayList cList;

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
