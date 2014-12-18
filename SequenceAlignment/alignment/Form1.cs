using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Alignment
{
    public partial class FormAlignment : Form
    {
        public FormAlignment()
        {
            InitializeComponent();
        }

        private String open()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
                        {
                            richTextBoxOutput.Clear();
                            return streamReader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return null;
        }
        private void save(String s)
        {
            if (!s.Equals(""))
            {
                string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.Title = "Save a Text File";
                saveFileDialog.FileName = "output";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    // TODO: cluu-- This file method should probably be changed to WriteAllLines
                    File.WriteAllLines(saveFileDialog.FileName, lines);
                }
            }
            else
            {
                MessageBox.Show("There must be output text.");
                return;
            }


        }

        private void buttonParameters_Click(object sender, EventArgs e) {  richTextBoxParameters.Text = open(); }
        private void buttonBrowse_Click(object sender, EventArgs e) { richTextBoxInput.Text = open(); }
        private void buttonParametersClear_Click(object sender, EventArgs e) { richTextBoxParameters.Clear(); richTextBoxOutput.Clear(); }
        private void buttonInputClear_Click(object sender, EventArgs e) { richTextBoxInput.Clear(); richTextBoxOutput.Clear(); }
        private void buttonOutputClear_Click(object sender, EventArgs e) { richTextBoxOutput.Clear(); }
        private void buttonSaveParameters_Click(object sender, EventArgs e) { save(richTextBoxParameters.Text); }
        private void buttonSaveInput_Click(object sender, EventArgs e) { save(richTextBoxInput.Text); }
        private void buttonSave_Click(object sender, EventArgs e) { save(richTextBoxOutput.Text); }

        private void buttonGlobalAlignment_Click(object sender, EventArgs e)
        {
            try
            {
                Aligner.Aligner aligner = new Aligner.Aligner();
                aligner.Input = richTextBoxInput.Text;
                aligner.Parameters = richTextBoxParameters.Text;
                richTextBoxOutput.Text = aligner.getGlobalAlignment();
            }
            catch (Exception ex) { MessageBox.Show("Invalid parameter or input data\r\n" + ex.Message); }
        }

        private void buttonLocalAlignment_Click(object sender, EventArgs e)
        {
            try
            {
                Aligner.Aligner aligner = new Aligner.Aligner();
                aligner.Input = richTextBoxInput.Text;
                aligner.Parameters = richTextBoxParameters.Text;
                richTextBoxOutput.Text = aligner.getLocalAlignment();
            }
            catch (Exception ex) { MessageBox.Show("Invalid parameter or input data\r\n" + ex.Message); }
        }
        //
        private void FormAlignment_Load(object sender, EventArgs e)
        {

        }
    }
}
