using System;
using System.Collections.Generic;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;
using LitMath;

namespace ZacCAD.UI
{
    internal class PolylineHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Polyline polyline = entity as Polyline;
            if (polyline == null)
                return false;

            Bounding pkBounding = pkbox.reservedBounding;
            for (int i = 1; i < polyline.NumberOfVertices; ++i)
            {
                LitMath.Vector2b p1 = polyline.GetPointAt(i - 1);
                LitMath.Vector2b p2 = polyline.GetPointAt(i);

                LitMath.Line2 line = new LitMath.Line2(new Vector2(p1.x, p1.y), new Vector2(p2.x, p2.y));

                if (LineHitter.BoundingIntersectWithLine(pkBounding, line))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
