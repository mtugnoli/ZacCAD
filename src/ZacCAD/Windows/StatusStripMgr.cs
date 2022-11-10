using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ZacCAD.Windows.Controls;
using ZacCAD.UI;

namespace ZacCAD.Windows
{
    public class StatusStripMgr
    {
        // Tool entry
        private List<ToolStripItem> _toolbarItems = new List<ToolStripItem>();

        // Statusbar
        private StatusStrip _statusStrip = new StatusStrip();
        private Panel _panelStrip;
        
        public StatusStripMgr()
        {

        }

        public StatusStrip GetStatusStrip(bool createWhenNotExist = true)
        {
            if (_statusStrip != null)
            {
                return _statusStrip;
            }

            if (createWhenNotExist)
            {
                StatusStrip statusStrip = new StatusStrip();
                _statusStrip = statusStrip;

                return statusStrip;
            }
            else
            {
                return null;
            }
        }


        public void AppendCommandLine(string text)
        {
            TextBox textbox = (TextBox)_panelStrip.Controls["textboxHistory"];

            textbox.AppendText(text + "\r\n");
        }

        public void SetCommandLabel(string text)
        {
            ToolStripLabel label = (ToolStripLabel)_statusStrip.Items["toolStripLabelCommand"];

            label.Text = text;
        }

        public void SetCommandInfoLabel(string text)
        {
            ToolStripLabel label = (ToolStripLabel)_statusStrip.Items["toolStripLabelCommandInfo"];

            label.Text = text;
        }

        public void CommandTextFocus()
        {
            ToolStripTextBox _xTextBox = (ToolStripTextBox)_statusStrip.Items["toolStripTextBoxCommands"];

            _xTextBox.Focus();
            _xTextBox.Select(_xTextBox.Text.Length, 0);
        }

        public void CommandFinish()
        {
            ToolStripLabel labelCmd = (ToolStripLabel)_statusStrip.Items["toolStripLabelCommand"];
            ToolStripLabel labelInfo = (ToolStripLabel)_statusStrip.Items["toolStripLabelCommandInfo"];
            ToolStripTextBox _xTextBox = (ToolStripTextBox)_statusStrip.Items["toolStripTextBoxCommands"];

            labelCmd.Text = "";
            labelInfo.Text = "";
            _xTextBox.Text = "";
        }


        public StatusStrip GetStatusStrip(DockStyle dockStyle, bool createWhenNotExist = true)
        {
            if (_statusStrip != null)
            {
                return _statusStrip;
            }

            if (createWhenNotExist)
            {
                StatusStrip statusStrip = new StatusStrip();
                statusStrip.Dock = dockStyle;
                _statusStrip = statusStrip;

                return statusStrip;
            }
            else
            {
                return null;
            }
        }

        public Panel GetPanel(DockStyle dockStyle, bool createWhenNotExist = true)
        {
            if (_panelStrip != null)
            {
                return _panelStrip;
            }

            if (createWhenNotExist)
            {
                TextBox _textboxHistory = new TextBox();
                _textboxHistory.BackColor = System.Drawing.Color.Gainsboro;
                _textboxHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
                _textboxHistory.Dock = System.Windows.Forms.DockStyle.Fill;
                _textboxHistory.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _textboxHistory.Location = new System.Drawing.Point(0, 0);
                _textboxHistory.Multiline = true;
                _textboxHistory.Name = "textboxHistory";

                Panel _panelHistory = new Panel();
                _panelHistory.Name = "panelHistory";
                _panelHistory.BackColor = System.Drawing.Color.Gainsboro;
                _panelHistory.Size = new System.Drawing.Size(100, 100);
                _panelHistory.Dock = dockStyle;
                _panelHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                _panelHistory.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
                _panelHistory.AutoScroll = true;
                _panelHistory.Controls.Add(_textboxHistory);
                _panelHistory.Visible = false;

                _panelStrip = _panelHistory;

                return _panelHistory;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// New tool entry
        /// </summary>
        public ToolStripButton NewToolStripButton(string text, Image image, EventHandler eventHandler)
        {
            ToolStripButton tsbtn = new ToolStripButton();
            tsbtn.Text = text;
            tsbtn.Image = image;
            tsbtn.Click += eventHandler;

            _toolbarItems.Add(tsbtn);
            return tsbtn;
        }

        /// <summary>
        /// 添加工具条项
        /// </summary>
        public void AddToolStripItem(ToolStripItem tsitem)
        {
            _toolbarItems.Add(tsitem);
        }

        /// <summary>
        /// Disable all
        /// </summary>
        public void DisableAll()
        {
            foreach (var item in _statusStrip.Items)
            {
                if (item is DynamicInputToolStripTextBox)
                {
                    ((DynamicInputToolStripTextBox)item).Text = "";
                    ((DynamicInputToolStripTextBox)item).Enabled = false;
                }
                else if (item is ToolStripLabel)
                {
                    if(((ToolStripLabel)item).Tag != null && ((ToolStripLabel)item).Tag.ToString() == "CMD")
                        ((ToolStripLabel)item).Text = "";
                }
            }
        }

        public void EnableAll()
        {
            foreach (var item in _statusStrip.Items)
            {
                if (item is DynamicInputToolStripTextBox)
                {
                    ((DynamicInputToolStripTextBox)item).Text = "";
                    ((DynamicInputToolStripTextBox)item).Enabled = true;
                }
                else if (item is ToolStripLabel)
                {
                    if (((ToolStripLabel)item).Tag != null && ((ToolStripLabel)item).Tag.ToString() == "CMD")
                        ((ToolStripLabel)item).Text = "";
                }
            }
        }
    }
}
