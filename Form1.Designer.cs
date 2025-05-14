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
            this.textBoxRisk = new TextBox();
            this.buttonStart = new Button();
            this.buttonDates = new Button();
            this.label1 = new Label();
            this.buttonBtcSigs = new Button();
            this.buttonEthSigs = new Button();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.checkBoxLongs = new CheckBox();
            this.checkBoxShorts = new CheckBox();
            this.progressBar1 = new ProgressBar();
            this.label5 = new Label();
            this.button2 = new Button();
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
            this.textBoxCandels.Location = new Point(12, 194);
            this.textBoxCandels.Name = "textBoxCandels";
            this.textBoxCandels.Size = new Size(148, 23);
            this.textBoxCandels.TabIndex = 1;
            this.textBoxCandels.Text = "5";
            this.textBoxCandels.TextChanged += this.textBoxCandels_TextChanged;
            // 
            // textBoxOffsetMinimal
            // 
            this.textBoxOffsetMinimal.Location = new Point(12, 238);
            this.textBoxOffsetMinimal.Name = "textBoxOffsetMinimal";
            this.textBoxOffsetMinimal.Size = new Size(148, 23);
            this.textBoxOffsetMinimal.TabIndex = 2;
            this.textBoxOffsetMinimal.Text = "0,002";
            this.textBoxOffsetMinimal.TextChanged += this.textBoxOffsetMinimal_TextChanged;
            // 
            // textBoxRR
            // 
            this.textBoxRR.Location = new Point(12, 282);
            this.textBoxRR.Name = "textBoxRR";
            this.textBoxRR.Size = new Size(148, 23);
            this.textBoxRR.TabIndex = 3;
            this.textBoxRR.Text = "3";
            // 
            // textBoxRisk
            // 
            this.textBoxRisk.Location = new Point(12, 326);
            this.textBoxRisk.Name = "textBoxRisk";
            this.textBoxRisk.Size = new Size(148, 23);
            this.textBoxRisk.TabIndex = 4;
            this.textBoxRisk.Text = "100";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new Point(12, 355);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new Size(148, 23);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Тест";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += this.buttonStart_Click;
            // 
            // buttonDates
            // 
            this.buttonDates.Enabled = false;
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
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(38, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            // 
            // buttonBtcSigs
            // 
            this.buttonBtcSigs.Location = new Point(12, 35);
            this.buttonBtcSigs.Name = "buttonBtcSigs";
            this.buttonBtcSigs.Size = new Size(75, 23);
            this.buttonBtcSigs.TabIndex = 10;
            this.buttonBtcSigs.Text = "BTCUSDT";
            this.buttonBtcSigs.UseVisualStyleBackColor = true;
            this.buttonBtcSigs.Click += this.buttonBtcSigs_Click;
            // 
            // buttonEthSigs
            // 
            this.buttonEthSigs.Enabled = false;
            this.buttonEthSigs.Location = new Point(93, 35);
            this.buttonEthSigs.Name = "buttonEthSigs";
            this.buttonEthSigs.Size = new Size(75, 23);
            this.buttonEthSigs.TabIndex = 11;
            this.buttonEthSigs.Text = "ETHUSDT";
            this.buttonEthSigs.UseVisualStyleBackColor = true;
            this.buttonEthSigs.Click += this.buttonEthSigs_Click;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 176);
            this.label2.Name = "label2";
            this.label2.Size = new Size(138, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Количество сыечей, шт";
            this.label2.Click += this.label2_Click;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 220);
            this.label3.Name = "label3";
            this.label3.Size = new Size(139, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "Отступ от минимума, %";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(12, 264);
            this.label4.Name = "label4";
            this.label4.Size = new Size(114, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Риск к профиту, кф";
            // 
            // checkBoxLongs
            // 
            this.checkBoxLongs.AutoSize = true;
            this.checkBoxLongs.Location = new Point(20, 384);
            this.checkBoxLongs.Name = "checkBoxLongs";
            this.checkBoxLongs.Size = new Size(60, 19);
            this.checkBoxLongs.TabIndex = 15;
            this.checkBoxLongs.Text = "Лонги";
            this.checkBoxLongs.UseVisualStyleBackColor = true;
            this.checkBoxLongs.CheckedChanged += this.checkBoxLongs_CheckedChanged;
            // 
            // checkBoxShorts
            // 
            this.checkBoxShorts.AutoSize = true;
            this.checkBoxShorts.Location = new Point(86, 384);
            this.checkBoxShorts.Name = "checkBoxShorts";
            this.checkBoxShorts.Size = new Size(65, 19);
            this.checkBoxShorts.TabIndex = 16;
            this.checkBoxShorts.Text = "Шорты";
            this.checkBoxShorts.UseVisualStyleBackColor = true;
            this.checkBoxShorts.CheckedChanged += this.checkBoxShorts_CheckedChanged;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new Point(12, 415);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(776, 23);
            this.progressBar1.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(12, 308);
            this.label5.Name = "label5";
            this.label5.Size = new Size(101, 15);
            this.label5.TabIndex = 18;
            this.label5.Text = "Риск на сделку, $";
            // 
            // button2
            // 
            this.button2.Location = new Point(188, 355);
            this.button2.Name = "button2";
            this.button2.Size = new Size(120, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkBoxShorts);
            this.Controls.Add(this.checkBoxLongs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonEthSigs);
            this.Controls.Add(this.buttonBtcSigs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDates);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxRisk);
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
        private TextBox textBoxRisk;
        private Button buttonStart;
        private Button buttonDates;
        private Label label1;
        private Button buttonBtcSigs;
        private Button buttonEthSigs;
        private Label label2;
        private Label label3;
        private Label label4;
        private CheckBox checkBoxLongs;
        private CheckBox checkBoxShorts;
        private ProgressBar progressBar1;
        private Label label5;
        private Button button2;
    }
}
