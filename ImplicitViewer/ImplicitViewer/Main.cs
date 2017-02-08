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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openPanel.Filter = "Raw Data (*raw.txt)|*raw.txt|All files (*.*)|*.*";
            if (openPanel.ShowDialog() == DialogResult.OK)
                textBox1.Text = openPanel.SafeFileName;
            else
                textBox1.Text = "None";
        }
    }
}
