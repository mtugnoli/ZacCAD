﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;
using lcColor = ZacCAD.Colors.Color;
using lcLineType = ZacCAD.DatabaseServices.LineType;

namespace ZacCAD.Windows
{
    internal partial class LayerItemForm : Form
    {
        private readonly Layer _layer = null;
        private lcColor _lastColor;
        private lcLineType _lastLineType;
        private readonly Database _database;

        /// <summary>
        /// 新增还是修改
        /// </summary>
        public enum Mode
        {
            Add = 0,
            Modify = 1,
        }
        private Mode _mode = Mode.Add;

        //
        public LayerItemForm(Mode mode, Layer layer, Database database)
        {
            _mode = mode;
            _database = database;
            if (_mode == Mode.Modify)
            {
                _layer = layer;
            }
            else
            {
                _layer = new Layer();
                _layer.name = "";
                _layer.description = "";
                _layer.color = lcColor.FromRGB(255, 255, 255);
            }

            InitializeComponent();
            InitializePredefinedColors();
            InitializePredefinedLineTypes();

            if (_layer != null)
            {
                this.textboxName.Text = _layer.name;
                this.textboxDescription.Text = _layer.description;
            }

            InitializeColorCombo();
            InitializeLineTypeCombo();

            this.Text = GlobalData.GlobalLanguage.Document_LayerItemTitle;
            this.lblLayerItemLayerName.Text = GlobalData.GlobalLanguage.Document_LayerItemLayerName;
            this.lblLayerItemLayerDesc.Text = GlobalData.GlobalLanguage.Document_LayerItemLayerDesc;
            this.lblLayerItemLayerColor.Text = GlobalData.GlobalLanguage.Document_LayerItemLayerColor;

            this.btnOK.Text = GlobalData.GlobalLanguage.Button_Ok;
            this.btnCancel.Text = GlobalData.GlobalLanguage.Button_Cancel;
        }

        /// <summary>
        /// 预制颜色
        /// </summary>
        private Dictionary<lcColor, string> _predefinedColors = new Dictionary<lcColor, string>();
        private Dictionary<lcLineType, string> _predefinedLineTypes = new Dictionary<lcLineType, string>();

        private void InitializePredefinedColors()
        {
            _predefinedColors.Add(lcColor.FromRGB(255, 0, 0), GlobalData.GlobalLanguage.Color_Red);
            _predefinedColors.Add(lcColor.FromRGB(255, 255, 0), GlobalData.GlobalLanguage.Color_Yellow);
            _predefinedColors.Add(lcColor.FromRGB(0, 255, 0), GlobalData.GlobalLanguage.Color_Green);
            _predefinedColors.Add(lcColor.FromRGB(0, 255, 255), GlobalData.GlobalLanguage.Color_Cyan);
            _predefinedColors.Add(lcColor.FromRGB(0, 0, 255), GlobalData.GlobalLanguage.Color_Blue);
            _predefinedColors.Add(lcColor.FromRGB(255, 0, 255), GlobalData.GlobalLanguage.Color_Magenta);
            _predefinedColors.Add(lcColor.FromRGB(255, 255, 255), GlobalData.GlobalLanguage.Color_White);

            if (_layer != null)
            {
                if (!_predefinedColors.ContainsKey(_layer.color))
                {
                    _predefinedColors.Add(_layer.color, _layer.color.Name);
                }
            }
        }

        private void InitializePredefinedLineTypes()
        {
            _predefinedLineTypes.Add(lcLineType.Solid, GlobalData.GlobalLanguage.LineType_Solid);
            _predefinedLineTypes.Add(lcLineType.Dash, GlobalData.GlobalLanguage.LineType_Dash);
            _predefinedLineTypes.Add(lcLineType.Dot, GlobalData.GlobalLanguage.LineType_Dot);
            _predefinedLineTypes.Add(lcLineType.DashDot, GlobalData.GlobalLanguage.LineType_DashDot);
            _predefinedLineTypes.Add(lcLineType.DashDotDot, GlobalData.GlobalLanguage.LineType_DashDotDot);
            _predefinedLineTypes.Add(lcLineType.Custom, GlobalData.GlobalLanguage.LineType_Custom);

            //if (_layer != null)
            //{
            //    if (!_predefinedLineTypes.ContainsKey(_layer.lineType))
            //    {
            //        _predefinedLineTypes.Add(_layer.lineType, _layer.lineType.ToString());
            //    }
            //}
        }

        /// <summary>
        /// Color combobox
        /// </summary>
        private int _indexToInsertCustomColor = -1;
        private void InitializeColorCombo()
        {
            // predefined colors
            foreach (KeyValuePair<lcColor, string> kvp in _predefinedColors)
            {
                ColorItem colorItem = new ColorItem();
                colorItem.color = kvp.Key;
                colorItem.text = kvp.Value;

                this.comboColor.Items.Add(colorItem);
            }
            _indexToInsertCustomColor = this.comboColor.Items.Count;

            // select custom color
            ColorItem selectColorItem = new ColorItem();
            selectColorItem.text = null;
            this.comboColor.Items.Add(selectColorItem);

            //
            _lastColor = _layer.color;
            this.SetColorComboValue(_lastColor);
            this.comboColor.SelectedIndexChanged += this.OnColorComboSelectedIndexChanged;
        }

