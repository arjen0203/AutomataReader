using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Automata_Reader
{
    public partial class Form1 : Form
    {
        Logic Logic = new Logic();
        RegexLogic RegexLogic = new RegexLogic();
        List<TestWord> savedTestWords = new List<TestWord>();
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

                Logic.CreateAutomatePicture(Logic.GetDotString());
                Logic.RunGraphicviz(automataPictureBox);
            
                if (Logic.isPDA())
                {
                    isPdaBox.Checked = true;
                    ConvertPDA.Enabled = true;
                    createRegexButton.Enabled = false;
                } 
                else
                {
                    chechAndConvertNFA();
                    isPdaBox.Checked = false;
                    ConvertPDA.Enabled = false;
                    createRegexButton.Enabled = true;
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
                processNewRegex(regexBox.Text);
            }
        }

        private void processNewRegex(string regex)
        {
            Logic.automata = RegexLogic.processRegex(regex);
            Logic.CreateAutomatePicture(Logic.GetDotString());
            Logic.RunGraphicviz(automataPictureBox);

            chechAndConvertNFA();
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

        private void ConvertNFAToRegex_Click(object sender, EventArgs e)
        {
            savedTestWords = Logic.automata.testWords.ConvertAll(word => new TestWord(word));
            (convertedRegexBox.Text, prefixRegexBox.Text) = Logic.ConvertNFAToRegEx();
            Logic.RunGraphicviz(automataPictureBox);
            testRegexButton.Enabled = true;
        }

        private void testRegexButton_Click(object sender, EventArgs e)
        {
            processNewRegex(prefixRegexBox.Text);
            string testOutput = "";
            foreach (TestWord testWord in savedTestWords)
            {
                testOutput += Logic.addTestWord(new string(testWord.word), testWord.accapted, DFACheckBox.Checked);
            }
            testBox.Text = testOutput;
        }
    }
}
