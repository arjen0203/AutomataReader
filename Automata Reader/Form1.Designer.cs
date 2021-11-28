﻿namespace Automata_Reader
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
            this.pathBox.Text = "C:\\Users\\arjen\\Documents\\Fontys\\GitKraken\\AutomataReader\\Automata input files\\exa" +
    "mplePDA.txt";
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
            this.DFACheckBox.Location = new System.Drawing.Point(646, 14);
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
            this.automataPictureBox.Size = new System.Drawing.Size(578, 367);
            this.automataPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.automataPictureBox.TabIndex = 4;
            this.automataPictureBox.TabStop = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(37, 69);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(581, 398);
            this.tabControl.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.automataPictureBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(573, 372);
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
            this.tabPage2.Size = new System.Drawing.Size(573, 372);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Converted";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DFAconvertBox
            // 
            this.DFAconvertBox.Location = new System.Drawing.Point(5, 2);
            this.DFAconvertBox.Margin = new System.Windows.Forms.Padding(2);
            this.DFAconvertBox.Name = "DFAconvertBox";
            this.DFAconvertBox.Size = new System.Drawing.Size(566, 374);
            this.DFAconvertBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DFAconvertBox.TabIndex = 5;
            this.DFAconvertBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(643, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tests:";
            // 
            // testBox
            // 
            this.testBox.Location = new System.Drawing.Point(646, 91);
            this.testBox.Multiline = true;
            this.testBox.Name = "testBox";
            this.testBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.testBox.Size = new System.Drawing.Size(205, 324);
            this.testBox.TabIndex = 7;
            // 
            // finiteBox
            // 
            this.finiteBox.AutoCheck = false;
            this.finiteBox.AutoSize = true;
            this.finiteBox.Location = new System.Drawing.Point(719, 14);
            this.finiteBox.Name = "finiteBox";
            this.finiteBox.Size = new System.Drawing.Size(51, 17);
            this.finiteBox.TabIndex = 5;
            this.finiteBox.Text = "Finite";
            this.finiteBox.UseVisualStyleBackColor = true;
            // 
            // testWordButton
            // 
            this.testWordButton.Location = new System.Drawing.Point(776, 421);
            this.testWordButton.Name = "testWordButton";
            this.testWordButton.Size = new System.Drawing.Size(75, 42);
            this.testWordButton.TabIndex = 8;
            this.testWordButton.Text = "Test word";
            this.testWordButton.UseVisualStyleBackColor = true;
            this.testWordButton.Click += new System.EventHandler(this.testWordButton_Click);
            // 
            // testWordBox
            // 
            this.testWordBox.Location = new System.Drawing.Point(646, 421);
            this.testWordBox.Name = "testWordBox";
            this.testWordBox.Size = new System.Drawing.Size(124, 20);
            this.testWordBox.TabIndex = 9;
            // 
            // testWordAcceptanceBox
            // 
            this.testWordAcceptanceBox.AutoSize = true;
            this.testWordAcceptanceBox.Location = new System.Drawing.Point(646, 446);
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
            this.isPdaBox.Location = new System.Drawing.Point(646, 49);
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
            this.isNfaBox.Location = new System.Drawing.Point(646, 31);
            this.isNfaBox.Margin = new System.Windows.Forms.Padding(2);
            this.isNfaBox.Name = "isNfaBox";
            this.isNfaBox.Size = new System.Drawing.Size(47, 17);
            this.isNfaBox.TabIndex = 12;
            this.isNfaBox.Text = "NFA";
            this.isNfaBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 479);
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
    }
}

