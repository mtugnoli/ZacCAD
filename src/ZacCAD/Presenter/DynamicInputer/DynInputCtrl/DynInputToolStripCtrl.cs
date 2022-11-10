using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZacCAD.UI
{
    /// <summary>
    /// 动态输入控件
    /// </summary>
    internal abstract class DynInputToolStripCtrl
    {
        protected Label _tipLabel = null;
        protected Label _infoLabel = null;
        protected Presenter _presenter = null;

        /// <summary>
        /// 独占
        /// </summary>
        private bool _exclusive = false;
        public bool exclusive
        {
            get { return _exclusive; }
            set { _exclusive = value; }
        }

        /// <summary>
        /// 位置
        /// </summary>
        protected LitMath.Vector2 _position = new LitMath.Vector2(0, 0);
        public virtual LitMath.Vector2 position
        {
            get { return _position; }
            set
            {
                _position = value;
                this.UpdatePosition();
            }
        }
        public abstract void UpdatePosition();


        /// <summary>
        /// 事件
        /// </summary>
        public delegate void Handler(DynInputToolStripCtrl sender, DynInputResult result);
        public event Handler finish;
        public event Handler cancel;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DynInputToolStripCtrl(Presenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// Finish
        /// </summary>
        public virtual void End()
        {

        }

        public virtual void Focus()
        {

        }

        /// <summary>
        /// start
        /// </summary>
        public virtual void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// Enter Pressed
        /// </summary>
        public virtual void Finish()
        {
            if (finish != null)
            {
                finish.Invoke(this, this.finishResult);
            }
        }

        protected abstract DynInputResult finishResult
        {
            get;
        }

        /// <summary>
        /// 取消
        /// </summary>
        public virtual void Cancel()
        {
            this.End();

            if (cancel != null)
            {
                cancel.Invoke(this, this.cancelResult);
            }
        }

        protected abstract DynInputResult cancelResult
        {
            get;
        }
    }
}
