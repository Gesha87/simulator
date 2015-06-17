namespace FootballSimulator
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.dataGridViewResults = new FootballSimulator.Controls.TransparentDataGridView();
            this.countryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataDataSet = new FootballSimulator.dataDataSet();
            this.countryTableAdapter = new FootballSimulator.dataDataSetTableAdapters.CountryTableAdapter();
            this.labelStats = new System.Windows.Forms.Label();
            this.checkBoxStats = new System.Windows.Forms.CheckBox();
            this.listBoxCountry = new FootballSimulator.Controls.TransparentListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonClose.Location = new System.Drawing.Point(1023, 15);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(143, 42);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Выход";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.Color.Transparent;
            this.buttonReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonReset.Location = new System.Drawing.Point(17, 15);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(143, 42);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Сброс";
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSimulate.Location = new System.Drawing.Point(176, 15);
            this.buttonSimulate.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(143, 42);
            this.buttonSimulate.TabIndex = 2;
            this.buttonSimulate.Text = "Запуск";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.AllowUserToResizeColumns = false;
            this.dataGridViewResults.AllowUserToResizeRows = false;
            this.dataGridViewResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewResults.CausesValidation = false;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewResults.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewResults.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewResults.EnableHeadersVisualStyles = false;
            this.dataGridViewResults.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewResults.Location = new System.Drawing.Point(215, 74);
            this.dataGridViewResults.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewResults.MultiSelect = false;
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.HotPink;
            this.dataGridViewResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewResults.RowHeadersVisible = false;
            this.dataGridViewResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewResults.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewResults.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResults.ShowCellToolTips = false;
            this.dataGridViewResults.Size = new System.Drawing.Size(378, 255);
            this.dataGridViewResults.TabIndex = 3;
            this.dataGridViewResults.Visible = false;
            this.dataGridViewResults.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewResults_CellPainting);
            // 
            // countryBindingSource
            // 
            this.countryBindingSource.DataMember = "Country";
            this.countryBindingSource.DataSource = this.dataDataSet;
            // 
            // dataDataSet
            // 
            this.dataDataSet.DataSetName = "dataDataSet";
            this.dataDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // countryTableAdapter
            // 
            this.countryTableAdapter.ClearBeforeFill = true;
            // 
            // labelStats
            // 
            this.labelStats.AutoSize = true;
            this.labelStats.BackColor = System.Drawing.Color.YellowGreen;
            this.labelStats.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStats.Location = new System.Drawing.Point(215, 74);
            this.labelStats.Name = "labelStats";
            this.labelStats.Padding = new System.Windows.Forms.Padding(10);
            this.labelStats.Size = new System.Drawing.Size(78, 38);
            this.labelStats.TabIndex = 6;
            this.labelStats.Text = "Пусто";
            this.labelStats.Visible = false;
            // 
            // checkBoxStats
            // 
            this.checkBoxStats.AutoSize = true;
            this.checkBoxStats.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxStats.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBoxStats.Location = new System.Drawing.Point(330, 23);
            this.checkBoxStats.Name = "checkBoxStats";
            this.checkBoxStats.Size = new System.Drawing.Size(146, 29);
            this.checkBoxStats.TabIndex = 7;
            this.checkBoxStats.Text = "Статистика";
            this.checkBoxStats.UseVisualStyleBackColor = false;
            this.checkBoxStats.CheckedChanged += new System.EventHandler(this.checkBoxStats_CheckedChanged);
            // 
            // listBoxCountry
            // 
            this.listBoxCountry.BackColor = System.Drawing.Color.Transparent;
            this.listBoxCountry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxCountry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listBoxCountry.DataSource = this.countryBindingSource;
            this.listBoxCountry.DisplayMember = "name";
            this.listBoxCountry.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxCountry.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxCountry.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.listBoxCountry.FormattingEnabled = true;
            this.listBoxCountry.ItemHeight = 34;
            this.listBoxCountry.Location = new System.Drawing.Point(17, 74);
            this.listBoxCountry.Name = "listBoxCountry";
            this.listBoxCountry.Size = new System.Drawing.Size(180, 680);
            this.listBoxCountry.TabIndex = 8;
            this.listBoxCountry.ValueMember = "id";
            this.listBoxCountry.SelectedIndexChanged += new System.EventHandler(this.listBoxCountry_SelectedIndexChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.YellowGreen;
            this.BackgroundImage = global::FootballSimulator.Properties.Resources.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1183, 788);
            this.Controls.Add(this.listBoxCountry);
            this.Controls.Add(this.checkBoxStats);
            this.Controls.Add(this.labelStats);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.buttonSimulate);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonClose);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Футбольный симулятор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSimulate;
        private FootballSimulator.Controls.TransparentDataGridView dataGridViewResults;
        private dataDataSet dataDataSet;
        private System.Windows.Forms.BindingSource countryBindingSource;
        private dataDataSetTableAdapters.CountryTableAdapter countryTableAdapter;
        private System.Windows.Forms.Label labelStats;
        private System.Windows.Forms.CheckBox checkBoxStats;
        private FootballSimulator.Controls.TransparentListBox listBoxCountry;
    }
}

