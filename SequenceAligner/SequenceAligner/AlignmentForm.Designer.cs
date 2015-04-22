namespace SequenceAlignment
{
    partial class AlignmentForm
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
            this.inputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.outputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.browseInputButton = new System.Windows.Forms.Button();
            this.clearInputButton = new System.Windows.Forms.Button();
            this.saveOutputButton = new System.Windows.Forms.Button();
            this.clearOutputButton = new System.Windows.Forms.Button();
            this.globalAlignmentButton = new System.Windows.Forms.Button();
            this.localAlignmentButton = new System.Windows.Forms.Button();
            this.browseParametersButton = new System.Windows.Forms.Button();
            this.parametersRichTextBox = new System.Windows.Forms.RichTextBox();
            this.parametersLabel = new System.Windows.Forms.Label();
            this.clearParametersButton = new System.Windows.Forms.Button();
            this.saveParametersButton = new System.Windows.Forms.Button();
            this.saveInputButton = new System.Windows.Forms.Button();
            this.lcsButton = new System.Windows.Forms.Button();
            this.dynamicProgrammingTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.examplesComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.validateButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.deletionLabel = new System.Windows.Forms.Label();
            this.insertionLabel = new System.Windows.Forms.Label();
            this.mismatchLabel = new System.Windows.Forms.Label();
            this.matchLabel = new System.Windows.Forms.Label();
            this.previousAlignmentButton = new System.Windows.Forms.Button();
            this.nextAlignmentButton = new System.Windows.Forms.Button();
            this.nextPathButton = new System.Windows.Forms.Button();
            this.previousPathButton = new System.Windows.Forms.Button();
            this.lastPathButton = new System.Windows.Forms.Button();
            this.lastAlignmentButton = new System.Windows.Forms.Button();
            this.firstPathButton = new System.Windows.Forms.Button();
            this.firstAlignmentButton = new System.Windows.Forms.Button();
            this.alignmentLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputRichTextBox
            // 
            this.inputRichTextBox.Location = new System.Drawing.Point(112, 311);
            this.inputRichTextBox.Name = "inputRichTextBox";
            this.inputRichTextBox.Size = new System.Drawing.Size(436, 212);
            this.inputRichTextBox.TabIndex = 4;
            this.inputRichTextBox.Text = "";
            this.inputRichTextBox.TextChanged += new System.EventHandler(this.inputRichTextBox_TextChanged);
            // 
            // outputRichTextBox
            // 
            this.outputRichTextBox.Location = new System.Drawing.Point(112, 549);
            this.outputRichTextBox.Name = "outputRichTextBox";
            this.outputRichTextBox.Size = new System.Drawing.Size(436, 134);
            this.outputRichTextBox.TabIndex = 1;
            this.outputRichTextBox.Text = "";
            this.outputRichTextBox.TextChanged += new System.EventHandler(this.outputRichTextBox_TextChanged);
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(109, 295);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(31, 13);
            this.inputLabel.TabIndex = 2;
            this.inputLabel.Text = "Input";
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(109, 533);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(39, 13);
            this.outputLabel.TabIndex = 3;
            this.outputLabel.Text = "Output";
            // 
            // browseInputButton
            // 
            this.browseInputButton.Location = new System.Drawing.Point(23, 442);
            this.browseInputButton.Name = "browseInputButton";
            this.browseInputButton.Size = new System.Drawing.Size(83, 23);
            this.browseInputButton.TabIndex = 3;
            this.browseInputButton.Text = "Browse";
            this.browseInputButton.UseVisualStyleBackColor = true;
            this.browseInputButton.Click += new System.EventHandler(this.browseInputButton_Click);
            // 
            // clearInputButton
            // 
            this.clearInputButton.Location = new System.Drawing.Point(23, 500);
            this.clearInputButton.Name = "clearInputButton";
            this.clearInputButton.Size = new System.Drawing.Size(83, 23);
            this.clearInputButton.TabIndex = 5;
            this.clearInputButton.Text = "Clear";
            this.clearInputButton.UseVisualStyleBackColor = true;
            this.clearInputButton.Click += new System.EventHandler(this.clearInputButton_Click);
            // 
            // saveOutputButton
            // 
            this.saveOutputButton.Location = new System.Drawing.Point(23, 631);
            this.saveOutputButton.Name = "saveOutputButton";
            this.saveOutputButton.Size = new System.Drawing.Size(83, 23);
            this.saveOutputButton.TabIndex = 6;
            this.saveOutputButton.Text = "Save";
            this.saveOutputButton.UseVisualStyleBackColor = true;
            this.saveOutputButton.Click += new System.EventHandler(this.saveOutputButton_Click);
            // 
            // clearOutputButton
            // 
            this.clearOutputButton.Location = new System.Drawing.Point(23, 660);
            this.clearOutputButton.Name = "clearOutputButton";
            this.clearOutputButton.Size = new System.Drawing.Size(83, 23);
            this.clearOutputButton.TabIndex = 7;
            this.clearOutputButton.Text = "Clear";
            this.clearOutputButton.UseVisualStyleBackColor = true;
            this.clearOutputButton.Click += new System.EventHandler(this.clearOutputButton_Click);
            // 
            // globalAlignmentButton
            // 
            this.globalAlignmentButton.Location = new System.Drawing.Point(111, 689);
            this.globalAlignmentButton.Name = "globalAlignmentButton";
            this.globalAlignmentButton.Size = new System.Drawing.Size(96, 23);
            this.globalAlignmentButton.TabIndex = 5;
            this.globalAlignmentButton.Text = "Global Alignment";
            this.globalAlignmentButton.UseVisualStyleBackColor = true;
            this.globalAlignmentButton.Click += new System.EventHandler(this.globalAlignmentButton_Click);
            // 
            // localAlignmentButton
            // 
            this.localAlignmentButton.Location = new System.Drawing.Point(213, 689);
            this.localAlignmentButton.Name = "localAlignmentButton";
            this.localAlignmentButton.Size = new System.Drawing.Size(91, 23);
            this.localAlignmentButton.TabIndex = 6;
            this.localAlignmentButton.Text = "Local Alignment";
            this.localAlignmentButton.UseVisualStyleBackColor = true;
            this.localAlignmentButton.Click += new System.EventHandler(this.localAlignmentButton_Click);
            // 
            // browseParametersButton
            // 
            this.browseParametersButton.Location = new System.Drawing.Point(23, 203);
            this.browseParametersButton.Name = "browseParametersButton";
            this.browseParametersButton.Size = new System.Drawing.Size(83, 23);
            this.browseParametersButton.TabIndex = 1;
            this.browseParametersButton.Text = "Browse";
            this.browseParametersButton.UseVisualStyleBackColor = true;
            this.browseParametersButton.Click += new System.EventHandler(this.browseParametersButton_Click);
            // 
            // parametersRichTextBox
            // 
            this.parametersRichTextBox.Location = new System.Drawing.Point(112, 72);
            this.parametersRichTextBox.Name = "parametersRichTextBox";
            this.parametersRichTextBox.Size = new System.Drawing.Size(436, 212);
            this.parametersRichTextBox.TabIndex = 2;
            this.parametersRichTextBox.Text = "";
            this.parametersRichTextBox.TextChanged += new System.EventHandler(this.parametersRichTextBox_TextChanged);
            // 
            // parametersLabel
            // 
            this.parametersLabel.AutoSize = true;
            this.parametersLabel.Location = new System.Drawing.Point(112, 53);
            this.parametersLabel.Name = "parametersLabel";
            this.parametersLabel.Size = new System.Drawing.Size(60, 13);
            this.parametersLabel.TabIndex = 13;
            this.parametersLabel.Text = "Parameters";
            // 
            // clearParametersButton
            // 
            this.clearParametersButton.Location = new System.Drawing.Point(23, 261);
            this.clearParametersButton.Name = "clearParametersButton";
            this.clearParametersButton.Size = new System.Drawing.Size(83, 23);
            this.clearParametersButton.TabIndex = 3;
            this.clearParametersButton.Text = "Clear";
            this.clearParametersButton.UseVisualStyleBackColor = true;
            this.clearParametersButton.Click += new System.EventHandler(this.clearParametersButton_Click);
            // 
            // saveParametersButton
            // 
            this.saveParametersButton.Location = new System.Drawing.Point(23, 232);
            this.saveParametersButton.Name = "saveParametersButton";
            this.saveParametersButton.Size = new System.Drawing.Size(83, 23);
            this.saveParametersButton.TabIndex = 2;
            this.saveParametersButton.Text = "Save";
            this.saveParametersButton.UseVisualStyleBackColor = true;
            this.saveParametersButton.Click += new System.EventHandler(this.saveParametersButton_Click);
            // 
            // saveInputButton
            // 
            this.saveInputButton.Location = new System.Drawing.Point(23, 471);
            this.saveInputButton.Name = "saveInputButton";
            this.saveInputButton.Size = new System.Drawing.Size(83, 23);
            this.saveInputButton.TabIndex = 14;
            this.saveInputButton.Text = "Save";
            this.saveInputButton.UseVisualStyleBackColor = true;
            this.saveInputButton.Click += new System.EventHandler(this.saveInputButton_Click);
            // 
            // lcsButton
            // 
            this.lcsButton.Location = new System.Drawing.Point(308, 690);
            this.lcsButton.Name = "lcsButton";
            this.lcsButton.Size = new System.Drawing.Size(75, 23);
            this.lcsButton.TabIndex = 15;
            this.lcsButton.Text = "LCS";
            this.lcsButton.UseVisualStyleBackColor = true;
            this.lcsButton.Click += new System.EventHandler(this.lcsButton_Click);
            // 
            // dynamicProgrammingTableLayoutPanel
            // 
            this.dynamicProgrammingTableLayoutPanel.AutoScroll = true;
            this.dynamicProgrammingTableLayoutPanel.AutoSize = true;
            this.dynamicProgrammingTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dynamicProgrammingTableLayoutPanel.ColumnCount = 2;
            this.dynamicProgrammingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.dynamicProgrammingTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.dynamicProgrammingTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dynamicProgrammingTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.dynamicProgrammingTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.dynamicProgrammingTableLayoutPanel.MaximumSize = new System.Drawing.Size(723, 407);
            this.dynamicProgrammingTableLayoutPanel.Name = "dynamicProgrammingTableLayoutPanel";
            this.dynamicProgrammingTableLayoutPanel.RowCount = 2;
            this.dynamicProgrammingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.dynamicProgrammingTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.dynamicProgrammingTableLayoutPanel.Size = new System.Drawing.Size(723, 407);
            this.dynamicProgrammingTableLayoutPanel.TabIndex = 16;
            // 
            // examplesComboBox
            // 
            this.examplesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.examplesComboBox.FormattingEnabled = true;
            this.examplesComboBox.Items.AddRange(new object[] {
            "Match = 2, Mismatch = -1, Indel = -2",
            "Item Specific Match/Mismatch Scores, Afine Gap Penalty for Indels"});
            this.examplesComboBox.Location = new System.Drawing.Point(112, 16);
            this.examplesComboBox.Name = "examplesComboBox";
            this.examplesComboBox.Size = new System.Drawing.Size(436, 21);
            this.examplesComboBox.TabIndex = 17;
            this.examplesComboBox.SelectedIndexChanged += new System.EventHandler(this.examplesComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Examples:";
            // 
            // validateButton
            // 
            this.validateButton.Location = new System.Drawing.Point(465, 689);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(83, 23);
            this.validateButton.TabIndex = 19;
            this.validateButton.Text = "Validate";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.validateButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dynamicProgrammingTableLayoutPanel);
            this.panel1.Location = new System.Drawing.Point(616, 253);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(723, 407);
            this.panel1.TabIndex = 20;
            // 
            // deletionLabel
            // 
            this.deletionLabel.AutoSize = true;
            this.deletionLabel.BackColor = System.Drawing.Color.Yellow;
            this.deletionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deletionLabel.Location = new System.Drawing.Point(840, 168);
            this.deletionLabel.Name = "deletionLabel";
            this.deletionLabel.Size = new System.Drawing.Size(68, 20);
            this.deletionLabel.TabIndex = 22;
            this.deletionLabel.Text = "Deletion";
            // 
            // insertionLabel
            // 
            this.insertionLabel.AutoSize = true;
            this.insertionLabel.BackColor = System.Drawing.Color.LightBlue;
            this.insertionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insertionLabel.Location = new System.Drawing.Point(763, 168);
            this.insertionLabel.Name = "insertionLabel";
            this.insertionLabel.Size = new System.Drawing.Size(71, 20);
            this.insertionLabel.TabIndex = 23;
            this.insertionLabel.Text = "Insertion";
            // 
            // mismatchLabel
            // 
            this.mismatchLabel.AutoSize = true;
            this.mismatchLabel.BackColor = System.Drawing.Color.LightPink;
            this.mismatchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mismatchLabel.Location = new System.Drawing.Point(680, 168);
            this.mismatchLabel.Name = "mismatchLabel";
            this.mismatchLabel.Size = new System.Drawing.Size(77, 20);
            this.mismatchLabel.TabIndex = 24;
            this.mismatchLabel.Text = "Mismatch";
            // 
            // matchLabel
            // 
            this.matchLabel.AutoSize = true;
            this.matchLabel.BackColor = System.Drawing.Color.LightGreen;
            this.matchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matchLabel.Location = new System.Drawing.Point(621, 168);
            this.matchLabel.Name = "matchLabel";
            this.matchLabel.Size = new System.Drawing.Size(53, 20);
            this.matchLabel.TabIndex = 27;
            this.matchLabel.Text = "Match";
            // 
            // previousAlignmentButton
            // 
            this.previousAlignmentButton.Location = new System.Drawing.Point(728, 191);
            this.previousAlignmentButton.Name = "previousAlignmentButton";
            this.previousAlignmentButton.Size = new System.Drawing.Size(106, 23);
            this.previousAlignmentButton.TabIndex = 28;
            this.previousAlignmentButton.Text = "Previous Alignment";
            this.previousAlignmentButton.UseVisualStyleBackColor = true;
            this.previousAlignmentButton.Click += new System.EventHandler(this.previousAlignmentButton_Click);
            // 
            // nextAlignmentButton
            // 
            this.nextAlignmentButton.Location = new System.Drawing.Point(913, 191);
            this.nextAlignmentButton.Name = "nextAlignmentButton";
            this.nextAlignmentButton.Size = new System.Drawing.Size(106, 23);
            this.nextAlignmentButton.TabIndex = 29;
            this.nextAlignmentButton.Text = "Next Alignment";
            this.nextAlignmentButton.UseVisualStyleBackColor = true;
            this.nextAlignmentButton.Click += new System.EventHandler(this.nextAlignmentButton_Click);
            // 
            // nextPathButton
            // 
            this.nextPathButton.Location = new System.Drawing.Point(913, 220);
            this.nextPathButton.Name = "nextPathButton";
            this.nextPathButton.Size = new System.Drawing.Size(106, 23);
            this.nextPathButton.TabIndex = 31;
            this.nextPathButton.Text = "Next Path";
            this.nextPathButton.UseVisualStyleBackColor = true;
            this.nextPathButton.Click += new System.EventHandler(this.nextPathButton_Click);
            // 
            // previousPathButton
            // 
            this.previousPathButton.Location = new System.Drawing.Point(728, 220);
            this.previousPathButton.Name = "previousPathButton";
            this.previousPathButton.Size = new System.Drawing.Size(106, 23);
            this.previousPathButton.TabIndex = 30;
            this.previousPathButton.Text = "Previous Path";
            this.previousPathButton.UseVisualStyleBackColor = true;
            this.previousPathButton.Click += new System.EventHandler(this.previousPathButton_Click);
            // 
            // lastPathButton
            // 
            this.lastPathButton.Location = new System.Drawing.Point(1025, 220);
            this.lastPathButton.Name = "lastPathButton";
            this.lastPathButton.Size = new System.Drawing.Size(106, 23);
            this.lastPathButton.TabIndex = 32;
            this.lastPathButton.Text = "Last Path";
            this.lastPathButton.UseVisualStyleBackColor = true;
            this.lastPathButton.Click += new System.EventHandler(this.lastPathButton_Click);
            // 
            // lastAlignmentButton
            // 
            this.lastAlignmentButton.Location = new System.Drawing.Point(1025, 191);
            this.lastAlignmentButton.Name = "lastAlignmentButton";
            this.lastAlignmentButton.Size = new System.Drawing.Size(106, 23);
            this.lastAlignmentButton.TabIndex = 33;
            this.lastAlignmentButton.Text = "Last Alignment";
            this.lastAlignmentButton.UseVisualStyleBackColor = true;
            this.lastAlignmentButton.Click += new System.EventHandler(this.lastAlignmentButton_Click);
            // 
            // firstPathButton
            // 
            this.firstPathButton.Location = new System.Drawing.Point(616, 220);
            this.firstPathButton.Name = "firstPathButton";
            this.firstPathButton.Size = new System.Drawing.Size(106, 23);
            this.firstPathButton.TabIndex = 34;
            this.firstPathButton.Text = "First Path";
            this.firstPathButton.UseVisualStyleBackColor = true;
            this.firstPathButton.Click += new System.EventHandler(this.firstPathButton_Click);
            // 
            // firstAlignmentButton
            // 
            this.firstAlignmentButton.Location = new System.Drawing.Point(616, 191);
            this.firstAlignmentButton.Name = "firstAlignmentButton";
            this.firstAlignmentButton.Size = new System.Drawing.Size(106, 23);
            this.firstAlignmentButton.TabIndex = 35;
            this.firstAlignmentButton.Text = "First Alignment";
            this.firstAlignmentButton.UseVisualStyleBackColor = true;
            this.firstAlignmentButton.Click += new System.EventHandler(this.firstAlignmentButton_Click);
            // 
            // alignmentLabel
            // 
            this.alignmentLabel.AutoSize = true;
            this.alignmentLabel.BackColor = System.Drawing.SystemColors.Control;
            this.alignmentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignmentLabel.Location = new System.Drawing.Point(840, 194);
            this.alignmentLabel.Name = "alignmentLabel";
            this.alignmentLabel.Size = new System.Drawing.Size(67, 20);
            this.alignmentLabel.TabIndex = 36;
            this.alignmentLabel.Text = "00 of 00";
            this.alignmentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.BackColor = System.Drawing.SystemColors.Control;
            this.pathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathLabel.Location = new System.Drawing.Point(840, 220);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(67, 20);
            this.pathLabel.TabIndex = 37;
            this.pathLabel.Text = "00 of 00";
            this.pathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlignmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 722);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.alignmentLabel);
            this.Controls.Add(this.firstAlignmentButton);
            this.Controls.Add(this.firstPathButton);
            this.Controls.Add(this.lastAlignmentButton);
            this.Controls.Add(this.lastPathButton);
            this.Controls.Add(this.nextPathButton);
            this.Controls.Add(this.previousPathButton);
            this.Controls.Add(this.nextAlignmentButton);
            this.Controls.Add(this.previousAlignmentButton);
            this.Controls.Add(this.matchLabel);
            this.Controls.Add(this.mismatchLabel);
            this.Controls.Add(this.insertionLabel);
            this.Controls.Add(this.deletionLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.validateButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.examplesComboBox);
            this.Controls.Add(this.lcsButton);
            this.Controls.Add(this.saveInputButton);
            this.Controls.Add(this.saveParametersButton);
            this.Controls.Add(this.clearParametersButton);
            this.Controls.Add(this.parametersLabel);
            this.Controls.Add(this.parametersRichTextBox);
            this.Controls.Add(this.browseParametersButton);
            this.Controls.Add(this.localAlignmentButton);
            this.Controls.Add(this.globalAlignmentButton);
            this.Controls.Add(this.clearOutputButton);
            this.Controls.Add(this.saveOutputButton);
            this.Controls.Add(this.clearInputButton);
            this.Controls.Add(this.browseInputButton);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.inputLabel);
            this.Controls.Add(this.outputRichTextBox);
            this.Controls.Add(this.inputRichTextBox);
            this.Name = "AlignmentForm";
            this.Text = "Sequence Aligner";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox inputRichTextBox;
        private System.Windows.Forms.RichTextBox outputRichTextBox;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Button browseInputButton;
        private System.Windows.Forms.Button clearInputButton;
        private System.Windows.Forms.Button saveOutputButton;
        private System.Windows.Forms.Button clearOutputButton;
        private System.Windows.Forms.Button globalAlignmentButton;
        private System.Windows.Forms.Button localAlignmentButton;
        private System.Windows.Forms.Button browseParametersButton;
        private System.Windows.Forms.RichTextBox parametersRichTextBox;
        private System.Windows.Forms.Label parametersLabel;
        private System.Windows.Forms.Button clearParametersButton;
        private System.Windows.Forms.Button saveParametersButton;
        private System.Windows.Forms.Button saveInputButton;
        private System.Windows.Forms.Button lcsButton;
        private System.Windows.Forms.TableLayoutPanel dynamicProgrammingTableLayoutPanel;
        private System.Windows.Forms.ComboBox examplesComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label deletionLabel;
        private System.Windows.Forms.Label insertionLabel;
        private System.Windows.Forms.Label mismatchLabel;
        private System.Windows.Forms.Label matchLabel;
        private System.Windows.Forms.Button previousAlignmentButton;
        private System.Windows.Forms.Button nextAlignmentButton;
        private System.Windows.Forms.Button nextPathButton;
        private System.Windows.Forms.Button previousPathButton;
        private System.Windows.Forms.Button lastPathButton;
        private System.Windows.Forms.Button lastAlignmentButton;
        private System.Windows.Forms.Button firstPathButton;
        private System.Windows.Forms.Button firstAlignmentButton;
        private System.Windows.Forms.Label alignmentLabel;
        private System.Windows.Forms.Label pathLabel;
    }
}

