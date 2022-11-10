using System;
using System.Collections.Generic;

using ZacCAD.DatabaseServices;

namespace ZacCAD
{
    internal class TextRS : EntityRS
    {
        internal override bool Cross(Bounding selectBound, Entity entity)
        {
            Text text = entity as Text;
            if (text == null)
            {
                return false;
            }

            Bounding textBound = text.bounding;
            if (selectBound.Contains(textBound))
            {
                return true;
            }

            if (textBound.IntersectWith(selectBound))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
