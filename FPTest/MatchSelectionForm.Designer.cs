namespace Ace
{
    partial class MatchSelectionForm
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
            this.firstBloodCheckBox = new System.Windows.Forms.CheckBox();
            this.maskVsMaskCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // firstBloodCheckBox
            // 
            this.firstBloodCheckBox.AutoSize = true;
            this.firstBloodCheckBox.Location = new System.Drawing.Point(13, 13);
            this.firstBloodCheckBox.Name = "firstBloodCheckBox";
            this.firstBloodCheckBox.Size = new System.Drawing.Size(75, 17);
            this.firstBloodCheckBox.TabIndex = 0;
            this.firstBloodCheckBox.Text = "First Blood";
            this.firstBloodCheckBox.UseVisualStyleBackColor = true;
            // 
            // maskVsMaskCheckBox
            // 
            this.maskVsMaskCheckBox.AutoSize = true;
            this.maskVsMaskCheckBox.Location = new System.Drawing.Point(12, 36);
            this.maskVsMaskCheckBox.Name = "maskVsMaskCheckBox";
            this.maskVsMaskCheckBox.Size = new System.Drawing.Size(99, 17);
            this.maskVsMaskCheckBox.TabIndex = 1;
            this.maskVsMaskCheckBox.Text = "Mask Vs. Mask";
            this.maskVsMaskCheckBox.UseVisualStyleBackColor = true;
            // 
            // MatchSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(215, 206);
            this.Controls.Add(this.maskVsMaskCheckBox);
            this.Controls.Add(this.firstBloodCheckBox);
            this.Name = "MatchSelectionForm";
            this.Text = "MatchSelectionForm";
            this.Load += new System.EventHandler(this.MatchSelectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox firstBloodCheckBox;
        public System.Windows.Forms.CheckBox maskVsMaskCheckBox;
    }
}