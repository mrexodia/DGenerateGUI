namespace DGenerateGUI
{
    partial class DGenerateGUI
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
            this.buttonStartProcess = new System.Windows.Forms.Button();
            this.buttonCheckDtrace = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.listBoxCalls = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.treeViewData = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartProcess
            // 
            this.buttonStartProcess.Location = new System.Drawing.Point(162, 12);
            this.buttonStartProcess.Name = "buttonStartProcess";
            this.buttonStartProcess.Size = new System.Drawing.Size(80, 23);
            this.buttonStartProcess.TabIndex = 1;
            this.buttonStartProcess.Text = "Start &Process";
            this.buttonStartProcess.UseVisualStyleBackColor = true;
            this.buttonStartProcess.Click += new System.EventHandler(this.buttonStartProcess_Click);
            // 
            // buttonCheckDtrace
            // 
            this.buttonCheckDtrace.Location = new System.Drawing.Point(12, 12);
            this.buttonCheckDtrace.Name = "buttonCheckDtrace";
            this.buttonCheckDtrace.Size = new System.Drawing.Size(144, 23);
            this.buttonCheckDtrace.TabIndex = 1;
            this.buttonCheckDtrace.Text = "Check DTrace Installation";
            this.buttonCheckDtrace.UseVisualStyleBackColor = true;
            this.buttonCheckDtrace.Click += new System.EventHandler(this.buttonCheckDtrace_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(248, 12);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // listBoxCalls
            // 
            this.listBoxCalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxCalls.FormattingEnabled = true;
            this.listBoxCalls.IntegralHeight = false;
            this.listBoxCalls.Location = new System.Drawing.Point(0, 26);
            this.listBoxCalls.Name = "listBoxCalls";
            this.listBoxCalls.Size = new System.Drawing.Size(254, 436);
            this.listBoxCalls.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(254, 20);
            this.textBox1.TabIndex = 3;
            // 
            // treeViewData
            // 
            this.treeViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewData.Location = new System.Drawing.Point(3, 26);
            this.treeViewData.Name = "treeViewData";
            this.treeViewData.Size = new System.Drawing.Size(506, 436);
            this.treeViewData.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 41);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxCalls);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Panel2.Controls.Add(this.treeViewData);
            this.splitContainer1.Size = new System.Drawing.Size(773, 465);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(3, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(506, 20);
            this.textBox2.TabIndex = 4;
            // 
            // DGenerateGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 514);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonCheckDtrace);
            this.Controls.Add(this.buttonStartProcess);
            this.Name = "DGenerateGUI";
            this.Text = "D-Generate";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartProcess;
        private System.Windows.Forms.Button buttonCheckDtrace;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.ListBox listBoxCalls;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TreeView treeViewData;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

