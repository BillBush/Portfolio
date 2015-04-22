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
using System.Text.RegularExpressions;

namespace SequenceAlignment
{
    public partial class AlignmentForm : Form
    {
        private Aligner aligner = null;
        private AlignmentTable at = null;
        private int selectedAlignment = int.MinValue;
        private int selectedPath = int.MinValue;
        public AlignmentForm()
        {
            InitializeComponent();

            examplesComboBox.SelectedIndex = 0;
            examplesComboBox_SelectedIndexChanged(null, null);
            aligner = new Aligner();
            globalAlignmentButton_Click(null, null);
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
                            outputRichTextBox.Clear();
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
        private void save(String s, String name)
        {
            if (!s.Equals(""))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.Title = "Save an Text File";
                saveFileDialog.DefaultExt = ".txt";
                saveFileDialog.FileName = name;
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    File.WriteAllText(saveFileDialog.FileName, s);
                }
            }
            else
            {
                MessageBox.Show("There must be output text.");
                return;
            }
        }
        private void browseParametersButton_Click(object sender, EventArgs e){ parametersRichTextBox.Text = open(); }
        private void browseInputButton_Click(object sender, EventArgs e) { inputRichTextBox.Text = open(); }
        private void saveParametersButton_Click(object sender, EventArgs e) { save(parametersRichTextBox.Text, "parameters"); }
        private void saveInputButton_Click(object sender, EventArgs e) { save(inputRichTextBox.Text, "input"); }
        private void saveOutputButton_Click(object sender, EventArgs e) { save(outputRichTextBox.Text, "output"); }
        private void clearParametersButton_Click(object sender, EventArgs e) { parametersRichTextBox.Clear(); }
        private void clearInputButton_Click(object sender, EventArgs e) { inputRichTextBox.Clear(); }
        private void clearOutputButton_Click(object sender, EventArgs e) { outputRichTextBox.Clear(); }
        private void parametersRichTextBox_TextChanged(object sender, EventArgs e) { if (parametersRichTextBox.Text.Trim().Equals("")) { outputRichTextBox.Clear(); at = null; } }
        private void inputRichTextBox_TextChanged(object sender, EventArgs e) { if (inputRichTextBox.Text.Trim().Equals("")) { outputRichTextBox.Clear(); } }
        private void outputRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!outputRichTextBox.Text.Trim().Equals("")) { validateButton.Enabled = true; }
            else
            {
                clearDynamicProgrammingTable();
                lastAlignmentButton.Enabled = nextAlignmentButton.Enabled = false;
                firstAlignmentButton.Enabled = previousAlignmentButton.Enabled = false;
                lastPathButton.Enabled = nextPathButton.Enabled = false;
                firstPathButton.Enabled = previousPathButton.Enabled = false;
                validateButton.Enabled = false;
            }
        }
        private void examplesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (examplesComboBox.SelectedItem.ToString())
            {
                case "Clear All":
                    parametersRichTextBox.Text = "";
                    inputRichTextBox.Text = "";
                    outputRichTextBox.Text = "";
                    break;
                case "Match = 2, Mismatch = -1, Indel = -2":
                    this.parametersRichTextBox.Text = "; Everything after a semi-colon is a comment.\n\n"
                        + "; Below is the alphabet\n"
                        + "A C G T\n\n"
                        + "; Below is the similarity matrix for the alphabet\n"
                        + "2 -1 -1 -1\n"
                        + "-1 2 -1 -1\n"
                        + "-1 -1 2 -1\n"
                        + "-1 -1 -1 2\n\n"
                        + "; This gives a fixed penalty of -2 for each indel.\n"
                        + "-2\n";
                    this.inputRichTextBox.Text = "; Everything after a semi-colon is a comment.\n\n"
                        + ">seq1\n"
                        + "ACGGTTAT\n\n"
                        + ">seq2\n"
                        + "CTGGATC\n";
                    break;
                case "Item Specific Match/Mismatch Scores, Afine Gap Penalty for Indels":
                    this.parametersRichTextBox.Text = "; Everything after a semi-colon is a comment.\n\n"
                        + "; Below is the set of amino acids\n"
                        + "A R N D C Q E G H I L K M F P S T W Y V\n\n"
                        + "; Below is the BLOSUM50 substitution matrix\n"
                        + "5 -2 -1 -2 -1 -1 -1  0 -2 -1 -2 -1 -1 -3 -1  1  0 -3 -2  0\n"
                        + "-2  7 -1 -2 -4  1  0 -3  0 -4 -3  3 -2 -3 -3 -1 -1 -3 -1 -3\n"
                        + "-1 -1  7  2 -2  0  0  0  1 -3 -4  0 -2 -4 -2  1  0 -4 -2 -3\n"
                        + "-2 -2  2  8 -4  0  2 -1 -1 -4 -4 -1 -4 -5 -1  0 -1 -5 -3 -4\n"
                        + "-1 -4 -2 -4 13 -3 -3 -3 -3 -2 -2 -3 -2 -2 -4 -1 -1 -5 -3 -1\n"
                        + "-1  1  0  0 -3  7  2 -2  1 -3 -2  2  0 -4 -1  0 -1 -1 -1 -3\n"
                        + "-1  0  0  2 -3  2  6 -3  0 -4 -3  1 -2 -3 -1 -1 -1 -3 -2 -3\n"
                        + "0 -3  0 -1 -3 -2 -3  8 -2 -4 -4 -2 -3 -4 -2  0 -2 -3 -3 -4\n"
                        + "-2  0  1 -1 -3  1  0 -2 10 -4 -3  0 -1 -1 -2 -1 -2 -3  2 -4\n"
                        + "-1 -4 -3 -4 -2 -3 -4 -4 -4  5  2 -3  2  0 -3 -3 -1 -3 -1  4\n"
                        + "-2 -3 -4 -4 -2 -2 -3 -4 -3  2  5 -3  3  1 -4 -3 -1 -2 -1  1\n"
                        + "-1  3  0 -1 -3  2  1 -2  0 -3 -3  6 -2 -4 -1  0 -1 -3 -2 -3\n"
                        + "-1 -2 -2 -4 -2  0 -2 -3 -1  2  3 -2  7  0 -3 -2 -1 -1  0  1\n"
                        + "-3 -3 -4 -5 -2 -4 -3 -4 -1  0  1 -4  0  8 -4 -3 -2  1  4 -1\n"
                        + "-1 -3 -2 -1 -4 -1 -1 -2 -2 -3 -4 -1 -3 -4 10 -1 -1 -4 -3 -3\n"
                        + " 1 -1  1  0 -1  0 -1  0 -1 -3 -3  0 -2 -3 -1  5  2 -4 -2 -2\n"
                        + " 0 -1  0 -1 -1 -1 -1 -2 -2 -1 -1 -1 -1 -2 -1  2  5 -3 -2  0\n"
                        + "-3 -3 -4 -5 -5 -1 -3 -3 -3 -3 -2 -3 -1  1 -4 -4 -3 15  2 -3\n"
                        + "-2 -1 -2 -3 -3 -1 -2 -3  2 -1 -1 -2  0  4 -3 -2 -2  2  8 -1\n"
                        + " 0 -3 -3 -4 -1 -3 -3 -4 -4  4  1 -3  1 -1 -3 -2  0 -3 -1  5\n\n"
                        + "; Affine gap penalty is defined as g(q) = h + sq.\n"
                        + "; h is initialized as 5, q is the gap length, and s is initialized as 2\n"
                        + "g(q) = 5 + 2q\n";
                    this.inputRichTextBox.Text = "; Everything after a semi-colon is a comment.\n\n"
                        + "> Unknown sequence\n"
                        + (""
                        + "EGSQKHEEL\n"
                        + "\n").Replace(" ", "").Replace("\n", "")
                        + "\n\n"
                        + "> Q09332 - Drosophila Melanogaster\n"
                        + (""
                        + "HEPKHGEL\n"
                        + "").Replace(" ","").Replace("\n","")
                        + "\n";
                        break;
            }
            outputRichTextBox.Text = "";
            dynamicProgrammingTableLayoutPanel.Controls.Clear();
            dynamicProgrammingTableLayoutPanel.RowCount = 0;
            dynamicProgrammingTableLayoutPanel.ColumnCount = 0;
            dynamicProgrammingTableLayoutPanel.Refresh();
            if (at != null)
            {
                if (at.GetType() == typeof(LcsTable))
                {
                    lcsButton_Click(null, null);
                }
                else if (at.GetType() == typeof(AffineLocalAlignmentTable) || at.GetType() == typeof(LocalAlignmentTable))
                {
                    localAlignmentButton_Click(null, null);
                }
                else
                {
                    globalAlignmentButton_Click(null, null);
                }
            }
        }
        private void globalAlignmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                Aligner aligner = new Aligner();
                aligner.Input = inputRichTextBox.Text;
                aligner.Parameters = parametersRichTextBox.Text;
                at = aligner.getGlobalAlignmentTable();
                generateAndDisplayAlignments();
            }
            catch (Exception ex) { MessageBox.Show("Invalid parameter or input data:\r\n" + ex.Message); }
        }
        private void localAlignmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                Aligner aligner = new Aligner();
                aligner.Input = inputRichTextBox.Text;
                aligner.Parameters = parametersRichTextBox.Text;
                at = aligner.getLocalAlignmentTable();
                selectedPath = 0;
                generateAndDisplayAlignments();
            }
            catch (Exception ex) { MessageBox.Show("Invalid parameter or input data:\r\n" + ex.Message); }
        }
        private void lcsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Aligner aligner = new Aligner();
                aligner.Input = this.inputRichTextBox.Text;
                at = aligner.getLcsTable();
                selectedPath = 0;
                generateAndDisplayAlignments();
            }
            catch (Exception ex) { MessageBox.Show("Invalid parameter or input data:\r\n" + ex.Message); }
        }
        private void clearDynamicProgrammingTable()
        {
            dynamicProgrammingTableLayoutPanel.Controls.Clear();
            dynamicProgrammingTableLayoutPanel.RowCount = 0;
            dynamicProgrammingTableLayoutPanel.ColumnCount = 0;
            dynamicProgrammingTableLayoutPanel.Refresh();
        }
        private void setCellValue(String val, int col, int row, TableLayoutPanel tlp)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
            lbl.Text = val;
            tlp.Controls.Add(lbl, col, row);
        }
        private void generateAndDisplayTable()
        {
            selectedAlignment = 0;
            at.GenTable();
            clearDynamicProgrammingTable();
            int width = at.Top.Value.Length + 2;
            dynamicProgrammingTableLayoutPanel.ColumnCount = width;
            int height = at.Left.Value.Length + 2;
            dynamicProgrammingTableLayoutPanel.RowCount = height;
            setCellValue("_", 0, 0, dynamicProgrammingTableLayoutPanel);
            setCellValue("_", 1, 0, dynamicProgrammingTableLayoutPanel);
            for (int i = 0; i < at.Top.Value.Length; i++) { setCellValue(at.Top.Value[i].ToString(), i + 2, 0, dynamicProgrammingTableLayoutPanel); }
            for (int row = 0; row < at.Left.Value.Length + 1; row++)
            {
                if (row > 0) { setCellValue(at.Left.Value[row - 1].ToString(), 0, row + 1, dynamicProgrammingTableLayoutPanel); }
                else { setCellValue("_", 0, row + 1, dynamicProgrammingTableLayoutPanel); }
                for (int col = 0; col < at.Top.Value.Length + 1; col++) { setCellValue(at[row, col].ToString(), col + 1, row + 1, dynamicProgrammingTableLayoutPanel); }
            }
            foreach (Control c in dynamicProgrammingTableLayoutPanel.Controls) { ((Label)c).BackColor = SystemColors.Control; }
            dynamicProgrammingTableLayoutPanel.Refresh();
        }
        private void displayAlignment()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(";Highest Alignment Score = ");
            sb.Append(at.MaxScore);
            sb.Append("\r\n");
            sb.Append(";Number of Alignments Found = ");
            sb.Append(at.NumberOfAlignments);
            sb.Append("\r\n\r\n");
            sb.Append(";Example Alignment ");
            sb.Append(selectedAlignment + 1);
            sb.Append(" of ");
            sb.Append(at.NumberOfAlignments);
            sb.Append(":\r\n\r\n");
            alignmentLabel.Text = (selectedAlignment + 1) + "/" + at.NumberOfAlignments;
            if (at.GetType() == typeof(LcsTable)) 
            {
                drawPath(sb, at[selectedAlignment].Lcs);
                sb.Append(">LCS\r\n");
                sb.Append(at[selectedAlignment].Lcs.Replace("\n", "").Replace("\r", "").Trim());
            }
            else {
                drawPath(sb, at[selectedAlignment].top.Value, at[selectedAlignment].left.Value);
                sb.Append(at[selectedAlignment].ToString());
                lastPathButton.Enabled = nextPathButton.Enabled = false;
                firstPathButton.Enabled = previousPathButton.Enabled = false;
            }
            outputRichTextBox.Text = sb.ToString();
        }
        private void generateAndDisplayAlignments()
        {
            generateAndDisplayTable();
            at.MaxAlignmentCount = int.MaxValue - 1;
            at.GenAlignments();
            at.removeDuplicateAlignments();
            displayAlignment();
        }
        private void getLcsPathsFromStart(int row, int col, String lcs, List<List<Tuple<int, int, StringEdit.EditType>>> paths, List<Tuple<int, int, StringEdit.EditType>> path)
        {
            if (lcs.Length == 0) { paths.Add(path); return; }
            StringEdit se = at[row + 1, col + 1];
            if (se.Value == 0) { return; }
            if (se.Edits.HasFlag(StringEdit.EditType.Delete))
            {
                Tuple<int, int, StringEdit.EditType> curCoord = new Tuple<int, int, SequenceAlignment.StringEdit.EditType>(row, col, StringEdit.EditType.Delete);
                List<Tuple<int, int, StringEdit.EditType>> newPath = new List<Tuple<int, int, StringEdit.EditType>>();
                foreach (Tuple<int, int, StringEdit.EditType> t in path) { newPath.Add(t); }
                newPath.Add(curCoord);
                getLcsPathsFromStart(row, col - 1, lcs, paths, newPath);
            }
            if (se.Edits.HasFlag(SequenceAlignment.StringEdit.EditType.Insert))
            {
                Tuple<int, int, StringEdit.EditType> curCoord = new Tuple<int, int, SequenceAlignment.StringEdit.EditType>(row, col, StringEdit.EditType.Insert);
                List<Tuple<int, int, StringEdit.EditType>> newPath = new List<Tuple<int, int, StringEdit.EditType>>();
                foreach (Tuple<int, int, StringEdit.EditType> t in path) { newPath.Add(t); }
                newPath.Add(curCoord);
                getLcsPathsFromStart(row - 1, col, lcs, paths, newPath);
            }
            if (se.Edits.HasFlag(SequenceAlignment.StringEdit.EditType.Mismatch))
            {
                Tuple<int, int, StringEdit.EditType> curCoord = new Tuple<int, int, SequenceAlignment.StringEdit.EditType>(row, col, StringEdit.EditType.Mismatch);
                List<Tuple<int, int, StringEdit.EditType>> newPath = new List<Tuple<int, int, StringEdit.EditType>>();
                foreach (Tuple<int, int, StringEdit.EditType> t in path) { newPath.Add(t); }
                newPath.Add(curCoord);
                getLcsPathsFromStart(row - 1, col - 1, lcs, paths, newPath);
            }
            if (se.Edits.HasFlag(SequenceAlignment.StringEdit.EditType.Match))
            {
                if (at.Top.Value[col] != lcs[lcs.Length - 1] || at.Left.Value[row] != lcs[lcs.Length - 1]) { return; }
                Tuple<int, int, StringEdit.EditType> curCoord = new Tuple<int, int, SequenceAlignment.StringEdit.EditType>(row, col, StringEdit.EditType.Match);
                List<Tuple<int, int, StringEdit.EditType>> newPath = new List<Tuple<int, int, StringEdit.EditType>>();
                foreach (Tuple<int, int, StringEdit.EditType> t in path) { newPath.Add(t); }
                newPath.Add(curCoord);
                getLcsPathsFromStart(row - 1, col - 1, lcs.Substring(0, lcs.Length - 1), paths, newPath);
            }
        }
        private void drawPath(StringBuilder output, String top, String left)
        {
            if (at == null) { return; }
            foreach (Control c in dynamicProgrammingTableLayoutPanel.Controls) { ((Label)c).BackColor = SystemColors.Control; }
            lastAlignmentButton.Enabled = nextAlignmentButton.Enabled = at.NumberOfAlignments > 0 && selectedAlignment < (at.NumberOfAlignments - 1);
            firstAlignmentButton.Enabled = previousAlignmentButton.Enabled = at.NumberOfAlignments > 0 && selectedAlignment > 0;
            try
            {
                if (at.GetType() == typeof(AffineLocalAlignmentTable) || at.GetType() == typeof(LocalAlignmentTable))
                {
                    List<Tuple<int, int>> startingPoints = new List<Tuple<int, int>>();
                    for (int row = at.Left.Value.Length; row > 0; row--)
                    {
                        for (int col = at.Top.Value.Length; col > 0; col--)
                        {
                            if (at[row, col].Value == at.MaxScore) { startingPoints.Add(new Tuple<int, int>(row, col)); }
                        }
                    }
                    List<Label> path = new List<Label>();
                    foreach (Tuple<int, int> start in startingPoints)
                    {
                        int row = start.Item1 + 1;
                        int col = start.Item2 + 1;
                        try
                        {
                            for (int i = top.Length - 1; i >= 0; i--)
                            {
                                if (top[i] == left[i] && left[i] == '_') { throw new Exception("Cannot have both insert and delete at the same position!"); }
                                StringEdit se = at[row - 1, col - 1];
                                if (top[i] == '_' && se.Edits.HasFlag(StringEdit.EditType.Insert))
                                {
                                    if (left[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(0, row)).Text.Trim()[0])
                                    {
                                        throw new Exception("Value specified in alignment for left sequence does not match that of table row!");
                                    }
                                    ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.LightBlue;
                                    path.Add(((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)));
                                    --row;
                                }
                                else if (left[i] == '_' && se.Edits.HasFlag(StringEdit.EditType.Delete))
                                {
                                    if (top[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, 0)).Text.Trim()[0])
                                    {
                                        throw new Exception("Value specified in alignment for top sequence does not match that of table column!");
                                    }
                                    ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.Yellow;
                                    path.Add(((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)));
                                    --col;
                                }
                                else
                                {
                                    if (top[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, 0)).Text.Trim()[0])
                                    {
                                        throw new Exception("Value specified in alignment for top sequence does not match that of table column!");
                                    }
                                    if (left[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(0, row)).Text.Trim()[0])
                                    {
                                        throw new Exception("Value specified in alignment for left sequence does not match that of table row!");
                                    }
                                    if (top[i] == left[i])
                                    {
                                        ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.LightGreen;
                                    }
                                    else
                                    {
                                        ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.LightPink;
                                    }
                                    path.Add(((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)));
                                    --row;
                                    --col;
                                }
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            foreach (Label l in path)
                            {
                                l.BackColor = Color.White;
                            }
                            path.Clear();
                            continue;
                        }
                    }
                    if (path.Count == 0)
                    {
                        outputRichTextBox.Text += "\r\nValidation failed!";
                    }
                }
                else
                {
                    int row = at.Left.Value.Length + 1;
                    int col = at.Top.Value.Length + 1;
                    int i = top.Length - 1;
                    try
                    {
                        for (; i >= 0; i--)
                        {
                            if (top[i] == left[i] && left[i] == '_')
                            {
                                throw new Exception("Cannot have both insert and delete at the same position!");
                            }
                            StringEdit se = at[row - 1, col - 1];
                            if (top[i] == '_' && se.Edits.HasFlag(StringEdit.EditType.Insert))
                            {
                                if (left[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(0, row)).Text.Trim()[0])
                                {
                                    throw new Exception("Value specified in alignment for left sequence does not match that of table row!");
                                }
                                ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.LightBlue;
                                --row;
                            }
                            else if (left[i] == '_' && se.Edits.HasFlag(StringEdit.EditType.Delete))
                            {
                                if (top[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, 0)).Text.Trim()[0])
                                {
                                    throw new Exception("Value specified in alignment for top sequence does not match that of table column!");
                                }
                                ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.Yellow;
                                --col;
                            }
                            else
                            {
                                if (top[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, 0)).Text.Trim()[0])
                                {
                                    throw new Exception("Value specified in alignment for top sequence does not match that of table column!");
                                }
                                if (left[i] != ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(0, row)).Text.Trim()[0])
                                {
                                    throw new Exception("Value specified in alignment for left sequence does not match that of table row!");
                                }
                                if (top[i] == left[i])
                                {
                                    ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.LightGreen;
                                }
                                else
                                {
                                    ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.LightPink;
                                }
                                --row;
                                --col;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(col, row)).BackColor = Color.Red;
                        outputRichTextBox.Text += "\r\nValidation failed at table row = " + row + ", table column = "
                            + col + ", alignment index = " + i + ":\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                    }
                }
            }
            catch (Exception ex)
            {
                outputRichTextBox.Text = "Invalid parameter or input data\r\n" + ex.Message + "\r\n" + ex.StackTrace;
            }
        }
        private void drawPath(StringBuilder output, String lcs)
        {
            if (at == null) { return; }
            foreach (Control c in dynamicProgrammingTableLayoutPanel.Controls) { ((Label)c).BackColor = SystemColors.Control; }
            lastAlignmentButton.Enabled = nextAlignmentButton.Enabled = at.NumberOfAlignments > 0 && selectedAlignment < (at.NumberOfAlignments - 1);
            firstAlignmentButton.Enabled = previousAlignmentButton.Enabled = at.NumberOfAlignments > 0 && selectedAlignment > 0;
            try
            {
                List<List<Tuple<int, int, StringEdit.EditType>>> paths = new List<List<Tuple<int, int, StringEdit.EditType>>>();
                for (int col = at.Top.Value.Length - 1; col >= 0; col--)
                {
                    for (int row = at.Left.Value.Length - 1; row >= 0; row--)
                    {
                        if (at.Left.Value[row] == at.Top.Value[col] && at.Top.Value[col] == lcs[lcs.Length - 1] && at[row + 1, col + 1].Value == at.MaxScore)
                        {
                            Tuple<int, int, StringEdit.EditType> curCoord = new Tuple<int, int, SequenceAlignment.StringEdit.EditType>(row, col, StringEdit.EditType.Match);
                            List<Tuple<int, int, StringEdit.EditType>> newPath = new List<Tuple<int, int, StringEdit.EditType>>();
                            newPath.Add(curCoord);
                            getLcsPathsFromStart(row - 1, col - 1, lcs.Substring(0, lcs.Length - 1), paths, newPath);
                        }
                    }
                }
                if (selectedPath >= paths.Count) { selectedPath = paths.Count - 1; }
                lastPathButton.Enabled = nextPathButton.Enabled = selectedPath < paths.Count - 1;
                firstPathButton.Enabled = previousPathButton.Enabled = selectedPath > 0;
                output.Append("Path ");
                output.Append(selectedPath + 1);
                output.Append(" of ");
                output.Append(paths.Count);
                output.Append("\r\n\r\n");
                pathLabel.Text = (selectedPath + 1) + "/" + (paths.Count);
                List<Tuple<int, int, StringEdit.EditType>> path = paths[selectedPath];
                foreach (Tuple<int, int, StringEdit.EditType> coord in path)
                {
                    Color c = SystemColors.Control;
                    switch (coord.Item3)
                    {
                        case StringEdit.EditType.Delete: c = Color.Yellow; break;
                        case StringEdit.EditType.Insert: c = Color.LightBlue; break;
                        case StringEdit.EditType.Mismatch: c = Color.LightPink; break;
                        case StringEdit.EditType.Match: c = Color.LightGreen; break;

                    }
                    ((Label)dynamicProgrammingTableLayoutPanel.GetControlFromPosition(coord.Item2 + 2, coord.Item1 + 2)).BackColor = c;
                }
            }
            catch (Exception ex)
            {
                outputRichTextBox.Text = "Invalid parameter or input data\r\n" + ex.Message + "\r\n" + ex.StackTrace;
            }
        }
        private void previousAlignmentButton_Click(object sender, EventArgs e)
        {
            selectedAlignment--;
            selectedPath = 0;
            displayAlignment();
        }
        private void nextAlignmentButton_Click(object sender, EventArgs e)
        {
            selectedAlignment++;
            selectedPath = 0;
            displayAlignment();
        }
        private void previousPathButton_Click(object sender, EventArgs e)
        {
            selectedPath--;
            displayAlignment();
        }
        private void nextPathButton_Click(object sender, EventArgs e)
        {
            selectedPath++;
            displayAlignment();
        }
        private void firstAlignmentButton_Click(object sender, EventArgs e)
        {
            selectedAlignment = 0;
            selectedPath = 0;
            displayAlignment();
        }
        private void lastAlignmentButton_Click(object sender, EventArgs e)
        {
            selectedAlignment = at.NumberOfAlignments - 1;
            selectedPath = 0;
            displayAlignment();
        }
        private void firstPathButton_Click(object sender, EventArgs e)
        {
            selectedPath = 0;
            displayAlignment();
        }
        private void lastPathButton_Click(object sender, EventArgs e)
        {
            selectedPath = int.MaxValue;
            displayAlignment();
        }
        private List<Sequence> getExampleAlignment(String input)
        {
            while ((input = input.Trim()).Length > 0)//process parameters one line at a time
            {
                //take 1 line out of input string
                String line = input.Substring(0, Regex.Match(input, "^.*$", RegexOptions.Multiline | RegexOptions.CultureInvariant).Length);
                input = input.Substring(line.Length);
                //remove comments
                if (line.Contains(";")) { line = line.Substring(0, line.IndexOf(";")); }
                line = line.Trim();
                //skip blank or comment-only lines
                if (line.Equals("")) { continue; }
                //remove consecutive spaces;  This actually replaces all "one or more" with one space
                for (Match m = Regex.Match(line, "[ ][ ][ ]*"); m.Success; m = Regex.Match(line, "[ ][ ][ ]*")) { line = line.Replace(m.ToString(), " "); }
                if (!line.StartsWith(">")) { continue; }
                else { return Sequence.ExtractSequences(line + "\r\n" + input); }
            }
            throw new Exception("Sample sequences not found or improperly formatted!");
        }
        private void validateButton_Click(object sender, EventArgs e)
        {
            if (at.GetType() == typeof(LcsTable))
            {
                String lcs = getExampleAlignment(outputRichTextBox.Text)[0].Value;
                int i = 0;
                for (; i < at.NumberOfAlignments; i++) { if (lcs.Equals(at[i].Lcs)) { break; } }
                if (i < at.NumberOfAlignments) { selectedAlignment = i; MessageBox.Show("Valid Alignment!"); }
                else { MessageBox.Show("Invalid Alignment!"); }
                displayAlignment();
            }
            else
            {
                List<Sequence> seqs = getExampleAlignment(outputRichTextBox.Text);
                String top = seqs[0].Value;
                String left = seqs[1].Value;
                int i = 0;
                for (; i < at.NumberOfAlignments; i++) { if (top.Equals(at[i].top.Value) && left.Equals(at[i].left.Value)) { break; } }
                if (i < at.NumberOfAlignments) { selectedAlignment = i; MessageBox.Show("Valid Alignment!"); }
                else { MessageBox.Show("Invalid Alignment!"); }
                displayAlignment();
            }
        }
    }
}
