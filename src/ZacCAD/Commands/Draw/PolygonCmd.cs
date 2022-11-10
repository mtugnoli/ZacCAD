using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class PolygonCmd : DrawCmd
    {
        private Polyline _polygon = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Polyline[1] { _polygon }; }
        }

        /// <summary>
        /// 多边形边数,>=3
        /// </summary>
        private uint _sides = 5;

        /// <summary>
        /// 定位点
        /// </summary>
        private LitMath.Vector2 _center = new LitMath.Vector2(0, 0);
        private LitMath.Vector2 _point = new LitMath.Vector2(0, 0);

        /// <summary>
        /// 选项
        /// 内接于圆,外切于圆
        /// </summary>
        private enum Option
        {
            InscribedInCircle = 0,
            CircumscribedAboutCircle = 1,
        }
        private Option _option = Option.CircumscribedAboutCircle;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifySidesCount;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("POLIGONO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_ValueEdges);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Polygon);
        }

        public override void Terminate()
        {
            this.presenter.statusStripMgr.CommandFinish();

            base.Terminate();
        }

        /// <summary>
        /// 刷新正多边形
        /// </summary>
        private void UpdatePolygon()
        {
            if (_polygon == null)
            {
                _polygon = new Polyline();
                _polygon.closed = true;
                for (int i = 0; i < _sides; ++i)
                {
                    _polygon.AddVertexAt(i, new LitMath.Vector2b(0, 0, 0));
                }
            }

            switch (_option)
            {
                case Option.InscribedInCircle:
                    {
                        LitMath.Vector2 sPnt = _point - _center;
                        _polygon.SetPointAt(0, _center + sPnt);
                        for (int i = 1; i < _sides; ++i)
                        {
                            LitMath.Vector2 vPnt = LitMath.Vector2.Rotate(sPnt, 360.0 / _sides * i);
                            _polygon.SetPointAt(i, _center + vPnt);
                        }
                    }
                    break;

                case Option.CircumscribedAboutCircle:
                    {
                        double radius = (_point - _center).length;
                        double angle = (LitMath.Utils.PI * 2) / _sides;
                        double l = radius / Math.Cos(angle / 2.0);

                        LitMath.Vector2 sPnt = (_point - _center).normalized * l;
                        sPnt = LitMath.Vector2.RotateInRadian(sPnt, angle / 2);
                        _polygon.SetPointAt(0, _center + sPnt);

                        for (int i = 1; i < _sides; ++i)
                        {
                            LitMath.Vector2 vPnt = LitMath.Vector2.RotateInRadian(sPnt, angle * i);
                            _polygon.SetPointAt(i, _center + vPnt);
                        }
                    }
                    break;
            }

            _polygon.layerId = this.document.currentLayerId;
            _polygon.color = this.document.currentColor;
            _polygon.lineType = this.document.currentLineType;
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            // 边数
            Step1_SpecifySidesCount = 1,
            // 内接于圆还是外切于圆
            Step2_IORC = 2,
            Step3_SpecifyPointCenter = 3,
            Step4_SpecifyPointOther = 4,
        }
        private Step _step = Step.Step1_SpecifySidesCount;


        private void GotoStep(Step step, string value)
        {
            if (_step == Step.Step1_SpecifySidesCount)
            {
                _sides = Convert.ToUInt32(value);
                _step = Step.Step2_IORC;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValueEdges + " " + _sides.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_ValuePolygonOptions);
                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_IORC)
            {
                if (value.Trim().ToUpper() == "I")
                {
                    _option = Option.InscribedInCircle;
                }
                else
                {
                    _option = Option.CircumscribedAboutCircle;
                }

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_ValuePolygonOptions + " " + value.Trim().ToUpper());

                _step = Step.Step3_SpecifyPointCenter;

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSpecified);
                this.presenter.statusStripMgr.CommandTextFocus();

            }
            else if (_step == Step.Step3_SpecifyPointCenter)
            {
                LitMath.Vector2 point = LitMath.Vector2.StringToVector(value);

                if (point.isvalid)
                {
                    _center = point;
                    _step = Step.Step4_SpecifyPointOther;

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + point.ToString());

                    this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSpecified);
                    this.presenter.statusStripMgr.CommandTextFocus();
                }
            }
            else if (_step == Step.Step4_SpecifyPointOther)
            {
                LitMath.Vector2 point = LitMath.Vector2.StringToVector(value);

                if (point.isvalid)
                {
                    _point = point;
                    this.UpdatePolygon();

                    _mgr.FinishCurrentCommand();

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + point.ToString());

                    this.presenter.statusStripMgr.CommandFinish();
                }
            }
        }

        private void GotoStep(Step step, LitMath.Vector2 point)
        {
            if (_step == Step.Step3_SpecifyPointCenter)
            {
                _center = this.pointer.currentSnapPoint;
                _step = Step.Step4_SpecifyPointOther;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + _center.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSpecified);
                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step4_SpecifyPointOther)
            {
                LitMath.Vector2 curPoint = LitMath.Vector2.PointOrthoMode(_center, point, presenter.IsOrtho);

                _point = curPoint;
                this.UpdatePolygon();

                _mgr.FinishCurrentCommand();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + curPoint.ToString());

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

            if (_step == Step.Step4_SpecifyPointOther)
            {
                _point = LitMath.Vector2.PointOrthoMode(_center, this.pointer.currentSnapPoint, presenter.IsOrtho);
                this.UpdatePolygon();
            }

            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_polygon != null)
            {
                this.presenter.DrawEntity(g, _polygon);
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

                // move the mouse cursor 0 pixels for redrawing
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
            }
        }

    }
}
