namespace Task_1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            progressBar = new ProgressBar();
            txtResultSearch = new TextBox();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.AutoSize = true;
            btnStart.Location = new Point(360, 384);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 25);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += BtnStart_Click;
            // 
            // progressBar
            // 
            progressBar.BackColor = SystemColors.ActiveCaption;
            progressBar.ForeColor = Color.GreenYellow;
            progressBar.Location = new Point(9, 415);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(776, 23);
            progressBar.TabIndex = 1;
            // 
            // txtResultSearch
            // 
            txtResultSearch.Location = new Point(9, 12);
            txtResultSearch.Multiline = true;
            txtResultSearch.Name = "txtResultSearch";
            txtResultSearch.ReadOnly = true;
            txtResultSearch.ScrollBars = ScrollBars.Vertical;
            txtResultSearch.Size = new Size(776, 366);
            txtResultSearch.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtResultSearch);
            Controls.Add(progressBar);
            Controls.Add(btnStart);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private ProgressBar progressBar;
        private TextBox txtResultSearch;
    }
}