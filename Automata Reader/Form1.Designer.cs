namespace Automata_Reader
{
    partial class Form1
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
            this.ReadFileBtn = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DFACheckBox = new System.Windows.Forms.CheckBox();
            this.automataPictureBox = new System.Windows.Forms.PictureBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DFAconvertBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.testBox = new System.Windows.Forms.TextBox();
            this.finiteBox = new System.Windows.Forms.CheckBox();
            this.testWordButton = new System.Windows.Forms.Button();
            this.testWordBox = new System.Windows.Forms.TextBox();
            this.testWordAcceptanceBox = new System.Windows.Forms.CheckBox();
            this.isPdaBox = new System.Windows.Forms.CheckBox();
            this.isNfaBox = new System.Windows.Forms.CheckBox();
            this.regexBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.processRegex = new System.Windows.Forms.Button();
            this.ConvertPDA = new System.Windows.Forms.Button();
            this.OpenCFGFile = new System.Windows.Forms.Button();
            this.createRegexButton = new System.Windows.Forms.Button();
            this.convertedRegexBox = new System.Windows.Forms.TextBox();
            this.testRegexButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.prefixRegexBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.automataPictureBox)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DFAconvertBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ReadFileBtn
            // 
            this.ReadFileBtn.Location = new System.Drawing.Point(303, 23);
            this.ReadFileBtn.Margin = new System.Windows.Forms.Padding(2);
            this.ReadFileBtn.Name = "ReadFileBtn";
            this.ReadFileBtn.Size = new System.Drawing.Size(56, 19);
            this.ReadFileBtn.TabIndex = 0;
            this.ReadFileBtn.Text = "Read file";
            this.ReadFileBtn.UseVisualStyleBackColor = true;
            this.ReadFileBtn.Click += new System.EventHandler(this.ReadFileBtn_Click);
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(38, 24);
            this.pathBox.Margin = new System.Windows.Forms.Padding(2);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(261, 20);
            this.pathBox.TabIndex = 1;
            this.pathBox.Text = "regexAutomata";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input file path:";
            // 
            // DFACheckBox
            // 
            this.DFACheckBox.AutoCheck = false;
            this.DFACheckBox.AutoSize = true;
            this.DFACheckBox.Location = new System.Drawing.Point(872, 90);
            this.DFACheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.DFACheckBox.Name = "DFACheckBox";
            this.DFACheckBox.Size = new System.Drawing.Size(47, 17);
            this.DFACheckBox.TabIndex = 3;
            this.DFACheckBox.Text = "DFA";
            this.DFACheckBox.UseVisualStyleBackColor = true;
            // 
            // automataPictureBox
            // 
            this.automataPictureBox.Location = new System.Drawing.Point(-10, 5);
            this.automataPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.automataPictureBox.Name = "automataPictureBox";
            this.automataPictureBox.Size = new System.Drawing.Size(817, 558);
            this.automataPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.automataPictureBox.TabIndex = 4;
            this.automataPictureBox.TabStop = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(36, 107);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(811, 589);
            this.tabControl.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.automataPictureBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(803, 563);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Normal";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DFAconvertBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(803, 563);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Converted";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DFAconvertBox
            // 
            this.DFAconvertBox.Location = new System.Drawing.Point(5, 2);
            this.DFAconvertBox.Margin = new System.Windows.Forms.Padding(2);
            this.DFAconvertBox.Name = "DFAconvertBox";
            this.DFAconvertBox.Size = new System.Drawing.Size(798, 561);
            this.DFAconvertBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DFAconvertBox.TabIndex = 5;
            this.DFAconvertBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(869, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tests:";
            // 
            // testBox
            // 
            this.testBox.BackColor = System.Drawing.SystemColors.Window;
            this.testBox.Location = new System.Drawing.Point(868, 275);
            this.testBox.Multiline = true;
            this.testBox.Name = "testBox";
            this.testBox.ReadOnly = true;
            this.testBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.testBox.Size = new System.Drawing.Size(205, 369);
            this.testBox.TabIndex = 7;
            // 
            // finiteBox
            // 
            this.finiteBox.AutoCheck = false;
            this.finiteBox.AutoSize = true;
            this.finiteBox.Location = new System.Drawing.Point(1022, 90);
            this.finiteBox.Name = "finiteBox";
            this.finiteBox.Size = new System.Drawing.Size(51, 17);
            this.finiteBox.TabIndex = 5;
            this.finiteBox.Text = "Finite";
            this.finiteBox.UseVisualStyleBackColor = true;
            // 
            // testWordButton
            // 
            this.testWordButton.Location = new System.Drawing.Point(999, 650);
            this.testWordButton.Name = "testWordButton";
            this.testWordButton.Size = new System.Drawing.Size(75, 42);
            this.testWordButton.TabIndex = 8;
            this.testWordButton.Text = "Test word";
            this.testWordButton.UseVisualStyleBackColor = true;
            this.testWordButton.Click += new System.EventHandler(this.testWordButton_Click);
            // 
            // testWordBox
            // 
            this.testWordBox.Location = new System.Drawing.Point(869, 651);
            this.testWordBox.Name = "testWordBox";
            this.testWordBox.Size = new System.Drawing.Size(124, 20);
            this.testWordBox.TabIndex = 9;
            // 
            // testWordAcceptanceBox
            // 
            this.testWordAcceptanceBox.AutoSize = true;
            this.testWordAcceptanceBox.Location = new System.Drawing.Point(868, 675);
            this.testWordAcceptanceBox.Name = "testWordAcceptanceBox";
            this.testWordAcceptanceBox.Size = new System.Drawing.Size(78, 17);
            this.testWordAcceptanceBox.TabIndex = 10;
            this.testWordAcceptanceBox.Text = "Accepted?";
            this.testWordAcceptanceBox.UseVisualStyleBackColor = true;
            // 
            // isPdaBox
            // 
            this.isPdaBox.AutoCheck = false;
            this.isPdaBox.AutoSize = true;
            this.isPdaBox.Location = new System.Drawing.Point(868, 32);
            this.isPdaBox.Name = "isPdaBox";
            this.isPdaBox.Size = new System.Drawing.Size(48, 17);
            this.isPdaBox.TabIndex = 11;
            this.isPdaBox.Text = "PDA";
            this.isPdaBox.UseVisualStyleBackColor = true;
            // 
            // isNfaBox
            // 
            this.isNfaBox.AutoCheck = false;
            this.isNfaBox.AutoSize = true;
            this.isNfaBox.Location = new System.Drawing.Point(946, 90);
            this.isNfaBox.Margin = new System.Windows.Forms.Padding(2);
            this.isNfaBox.Name = "isNfaBox";
            this.isNfaBox.Size = new System.Drawing.Size(47, 17);
            this.isNfaBox.TabIndex = 12;
            this.isNfaBox.Text = "NFA";
            this.isNfaBox.UseVisualStyleBackColor = true;
            // 
            // regexBox
            // 
            this.regexBox.Location = new System.Drawing.Point(40, 74);
            this.regexBox.Name = "regexBox";
            this.regexBox.Size = new System.Drawing.Size(261, 20);
            this.regexBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Input regex:";
            // 
            // processRegex
            // 
            this.processRegex.Location = new System.Drawing.Point(303, 73);
            this.processRegex.Name = "processRegex";
            this.processRegex.Size = new System.Drawing.Size(96, 21);
            this.processRegex.TabIndex = 15;
            this.processRegex.Text = "Create NFA/DFA";
            this.processRegex.UseVisualStyleBackColor = true;
            this.processRegex.Click += new System.EventHandler(this.processRegex_Click);
            // 
            // ConvertPDA
            // 
            this.ConvertPDA.Enabled = false;
            this.ConvertPDA.Location = new System.Drawing.Point(937, 28);
            this.ConvertPDA.Name = "ConvertPDA";
            this.ConvertPDA.Size = new System.Drawing.Size(112, 23);
            this.ConvertPDA.TabIndex = 16;
            this.ConvertPDA.Text = "Convert to CFG";
            this.ConvertPDA.UseVisualStyleBackColor = true;
            this.ConvertPDA.Click += new System.EventHandler(this.ConvertPDA_Click);
            // 
            // OpenCFGFile
            // 
            this.OpenCFGFile.Location = new System.Drawing.Point(937, 58);
            this.OpenCFGFile.Name = "OpenCFGFile";
            this.OpenCFGFile.Size = new System.Drawing.Size(112, 23);
            this.OpenCFGFile.TabIndex = 17;
            this.OpenCFGFile.Text = "Open CFG";
            this.OpenCFGFile.UseVisualStyleBackColor = true;
            this.OpenCFGFile.Click += new System.EventHandler(this.OpenCFGFile_Click);
            // 
            // createRegexButton
            // 
            this.createRegexButton.Enabled = false;
            this.createRegexButton.Location = new System.Drawing.Point(868, 134);
            this.createRegexButton.Name = "createRegexButton";
            this.createRegexButton.Size = new System.Drawing.Size(94, 23);
            this.createRegexButton.TabIndex = 18;
            this.createRegexButton.Text = "Create regex";
            this.createRegexButton.UseVisualStyleBackColor = true;
            this.createRegexButton.Click += new System.EventHandler(this.ConvertNFAToRegex_Click);
            // 
            // convertedRegexBox
            // 
            this.convertedRegexBox.BackColor = System.Drawing.SystemColors.Window;
            this.convertedRegexBox.Location = new System.Drawing.Point(868, 176);
            this.convertedRegexBox.Name = "convertedRegexBox";
            this.convertedRegexBox.ReadOnly = true;
            this.convertedRegexBox.Size = new System.Drawing.Size(205, 20);
            this.convertedRegexBox.TabIndex = 19;
            // 
            // testRegexButton
            // 
            this.testRegexButton.Enabled = false;
            this.testRegexButton.Location = new System.Drawing.Point(969, 134);
            this.testRegexButton.Name = "testRegexButton";
            this.testRegexButton.Size = new System.Drawing.Size(105, 23);
            this.testRegexButton.TabIndex = 20;
            this.testRegexButton.Text = "Test Regex";
            this.testRegexButton.UseVisualStyleBackColor = true;
            this.testRegexButton.Click += new System.EventHandler(this.testRegexButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(869, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Infix:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(869, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Prefix:";
            // 
            // prefixRegexBox
            // 
            this.prefixRegexBox.BackColor = System.Drawing.SystemColors.Window;
            this.prefixRegexBox.Location = new System.Drawing.Point(868, 216);
            this.prefixRegexBox.Name = "prefixRegexBox";
            this.prefixRegexBox.ReadOnly = true;
            this.prefixRegexBox.Size = new System.Drawing.Size(205, 20);
            this.prefixRegexBox.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 708);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.prefixRegexBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.testRegexButton);
            this.Controls.Add(this.convertedRegexBox);
            this.Controls.Add(this.createRegexButton);
            this.Controls.Add(this.OpenCFGFile);
            this.Controls.Add(this.ConvertPDA);
            this.Controls.Add(this.processRegex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.regexBox);
            this.Controls.Add(this.isNfaBox);
            this.Controls.Add(this.isPdaBox);
            this.Controls.Add(this.testWordAcceptanceBox);
            this.Controls.Add(this.testWordBox);
            this.Controls.Add(this.testWordButton);
            this.Controls.Add(this.finiteBox);
            this.Controls.Add(this.testBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.DFACheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.ReadFileBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.automataPictureBox)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DFAconvertBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadFileBtn;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DFACheckBox;
        private System.Windows.Forms.PictureBox automataPictureBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox DFAconvertBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox testBox;
        private System.Windows.Forms.CheckBox finiteBox;
        private System.Windows.Forms.Button testWordButton;
        private System.Windows.Forms.TextBox testWordBox;
        private System.Windows.Forms.CheckBox testWordAcceptanceBox;
        private System.Windows.Forms.CheckBox isPdaBox;
        private System.Windows.Forms.CheckBox isNfaBox;
        private System.Windows.Forms.TextBox regexBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button processRegex;
        private System.Windows.Forms.Button ConvertPDA;
        private System.Windows.Forms.Button OpenCFGFile;
        private System.Windows.Forms.Button createRegexButton;
        private System.Windows.Forms.TextBox convertedRegexBox;
        private System.Windows.Forms.Button testRegexButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox prefixRegexBox;
    }
}

