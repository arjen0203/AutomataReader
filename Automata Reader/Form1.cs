using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try
            {
                Logic.ReadLines(path);
                Logic.CreateAutomatePicture();
                DFACheckBox.Checked = Logic.GraphIsDFA();
                Logic.RunGraphicviz(automataPictureBox);

                if (!DFACheckBox.Checked)
                {
                    Logic.ConvertNFAtoDFA();
                    Logic.CreateDFAAutomatePicture();
                    Logic.RunGraphicvizDFA(DFAconvertBox);
                    Logic.CreateDFAfile();
                }
                finiteBox.Checked = Logic.IsFinite(DFACheckBox.Checked);
                testBox.Text = Logic.TestOutputString(DFACheckBox.Checked, finiteBox.Checked);

        } catch
            {
                MessageBox.Show("Could not create automata");
            }

}

        private void automataPictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
