namespace RomashkaParser
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
            this.button1 = new Button();
            this.textBoxCandels = new TextBox();
            this.textBoxOffsetMinimal = new TextBox();
            this.textBoxRR = new TextBox();
            this.textBoxTakeProfit = new TextBox();
            this.buttonStart = new Button();
            this.buttonDates = new Button();
            this.label1 = new Label();
            this.buttonBtcSigs = new Button();
            this.buttonEthSigs = new Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new Point(713, 12);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Парсинг";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // textBoxCandels
            // 
            this.textBoxCandels.Location = new Point(12, 13);
            this.textBoxCandels.Name = "textBoxCandels";
            this.textBoxCandels.Size = new Size(100, 23);
            this.textBoxCandels.TabIndex = 1;
            this.textBoxCandels.Text = "5";
            this.textBoxCandels.TextChanged += this.textBoxCandels_TextChanged;
            // 
            // textBoxOffsetMinimal
            // 
            this.textBoxOffsetMinimal.Location = new Point(12, 42);
            this.textBoxOffsetMinimal.Name = "textBoxOffsetMinimal";
            this.textBoxOffsetMinimal.Size = new Size(100, 23);
            this.textBoxOffsetMinimal.TabIndex = 2;
            this.textBoxOffsetMinimal.Text = "100";
            // 
            // textBoxRR
            // 
            this.textBoxRR.Location = new Point(12, 71);
            this.textBoxRR.Name = "textBoxRR";
            this.textBoxRR.Size = new Size(100, 23);
            this.textBoxRR.TabIndex = 3;
            this.textBoxRR.Text = "3";
            // 
            // textBoxTakeProfit
            // 
            this.textBoxTakeProfit.Location = new Point(12, 100);
            this.textBoxTakeProfit.Name = "textBoxTakeProfit";
            this.textBoxTakeProfit.Size = new Size(100, 23);
            this.textBoxTakeProfit.TabIndex = 4;
            this.textBoxTakeProfit.Text = "3";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new Point(12, 129);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(100, 23);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Тест";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += this.buttonStart_Click;
            // 
            // buttonDates
            // 
            this.buttonDates.Location = new Point(713, 41);
            this.buttonDates.Name = "buttonDates";
            this.buttonDates.Size = new Size(75, 23);
            this.buttonDates.TabIndex = 6;
            this.buttonDates.Text = "Котировки";
            this.buttonDates.UseVisualStyleBackColor = true;
            this.buttonDates.Click += this.buttonDates_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(218, 16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(38, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            // 
            // buttonBtcSigs
            // 
            this.buttonBtcSigs.Location = new Point(218, 42);
            this.buttonBtcSigs.Name = "buttonBtcSigs";
            this.buttonBtcSigs.Size = new Size(75, 23);
            this.buttonBtcSigs.TabIndex = 10;
            this.buttonBtcSigs.Text = "BTCUSDT";
            this.buttonBtcSigs.UseVisualStyleBackColor = true;
            this.buttonBtcSigs.Click += this.buttonBtcSigs_Click;
            // 
            // buttonEthSigs
            // 
            this.buttonEthSigs.Location = new Point(299, 42);
            this.buttonEthSigs.Name = "buttonEthSigs";
            this.buttonEthSigs.Size = new Size(75, 23);
            this.buttonEthSigs.TabIndex = 11;
            this.buttonEthSigs.Text = "ETHUSDT";
            this.buttonEthSigs.UseVisualStyleBackColor = true;
            this.buttonEthSigs.Click += this.buttonEthSigs_Click;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.buttonEthSigs);
            this.Controls.Add(this.buttonBtcSigs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDates);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxTakeProfit);
            this.Controls.Add(this.textBoxRR);
            this.Controls.Add(this.textBoxOffsetMinimal);
            this.Controls.Add(this.textBoxCandels);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += this.Form1_Load;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBoxCandels;
        private TextBox textBoxOffsetMinimal;
        private TextBox textBoxRR;
        private TextBox textBoxTakeProfit;
        private Button buttonStart;
        private Button buttonDates;
        private Label label1;
        private Button buttonBtcSigs;
        private Button buttonEthSigs;
    }
}
