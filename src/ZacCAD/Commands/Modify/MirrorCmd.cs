using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;
using ZacCAD.UI;

namespace ZacCAD.Commands.Modify
{
    /// <summary>
    /// 镜像命令
    /// </summary>
    internal class MirrorCmd : ModifyCmd
    {
        /// <summary>
        /// 源图元
        /// </summary>
        private List<Entity> _entities = new List<Entity>();

        /// <summary>
        /// 结果图元
        /// </summary>
        private List<Entity> _resultEntities = new List<Entity>();

        /// <summary>
        /// 源图元是否被删除
        /// </summary>
        private bool _isSrcDeleted = false;

        /// <summary>
        /// 镜像线
        /// </summary>
        private Line _mirrorLine = null;

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            // Choice object
            Step1_SelectObject = 1,
            // Specify the first point of the mirror line
            Step2_SpecifyMirrorLinePoint1st = 2,
            // Specify the mirror line second point
            Step3_SpecifyMirrorLinePoint2nd = 3,
            // Whether to delete the source object
            Step4_WhetherDelSrc = 4,
        }
        private Step _step = Step.Step1_SelectObject;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.presenter.statusStripMgr.SetCommandLabel("SPECCHIA");
            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Modify_Mirror);


            if (this.presenter.selections.Count > 0)
            {
                foreach (Selection sel in this.presenter.selections)
                {
                    Entity entity = this.database.GetObject(sel.objectId) as Entity;
                    if (entity != null)
                    {
                        _entities.Add(entity);
                    }
                }
            }

            if (_entities.Count > 0)
            {
                this.pointer.mode = Pointer.Mode.Locate;
                _step = Step.Step2_SpecifyMirrorLinePoint1st;
                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
            }
            else
            {
                this.presenter.selections.Clear();
                _step = Step.Step1_SelectObject;
                this.pointer.mode = Pointer.Mode.Select;
                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_SelectObjects);
            }
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected override void Commit()
        {
            foreach (Entity item in _resultEntities)
            {
                _mgr.presenter.AppendEntity(item);
            }
        }

        /// <summary>
        /// 回滚撤销
        /// </summary>
        protected override void Rollback()
        {
            foreach (Entity item in _resultEntities)
            {
                item.Erase();
            }
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
                case Step.Step1_SelectObject:
                    this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
                    this.presenter.statusStripMgr.CommandTextFocus();

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_SelectObjects + " " + this.presenter.selections.Count.ToString());

                    _step = Step.Step2_SpecifyMirrorLinePoint1st;
                    break;

                case Step.Step2_SpecifyMirrorLinePoint1st:
                    {
                        _mirrorLine = new Line();
                        _mirrorLine.startPoint = point;
                        _mirrorLine.endPoint = _mirrorLine.startPoint;

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointFirst + " " + point.ToString());

                        this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSecond);
                        _step = Step.Step3_SpecifyMirrorLinePoint2nd;

                        this.presenter.statusStripMgr.CommandTextFocus();
                    }
                    break;

                case Step.Step3_SpecifyMirrorLinePoint2nd:
                    {
                        _mirrorLine.endPoint = point;
                        this.UpdateResultEntities();

                        _step = Step.Step4_WhetherDelSrc;
                        _mgr.FinishCurrentCommand();

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSecond + " " + point.ToString());

                        this.presenter.statusStripMgr.CommandFinish();
                    }
                    break;
            }
        }

        public override EventResult OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _step != Step.Step1_SelectObject)
            {
                GotoStep(_step, this.pointer.currentSnapPoint);
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseUp(MouseEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SelectObject:
                    if (e.Button == MouseButtons.Right)
                    {
                        if (this.presenter.selections.Count > 0)
                        {
                            foreach (Selection sel in _mgr.presenter.selections)
                            {
                                DBObject dbobj = this.database.GetObject(sel.objectId);
                                Entity entity = dbobj as Entity;
                                _entities.Add(entity);
                            }

                            GotoStep(Step.Step1_SelectObject, this.pointer.currentSnapPoint);

                            this.pointer.mode = Pointer.Mode.Locate;
                        }
                        else
                        {
                            _mgr.CancelCurrentCommand();
                        }
                    }
                    break;

                case Step.Step2_SpecifyMirrorLinePoint1st:
                    break;

                case Step.Step3_SpecifyMirrorLinePoint2nd:
                    break;

                case Step.Step4_WhetherDelSrc:
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseMove(MouseEventArgs e)
        {
            if (_step == Step.Step3_SpecifyMirrorLinePoint2nd)
            {
                _mirrorLine.endPoint = this.pointer.currentSnapPoint;
                this.UpdateResultEntities();
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _mgr.CancelCurrentCommand();
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyUp(KeyEventArgs e)
        {
            return EventResult.Handled;
        }

        public override void OnPaint(Graphics g)
        {
            if (_step == Step.Step3_SpecifyMirrorLinePoint2nd)
            {
                this.presenter.DrawEntity(g, _mirrorLine);
            }

            if (_step == Step.Step3_SpecifyMirrorLinePoint2nd || _step == Step.Step4_WhetherDelSrc)
            {
                foreach (Entity entity in _resultEntities)
                {
                    this.presenter.DrawEntity(g, entity);
                }
            }
        }

        /// <summary>
        /// 刷新结果图元
        /// </summary>
        private void UpdateResultEntities()
        {
            LitMath.Matrix3 mirrorMatrix = MathUtils.MirrorMatrix(new LitMath.Line2(_mirrorLine.startPoint, _mirrorLine.endPoint));
            _resultEntities.Clear();
            foreach (Entity entity in _entities)
            {
                Entity copy = entity.Clone() as Entity;
                copy.TransformBy(mirrorMatrix);
                _resultEntities.Add(copy);
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
