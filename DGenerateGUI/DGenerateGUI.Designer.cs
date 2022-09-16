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
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.listBoxCalls = new System.Windows.Forms.ListBox();
            this.textBoxFilterCalls = new System.Windows.Forms.TextBox();
            this.treeViewData = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxFilterData = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartProcess
            // 
            this.buttonStartProcess.Location = new System.Drawing.Point(93, 12);
            this.buttonStartProcess.Name = "buttonStartProcess";
            this.buttonStartProcess.Size = new System.Drawing.Size(80, 23);
            this.buttonStartProcess.TabIndex = 1;
            this.buttonStartProcess.Text = "Start &Process";
            this.buttonStartProcess.UseVisualStyleBackColor = true;
            this.buttonStartProcess.Click += new System.EventHandler(this.buttonStartProcess_Click);
            // 
            // buttonCheckDtrace
            // 
            this.buttonCheckDtrace.Location = new System.Drawing.Point(179, 12);
            this.buttonCheckDtrace.Name = "buttonCheckDtrace";
            this.buttonCheckDtrace.Size = new System.Drawing.Size(144, 23);
            this.buttonCheckDtrace.TabIndex = 2;
            this.buttonCheckDtrace.Text = "Check DTrace &Installation";
            this.buttonCheckDtrace.UseVisualStyleBackColor = true;
            this.buttonCheckDtrace.Click += new System.EventHandler(this.buttonCheckDtrace_Click);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(12, 12);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadFile.TabIndex = 0;
            this.buttonLoadFile.Text = "Load &File";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // listBoxCalls
            // 
            this.listBoxCalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxCalls.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxCalls.FormattingEnabled = true;
            this.listBoxCalls.IntegralHeight = false;
            this.listBoxCalls.Location = new System.Drawing.Point(0, 26);
            this.listBoxCalls.Name = "listBoxCalls";
            this.listBoxCalls.Size = new System.Drawing.Size(384, 513);
            this.listBoxCalls.TabIndex = 4;
            // 
            // textBoxFilterCalls
            // 
            this.textBoxFilterCalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilterCalls.Location = new System.Drawing.Point(0, 0);
            this.textBoxFilterCalls.Name = "textBoxFilterCalls";
            this.textBoxFilterCalls.Size = new System.Drawing.Size(384, 20);
            this.textBoxFilterCalls.TabIndex = 3;
            // 
            // treeViewData
            // 
            this.treeViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewData.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewData.Location = new System.Drawing.Point(3, 26);
            this.treeViewData.Name = "treeViewData";
            this.treeViewData.Size = new System.Drawing.Size(770, 513);
            this.treeViewData.TabIndex = 6;
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
            this.splitContainer1.Panel1.Controls.Add(this.textBoxFilterCalls);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFilterData);
            this.splitContainer1.Panel2.Controls.Add(this.treeViewData);
            this.splitContainer1.Size = new System.Drawing.Size(1167, 542);
            this.splitContainer1.SplitterDistance = 387;
            this.splitContainer1.TabIndex = 5;
            // 
            // textBoxFilterData
            // 
            this.textBoxFilterData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilterData.Location = new System.Drawing.Point(3, 0);
            this.textBoxFilterData.Name = "textBoxFilterData";
            this.textBoxFilterData.Size = new System.Drawing.Size(770, 20);
            this.textBoxFilterData.TabIndex = 5;
            // 
            // DGenerateGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 591);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonLoadFile);
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
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.ListBox listBoxCalls;
        private System.Windows.Forms.TextBox textBoxFilterCalls;
        private System.Windows.Forms.TreeView treeViewData;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxFilterData;
    }
}

