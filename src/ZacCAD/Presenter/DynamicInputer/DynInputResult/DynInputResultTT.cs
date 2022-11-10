using System;
using System.Linq;

namespace ZacCAD.UI
{
    /// <summary>
    /// 动态输入结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DynInputResult<TOne, TTwo>  : DynInputResult 
    {
        /// <summary>
        /// 值
        /// </summary>
        protected TOne _value;
        public TOne value
        {
            get { return _value; }
        }

        protected TTwo _angle;
        public TTwo angle
        {
            get { return _angle; }
        }

        public DynInputResult(DynInputStatus status, TOne value, TTwo value2) : base(status)
        {
            _value = value;
            _angle = value2;
        }
    }
}
