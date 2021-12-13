﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Automata_Reader
{
    public partial class Form1 : Form
    {
        Logic Logic = new Logic();
        RegexLogic RegexLogic = new RegexLogic();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ReadFileBtn_Click(object sender, EventArgs e)
        {

            string path = pathBox.Text;
            //try
            //{
                Logic.ReadLines(path);
                if (Logic.automata == null) return;

                Logic.CreateAutomatePicture();
                Logic.RunGraphicviz(automataPictureBox);
            
                if (Logic.isPDA())
                {
                    isPdaBox.Checked = true;
                    ConvertPDA.Enabled = true;
                } 
                else
                {
                    chechAndConvertNFA();
                isPdaBox.Checked = false;
                ConvertPDA.Enabled = false;
            }

                testBox.Text = Logic.TestOutputString(DFACheckBox.Checked, finiteBox.Checked, isPdaBox.Checked);

            //} catch
            //    {
            //        MessageBox.Show("Could not create automata");
            //    }
        }


        private void testWordButton_Click(object sender, EventArgs e)
        {
            testBox.Text += Logic.addTestWord(testWordBox.Text, testWordAcceptanceBox.Checked, DFACheckBox.Checked);
        }

        private void processRegex_Click(object sender, EventArgs e)
        {
            if (regexBox.Text.Length > 0)
            {
                Logic.automata = RegexLogic.processRegex(regexBox.Text);
                Logic.CreateAutomatePicture();
                Logic.RunGraphicviz(automataPictureBox);

                chechAndConvertNFA();
            }

        }

        public void chechAndConvertNFA()
        {
            DFACheckBox.Checked = Logic.GraphIsDFA();

            if (!DFACheckBox.Checked)
            {
                Logic.ConvertNFAtoDFA();
                Logic.CreateDFAAutomatePicture();
                Logic.RunGraphicvizDFA(DFAconvertBox);
                Logic.CreateDFAfile();
            }

            finiteBox.Checked = Logic.IsFinite(DFACheckBox.Checked);
        }

        private void ConvertPDA_Click(object sender, EventArgs e)
        {
            Logic.ConvertPDAToCFG();
        }

        private void OpenCFGFile_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\CFGoutput\\CFGoutput.txt");
        }
    }
}
