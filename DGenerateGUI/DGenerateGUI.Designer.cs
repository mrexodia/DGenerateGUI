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
            // DGenerateGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonCheckDtrace);
            this.Controls.Add(this.buttonStartProcess);
            this.Name = "DGenerateGUI";
            this.Text = "D-Generate";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartProcess;
        private System.Windows.Forms.Button buttonCheckDtrace;
        private System.Windows.Forms.Button buttonTest;
    }
}

