using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ZacCAD.Windows.Controls
{

    internal class ToolStripStatusLabelCmds : ToolStripStatusLabel
    {

        protected override void OnPaint(PaintEventArgs e)
        {
            Point drawPoint = new Point(0, 0);
            string text = Text;
            string txt = "";
            string txtBis = "";
            int posX = 0;
            int posS1 = 0;
            int posS2 = 0;
            Size normalSize = new Size();
            Size boldSize = new Size();
            Rectangle normalRect = new Rectangle();
            Rectangle boldRect = new Rectangle();

            if (text.Contains("["))
            {
                // Create pen.
                //Pen blackPen = new Pen(BackColor, 1);
                SolidBrush whiteBrush = new SolidBrush(BackColor);

                // Draw rectangle to screen.
                //e.Graphics.DrawRectangle(blackPen, new Rectangle(2, 0, 1000, this.Height));
                e.Graphics.FillRectangle(whiteBrush, new Rectangle(2, 0, 1000, this.Height));


                Font normalFont = this.Font;
                Font boldFont = new Font(normalFont, FontStyle.Bold);
                TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

                do
                {
                    // text before tag
                    posS1 = text.IndexOf("[");
                    txt = text.Substring(0, posS1 + 1);

                    // text before link
                    posS2 = text.IndexOf("(");
                    txtBis = text.Substring(posS1 + 1, posS2 - posS1 - 1);
                    if (txtBis != "")
                        txt += txtBis;

                    posS1 = posS2;

                    normalSize = TextRenderer.MeasureText(txt, normalFont, this.Size, flags);
                    normalRect = new Rectangle(posX, 0, normalSize.Width, normalSize.Height + 0);
                    TextRenderer.DrawText(e.Graphics, txt, normalFont, normalRect, ForeColor, BackColor, flags);
                    posX += normalSize.Width - 5;
                    text = text.Substring(posS1 + 1);


                    // text link
                    posS1 = text.IndexOf(")");
                    txt = text.Substring(0, posS1);
                    boldSize = TextRenderer.MeasureText(txt, boldFont, this.Size, flags);
                    boldRect = new Rectangle(posX, 0, boldSize.Width, boldSize.Height + 0);
                    TextRenderer.DrawText(e.Graphics, txt, boldFont, boldRect, Color.Blue, BackColor, flags);
                    posX += boldSize.Width - 5;
                    text = text.Substring(posS1 + 1);


                    // text after link
                    posS1 = text.IndexOf("]");
                    txt = text.Substring(0, posS1 + 1);
                    if (txt != "")
                    {
                        normalSize = TextRenderer.MeasureText(txt, normalFont, this.Size, flags);
                        normalRect = new Rectangle(posX, 0, normalSize.Width, normalSize.Height + 0);
                        TextRenderer.DrawText(e.Graphics, txt, normalFont, normalRect, ForeColor, BackColor, flags);
                        posX += normalSize.Width - 5;
                    }
                    text = text.Substring(posS1 + 1);

                    if (!text.Contains("["))
                    {
                        normalSize = TextRenderer.MeasureText(text, normalFont, this.Size, flags);
                        normalRect = new Rectangle(posX, 0, normalSize.Width, normalSize.Height + 0);
                        TextRenderer.DrawText(e.Graphics, text, normalFont, normalRect, ForeColor, BackColor, flags);
                        posX += normalSize.Width - 5;

                        break;
                    }

                } while (true);
            }
            else
            {
                TextFormatFlags flags = TextFormatFlags.Default;

                TextRenderer.DrawText(e.Graphics, Text, Font, drawPoint, ForeColor, BackColor, flags);
            }

        }

    }

}
