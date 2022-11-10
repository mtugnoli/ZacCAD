using System;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;

namespace ZacCAD
{
    internal class XlineRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Xline xline = entity as Xline;
            if (xline == null)
            {
                return false;
            }

            return ZacCAD.UI.XlineHitter.BoundingIntersectWithXline(selectBound, xline);
        }
    }
}
