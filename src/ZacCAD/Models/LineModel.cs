using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Drawing.Design;
using System.ComponentModel;
using ZacCAD.DatabaseServices;

namespace ZacCAD
{
    [Serializable()]
    public class LineModel
    {
        private string iId = "";
        private PointF startPoint = new PointF();
        private PointF endPoint = new PointF();
        private Color m_Color;
        private string _layer = "";
        //private List<string> _layers = new List<string>();

        //[CategoryAttribute("Data")]
        //public List<string> Layers
        //{
        //    get
        //    {
        //        return _layers;
        //    }
        //    set
        //    {
        //        _layers = value;
        //    }
        //}

        [CategoryAttribute("Data")]
        [Description("Defines the Start Point")]
        [TypeConverter(typeof(PointFConverter))]

        public PointF StartPoint
        {
            get
            {
                return startPoint;
            }
            set
            {
                startPoint = value;
            }
        }

        [CategoryAttribute("Data")]
        [Description("Defines the End Point")]
        [TypeConverter(typeof(PointFConverter))]

        public PointF EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                endPoint = value;
            }
        }


        [CategoryAttribute("Data")]
        [ReadOnly(true)]
        public string Id
        {
            get
            {
                return iId;
            }
            set
            {
                iId = value;
            }
        }

        [CategoryAttribute("Data")]
        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        private LineWeight _lineWeight = LineWeight.ByLayer;
        [Category("Data")]
        public LineWeight lineWeight
        {
            get { return _lineWeight; }
            set { _lineWeight = value; }
        }

        [Category("Data")]
        [DisplayName("Layer")]
        [Description("Entity Layer.")]
        [DefaultValue("")]
        public String Layer { get { return _layer; } set { _layer = value; } }










        //private List<string> myList = new List<string>();
        //[Editor("System.Windows.Forms.Design.StringCollectionEditor, " + "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",            typeof(UITypeEditor))]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public List<string> MyList
        //{
        //    get
        //    {
        //        return myList;
        //    }
        //    set
        //    {
        //        myList = value;
        //    }
        //}


        //public class LayerStringConverter : StringConverter
        //{
        //    public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        //    public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //    {
        //        List<String> list = new List<String>();
        //        list.Add("");
        //        list.Add("Currency");
        //        list.Add("Scientific Notation");
        //        list.Add("General Number");
        //        list.Add("Number");
        //        list.Add("Percent");
        //        list.Add("Time");
        //        list.Add("Date");

        //        return new StandardValuesCollection(list);
        //    }
        //}
    }
}
