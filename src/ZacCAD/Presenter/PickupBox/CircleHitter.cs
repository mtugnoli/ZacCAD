using System;
using System.Collections.Generic;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.UI
{
    internal class CircleHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Circle circle = entity as Circle;
            if (circle == null)
                return false;

            return MathUtils.BoundingCross(pkbox.reservedBounding, circle);
        }
    }
}
