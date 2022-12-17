
namespace GSK_74
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.comboBoxFigures = new System.Windows.Forms.ComboBox();
            this.comboBoxGeometric = new System.Windows.Forms.ComboBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.comboBoxTMO = new System.Windows.Forms.ComboBox();
            this.buttonTMO = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(484, 426);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureMouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMouseMove);
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {"Черный", "Красный", "Зеленый", "Синий"});
            this.comboBoxColor.Location = new System.Drawing.Point(585, 12);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxColor.TabIndex = 1;
            this.comboBoxColor.Text = "Цвет";
            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxColor_);
            // 
            // comboBoxFigures
            // 
            this.comboBoxFigures.FormattingEnabled = true;
            this.comboBoxFigures.Items.AddRange(new object[] {"Кубический сплайн", "Параллелограмм", "Флаг"});
            this.comboBoxFigures.Location = new System.Drawing.Point(585, 66);
            this.comboBoxFigures.Name = "comboBoxFigures";
            this.comboBoxFigures.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFigures.TabIndex = 2;
            this.comboBoxFigures.Text = "Фигура";
            this.comboBoxFigures.SelectedIndexChanged += new System.EventHandler(this.comboBoxFigures_);
            // 
            // comboBoxGeometric
            // 
            this.comboBoxGeometric.FormattingEnabled = true;
            this.comboBoxGeometric.Items.AddRange(new object[] {"Закрашивание", "Поворот вокруг заданного центра относительно заданного центра", "Зеркальное отражение относительно заданного центра", "Зеркальное отражение относительно горизонтальной прямой", "Перемещение"});
            this.comboBoxGeometric.Location = new System.Drawing.Point(502, 260);
            this.comboBoxGeometric.Name = "comboBoxGeometric";
            this.comboBoxGeometric.Size = new System.Drawing.Size(294, 21);
            this.comboBoxGeometric.TabIndex = 3;
            this.comboBoxGeometric.Text = "Геометрическое преобразование";
            this.comboBoxGeometric.SelectedIndexChanged += new System.EventHandler(this.comboBoxGeometric_SelectedIndexChanged);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(598, 415);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Очистить";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_);
            // 
            // comboBoxTMO
            // 
            this.comboBoxTMO.FormattingEnabled = true;
            this.comboBoxTMO.Items.AddRange(new object[] {"Объединение", "Симметричная разность"});
            this.comboBoxTMO.Location = new System.Drawing.Point(512, 155);
            this.comboBoxTMO.Name = "comboBoxTMO";
            this.comboBoxTMO.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTMO.TabIndex = 5;
            this.comboBoxTMO.Text = "ТМО";
            this.comboBoxTMO.SelectedIndexChanged += new System.EventHandler(this.ComboBoxTmo);
            // 
            // buttonTMO
            // 
            this.buttonTMO.Location = new System.Drawing.Point(512, 182);
            this.buttonTMO.Name = "buttonTMO";
            this.buttonTMO.Size = new System.Drawing.Size(75, 23);
            this.buttonTMO.TabIndex = 6;
            this.buttonTMO.Text = "Применить ТМО";
            this.buttonTMO.UseVisualStyleBackColor = true;
            this.buttonTMO.Click += new System.EventHandler(this.ButtonTmo);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(502, 320);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonTMO);
            this.Controls.Add(this.comboBoxTMO);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.comboBoxGeometric);
            this.Controls.Add(this.comboBoxFigures);
            this.Controls.Add(this.comboBoxColor);
            this.Controls.Add(this.pictureBox1);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "Form1";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBoxMouseMove);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.Button buttonTMO;

        private System.Windows.Forms.ComboBox comboBoxTMO;

        private System.Windows.Forms.Button buttonClear;

        private System.Windows.Forms.ComboBox comboBoxGeometric;

        private System.Windows.Forms.ComboBox comboBoxFigures;

        private System.Windows.Forms.ComboBox comboBoxColor;

        private System.Windows.Forms.PictureBox pictureBox1;

        #endregion
    }
}

