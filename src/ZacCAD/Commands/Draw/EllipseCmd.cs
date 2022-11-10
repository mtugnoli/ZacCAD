using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class EllipseCmd : DrawCmd
    {
        private Ellipse _ellipse = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Ellipse[1] { _ellipse }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyCenter = 1,
            Step2_SpecityRadiusX = 2,
            Step3_SpecityRadiusY = 3,
        }
        private Step _step = Step.Step1_SpecifyCenter;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.presenter.statusStripMgr.SetCommandLabel("ELLISSE");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointCenter);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Ellipse);
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
            switch (step)
            {
                case Step.Step1_SpecifyCenter:
                    {
                        _ellipse = new Ellipse();
                        _ellipse.center = point;
                        _ellipse.radiusX = 0;
                        _ellipse.radiusY = 0;
                        _ellipse.layerId = this.document.currentLayerId;
                        _ellipse.color = this.document.currentColor;
                        _ellipse.lineType = this.document.currentLineType;

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointCenter + " " + point.ToString());


                        this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointAxisX);
                        _step = Step.Step2_SpecityRadiusX;

                        this.presenter.statusStripMgr.CommandTextFocus();
                    }
                    break;

                case Step.Step2_SpecityRadiusX:
                    {
                        _ellipse.radiusX = Math.Abs(_ellipse.center.x - point.x);
                        _ellipse.radiusY = _ellipse.radiusX / 2.0;
                        _ellipse.layerId = this.document.currentLayerId;
                        _ellipse.color = this.document.currentColor;
                        _ellipse.lineType = this.document.currentLineType;

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointAxisX + " " + point.ToString());


                        this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointAxisY);
                        _step = Step.Step3_SpecityRadiusY;

                        this.presenter.statusStripMgr.CommandTextFocus();
                    }
                    break;

                case Step.Step3_SpecityRadiusY:
                    {
                        _ellipse.radiusY = Math.Abs(_ellipse.center.y - point.y);
                        _ellipse.layerId = this.document.currentLayerId;
                        _ellipse.color = this.document.currentColor;
                        _ellipse.lineType = this.document.currentLineType;

                        _mgr.FinishCurrentCommand();

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointAxisY + " " + point.ToString());


                        this.presenter.statusStripMgr.CommandFinish();
                    }
                    break;
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

            if (_ellipse != null)
            {
                if (_step == Step.Step2_SpecityRadiusX)
                {
                    _ellipse.radiusX = Math.Abs(_ellipse.center.x - this.pointer.currentSnapPoint.x);
                    _ellipse.radiusY = _ellipse.radiusX / 2.0;
                }
                else if (_step == Step.Step3_SpecityRadiusY)
                {
                    _ellipse.radiusY = Math.Abs(_ellipse.center.y - this.pointer.currentSnapPoint.y);
                }
            }

            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_ellipse != null)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _ellipse);
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
