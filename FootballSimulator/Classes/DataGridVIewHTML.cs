﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace FootballSimulator.Classes
{
    public enum HTMLOverflow { NONE, ELIPSIS, RED_TRIANGLE };

    public class DataGridViewHTMLColumn : DataGridViewColumn
    {
        public DataGridViewHTMLColumn()
            : base(new DataGridViewHTMLCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (!(value is DataGridViewHTMLCell))
                    throw new InvalidCastException("CellTemplate must be a DataGridViewHTMLCell");

                base.CellTemplate = value;
            }
        }
    }

    public class DataGridViewHTMLCell : DataGridViewCell
    {
        public HTMLOverflow m_htmlOverflowType = HTMLOverflow.NONE;

        private readonly HtmlLabel m_editingControl = new HtmlLabel();

        private string m_htmlText = "";

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewHTMLEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
            set
            {
                base.ValueType = value;
            }
        }

        public override Type FormattedValueType
        {
            get
            {
                return typeof(string);
            }
        }

        private void SetHTMLPanelText(HtmlLabel ctl, string text)
        {
            try
            {
                ctl.Text = text;
                m_htmlText = text;
            }
            catch (ArgumentException)
            {
                ctl.Text = text;
            }
        }

        private Image GenerateHTMLImage(int rowIndex, object value, bool selected)
        {
            Size cellSize = GetSize(rowIndex);

            if (cellSize.Width < 1 || cellSize.Height < 1)
                return null;

            m_editingControl.Size = GetSize(rowIndex);
            SetHTMLPanelText(m_editingControl, Convert.ToString(value));

            if (m_editingControl != null)
            {
                Image htmlImage = null; Size fullImageSize;

                if (RenderHTMLImage(Convert.ToString(value), cellSize, out htmlImage, out fullImageSize) == false)
                {
                    Console.WriteLine("Failed to Generate HTML image");
                    return htmlImage;
                }

                htmlImage = Add1pxBottomBorder(htmlImage);

                return htmlImage;
            }

            return null;
        }

        Image Add1pxBottomBorder(Image img)
        {
            Console.WriteLine("Add1pxBorder");

            Graphics g = Graphics.FromImage(img);

            Color borderC = Color.FromArgb(171, 171, 171);

            int linePos = img.Height - 1;

            g.DrawLine(new Pen(borderC, 1), new Point(0, linePos), new Point(img.Width, linePos));

            return img;
        }

        Image Add1pxBorder(Image img)
        {
            Console.WriteLine("Add1pxBorder");
            Graphics g = Graphics.FromImage(img);

            g.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 0), new Point(0, img.Width));
            g.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 0), new Point(img.Width, 0));
            g.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 40), new Point(40, 40));
            g.DrawLine(new Pen(Brushes.Black, 1), new Point(40, 0), new Point(40, 40));

            return img;
        }

        private bool RenderHTMLImage(string htmlText, Size cellSize, out Image htmlImage, out Size fullImageSize)
        {
            Console.WriteLine("RenderHTMLImage");

            bool bResult = true;

            // Step 1: Set out parameters
            htmlImage = null;
            fullImageSize = new System.Drawing.Size();

            try
            {
                // Step 2: Check for numm html text
                if (string.IsNullOrEmpty(htmlText) == true)
                    htmlText = "This <b>cell</b> has a <span style=\"color: red\">null</span> value!";

                // Need to render image twice! Once to get the full size of image and once to get image clipped to the size of the cell

                // Step 3: Render the html image using the full height but keep the cell width.
                htmlImage = HtmlRender.RenderToImage(htmlText, maxWidth: cellSize.Width);

                // Step 4: Keep a record of the full image size to send back to caller.
                fullImageSize.Height = htmlImage.Height;
                fullImageSize.Width = htmlImage.Width;

                // Step 5: Render the HTML imaage a second time with the cell size width / height
                htmlImage = HtmlRender.RenderToImage(htmlText, new Size(cellSize.Width, cellSize.Height));

                m_editingControl.Text = htmlText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bResult = false;
            }

            return bResult;
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            HtmlLabel ctl = DataGridView.EditingControl as HtmlLabel;

            // initialFormattedValue = "This is an <b>HtmlLabel</b> control";
            if (ctl != null)
            {
                if (initialFormattedValue == null)
                {
                    string nullText = "This <b>cell</b> has a <span style=\"color: red\">null</span> value!";
                    SetHTMLPanelText(ctl, nullText);
                }
                else
                    SetHTMLPanelText(ctl, Convert.ToString(initialFormattedValue));
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return value;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, null, null, errorText, cellStyle, advancedBorderStyle, paintParts);

            Image img = GenerateHTMLImage(rowIndex, value, base.Selected);

            if (img != null)
            {
                graphics.DrawImage(img, cellBounds.Left, cellBounds.Top);
            }
        }

        #region Handlers of edit events, copyied from DataGridViewTextBoxCell

        private byte flagsState;

        protected override void OnEnter(int rowIndex, bool throughMouseClick)
        {
            base.OnEnter(rowIndex, throughMouseClick);

            if ((base.DataGridView != null) && throughMouseClick)
            {
                this.flagsState = (byte)(this.flagsState | 1);
            }
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            base.OnLeave(rowIndex, throughMouseClick);

            if (base.DataGridView != null)
            {
                this.flagsState = (byte)(this.flagsState & -2);
            }
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (base.DataGridView != null)
            {
                Point currentCellAddress = base.DataGridView.CurrentCellAddress;

                if (((currentCellAddress.X == e.ColumnIndex) && (currentCellAddress.Y == e.RowIndex)) && (e.Button == MouseButtons.Left))
                {
                    if ((this.flagsState & 1) != 0)
                    {
                        this.flagsState = (byte)(this.flagsState & -2);
                    }
                    else if (base.DataGridView.EditMode != DataGridViewEditMode.EditProgrammatically)
                    {
                        base.DataGridView.BeginEdit(false);
                    }
                }
            }
        }

        public override bool KeyEntersEditMode(KeyEventArgs e)
        {
            return (((((char.IsLetterOrDigit((char)((ushort)e.KeyCode)) && ((e.KeyCode < Keys.F1) || (e.KeyCode > Keys.F24))) || ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.Divide))) || (((e.KeyCode >= Keys.OemSemicolon) && (e.KeyCode <= Keys.OemBackslash)) || ((e.KeyCode == Keys.Space) && !e.Shift))) && (!e.Alt && !e.Control)) || base.KeyEntersEditMode(e));
        }

        #endregion
    }

    public class DataGridViewHTMLEditingControl : HtmlLabel, IDataGridViewEditingControl
    {
        private DataGridView m_dataGridView;
        private int m_rowIndex;
        private bool m_valueChanged;

        public DataGridViewHTMLEditingControl()
        {
            this.BorderStyle = BorderStyle.None;
            AutoSize = false;
        }

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return m_dataGridView;
            }
            set
            {
                m_dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value is string)
                    this.Text = value as string;
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return m_rowIndex;
            }
            set
            {
                m_rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return m_valueChanged;
            }
            set
            {
                m_valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Return:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }

            return !dataGridViewWantsInputKey;
        }

        public Cursor EditingPanelCursor
        {
            get { return this.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion
    }
}