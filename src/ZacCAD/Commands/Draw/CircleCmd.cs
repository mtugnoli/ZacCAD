using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class CircleCmd : DrawCmd
    {
        private Circle _circle = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Circle[1] { _circle }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyCenter = 1,
            Step2_SpecityRadius = 2,
        }
        private Step _step = Step.Step1_SpecifyCenter;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _step = Step.Step1_SpecifyCenter;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("CERCHIO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointCenter);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Circle);
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
            if (_step == Step.Step1_SpecifyCenter)
            {
                _circle = new Circle();
                _circle.center = point;
                _circle.radius = 0;
                _circle.layerId = this.document.currentLayerId;
                _circle.color = this.document.currentColor;
                _circle.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointCenter + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointNext);
                _step = Step.Step2_SpecityRadius;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecityRadius)
            {
                LitMath.Vector2 curPoint = LitMath.Vector2.PointOrthoMode(_circle.center, point, presenter.IsOrtho);

                _circle.radius = (_circle.center - curPoint).length;
                _circle.layerId = this.document.currentLayerId;
                _circle.color = this.document.currentColor;
                _circle.lineType = this.document.currentLineType;

                _mgr.FinishCurrentCommand();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValueRadius + " " + _circle.radius.ToString());

                this.presenter.statusStripMgr.CommandFinish();
            }
        }

        private void GotoStep(Step step, double radius)
        {
            if (_step == Step.Step2_SpecityRadius)
            {
                _circle.radius = radius;
                _circle.layerId = this.document.currentLayerId;
                _circle.color = this.document.currentColor;
                _circle.lineType = this.document.currentLineType;

                _mgr.FinishCurrentCommand();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValueRadius + " " + _circle.radius.ToString());

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

            if (_circle != null)
            {
                _circle.radius = (_circle.center - LitMath.Vector2.PointOrthoMode(_circle.center, this.pointer.currentSnapPoint, presenter.IsOrtho)).length;
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_circle != null)
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
            if (_circle != null)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _circle);
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
                if (parameter == "")
                {
                    _mgr.FinishCurrentCommand();

                    this.presenter.statusStripMgr.CommandFinish();
                }
                else if (parameter.Contains(","))
                {
                    LitMath.Vector2 pos = LitMath.Vector2.StringToVector(parameter);
                    if (pos.isvalid)
                    {
                        GotoStep(_step, pos);

                        // move the mouse cursor 0 pixels for redrawing
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
                    }
                }
                else
                {
                    double radius = 0;
                    bool ret = double.TryParse(parameter.Replace(".",","), out radius);
                    if (ret)
                    {
                        GotoStep(_step, radius);

                        // move the mouse cursor 0 pixels for redrawing
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);

                    }

                }
            }
        }

    }
}
