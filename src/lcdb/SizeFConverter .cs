namespace ZacCAD.DatabaseServices
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Drawing;
    using System.Reflection;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.ComponentModel.Design.Serialization;

    /// <summary>
    /// SizeFConverter 的摘要说明。
    /// </summary>
    public class SizeFConverter : TypeConverter
    {
        // Methods
        public SizeFConverter()
        {
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, culture, value);
            }
            string text = ((string)value).Trim();
            if (text.Length == 0)
            {
                return null;
            }
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            char ch = culture.TextInfo.ListSeparator[0];
            string[] textArray = text.Split(new char[] { ch });
            float[] numArray = new float[textArray.Length];
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = (float)converter.ConvertFromString(context, culture, textArray[i]);
            }
            if (numArray.Length != 2)
            {
                throw new ArgumentException("Incorrect format！");
            }
            return new SizeF(numArray[0], numArray[1]);

        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is SizeF))
            {
                SizeF size = (SizeF)value;
                if (culture == null)
                {
                    culture = CultureInfo.CurrentCulture;
                }
                string separator = culture.TextInfo.ListSeparator + " ";
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
                string[] textArray = new string[2];
                int num = 0;
                textArray[num++] = converter.ConvertToString(context, culture, size.Width);
                textArray[num++] = converter.ConvertToString(context, culture, size.Height);
                return string.Join(separator, textArray);
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is SizeF))
            {
                SizeF size2 = (SizeF)value;
                ConstructorInfo member = typeof(SizeF).GetConstructor(new Type[] { typeof(float), typeof(float) });
                if (member != null)
                {
                    return new InstanceDescriptor(member, new object[] { size2.Width, size2.Height });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);

        }
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new SizeF((float)propertyValues["Width"], (float)propertyValues["Height"]);
        }
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(SizeF), attributes).Sort(new string[] { "Width", "Height" });
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}