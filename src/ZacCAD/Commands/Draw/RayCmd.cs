using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class RayCmd : DrawCmd
    {
        private List<Ray> _xlines = new List<Ray>();
        private Ray _ray = null;

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

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Ray);
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
                _ray = new Ray();
                _ray.basePoint = point;
                _ray.layerId = this.document.currentLayerId;
                _ray.color = this.document.currentColor;
                _ray.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointFirst + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointNext);
                _step = Step.Step2_SpecifyOtherPoint;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecifyOtherPoint)
            {
                LitMath.Vector2 curPos = LitMath.Vector2.PointOrthoMode(_ray.basePoint, point, presenter.IsOrtho);

                LitMath.Vector2 dir = (curPos - _ray.basePoint).normalized;
                if (dir.x != 0 || dir.y != 0)
                {
                    _ray.direction = dir;
                    _ray.layerId = this.document.currentLayerId;
                    _ray.color = this.document.currentColor;
                    _ray.lineType = this.document.currentLineType;
                    _xlines.Add(_ray);

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointNext + " " + curPos.ToString());

                    _ray = _ray.Clone() as Ray;
                }

                _mgr.FinishCurrentCommand();

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

            if (_ray != null && _step == Step.Step2_SpecifyOtherPoint)
            {
                LitMath.Vector2 curPos = LitMath.Vector2.PointOrthoMode(_ray.basePoint, this.pointer.currentSnapPoint, presenter.IsOrtho);

                LitMath.Vector2 dir = (curPos - _ray.basePoint).normalized;
                if (dir.x != 0 || dir.y != 0)
                {
                    _ray.direction = dir;
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
            foreach (Ray xline in _xlines)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, xline);
            }

            if (_ray != null
                && _step == Step.Step2_SpecifyOtherPoint)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _ray);
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
