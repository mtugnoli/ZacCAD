using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    /// <summary>
    /// 绘制圆弧命令
    /// </summary>
    internal class ArcCmd : DrawCmd
    {
        /// <summary>
        /// 绘制的圆弧
        /// </summary>
        private Arc _arc = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Arc[1] { _arc }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyCenter = 1,
            Step2_SpecityStartPoint = 2,
            Step3_SpecifyEndPoint = 3,
            Step4_Finish = 4,
        }
        private Step _step = Step.Step1_SpecifyCenter;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyCenter;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("ARCO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointCenter);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Arc);
        }

        public override void Terminate()
        {
            this.presenter.statusStripMgr.CommandFinish();

            base.Terminate();
        }

        private void GotoStep(Step step, LitMath.Vector2 point)
        {
            if (_step == Step.Step1_SpecifyCenter)
            {
                _arc = new Arc();
                _arc.center = point;
                _arc.radius = 0;
                _arc.layerId = this.document.currentLayerId;
                _arc.color = this.document.currentColor;
                _arc.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointCenter + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
                _step = Step.Step2_SpecityStartPoint;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecityStartPoint)
            {
                _arc.radius = (_arc.center - point).length;
                _arc.layerId = this.document.currentLayerId;
                _arc.color = this.document.currentColor;
                _arc.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValueRadius + " " + _arc.radius.ToString());


                double startAngle = LitMath.Vector2.SignedAngleInRadian(new LitMath.Vector2(1, 0), point - _arc.center);
                startAngle = MathUtils.NormalizeRadianAngle(startAngle);
                _arc.startAngle = startAngle;
                _arc.endAngle = startAngle;

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSecond);
                _step = Step.Step3_SpecifyEndPoint;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step3_SpecifyEndPoint)
            {
                double endAngle = LitMath.Vector2.SignedAngleInRadian(new LitMath.Vector2(1, 0), point - _arc.center);
                endAngle = MathUtils.NormalizeRadianAngle(endAngle);
                _arc.endAngle = endAngle;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValueAngle + " " + (LitMath.Utils.RadianToDegree(_arc.endAngle - _arc.startAngle)).ToString());

                _mgr.FinishCurrentCommand();
                this.presenter.statusStripMgr.CommandFinish();

                _step = Step.Step4_Finish;
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

            switch (_step)
            {
                case Step.Step1_SpecifyCenter:
                    break;

                case Step.Step2_SpecityStartPoint:
                    break;

                case Step.Step3_SpecifyEndPoint:
                    double endAngle = LitMath.Vector2.SignedAngleInRadian(new LitMath.Vector2(1, 0), this.pointer.currentSnapPoint - _arc.center);
                    endAngle = MathUtils.NormalizeRadianAngle(endAngle);
                    _arc.endAngle = endAngle;
                    break;
            }

            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_arc != null)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _arc);
            }

            switch (_step)
            {
                case Step.Step1_SpecifyCenter:
                    break;

                case Step.Step2_SpecityStartPoint:
                case Step.Step3_SpecifyEndPoint:
                    _mgr.presenter.DrawLine(g, GDIResMgr.Instance.GetPen(Color.White, System.Drawing.Drawing2D.DashStyle.Solid, 0), _arc.center, this.pointer.currentSnapPoint, CSYS.Model);
                    break;
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
