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
            // C:\Users\20182942\Documents\Fontys\S4 AUT\Automata Reader\Automata input files\Test.txt

            string path = pathBox.Text;
            try{
                Logic.ReadLines(path);
                Logic.CreateAutomatePicture();
                DFACheckBox.Checked = Logic.GraphIsDFA();
                Logic.RunGraphicviz(automataPictureBox);

                if (!DFACheckBox.Checked)
                {
                    Logic.ConvertNFAtoDFA();
                    Logic.CreateDFAAutomatePicture();
                    Logic.RunGraphicvizDFA(DFAconvertBox);
                }
            } catch
            {
                MessageBox.Show("Could not create automata");
            }

        }
    }
}
