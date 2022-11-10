using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.Windows.Controls;

namespace ZacCAD.UI
{
    internal abstract class DynInputToolStripTextBox<T> : DynInputToolStripCtrl
    {
        protected DynamicInputToolStripTextBox _textBox = null;
        protected ToolStripLabel _commandLabel = null;
        protected ToolStripLabel _promptLabel = null;
        protected T _value;

        public string text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        public Size size
        {
            get { return _textBox.Size; }
            set { _textBox.Size = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DynInputToolStripTextBox(Presenter presenter, string text) : base(presenter)
        {
            //DynamicInputToolStripTextBox _commandText = (DynamicInputToolStripTextBox)statusStrip.Items["toolStripTextBoxCommands"];
            //_commandLabel = (ToolStripLabel)statusStrip.Items["toolStripLabelCommand"];
            //_promptLabel = (ToolStripLabel)statusStrip.Items["toolStripLabelCommandInfo"];

            //_textBox = _commandText;
            //_textBox.Text = text;

            //_textBox.keyEscDown += this.OnEscDown;
            //_textBox.keySpaceDown += this.OnSpaceDown;
            //_textBox.keyEnterDown += this.OnEnterDown;
            //_textBox.keyTabDown += this.OnTabDown;
        }



        public string CommandText
        {
            set { _commandLabel.Text = value; }
        }

        public string PromptText
        {
            set
            {
                _promptLabel.Visible = !string.IsNullOrEmpty(value);
                _promptLabel.Text = value;
            }
        }

        /// <summary>
        /// 更新值
        /// </summary>
        protected abstract bool UpdateValue();

        /// <summary>
        /// 开始
        /// </summary>
        public override void Start()
        {
            base.Start();

            _textBox.Focus();
            _textBox.Select(_textBox.Text.Length, 0);
        }

        public override void Focus()
        {
            _textBox.Focus();
            _textBox.Select(_textBox.Text.Length, 0);
        }

        /// <summary>
        /// 完成
        /// </summary>
        public override void Finish()
        {
            if (!UpdateValue())
            {
                return;
            }

            base.Finish();
        }

        public override void UpdatePosition()
        {

        }

        /// <summary>
        /// 完成结果
        /// </summary>
        protected override DynInputResult finishResult
        {
            get
            {
                _textBox.Text = "";

                return new DynInputResult<T>(DynInputStatus.OK, _value);
            }
        }

        public override void End()
        {
            CommandText = "";
            PromptText = "";
            _textBox.Text = "";

            base.End();
        }

        /// <summary>
        /// 取消
        /// </summary>
        public override void Cancel()
        {
            CommandText = "";
            PromptText = "";
            _textBox.Text = "";

            base.Cancel();
        }

        /// <summary>
        /// 取消结果
        /// </summary>
        protected override DynInputResult cancelResult
        {
            get
            {
                return new DynInputResult<T>(DynInputStatus.Cancel, _value);
            }
        }

        /// <summary>
        /// ESC 键响应
        /// </summary>
        protected virtual void OnEscDown(object sender)
        {
            this.Cancel();
        }

        /// <summary>
        /// Space 键响应
        /// </summary>
        protected virtual void OnSpaceDown(object sender)
        {
            //this.Finish();
        }

        /// <summary>
        /// Enter 键响应
        /// </summary>
        protected virtual void OnEnterDown(object sender)
        {
            this.Finish();
        }

        /// <summary>
        /// Tab 键响应
        /// </summary>
        protected virtual void OnTabDown(object sender)
        {

        }
    }
}
