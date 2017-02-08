namespace ImplicitViewer
{
    partial class Task2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.taskNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // taskNum
            // 
            this.taskNum.AutoSize = true;
            this.taskNum.BackColor = System.Drawing.Color.LightGray;
            this.taskNum.Location = new System.Drawing.Point(23, 19);
            this.taskNum.Name = "taskNum";
            this.taskNum.Size = new System.Drawing.Size(29, 12);
            this.taskNum.TabIndex = 0;
            this.taskNum.Text = "문항";
            // 
            // Task2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.taskNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Task2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Task1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label taskNum;
    }
}