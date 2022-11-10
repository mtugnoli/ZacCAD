using System;

using ZacCAD.DatabaseServices;

namespace ZacCAD.UI
{
    internal abstract class EntityHitter
    {
        internal abstract bool Hit(PickupBox pkbox, Entity entity);
    }
}
