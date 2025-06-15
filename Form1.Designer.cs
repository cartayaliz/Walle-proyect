namespace walleproyect
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.error_board = new System.Windows.Forms.ErrorProvider(this.components);
            this.error_size = new System.Windows.Forms.ErrorProvider(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lector = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.error_time = new System.Windows.Forms.ErrorProvider(this.components);
            this.time = new System.Windows.Forms.NumericUpDown();
            this.actual = new System.Windows.Forms.TextBox();
            this.size = new System.Windows.Forms.NumericUpDown();
            this.board = new System.Windows.Forms.NumericUpDown();
            this.colors = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.logText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExecuterTime = new System.Windows.Forms.NumericUpDown();
            this.lineTexts = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_board)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.board)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExecuterTime)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.button1.Font = new System.Drawing.Font("Microsoft Uighur", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1040, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(20, 100);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 500);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Uighur", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(318, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Uighur", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(156, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 29);
            this.label3.TabIndex = 6;
            this.label3.Text = "Brush:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Uighur", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 29);
            this.label4.TabIndex = 9;
            this.label4.Text = "Board:";
            // 
            // error_board
            // 
            this.error_board.ContainerControl = this;
            // 
            // error_size
            // 
            this.error_size.ContainerControl = this;
            // 
            // button2
            // 
            this.button2.AllowDrop = true;
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.button2.Font = new System.Drawing.Font("Microsoft Uighur", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(1350, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 38);
            this.button2.TabIndex = 10;
            this.button2.Text = "Reset";
            this.button2.UseCompatibleTextRendering = true;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.button3.Font = new System.Drawing.Font("Microsoft Uighur", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(1125, 26);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 38);
            this.button3.TabIndex = 11;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button4.BackgroundImage")));
            this.button4.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.button4.Font = new System.Drawing.Font("Microsoft Uighur", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(1450, 26);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 37);
            this.button4.TabIndex = 12;
            this.button4.Text = "Close";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lector
            // 
            this.lector.AcceptsTab = true;
            this.lector.Location = new System.Drawing.Point(999, 123);
            this.lector.Name = "lector";
            this.lector.Size = new System.Drawing.Size(534, 343);
            this.lector.TabIndex = 13;
            this.lector.Text = "";
            this.lector.VScroll += new System.EventHandler(this.lector_VScroll);
            this.lector.TextChanged += new System.EventHandler(this.lector_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label5.Location = new System.Drawing.Point(438, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 32);
            this.label5.TabIndex = 14;
            this.label5.Text = "Painting\r\n  Time";
            // 
            // error_time
            // 
            this.error_time.ContainerControl = this;
            // 
            // time
            // 
            this.time.Location = new System.Drawing.Point(510, 27);
            this.time.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(56, 22);
            this.time.TabIndex = 16;
            this.time.ValueChanged += new System.EventHandler(this.time_ValueChanged);
            // 
            // actual
            // 
            this.actual.Location = new System.Drawing.Point(966, 472);
            this.actual.Name = "actual";
            this.actual.Size = new System.Drawing.Size(576, 22);
            this.actual.TabIndex = 17;
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(377, 27);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(41, 22);
            this.size.TabIndex = 18;
            // 
            // board
            // 
            this.board.Location = new System.Drawing.Point(89, 26);
            this.board.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.board.Name = "board";
            this.board.Size = new System.Drawing.Size(49, 22);
            this.board.TabIndex = 19;
            // 
            // colors
            // 
            this.colors.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.colors.FormattingEnabled = true;
            this.colors.Items.AddRange(new object[] {
            "White",
            "Red",
            "Blue",
            "Green",
            "Black",
            "Yellow",
            "Orange",
            "Purple",
            "Transparent",
            "AzulCielo"});
            this.colors.Location = new System.Drawing.Point(229, 26);
            this.colors.Name = "colors";
            this.colors.Size = new System.Drawing.Size(70, 24);
            this.colors.TabIndex = 20;
            // 
            // button5
            // 
            this.button5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button5.BackgroundImage")));
            this.button5.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.button5.Font = new System.Drawing.Font("Microsoft Uighur", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(1240, 26);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 38);
            this.button5.TabIndex = 21;
            this.button5.Text = "Upload";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // logText
            // 
            this.logText.Location = new System.Drawing.Point(966, 510);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logText.Size = new System.Drawing.Size(576, 151);
            this.logText.TabIndex = 22;
            this.logText.TextChanged += new System.EventHandler(this.logText_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(591, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 32);
            this.label2.TabIndex = 23;
            this.label2.Text = "Executer Time";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ExecuterTime
            // 
            this.ExecuterTime.Location = new System.Drawing.Point(683, 30);
            this.ExecuterTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.ExecuterTime.Name = "ExecuterTime";
            this.ExecuterTime.Size = new System.Drawing.Size(56, 22);
            this.ExecuterTime.TabIndex = 24;
            // 
            // lineTexts
            // 
            this.lineTexts.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lineTexts.Location = new System.Drawing.Point(966, 123);
            this.lineTexts.Name = "lineTexts";
            this.lineTexts.ReadOnly = true;
            this.lineTexts.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.lineTexts.Size = new System.Drawing.Size(27, 343);
            this.lineTexts.TabIndex = 25;
            this.lineTexts.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.checkBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("checkBox1.BackgroundImage")));
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Uighur", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(970, 680);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 33);
            this.checkBox1.TabIndex = 26;
            this.checkBox1.Text = "Debug";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1366, 662);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lineTexts);
            this.Controls.Add(this.ExecuterTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logText);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.colors);
            this.Controls.Add(this.board);
            this.Controls.Add(this.size);
            this.Controls.Add(this.actual);
            this.Controls.Add(this.time);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lector);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_board)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.board)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExecuterTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider error_board;
        private System.Windows.Forms.ErrorProvider error_size;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox lector;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ErrorProvider error_time;
        private System.Windows.Forms.NumericUpDown time;
        protected System.Windows.Forms.TextBox actual;
        private System.Windows.Forms.NumericUpDown size;
        private System.Windows.Forms.NumericUpDown board;
        private System.Windows.Forms.ComboBox colors;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox logText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ExecuterTime;
        private System.Windows.Forms.RichTextBox lineTexts;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

