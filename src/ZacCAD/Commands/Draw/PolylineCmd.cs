using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;
using LitMath;

namespace ZacCAD.Commands.Draw
{
    internal class PolylineCmd : DrawCmd
    {
        private Polyline _polyline = null;
        private Arc _arc = null;
        private Line _line = null;
        private Line _line2 = null;
        private Line _line3 = null;
        private string _curcom = "L";

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new Polyline[1] { _polyline }; }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyStartPoint = 1,
            Step2_SpecifyOtherPoint = 2,
        }
        private Step _step = Step.Step1_SpecifyStartPoint;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //
            _step = Step.Step1_SpecifyStartPoint;
            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("PLINEA");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointStart);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Polyline);
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
            if (_step == Step.Step1_SpecifyStartPoint)
            {
                _polyline = new Polyline();
                _polyline.AddVertexAt(_polyline.NumberOfVertices, point);

                _line = new Line();
                _line.startPoint = _line.endPoint = point;

                _arc = new Arc();
                _arc.radius = 0;
                _arc.layerId = this.document.currentLayerId;
                _arc.color = this.document.currentColor;
                _arc.lineType = this.document.currentLineType;

                _line2 = new Line();
                _line2.startPoint = _line.endPoint = point;

                _line3 = new Line();
                _line3.startPoint = _line.endPoint = point;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointStart + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointPolilyneOptions);
                _step = Step.Step2_SpecifyOtherPoint;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecifyOtherPoint)
            {
                double bulge = 0;
                LitMath.Vector2b pos = _polyline.Vertices[_polyline.NumberOfVertices - 1];

                LitMath.Vector2 curPos = LitMath.Vector2.PointOrthoMode(new LitMath.Vector2(pos.x, pos.y), point, presenter.IsOrtho);

                if (_curcom == "A")
                {
                    bulge = DrawArcBetweenTwoPoints(new LitMath.Vector2(pos.x, pos.y), point);

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointNext + " " + curPos.ToString() + " bulge " + bulge.ToString());

                    _polyline.AddVertexAt(_polyline.NumberOfVertices, point);
                    _polyline.SetBulgeAt(_polyline.NumberOfVertices - 2, bulge);
                }
                else
                {
                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointNext + " " + curPos.ToString());

                    _polyline.AddVertexAt(_polyline.NumberOfVertices, curPos);
                }

                _line.startPoint = curPos;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
        }



        public double DrawArcBetweenTwoPoints(LitMath.Vector2 first, LitMath.Vector2 cur)
        {
            LitMath.Vector2 intersection = new LitMath.Vector2();
            LitMath.Vector2 lastVertex = new LitMath.Vector2();
            LitMath.Vector2 pv = new LitMath.Vector2();
            LitMath.Vector2 pc = new LitMath.Vector2();
            double bulge = 0;

            //if (_polyline.NumberOfVertices <= 1)
            //    lastVertex = Vector2.Polar(first, 10, Utils.PI);
            //else
            //    lastVertex = _polyline.Vertices[_polyline.NumberOfVertices - 2].Position;

            double distance = Vector2.Distance(first, cur);
            double anglePrev = Vector2.Angle(lastVertex, first);
            double angle = Vector2.Angle(first, cur);
            double angleBase = Math.Abs(Vector2.AnglesDifference(angle, anglePrev));

            if (!LitMath.Vector2.Clockwise(lastVertex, first, cur))

            {
                pv = Vector2.Polar(first, distance * 10000, anglePrev + Utils.HalfPI);
                pc = Vector2.Polar(cur, distance * 10000, angle + Utils.HalfPI + angleBase);
            }
            else
            {
                pv = Vector2.Polar(first, distance * 10000, anglePrev - Utils.HalfPI);
                pc = Vector2.Polar(cur, distance * 10000, angle - Utils.HalfPI - angleBase);
            }


            bool isInt = LitMath.Line2.Intersect(new LitMath.Line2(first, pv), new LitMath.Line2(cur, pc), ref intersection);

            if (isInt)
            {
                _line2.color = ZacCAD.Colors.Color.FromColor(Color.Orange);
                _line2.startPoint = first;
                _line2.endPoint = intersection;
                _line2.lineType = ZacCAD.DatabaseServices.LineType.Dash;

                _line3.color = ZacCAD.Colors.Color.FromColor(Color.Orange);
                _line3.startPoint = cur;
                _line3.endPoint = intersection;
                _line3.lineType = ZacCAD.DatabaseServices.LineType.Dash;

                distance = Vector2.Distance(intersection, first);
                _arc.color = ZacCAD.Colors.Color.FromColor(Color.Yellow);
                _arc.lineType = this.document.currentLineType;
                _arc.center = intersection;
                _arc.radius = distance;

                double startAngle = LitMath.Vector2.Angle(intersection, first);
                startAngle = MathUtils.NormalizeRadianAngle(startAngle);

                double endAngle = LitMath.Vector2.Angle(intersection, cur);
                endAngle = MathUtils.NormalizeRadianAngle(endAngle);

                if (!LitMath.Vector2.Clockwise(lastVertex, first, cur))
                {
                    _arc.startAngle = startAngle;
                    _arc.endAngle = endAngle;
                }
                else
                {
                    _arc.startAngle = endAngle;
                    _arc.endAngle = startAngle;
                }

                bulge = GetBulge(Math.Abs(Vector2.AnglesDifference(endAngle, startAngle)));

                if (LitMath.Vector2.Clockwise(lastVertex, first, cur))
                    bulge = -bulge;

            }



            return bulge;
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
                case Step.Step1_SpecifyStartPoint:
                    break;

                case Step.Step2_SpecifyOtherPoint:
                    if (_curcom == "A")
                    {
                        if (_line != null)
                        {
                            _line.endPoint = this.pointer.currentSnapPoint;
                            _line.color = ZacCAD.Colors.Color.FromColor(Color.White);
                            _line.lineType = this.document.currentLineType;

                            DrawArcBetweenTwoPoints(_line.startPoint, this.pointer.currentSnapPoint);
                        }
                    }
                    else
                    {
                        if (_line != null)
                        {
                            _line.endPoint = LitMath.Vector2.PointOrthoMode(_line.startPoint, this.pointer.currentSnapPoint, presenter.IsOrtho);
                            _line.color = ZacCAD.Colors.Color.FromColor(Color.White);
                            _line.lineType = this.document.currentLineType;
                        }
                    }

                    break;
            }

            return EventResult.Handled;
        }


        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_polyline.NumberOfVertices > 1)
                {
                    _polyline.layerId = this.document.currentLayerId;
                    _polyline.color = this.document.currentColor;
                    _line.color = ZacCAD.Colors.Color.FromColor(Color.White);
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
            if (_polyline != null)
            {
                _mgr.presenter.DrawEntity(g, _polyline);
            }
            if (_line != null && _curcom != "A")
            {
                _mgr.presenter.DrawEntity(g, _line);
            }
            if (_arc != null && _curcom == "A")
            {
                _mgr.presenter.DrawEntity(g, _arc);
            }
            if (_line2 != null && _curcom == "A")
            {
                _mgr.presenter.DrawEntity(g, _line2);
            }
            if (_line3 != null && _curcom == "A")
            {
                _mgr.presenter.DrawEntity(g, _line3);
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
                string cmd = parameter.ToUpper();

                if (cmd == "A" || cmd == "L")
                {
                    _curcom = cmd;

                    // move the mouse cursor 0 pixels for redrawing
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
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

        private double GetBulge(double angleRad)
        {
            double bulge = Math.Tan(angleRad * 0.25f);

            return bulge;
        }

        private LitMath.Vector2 BulgeCenter(LitMath.Vector2 p1, LitMath.Vector2 p2, double b)
        {
            LitMath.Vector2 delta = p2 - p1;

            double length = Vector2.Distance(p1, p2);
            double alpha = 4 * System.Math.Atan(b);
            double radius = length / (2d * System.Math.Abs(System.Math.Sin(alpha * 0.5d)));
            LitMath.Vector2 lnormalized = delta;
            lnormalized.Normalize();
            double bulgeSign = System.Math.Sign(b);
            LitMath.Vector2 lnormal = new LitMath.Vector2(-lnormalized.y, lnormalized.x) * bulgeSign;

            LitMath.Vector2 arcCenter = ((p2 + p1) * 0.5d) + lnormal * System.Math.Cos(alpha * 0.5d) * radius;

            return arcCenter;
        }

    }
}
