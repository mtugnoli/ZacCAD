using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Drawing.Design;
using System.ComponentModel;
using PropertyGridEx;

namespace ZacCAD
{
    public enum MyEnum
    {
        FirstEntry,
        SecondEntry,
        ThirdEntry
    }

    [Serializable()]
    public class MyOwnClass
    {
        private int iValue = 0;
        private string sText;
        public MyOwnClass(string Text, int Value)
        {
            sText = Text;
            iValue = Value;
        }
        public int Value
        {
            get
            {
                return iValue;
            }
            set
            {
                iValue = value;
            }
        }
        public string Text
        {
            get
            {
                return sText;
            }
            set
            {
                sText = value;
            }
        }
    }

    public class MyStringConverter : MultilineStringConverter
    {

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            return "(My multiline string editor)";
        }
    }

    public class MyEditor : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }
            if (MessageBox.Show("Please answer me", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                value = true;
            }
            else
            {
                value = false;
            }
            return value;
        }
    }

    public class MyTypeConverter : Int32Converter
    {
        private bool bMsgboxIsVisible = false;
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                return base.ConvertFrom(context, culture, value);
            }
            catch (Exception ex)
            {
                if (!bMsgboxIsVisible)
                {
                    bMsgboxIsVisible = true;
                    MessageBox.Show("Custom Message: " + ex.Message, "Custom message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                bMsgboxIsVisible = false;
                return context.PropertyDescriptor.GetValue(this);
            }
        }
    }

}
