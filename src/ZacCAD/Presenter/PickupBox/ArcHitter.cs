﻿using System;
using System.Collections.Generic;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.UI
{
    internal class ArcHitter : EntityHitter
    {
        internal override bool Hit(PickupBox pkbox, Entity entity)
        {
            Arc arc = entity as Arc;
            if (arc == null)
                return false;

            ArcRS arcRS = new ArcRS();
            return arcRS.Cross(pkbox.reservedBounding, arc);
        }
    }
}
