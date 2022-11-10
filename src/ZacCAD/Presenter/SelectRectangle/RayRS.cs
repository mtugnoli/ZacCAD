using System;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;

namespace ZacCAD
{
    internal class RayRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Ray ray = entity as Ray;
            if (ray == null)
            {
                return false;
            }

            return ZacCAD.UI.RayHitter.BoundingIntersectWithRay(selectBound, ray);
        }
    }
}
