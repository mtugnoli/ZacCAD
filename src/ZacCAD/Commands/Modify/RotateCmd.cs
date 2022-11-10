using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;
using ZacCAD.UI;
using LitMath;

namespace ZacCAD.Commands.Modify
{
    /// <summary>
    /// 移动命令
    /// </summary>
    internal class RotateCmd : ModifyCmd
    {
        /// <summary>
        /// 操作的图元
        /// </summary>
        private List<Entity> _itemsToRotate = new List<Entity>();
        private List<Entity> _tempItemsToDraw = new List<Entity>();
        private double _angle = 0;

        private void InitializeItemsToRotate()
        {
            Document doc = _mgr.presenter.document as Document;
            foreach (Selection sel in _mgr.presenter.selections)
            {
                DBObject dbobj = doc.database.GetObject(sel.objectId);
                if (dbobj != null && dbobj is Entity)
                {
                    Entity entity = dbobj as Entity;
                    _itemsToRotate.Add(entity);

                    //Entity copy = entity.Clone() as Entity;
                    //_tempItemsToDraw.Add(copy);
                }
            }
        }

        /// <summary>
        /// 步骤
        /// </summary>
        private enum Step
        {
            Step1_SelectObjects = 1,
            Step2_SpecifyBasePoint = 2,
            Step3_SpecifySecondPoint = 3,
        }
        private Step _step = Step.Step1_SelectObjects;

        /// <summary>
        /// 移动路径线
        /// </summary>
        private LitMath.Line2 _pathLine = new LitMath.Line2();

        public override void Initialize()
        {
            base.Initialize();

            this.presenter.statusStripMgr.SetCommandLabel("RUOTA");
            this.presenter.statusStripMgr.CommandTextFocus();
            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Modify_Rotate);

            //
            if (_mgr.presenter.selections.Count > 0)
            {
                _step = Step.Step2_SpecifyBasePoint;
                InitializeItemsToRotate();
                this.pointer.mode = Pointer.Mode.Locate;
                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
            }
            else
            {
                _step = Step.Step1_SelectObjects;
                this.pointer.mode = Pointer.Mode.Select;
                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_SelectObjects);
            }
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected override void Commit()
        {
            foreach (Entity item in _itemsToRotate)
            {
                item.Rotate(_pathLine.startPoint, _angle);
            }
        }

        /// <summary>
        /// 回滚撤销
        /// </summary>
        protected override void Rollback()
        {
            foreach (Entity item in _itemsToRotate)
            {
                item.Rotate(_pathLine.startPoint, -_angle);
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
                case Step.Step1_SelectObjects:
                    this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointFirst);
                    this.presenter.statusStripMgr.CommandTextFocus();

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_SelectObjects + " " + this.presenter.selections.Count.ToString());

                    _step = Step.Step2_SpecifyBasePoint;
                    break;

                case Step.Step2_SpecifyBasePoint:
                    {
                        _pathLine.startPoint = point;
                        _pathLine.endPoint = _pathLine.startPoint;

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointFirst + " " + point.ToString());

                        this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_PointSecond);
                        _step = Step.Step3_SpecifySecondPoint;

                        this.presenter.statusStripMgr.CommandTextFocus();
                    }
                    break;

                case Step.Step3_SpecifySecondPoint:
                    {
                        LitMath.Vector2 curPoint = LitMath.Vector2.PointOrthoMode(_pathLine.startPoint, point, presenter.IsOrtho);

                        _pathLine.endPoint = curPoint;
                        this.UpdateResultEntities();

                        _mgr.FinishCurrentCommand();

                        this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_PointSecond + " " + curPoint.ToString());

                        this.presenter.statusStripMgr.CommandFinish();
                    }
                    break;
            }
        }

        public override EventResult OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _step != Step.Step1_SelectObjects)
            {
                GotoStep(_step, this.pointer.currentSnapPoint);
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseUp(MouseEventArgs e)
        {
            if (_step == Step.Step1_SelectObjects)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (_mgr.presenter.selections.Count > 0)
                    {
                        InitializeItemsToRotate();

                        GotoStep(Step.Step1_SelectObjects, this.pointer.currentSnapPoint);

                        this.pointer.mode = Pointer.Mode.Locate;
                    }
                    else
                    {
                        _mgr.CancelCurrentCommand();
                    }
                }
            }
            else if (_step == Step.Step2_SpecifyBasePoint)
            {
            }
            else if (_step == Step.Step3_SpecifySecondPoint)
            {
            }

            return EventResult.Handled;
        }

        private void UpdateResultEntities()
        {
            _tempItemsToDraw.Clear();
            foreach (Entity entity in _itemsToRotate)
            {
                Entity copy = entity.Clone() as Entity;
                copy.Rotate(_pathLine.startPoint, _angle);
                _tempItemsToDraw.Add(copy);
            }
        }

        public override EventResult OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return EventResult.Unhandled;
            }

            if (_step == Step.Step1_SelectObjects)
            {
            }
            else if (_step == Step.Step2_SpecifyBasePoint)
            {
            }
            else if (_step == Step.Step3_SpecifySecondPoint)
            {
                _pathLine.endPoint = LitMath.Vector2.PointOrthoMode(_pathLine.startPoint, this.pointer.currentSnapPoint, presenter.IsOrtho); ;

                _angle = Vector2.Angle(_pathLine.startPoint, _pathLine.endPoint);

                this.UpdateResultEntities();
            }

            return EventResult.Handled;
        }

        public override EventResult OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_tempItemsToDraw.Count > 0)
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
            switch (_step)
            {
                case Step.Step1_SelectObjects:
                    break;

                case Step.Step2_SpecifyBasePoint:
                    break;

                case Step.Step3_SpecifySecondPoint:
                    foreach (Entity entity in _tempItemsToDraw)
                    {
                        this.presenter.DrawEntity(g, entity);
                    }
                    _mgr.presenter.DrawLine(g, GDIResMgr.Instance.GetPen(Color.White, System.Drawing.Drawing2D.DashStyle.Solid, 1), _pathLine.startPoint, _pathLine.endPoint, CSYS.Model);
                    break;

                default:
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
                if (parameter.Contains(","))
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
                    double ang = 0;
                    bool ret = double.TryParse(parameter, out ang);
                    if (ret)
                    {
                        _angle = LitMath.Utils.DegreeToRadian(ang);

                        this.UpdateResultEntities();

                        _mgr.FinishCurrentCommand();

                        this.presenter.statusStripMgr.CommandFinish();
                    }
                }
            }
        }

    }
}
