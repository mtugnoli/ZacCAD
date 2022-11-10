using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class XlineCmd : DrawCmd
    {
        private List<Xline> _xlines = new List<Xline>();
        private Xline _currXline = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return _xlines.ToArray(); }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyBasePoint = 1,
            Step2_SpecifyOtherPoint = 2,
        }
        private Step _step = Step.Step1_SpecifyBasePoint;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _step = Step.Step1_SpecifyBasePoint;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("RAGGIO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Xline);
        }

        /// <summary>
        /// 结束
        /// </summary>
        public override void Terminate()
        {
            this.presenter.statusStripMgr.CommandFinish();

            base.Terminate();
        }

        private void GotoStep(Step step, LitMath.Vector2 point)
        {
            if (_step == Step.Step1_SpecifyBasePoint)
            {
                _currXline = new Xline();
                _currXline.basePoint = point;
                _currXline.layerId = this.document.currentLayerId;
                _currXline.color = this.document.currentColor;
                _currXline.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointFirst + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointNext);
                _step = Step.Step2_SpecifyOtherPoint;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecifyOtherPoint)
            {
                LitMath.Vector2 curPos = LitMath.Vector2.PointOrthoMode(_currXline.basePoint, point, presenter.IsOrtho);

                LitMath.Vector2 dir = (curPos - _currXline.basePoint).normalized;
                if (dir.x != 0 || dir.y != 0)
                {
                    _currXline.direction = dir;
                    _currXline.layerId = this.document.currentLayerId;
                    _currXline.color = this.document.currentColor;
                    _currXline.lineType = this.document.currentLineType;
                    _xlines.Add(_currXline);

                    _currXline = _currXline.Clone() as Xline;
                }

                _mgr.FinishCurrentCommand();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointNext + " " + curPos.ToString());

                this.presenter.statusStripMgr.CommandFinish();
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

            if (_currXline != null && _step == Step.Step2_SpecifyOtherPoint)
            {
                LitMath.Vector2 curPos = LitMath.Vector2.PointOrthoMode(_currXline.basePoint, this.pointer.currentSnapPoint, presenter.IsOrtho);

                LitMath.Vector2 dir = (curPos - _currXline.basePoint).normalized;
                if (dir.x != 0 || dir.y != 0)
                {
                    _currXline.direction = dir;
                }
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_xlines.Count > 0)
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
            foreach (Xline xline in _xlines)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, xline);
            }

            if (_currXline != null
                && _step == Step.Step2_SpecifyOtherPoint)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _currXline);
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
                LitMath.Vector2 pos = LitMath.Vector2.StringToVector(parameter);
                if (pos.isvalid)
                {
                    GotoStep(_step, pos);

                    // move the mouse cursor 0 pixels for redrawing
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
                }
                else
                {
                    _mgr.FinishCurrentCommand();

                    this.presenter.statusStripMgr.CommandFinish();
                }
            }
        }

    }
}
