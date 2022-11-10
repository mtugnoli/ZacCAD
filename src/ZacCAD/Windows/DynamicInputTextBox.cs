using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZacCAD.Windows.Controls
{
    internal class DynamicInputTextBox : TextBox
    {
        public delegate void MessageHandler(object sender);
        public event MessageHandler keyEscDown;
        public event MessageHandler keySpaceDown;
        public event MessageHandler keyEnterDown;
        public event MessageHandler keyTabDown;

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

                case (char)Keys.Space:
                    if (keySpaceDown != null)
                    {
                        keySpaceDown.Invoke(this);
                    }
                    break;

                case (char)Keys.Enter:
                    if (keyEnterDown != null)
                    {
                        keyEnterDown.Invoke(this);
                    }
                    break;

                case (char)Keys.Tab:
                    if (keyTabDown != null)
                    {
                        keyTabDown.Invoke(this);
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
