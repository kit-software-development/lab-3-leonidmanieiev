namespace Escape_Button
{
    partial class mainForm
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
            this.pushMeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pushMeButton
            // 
            this.pushMeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pushMeButton.Location = new System.Drawing.Point(154, 94);
            this.pushMeButton.Name = "pushMeButton";
            this.pushMeButton.Size = new System.Drawing.Size(75, 23);
            this.pushMeButton.TabIndex = 0;
            this.pushMeButton.Text = "Push me";
            this.pushMeButton.UseVisualStyleBackColor = true;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.pushMeButton);
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "mainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Running Button";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button pushMeButton;
    }
}

