using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImplicitViewer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
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
                MessageBox.Show("데이터 파일을 선택해 주세요", "오류", MessageBoxButtons.OK);
            else
                showResult();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showResult() { }
    }
}
