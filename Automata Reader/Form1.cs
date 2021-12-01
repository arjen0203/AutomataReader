using System;
using System.Windows.Forms;

namespace Automata_Reader
{
    public partial class Form1 : Form
    {
        Logic Logic = new Logic();

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
                Logic.CreateAutomatePicture();
                Logic.RunGraphicviz(automataPictureBox);
            
                if (Logic.isPDA())
                {
                    isPdaBox.Checked = true;
                } 
                else
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
            
        }
    }
}
