using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Draw
{
    internal class LinesChainCmd : DrawCmd
    {
        private List<Line> _lines = new List<Line>();
        private Line _currLine = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return _lines.ToArray(); }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SpecifyStartPoint = 1,
            Step2_SpecifyEndPoint = 2,
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

            this.presenter.statusStripMgr.SetCommandLabel("LINEA");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Line);
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
                _currLine = new Line();
                _currLine.startPoint = point;
                _currLine.endPoint = point;
                _currLine.layerId = this.document.currentLayerId;
                _currLine.color = this.document.currentColor;
                _currLine.lineType = this.document.currentLineType;

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointFirst + " " + point.ToString());

                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointNext);
                _step = Step.Step2_SpecifyEndPoint;

                this.presenter.statusStripMgr.CommandTextFocus();
            }
            else if (_step == Step.Step2_SpecifyEndPoint)
            {
                LitMath.Vector2 curPoint = LitMath.Vector2.PointOrthoMode(_currLine.startPoint, point, presenter.IsOrtho);

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointNext + " " + curPoint.ToString());

                _currLine.endPoint = curPoint;
                _currLine.layerId = this.document.currentLayerId;
                _currLine.color = this.document.currentColor;
                _currLine.lineType = this.document.currentLineType;
                _lines.Add(_currLine);

                _currLine = new Line();
                _currLine.startPoint = curPoint;
                _currLine.endPoint = curPoint;
                _currLine.layerId = this.document.currentLayerId;
                _currLine.color = this.document.currentColor;
                _currLine.lineType = this.document.currentLineType;

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

            if (_currLine != null)
            {
                Presenter presenter = _mgr.presenter as Presenter;

                _currLine.endPoint = LitMath.Vector2.PointOrthoMode(_currLine.startPoint, this.pointer.currentSnapPoint, presenter.IsOrtho);
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_lines.Count > 0)
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
            foreach (Line line in _lines)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, line);
            }

            if (_currLine != null)
            {
                Presenter presenter = _mgr.presenter as Presenter;
                presenter.DrawEntity(g, _currLine);
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
