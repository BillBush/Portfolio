namespace Alignment
{
    partial class FormAlignment
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
            this.richTextBoxInput = new System.Windows.Forms.RichTextBox();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBrowseInput = new System.Windows.Forms.Button();
            this.buttonInputClear = new System.Windows.Forms.Button();
            this.buttonSaveOutput = new System.Windows.Forms.Button();
            this.buttonOutputClear = new System.Windows.Forms.Button();
            this.buttonGlobalAlignment = new System.Windows.Forms.Button();
            this.buttonLocalAlignment = new System.Windows.Forms.Button();
            this.buttonParameters = new System.Windows.Forms.Button();
            this.richTextBoxParameters = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonParametersClear = new System.Windows.Forms.Button();
            this.buttonSaveParameters = new System.Windows.Forms.Button();
            this.buttonSaveInput = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxInput
            // 
            this.richTextBoxInput.Location = new System.Drawing.Point(149, 231);
            this.richTextBoxInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxInput.Name = "richTextBoxInput";
            this.richTextBoxInput.Size = new System.Drawing.Size(419, 117);
            this.richTextBoxInput.TabIndex = 4;
            this.richTextBoxInput.Text = "";
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Location = new System.Drawing.Point(149, 378);
            this.richTextBoxOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(419, 164);
            this.richTextBoxOutput.TabIndex = 1;
            this.richTextBoxOutput.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 212);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 358);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output";
            // 
            // buttonBrowseInput
            // 
            this.buttonBrowseInput.Location = new System.Drawing.Point(41, 231);
            this.buttonBrowseInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonBrowseInput.Name = "buttonBrowseInput";
            this.buttonBrowseInput.Size = new System.Drawing.Size(100, 28);
            this.buttonBrowseInput.TabIndex = 3;
            this.buttonBrowseInput.Text = "Browse";
            this.buttonBrowseInput.UseVisualStyleBackColor = true;
            this.buttonBrowseInput.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonInputClear
            // 
            this.buttonInputClear.Location = new System.Drawing.Point(41, 303);
            this.buttonInputClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonInputClear.Name = "buttonInputClear";
            this.buttonInputClear.Size = new System.Drawing.Size(100, 28);
            this.buttonInputClear.TabIndex = 5;
            this.buttonInputClear.Text = "Clear";
            this.buttonInputClear.UseVisualStyleBackColor = true;
            this.buttonInputClear.Click += new System.EventHandler(this.buttonInputClear_Click);
            // 
            // buttonSaveOutput
            // 
            this.buttonSaveOutput.Location = new System.Drawing.Point(41, 375);
            this.buttonSaveOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSaveOutput.Name = "buttonSaveOutput";
            this.buttonSaveOutput.Size = new System.Drawing.Size(100, 28);
            this.buttonSaveOutput.TabIndex = 6;
            this.buttonSaveOutput.Text = "Save";
            this.buttonSaveOutput.UseVisualStyleBackColor = true;
            this.buttonSaveOutput.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOutputClear
            // 
            this.buttonOutputClear.Location = new System.Drawing.Point(41, 411);
            this.buttonOutputClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOutputClear.Name = "buttonOutputClear";
            this.buttonOutputClear.Size = new System.Drawing.Size(100, 28);
            this.buttonOutputClear.TabIndex = 7;
            this.buttonOutputClear.Text = "Clear";
            this.buttonOutputClear.UseVisualStyleBackColor = true;
            this.buttonOutputClear.Click += new System.EventHandler(this.buttonOutputClear_Click);
            // 
            // buttonGlobalAlignment
            // 
            this.buttonGlobalAlignment.Location = new System.Drawing.Point(148, 550);
            this.buttonGlobalAlignment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonGlobalAlignment.Name = "buttonGlobalAlignment";
            this.buttonGlobalAlignment.Size = new System.Drawing.Size(128, 28);
            this.buttonGlobalAlignment.TabIndex = 5;
            this.buttonGlobalAlignment.Text = "Global Alignment";
            this.buttonGlobalAlignment.UseVisualStyleBackColor = true;
            this.buttonGlobalAlignment.Click += new System.EventHandler(this.buttonGlobalAlignment_Click);
            // 
            // buttonLocalAlignment
            // 
            this.buttonLocalAlignment.Location = new System.Drawing.Point(284, 550);
            this.buttonLocalAlignment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonLocalAlignment.Name = "buttonLocalAlignment";
            this.buttonLocalAlignment.Size = new System.Drawing.Size(121, 28);
            this.buttonLocalAlignment.TabIndex = 6;
            this.buttonLocalAlignment.Text = "Local Alignment";
            this.buttonLocalAlignment.UseVisualStyleBackColor = true;
            this.buttonLocalAlignment.Click += new System.EventHandler(this.buttonLocalAlignment_Click);
            // 
            // buttonParameters
            // 
            this.buttonParameters.Location = new System.Drawing.Point(41, 78);
            this.buttonParameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonParameters.Name = "buttonParameters";
            this.buttonParameters.Size = new System.Drawing.Size(100, 28);
            this.buttonParameters.TabIndex = 1;
            this.buttonParameters.Text = "Browse";
            this.buttonParameters.UseVisualStyleBackColor = true;
            this.buttonParameters.Click += new System.EventHandler(this.buttonParameters_Click);
            // 
            // richTextBoxParameters
            // 
            this.richTextBoxParameters.Location = new System.Drawing.Point(149, 78);
            this.richTextBoxParameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBoxParameters.Name = "richTextBoxParameters";
            this.richTextBoxParameters.Size = new System.Drawing.Size(419, 117);
            this.richTextBoxParameters.TabIndex = 2;
            this.richTextBoxParameters.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Parameters";
            // 
            // buttonParametersClear
            // 
            this.buttonParametersClear.Location = new System.Drawing.Point(41, 149);
            this.buttonParametersClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonParametersClear.Name = "buttonParametersClear";
            this.buttonParametersClear.Size = new System.Drawing.Size(100, 28);
            this.buttonParametersClear.TabIndex = 3;
            this.buttonParametersClear.Text = "Clear";
            this.buttonParametersClear.UseVisualStyleBackColor = true;
            this.buttonParametersClear.Click += new System.EventHandler(this.buttonParametersClear_Click);
            // 
            // buttonSaveParameters
            // 
            this.buttonSaveParameters.Location = new System.Drawing.Point(41, 113);
            this.buttonSaveParameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSaveParameters.Name = "buttonSaveParameters";
            this.buttonSaveParameters.Size = new System.Drawing.Size(100, 28);
            this.buttonSaveParameters.TabIndex = 2;
            this.buttonSaveParameters.Text = "Save";
            this.buttonSaveParameters.UseVisualStyleBackColor = true;
            this.buttonSaveParameters.Click += new System.EventHandler(this.buttonSaveParameters_Click);
            // 
            // buttonSaveInput
            // 
            this.buttonSaveInput.Location = new System.Drawing.Point(41, 267);
            this.buttonSaveInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSaveInput.Name = "buttonSaveInput";
            this.buttonSaveInput.Size = new System.Drawing.Size(100, 28);
            this.buttonSaveInput.TabIndex = 14;
            this.buttonSaveInput.Text = "Save";
            this.buttonSaveInput.UseVisualStyleBackColor = true;
            this.buttonSaveInput.Click += new System.EventHandler(this.buttonSaveInput_Click);
            // 
            // FormAlignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 613);
            this.Controls.Add(this.buttonSaveInput);
            this.Controls.Add(this.buttonSaveParameters);
            this.Controls.Add(this.buttonParametersClear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBoxParameters);
            this.Controls.Add(this.buttonParameters);
            this.Controls.Add(this.buttonLocalAlignment);
            this.Controls.Add(this.buttonGlobalAlignment);
            this.Controls.Add(this.buttonOutputClear);
            this.Controls.Add(this.buttonSaveOutput);
            this.Controls.Add(this.buttonInputClear);
            this.Controls.Add(this.buttonBrowseInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxOutput);
            this.Controls.Add(this.richTextBoxInput);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormAlignment";
            this.Text = "Alignment";
            this.Load += new System.EventHandler(this.FormAlignment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxInput;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonBrowseInput;
        private System.Windows.Forms.Button buttonInputClear;
        private System.Windows.Forms.Button buttonSaveOutput;
        private System.Windows.Forms.Button buttonOutputClear;
        private System.Windows.Forms.Button buttonGlobalAlignment;
        private System.Windows.Forms.Button buttonLocalAlignment;
        private System.Windows.Forms.Button buttonParameters;
        private System.Windows.Forms.RichTextBox richTextBoxParameters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonParametersClear;
        private System.Windows.Forms.Button buttonSaveParameters;
        private System.Windows.Forms.Button buttonSaveInput;
    }
}

