using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class RectangleCmd : DrawCmd
    {
        private Polyline _rectangle = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Polyline[1] { _rectangle }; }
        }

        // 起点+终点
        private LitMath.Vector2 _point1st = new LitMath.Vector2(0, 0);
        private LitMath.Vector2 _point2nd = new LitMath.Vector2(0, 0);

        private void UpdateRectangle()
        {
            if (_rectangle == null)
            {
                _rectangle = new Polyline();
                _rectangle.closed = true;
                for (int i = 0; i < 4; ++i)
                {
                    _rectangle.AddVertexAt(0, new LitMath.Vector2b(0, 0, 0));
                }
            }

            _rectangle.SetPointAt(0, _point1st);
            _rectangle.SetPointAt(1, new LitMath.Vector2(_point2nd.x, _point1st.y));
            _rectangle.SetPointAt(2, _point2nd);
            _rectangle.SetPointAt(3, new LitMath.Vector2(_point1st.x, _point2nd.y));
            _rectangle.layerId = this.document.currentLayerId;
            _rectangle.color = this.document.currentColor;
            _rectangle.lineType = this.document.currentLineType;
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyPoint1st = 1,
            Step2_SpecifyPoint2nd = 2,
        }
        private Step _step = Step.Step1_SpecifyPoint1st;


        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyPoint1st;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("RETTANGOLO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Rectangle);
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
            if (_step == Step.Step1_SpecifyPoint1st)
            {
                _point1st = point;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointFirst + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointNext);
                _step = Step.Step2_SpecifyPoint2nd;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecifyPoint2nd)
            {
                _point2nd = point;
                this.UpdateRectangle();

                _mgr.FinishCurrentCommand();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointNext + " " + _point2nd.ToString());

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

            if (_step == Step.Step2_SpecifyPoint2nd)
            {
                _point2nd = this.pointer.currentSnapPoint;
                this.UpdateRectangle();
            }

            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_rectangle != null)
            {
                this.presenter.DrawEntity(g, _rectangle);
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
