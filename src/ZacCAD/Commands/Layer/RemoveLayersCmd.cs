using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ZacCAD.ApplicationServices;
using ZacCAD.DatabaseServices;

namespace ZacCAD.Commands
{
    /// <summary>
    /// 删除图层命令
    /// </summary>
    internal class RemoveLayersCmd : Command
    {
        private List<Layer> _layers = new List<Layer>();
        private List<Layer> _layersToDelete = new List<Layer>();

        public RemoveLayersCmd(List<Layer> layers)
        {
            _layers.AddRange(layers);
        }

        public override void Initialize()
        {
            DBUtils.DatabaseUtils dbUtils = new DBUtils.DatabaseUtils(this.database);

            foreach (Layer layer in _layers)
            {
                if (dbUtils.IsLayerCanDelete(layer.id))
                {
                    _layersToDelete.Add(layer);
                }
            }

            int cntCanNotToDelete = _layers.Count - _layersToDelete.Count;
            if (cntCanNotToDelete > 0)
            {
                string message = string.Format("{0} selected layers could not be deleted (of {1} selected layers)", cntCanNotToDelete, _layers.Count);
                MessageBox.Show(message);
            }

            if (_layersToDelete.Count > 0)
            {
                _mgr.FinishCurrentCommand();
            }
            else
            {
                _mgr.CancelCurrentCommand();
            }
        }

        public override void Undo()
        {
            foreach (Layer layer in _layersToDelete)
            {
                this.database.layerTable.Add(layer);
            }
        }

        public override void Redo()
        {
            this.Commit();
        }

        private void Commit()
        {
            foreach (Layer layer in _layersToDelete)
            {
                layer.Erase();
            }
        }

        public override void Finish()
        {
            this.Commit();

            base.Finish();
        }

        public override void Cancel()
        {
            base.Cancel();
        }
    }
}
