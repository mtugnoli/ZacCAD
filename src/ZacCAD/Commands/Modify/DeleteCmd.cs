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
    /// 删除命令
    /// </summary>
    internal class DeleteCmd : ModifyCmd
    {
        /// <summary>
        /// 操作的图元
        /// </summary>
        private List<Entity> _items = new List<Entity>();

        private void InitializeItemsToDelete()
        {
            Document doc = _mgr.presenter.document as Document;
            foreach (Selection sel in _mgr.presenter.selections)
            {
                DBObject dbobj = doc.database.GetObject(sel.objectId);
                if (dbobj != null && dbobj is Entity)
                {
                    Entity entity = dbobj as Entity;
                    _items.Add(entity);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            this.presenter.statusStripMgr.SetCommandLabel("ELIMINA");
            this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_Command + " _" + CommandNames.Modify_Delete);

            //
            if (_mgr.presenter.selections.Count > 0)
            {
                InitializeItemsToDelete();
                _mgr.FinishCurrentCommand();
            }
            else
            {
                this.pointer.mode = Pointer.Mode.Select;
                this.presenter.statusStripMgr.SetCommandInfoLabel(GlobalData.GlobalLanguage.Command_SelectObjects);
            }
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected override void Commit()
        {
            foreach (Entity item in _items)
            {
                item.Erase();
            }
        }

        /// <summary>
        /// 回滚撤销
        /// </summary>
        protected override void Rollback()
        {
            foreach (Entity item in _items)
            {
                _mgr.presenter.AppendEntity(item);
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

        public override EventResult OnMouseDown(MouseEventArgs e)
        {
            return EventResult.Handled;
        }

        public override EventResult OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_mgr.presenter.selections.Count > 0)
                {
                    InitializeItemsToDelete();
                    _mgr.FinishCurrentCommand();

                    this.presenter.AppendCommandLine(GlobalData.GlobalLanguage.Command_SelectObjects + " " + this.presenter.selections.Count.ToString());
                }
                else
                {
                    _mgr.CancelCurrentCommand();
                }
            }

            return EventResult.Handled;
        }

        public override EventResult OnMouseMove(MouseEventArgs e)
        {
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
                _mgr.FinishCurrentCommand();

                this.presenter.statusStripMgr.CommandFinish();
            }
        }
    }
}
