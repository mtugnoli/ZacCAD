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

    internal class LabelX : Label
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
                Font normalFont = this.Font;
                Font boldFont = new Font(normalFont, FontStyle.Bold);

                do
                {
                    // text before tag
                    posS1 = text.IndexOf("[");
                    txt = text.Substring(0, posS1);

                    // text before link
                    posS2 = text.IndexOf("(");
                    txtBis = text.Substring(posS1 + 1, posS2 - posS1 - 1);
                    if (txtBis != "")
                        txt += txtBis;

                    posS1 = posS2;

                    normalSize = TextRenderer.MeasureText(txt, normalFont);
                    normalRect = new Rectangle(posX, 0, normalSize.Width, normalSize.Height);
                    TextRenderer.DrawText(e.Graphics, txt, normalFont, normalRect, ForeColor);
                    posX += normalSize.Width - 5;
                    text = text.Substring(posS1 + 1);



                    // text link
                    posS1 = text.IndexOf(")");
                    txt = text.Substring(0, posS1);
                    boldSize = TextRenderer.MeasureText(txt, boldFont);
                    boldRect = new Rectangle(posX, 0, boldSize.Width, boldSize.Height);
                    TextRenderer.DrawText(e.Graphics, txt, boldFont, boldRect, Color.Blue);
                    posX += boldSize.Width - 5;
                    text = text.Substring(posS1 + 1);


                    // text after link
                    posS1 = text.IndexOf("]");
                    txt = text.Substring(0, posS1);
                    if (txt != "")
                    {
                        normalSize = TextRenderer.MeasureText(txt, normalFont);
                        normalRect = new Rectangle(posX, 0, normalSize.Width, normalSize.Height);
                        TextRenderer.DrawText(e.Graphics, txt, normalFont, normalRect, ForeColor);
                        posX += normalSize.Width - 5;
                    }
                    text = text.Substring(posS1 + 1);

                    if (!text.Contains("["))
                    {
                        normalSize = TextRenderer.MeasureText(text, normalFont);
                        normalRect = new Rectangle(posX, 0, normalSize.Width, normalSize.Height);
                        TextRenderer.DrawText(e.Graphics, text, normalFont, normalRect, ForeColor);

                        break;
                    }

                } while (true);




                //Size boldSize = TextRenderer.MeasureText(ary[0], boldFont);
                //Size normalSize = TextRenderer.MeasureText(ary[1], normalFont);

                //Rectangle boldRect = new Rectangle(drawPoint, boldSize);
                //Rectangle normalRect = new Rectangle(boldRect.Right, boldRect.Top, normalSize.Width, normalSize.Height);

                //TextRenderer.DrawText(e.Graphics, ary[0], boldFont, boldRect, ForeColor);
                //TextRenderer.DrawText(e.Graphics, ary[1], normalFont, normalRect, ForeColor);
            }
            else
            {

                TextRenderer.DrawText(e.Graphics, Text, Font, drawPoint, ForeColor);
            }





            //string[] ary = Text.Split(new char[] { '|' });
            //if (ary.Length == 2)
            //{
            //    Font normalFont = this.Font;

            //    Font boldFont = new Font(normalFont, FontStyle.Bold);

            //    Size boldSize = TextRenderer.MeasureText(ary[0], boldFont);
            //    Size normalSize = TextRenderer.MeasureText(ary[1], normalFont);

            //    Rectangle boldRect = new Rectangle(drawPoint, boldSize);
            //    Rectangle normalRect = new Rectangle(boldRect.Right, boldRect.Top, normalSize.Width, normalSize.Height);

            //    TextRenderer.DrawText(e.Graphics, ary[0], boldFont, boldRect, ForeColor);
            //    TextRenderer.DrawText(e.Graphics, ary[1], normalFont, normalRect, ForeColor);
            //}
            //else
            //{

            //    TextRenderer.DrawText(e.Graphics, Text, Font, drawPoint, ForeColor);
            //}

        }

    }

}
