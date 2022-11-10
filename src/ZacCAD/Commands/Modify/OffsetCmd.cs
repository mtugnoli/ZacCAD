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
    /// 偏移命令
    /// </summary>
    internal class OffsetCmd : ModifyCmd
    {
        /// <summary>
        /// 结果图元
        /// </summary>
        private List<Entity> _resultEntities = new List<Entity>();

        /// <summary>
        /// 当前正在操作的图元
        /// 参数
        /// </summary>
        private Entity _currEntity = null;
        private Offset._OffsetOperation _currOffsetOp = null;
        private double _offsetDis = 0.0;

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            // Specify the offset distance
            Step1_SpecifyOffsetDistance = 1,
            // Choose objects to be offset
            Step2_SelectObject = 2,
            // Specify the offset side
            Step3_SpecifyOffsetSide = 3,
        }
        private Step _step = Step.Step1_SpecifyOffsetDistance;

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            if (this.presenter.selections.Count == 1)
            {
                foreach (Selection sel in this.presenter.selections)
                {
                    Entity entity = this.database.GetObject(sel.objectId) as Entity;
                    _currEntity = entity;
                    _currOffsetOp = Offset._OffsetOpsMgr.Instance.NewOffsetOperation(_currEntity);
                    break;
                }
            }
            if (_currEntity == null)
            {
                this.presenter.selections.Clear();
            }

            _step = Step.Step1_SpecifyOffsetDistance;
            this.pointer.mode = Pointer.Mode.Locate;

            this.presenter.statusStripMgr.SetCommandLabel("OFFSET");
            this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_ValueOffsetDist);
            this.presenter.statusStripMgr.CommandTextFocus();

            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Modify_Offset);
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
                case Step.Step1_SpecifyOffsetDistance:


                    break;

                case Step.Step2_SelectObject:
                    {


                    }
                    break;

                case Step.Step3_SpecifyOffsetSide:
                    {


                    }
                    break;
            }
        }

        public override EventResult OnMouseDown(MouseEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SpecifyOffsetDistance:
                    break;

                case Step.Step2_SelectObject:
                    break;

                case Step.Step3_SpecifyOffsetSide:
                    if (e.Button == MouseButtons.Left
                        && _currOffsetOp != null)
                    {
                        if (_currOffsetOp.Do(_offsetDis, this.pointer.currentSnapPoint))
                        {
                            _resultEntities.Add(_currOffsetOp.result);
                            _mgr.FinishCurrentCommand();
                        }
                        else
                        {
                            _mgr.CancelCurrentCommand();
                        }
                    }
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseUp(MouseEventArgs e)
        {
            switch (_step)
            {
                case Step.Step1_SpecifyOffsetDistance:
                    break;

                case Step.Step2_SelectObject:
                    if (e.Button == MouseButtons.Right)
                    {
                        if (this.presenter.selections.Count > 0)
                        {
                            foreach (Selection sel in _mgr.presenter.selections)
                            {
                                DBObject dbobj = this.database.GetObject(sel.objectId);
                                _currEntity = dbobj as Entity;
                                _currOffsetOp = Offset._OffsetOpsMgr.Instance.NewOffsetOperation(_currEntity);
                                break;
                            }

                            this.pointer.mode = Pointer.Mode.Locate;
                            _step = Step.Step3_SpecifyOffsetSide;
                        }
                        else
                        {
                            _mgr.CancelCurrentCommand();
                        }
                    }
                    break;

                case Step.Step3_SpecifyOffsetSide:
                    break;

                default:
                    break;
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseMove(MouseEventArgs e)
        {
            if (_step == Step.Step3_SpecifyOffsetSide)
            {
                if (_currOffsetOp != null)
                {
                    _currOffsetOp.Do(_offsetDis, this.pointer.currentSnapPoint);
                }
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_resultEntities.Count > 0)
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
            if (_currOffsetOp != null
                && _currOffsetOp.result != null)
            {
                this.presenter.DrawEntity(g, _currOffsetOp.result);
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
