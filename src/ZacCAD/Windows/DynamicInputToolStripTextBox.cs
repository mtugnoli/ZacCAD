using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZacCAD.Windows.Controls
{
    internal class DynamicInputToolStripTextBox : ToolStripTextBox
    {
        public delegate void MessageHandler(object sender);
        public event MessageHandler keyEnterDown;
        public event MessageHandler keyEscDown;



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                keyEscDown.Invoke(this);

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    if (keyEscDown != null)
                    {
                        keyEscDown.Invoke(this);
                    }
                    break;

                case (char)Keys.Enter:
                    if (keyEnterDown != null)
                    {
                        keyEnterDown.Invoke(this);
                    }
                    break;

                default:
                    {
                        base.OnKeyPress(e);
                    }
                    break;
            }
        }

    }
}
