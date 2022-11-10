using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class TextCmd : DrawCmd
    {
        private ZacCAD.DatabaseServices.Text _text = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new ZacCAD.DatabaseServices.Text[1] { _text }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyInsertPoint = 1,
            Step2_SpecityText = 2,
        }
        private Step _step = Step.Step1_SpecifyInsertPoint;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyInsertPoint;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("TESTO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSpecified);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Text);
        }

        /// <summary>
        /// 结束
        /// </summary>
        public override void Terminate()
        {
            this.presenter.statusStripMgr.CommandFinish();

            base.Terminate();
        }


        private void GotoStep(Step step, string value)
        {
            if (_step == Step.Step1_SpecifyInsertPoint)
            {
                LitMath.Vector2 pos = LitMath.Vector2.StringToVector(value);
                if (pos.isvalid)
                    GotoStep(_step, pos);
            }
            else if (_step == Step.Step2_SpecityText)
            {
                _text.layerId = this.document.currentLayerId;
                _text.color = this.document.currentColor;
                _text.lineType = this.document.currentLineType;
                _text.text = value;

                _mgr.FinishCurrentCommand();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValueText + " " + value);

                this.presenter.statusStripMgr.CommandFinish();
            }
        }

        private void GotoStep(Step step, LitMath.Vector2 point)
        {
            if (_step == Step.Step1_SpecifyInsertPoint)
            {
                _text = new ZacCAD.DatabaseServices.Text();
                _text.position = point;
                _text.layerId = this.document.currentLayerId;
                _text.color = this.document.currentColor;
                _text.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + point.ToString());

                _step = Step.Step2_SpecityText;

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_ValueText);
                this.presenter.statusStripMgr.CommandTextFocus();
            }
        }



        public override EventResult OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                GotoStep(_step, this.pointer.currentSnapPoint);
            else
                this.presenter.statusStripMgr.CommandTextFocus();

            return EventResult.Handled;
        }

        public override EventResult OnMouseUp(MouseEventArgs e)
        {
            return EventResult.Handled;
        }

        public override EventResult OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                return EventResult.Handled;
            }

            //if (_text != null)
            //{
            //    _text.radius = (_text.center - this.pointer.currentSnapPoint).length;
            //}

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_text != null)
                {
                    _mgr.FinishCurrentCommand();
                }
                else
                {
                    _mgr.CancelCurrentCommand();
                }
            }
            return EventResult.Handled;
        }

        public override EventResult OnKeyUp(KeyEventArgs e)
        {
            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_text != null)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _text);
            }
        }

        public override void OnParameter(string parameter)
        {
            if (parameter == "Escape")
            {
                _mgr.CancelCurrentCommand();

                return;
            }
            else
            {
                GotoStep(_step, parameter);
            }
        }

    }
}
