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
    /// <summary>
    /// 绘制点命令
    /// </summary>
    internal class PointCmd : DrawCmd
    {
        private XPoint _point = null;

        /// <summary>
        /// 新增的图元
        /// </summary>
        protected override IEnumerable<Entity> newEntities
        {
            get { return new XPoint[1] { _point }; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.pointer.mode = UI.Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("PUNTO");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSpecified);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Draw_Point);
        }

        public override void Terminate()
        {
            this.presenter.statusStripMgr.CommandFinish();

            base.Terminate();
        }

        public override EventResult OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _point = new XPoint(this.pointer.currentSnapPoint);
                _point.color = this.document.currentColor;
                _mgr.FinishCurrentCommand();

                this.presenter.statusStripMgr.CommandFinish();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + this.pointer.currentSnapPoint.ToString());
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseUp(MouseEventArgs e)
        {
            return EventResult.Handled;
        }

        public override EventResult OnMouseMove(MouseEventArgs e)
        {
            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_point != null)
            {
                this.presenter.DrawEntity(g, _point);
            }
        }


        public override void OnParameter(string parameter)
        {
            if (parameter == "Escape")
            {
                _mgr.CancelCurrentCommand();

                this.presenter.statusStripMgr.CommandFinish();

                this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " " + GlobalData.GlobalLanguage.Command_Cancel);

                return;
            }
            else
            {
                LitMath.Vector2 pos = LitMath.Vector2.StringToVector(parameter);
                if (pos.isvalid)
                {
                    _point = new XPoint(pos);
                    _point.color = this.document.currentColor;
                    _mgr.FinishCurrentCommand();

                    this.presenter.statusStripMgr.CommandFinish();

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSpecified + " " + pos.ToString());
                }
                else
                {
                    _mgr.FinishCurrentCommand();

                    this.presenter.statusStripMgr.CommandFinish();

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " " + GlobalData.GlobalLanguage.Command_Cancel);
                }
            }
        }
    }
}
