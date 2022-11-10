using System;
using System.Windows.Forms;

namespace ZacCAD.UI
{
    /// <summary>
    /// 命令输入控件
    /// </summary>
    internal class DynInputBar : DynInputToolStripTextBox<string>
    {
        /// <summary>
        /// 更新值
        /// </summary>
        protected override bool UpdateValue()
        {
            _value = _textBox.Text;
            return true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DynInputBar(Presenter presenter, string value) : base(presenter, value)
        {

        }

    }
}
