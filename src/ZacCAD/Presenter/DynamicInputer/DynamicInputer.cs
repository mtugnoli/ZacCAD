using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;

namespace ZacCAD.UI
{
    internal class DynamicInputer
    {
        private Presenter _presenter = null;

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool active
        {
            get
            {
                return _currInputCtrl != null;
            }
        }

        /// <summary>
        /// 是否独占
        /// </summary>
        public bool exclusive
        {
            get
            {
                return _currInputCtrl != null ? _currInputCtrl.exclusive : false;
            }
        }


        /// <summary>
        /// 动态输入控件
        /// </summary>
        private DynInputToolStripCtrl _currInputCtrl = null;
        private DynInputBar _cmdInput = null;
        public DynInputBar cmdInput
        {
            get { return _cmdInput; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="presenter"></param>
        public DynamicInputer(Presenter presenter)
        {
            _presenter = presenter;

            _cmdInput = new DynInputBar(_presenter, "");
        }

        /// <summary>
        /// 启动命令动态输入
        /// </summary>
        public bool StartCmd(KeyEventArgs e)
        {
            // Non -character returns false
            if ((uint)e.KeyCode < 65
                || (uint)e.KeyCode > 90)
            {
                return false;
            }

            //
            _currInputCtrl = _cmdInput;
            _cmdInput.text = KeyDataToString(e.KeyData);
            _cmdInput.Start();

            return true;
        }

        public bool StartInput(DynInputToolStripCtrl inputCtrl)
        {
            _currInputCtrl = inputCtrl;
            _currInputCtrl.Start();

            return true;
        }

        /// <summary>
        /// Key value convert to string
        /// </summary>
        #region
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);

        public static string KeyDataToString(Keys keydata)
        {
            int nonVirtualKey = MapVirtualKey((uint)keydata, 2);
            char mappedChar = Convert.ToChar(nonVirtualKey);

            return mappedChar.ToString();
        }
        #endregion
    }
}
