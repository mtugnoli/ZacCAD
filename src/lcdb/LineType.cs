using System;
using System.Collections.Generic;
using System.Text;

namespace ZacCAD.DatabaseServices
{
    public enum LineType
    {
        //ByLineTypeDefault = -3,
        ByBlock = -2,
        ByLayer = -1,
        Solid = 0,      //     Specifies a solid line.        
        Dash,           //     Specifies a line consisting of dashes.        
        Dot,            //     Specifies a line consisting of dots.        
        DashDot,        //     Specifies a line consisting of a repeating pattern of dash-dot.        
        DashDotDot,     //     Specifies a line consisting of a repeating pattern of dash-dot-dot.
        Custom          //     Specifies a user-defined custom dash style.
    }
}
