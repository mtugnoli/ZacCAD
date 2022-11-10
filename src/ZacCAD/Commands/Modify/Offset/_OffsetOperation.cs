﻿using System;

using ZacCAD.DatabaseServices;

namespace ZacCAD.Commands.Modify.Offset
{
    internal abstract class _OffsetOperation
    {
        public abstract Entity result { get; }

        public abstract bool Do(double value, LitMath.Vector2 refPoint);
    }
}
