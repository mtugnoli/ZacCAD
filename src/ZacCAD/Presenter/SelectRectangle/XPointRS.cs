using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZacCAD.DatabaseServices;

namespace ZacCAD
{
    internal class XPointRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            XPoint xPoint = entity as XPoint;
            if (xPoint == null) return false;

            Bounding xPointBound = xPoint.bounding;
            return selectBound.Contains(xPointBound) || xPointBound.IntersectWith(selectBound);
        }
    }
}