        private void SetColorComboValue(lcColor color)
        {
            int index = -1;
            for (int i = 0; i < this.comboColor.Items.Count; ++i)
            {
                ColorItem colorItem = this.comboColor.Items[i] as ColorItem;
                if (colorItem.text != null)
                {
                    if (colorItem.color == color)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
            {
                ColorItem colorItem = new ColorItem();
                colorItem.color = color;
                this.comboColor.Items.Insert(_indexToInsertCustomColor, colorItem);
                index = _indexToInsertCustomColor;
            }

            this.comboColor.SelectedIndex = index;
        }

        private void OnColorComboSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboColor.SelectedItem == null)
            {
                return;
            }

            ColorItem selColorItem = this.comboColor.SelectedItem as ColorItem;
            if (selColorItem.text == null)
            {
                ColorDialog colorDlg = new ColorDialog();
                colorDlg.AllowFullOpen = true;
                colorDlg.SolidColorOnly = true;
                DialogResult dlgRet = colorDlg.ShowDialog();
                if (dlgRet == DialogResult.OK)
                {
                    lcColor color = lcColor.FromColor(colorDlg.Color);
                    _lastColor = color;
                    this.SetColorComboValue(color);
                }
                else
                {
                    this.SetColorComboValue(_lastColor);
                }
            }
            else
            {
                _lastColor = selColorItem.color;
            }
        }

        private class ColorItem
        {
            public lcColor color;
            public string text = "";

            public override string ToString()
            {
                if (text == null)
                {
                    return GlobalData.GlobalLanguage.Color_Choose;
                }
                else if (text == "")
                {
                    return color.Name;
                }
                else
                {
                    return text;
                }
            }
        }



        private int _indexToInsertCustomLineType = -1;
        private void InitializeLineTypeCombo()
        {
            foreach (var item in _predefinedLineTypes.Where(o => o.Key >= 0))
            {
                LineTypeItem linetypeItem = new LineTypeItem();
                linetypeItem.linetype = (lcLineType)item.Key;
                linetypeItem.text = item.Value;

                this.comboLineType.Items.Add(linetypeItem);

            }

            _indexToInsertCustomLineType = this.comboLineType.Items.Count;

            //// select custom color
            //LineTypeItem selectLineTypeItem = new LineTypeItem();
            //selectLineTypeItem.text = null;
            //this.comboLineType.Items.Add(selectLineTypeItem);

            _lastLineType = _layer.lineType;
            this.SetLineTypeComboValue(_lastLineType);
            this.comboLineType.SelectedIndexChanged += this.OnLineTypeComboSelectedIndexChanged;
        }

        private void SetLineTypeComboValue(lcLineType linetype)
        {
            int index = -1;
            for (int i = 0; i < this.comboLineType.Items.Count; ++i)
            {
                LineTypeItem linetypeItem = this.comboLineType.Items[i] as LineTypeItem;
                if (linetypeItem.text != null)
                {
                    if (linetypeItem.linetype == linetype)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
            {
                LineTypeItem linetypeItem = new LineTypeItem();
                linetypeItem.linetype = linetype;
                this.comboLineType.Items.Insert(_indexToInsertCustomLineType, linetypeItem);
                index = _indexToInsertCustomLineType;
            }

            this.comboLineType.SelectedIndex = index;
        }

        private void OnLineTypeComboSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboLineType.SelectedItem == null)
            {
                return;
            }

            LineTypeItem selectLineTypeItem = this.comboLineType.SelectedItem as LineTypeItem;

            _lastLineType = selectLineTypeItem.linetype;
        }

        private class LineTypeItem
        {
            public lcLineType linetype;
            public string text = "";

            public override string ToString()
            {
                if (text == "")
                {
                    return linetype.ToString();
                }
                else
                {
                    return text;
                }
            }
        }


        private string layerName
        {
            get
            {
                return this.textboxName.Text.Trim();
            }
        }

        private string layerDescription
        {
            get
            {
                return this.textboxDescription.Text.Trim();
            }
        }

        private lcColor layerColor
        {
            get
            {
                if (this.comboColor.SelectedItem == null)
                {
                    return lcColor.FromRGB(255, 255, 255);
                }

                ColorItem colorItem = this.comboColor.SelectedItem as ColorItem;
                return colorItem.color;
            }
        }

        private lcLineType layerLineType
        {
            get
            {
                if (this.comboLineType.SelectedItem == null)
                {
                    return lcLineType.Solid;
                }

                LineTypeItem selectLineTypeItem = this.comboLineType.SelectedItem as LineTypeItem;
                return selectLineTypeItem.linetype;
            }
        }

        /// <summary>
        /// 图层
        /// </summary>
        internal Layer layer
        {
            get
            {
                Layer layer = _layer.Clone() as Layer;
                layer.name = this.layerName;
                layer.color = this.layerColor;
                layer.lineType = this.layerLineType;
                layer.description = this.layerDescription;
                return layer;
            }
        }

        /// <summary>
        /// OK button
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.layerName == "")
            {
                MessageBox.Show("The layer name cannot be empty");
                return;
            }

            foreach (Layer layer in _database.layerTable)
            {
                if (layer != _layer)
                {
                    if (layer.name == this.layerName)
                    {
                        string msg = string.Format("Layer name: {0} already exists", this.layerName);
                        MessageBox.Show(msg);
                        return;
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancel button
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
