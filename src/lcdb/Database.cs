using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZacCAD.Colors;
using netDxf;
using LitMath;

namespace ZacCAD.DatabaseServices
{
    public class Database
    {
        /// <summary>
        /// 块表
        /// </summary>
        private BlockTable _blockTable = null;
        public BlockTable blockTable
        {
            get { return _blockTable; }
        }
        public static ObjectId BlockTableId
        {
            get { return new ObjectId(TableIds.BlockTableId); }
        }

        /// <summary>
        /// 图层表
        /// </summary>
        private LayerTable _layerTable = null;
        public static ObjectId LayerTableId
        {
            get { return new ObjectId(TableIds.LayerTableId); }
        }
        public LayerTable layerTable
        {
            get { return _layerTable; }
        }

        /// <summary>
        /// ID
        /// </summary>
        private Dictionary<ObjectId, DBObject> _dictId2Object = null;
        internal ObjectId currentMaxId
        {
            get
            {
                if (_dictId2Object == null || _dictId2Object.Count == 0)
                {
                    return ObjectId.Null;
                }
                else
                {
                    ObjectId id = ObjectId.Null;
                    foreach (KeyValuePair<ObjectId, DBObject> kvp in _dictId2Object)
                    {
                        if (kvp.Key.CompareTo(id) > 0)
                        {
                            id = kvp.Key;
                        }
                    }
                    return id;
                }
            }
        }

        private ObjectIdMgr _idMgr = null;

        /// <summary>
        /// 文件名
        /// </summary>
        private string _fileName = null;
        public string fileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Database()
        {
            _dictId2Object = new Dictionary<ObjectId, DBObject>();
            _idMgr = new ObjectIdMgr(this);

            _blockTable = new BlockTable(this);
            Block modelSpace = new Block();
            modelSpace.name = "ModelSpace";
            _blockTable.Add(modelSpace);
            IdentifyDBTable(_blockTable);

            _layerTable = new LayerTable(this);
            IdentifyDBTable(_layerTable);
        }

        /// <summary>
        /// 通过ID获取数据库对象
        /// </summary>
        public DBObject GetObject(ObjectId oid)
        {
            if (_dictId2Object.ContainsKey(oid))
            {
                return _dictId2Object[oid];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileFullPath">文件路径</param>
        public void Open(string fileFullPath)
        {
            if (_fileName == null || _fileName == "")
            {
                string ext = Path.GetExtension(fileFullPath);

                if (ext.ToLower() == ".dxf")
                {
                    OpenDxf(fileFullPath);
                }
                else
                {
                    XmlIn(fileFullPath);
                }
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void Save()
        {
            if (_fileName != null
                && System.IO.File.Exists(_fileName))
            {
                XmlOut(_fileName);
            }
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="fileFullPath">文件路径</param>
        /// <param name="rename">是否重命名</param>
        public void SaveAs(string fileFullPath, bool rename = false)
        {
            string ext = Path.GetExtension(fileFullPath);

            if (ext.ToLower() == ".dxf")
            {
                SaveAsDxf(fileFullPath);
            }
            else
            {
                XmlOut(fileFullPath);
            }

            if (rename)
            {
                _fileName = fileFullPath;
            }
        }

        internal void OpenDxf(string fileFullPath)
        {
            // this check is optional but recommended before loading a DXF file
            var dxfVersion = DxfDocument.CheckDxfFileVersion(fileFullPath);
            // netDxf is only compatible with AutoCad2000 and higher DXF versions
            if (dxfVersion < netDxf.Header.DxfVersion.AutoCad2000)
            {
                MessageBox.Show("Versione DXF non compatibile.");
                return;
            }

            // load file
            DxfDocument dxfDoc = DxfDocument.Load(fileFullPath);

            //
            ClearLayerTable();
            ClearBlockTable();

            //foreach (var lay in dxfDoc.Layers)
            //{
            //    _layerTable.Add(new Layer { name = lay.Name, color = Color.FromRGB(lay.Color.R, lay.Color.G, lay.Color.B), lineWeight = LineWeight.ByLineWeightDefault, lineType = LineType.ByLineTypeDefault, description = lay.Description });
            //}

            Block modelSpace = new Block();
            modelSpace.name = "ModelSpace";


            foreach (var ent in dxfDoc.Entities.Arcs)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Arc { center = new LitMath.Vector2(ent.Center.X, ent.Center.Y), radius = ent.Radius, startAngle = Utils.DegreeToRadian(ent.StartAngle), endAngle = Utils.DegreeToRadian(ent.EndAngle), color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.Circles)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Circle { center = new LitMath.Vector2(ent.Center.X, ent.Center.Y), radius = ent.Radius, color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.Ellipses)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Ellipse { center = new LitMath.Vector2(ent.Center.X, ent.Center.Y), radiusX = ent.MajorAxis, radiusY = ent.MinorAxis, color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.Lines)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Line { startPoint = new LitMath.Vector2(ent.StartPoint.X, ent.StartPoint.Y), endPoint = new LitMath.Vector2(ent.EndPoint.X, ent.EndPoint.Y), color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.Points)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.XPoint { endPoint = new LitMath.Vector2(ent.Position.X, ent.Position.Y), color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.Rays)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Ray { basePoint = new LitMath.Vector2(ent.Origin.X, ent.Origin.Y), direction = new LitMath.Vector2(ent.Direction.X, ent.Direction.Y), color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.XLines)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Xline { basePoint = new LitMath.Vector2(ent.Origin.X, ent.Origin.Y), direction = new LitMath.Vector2(ent.Direction.X, ent.Direction.Y), color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name });
            }

            foreach (var ent in dxfDoc.Entities.Texts)
            {
                modelSpace.AppendEntity(new ZacCAD.DatabaseServices.Text { position = new LitMath.Vector2(ent.Position.X, ent.Position.Y),  color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name, height = ent.Height, text = ent.Value });
            }

            foreach (var ent in dxfDoc.Entities.Polylines2D)
            {
                var poly = new ZacCAD.DatabaseServices.Polyline { color = Color.FromRGB(ent.Color.R, ent.Color.G, ent.Color.B), lineWeight = LineWeight.ByLineWeightDefault, layer = ent.Layer.Name };

                for (int i = 0; i < ent.Vertexes.Count; ++i)
                {
                    poly.AddVertexAt(i, new LitMath.Vector2b(ent.Vertexes[i].Position.X, ent.Vertexes[i].Position.Y, ent.Vertexes[i].Bulge));
                }

                poly.closed = ent.IsClosed;

                modelSpace.AppendEntity(poly);
            }

            _blockTable.Add(modelSpace);

            IdentifyDBTable(_blockTable);

            _fileName = fileFullPath;
            _idMgr.reset();
        }

        internal void SaveAsDxf(string fileFullPath)
        {
            // create a new document, by default it will create an AutoCad2000 DXF version
            DxfDocument doc = new DxfDocument();


            // LAYERS
            foreach (Layer layer in _layerTable)
            {
                netDxf.Tables.Layer lay = new netDxf.Tables.Layer(layer.name);

                lay.Color = new AciColor(layer.color.r, layer.color.g, layer.color.b);
                lay.Lineweight = Lineweight.Default;
                lay.Linetype = netDxf.Tables.Linetype.Continuous;
                lay.Description = layer.description;

                doc.Layers.Add(lay);
            }


            // ENTITIES
            foreach (Block block in _blockTable)
            {
                foreach (Entity entity in block)
                {
                    if (entity is Polyline)
                    {
                        var poly = (Polyline)entity;
                        string layername = string.IsNullOrEmpty(poly.layer) ? "0" : poly.layer;

                        //polyline
                        netDxf.Entities.Polyline2DVertex polyVertex;

                        List<netDxf.Entities.Polyline2DVertex> polyVertexes = new List<netDxf.Entities.Polyline2DVertex>();


                        foreach (var vertice in poly.Vertices)
                        {
                            polyVertex = new netDxf.Entities.Polyline2DVertex(new netDxf.Vector2(vertice.x, vertice.y));
                            polyVertex.Bulge = vertice.b;
                            polyVertexes.Add(polyVertex);
                        }

                        // an entity
                        netDxf.Entities.Polyline2D dxfEntity = new netDxf.Entities.Polyline2D(polyVertexes, poly.closed);

                        dxfEntity.Color = new AciColor(poly.color.r, poly.color.g, poly.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Circle)
                    {
                        var circle = (Circle)entity;
                        string layername = string.IsNullOrEmpty(circle.layer) ? "0" : circle.layer;

                        // an entity
                        netDxf.Entities.Circle dxfEntity = new netDxf.Entities.Circle(new netDxf.Vector2(circle.center.x, circle.center.y), circle.radius);
                        dxfEntity.Color = new AciColor(circle.color.r, circle.color.g, circle.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Ellipse)
                    {
                        var ellipse = (Ellipse)entity;
                        string layername = string.IsNullOrEmpty(ellipse.layer) ? "0" : ellipse.layer;

                        // an entity
                        netDxf.Entities.Ellipse dxfEntity = new netDxf.Entities.Ellipse(new netDxf.Vector2(ellipse.center.x, ellipse.center.y), ellipse.radiusX, ellipse.radiusY);
                        dxfEntity.Color = new AciColor(ellipse.color.r, ellipse.color.g, ellipse.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Arc)
                    {
                        var arc = (Arc)entity;
                        string layername = string.IsNullOrEmpty(arc.layer) ? "0" : arc.layer;

                        // an entity
                        netDxf.Entities.Arc dxfEntity = new netDxf.Entities.Arc(new netDxf.Vector2(arc.center.x, arc.center.y), arc.radius, Utils.RadianToDegree(arc.startAngle), Utils.RadianToDegree(arc.endAngle));
                        dxfEntity.Color = new AciColor(arc.color.r, arc.color.g, arc.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Line)
                    {
                        var line = (Line)entity;
                        string layername = string.IsNullOrEmpty(line.layer) ? "0" : line.layer;

                        // an entity
                        netDxf.Entities.Line dxfEntity = new netDxf.Entities.Line(new netDxf.Vector2(line.startPoint.x, line.startPoint.y), new netDxf.Vector2(line.endPoint.x, line.endPoint.y));
                        dxfEntity.Color = new AciColor(line.color.r, line.color.g, line.color.b);
                        dxfEntity.Linetype = LineTypeFromEntityLineType(line);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is XPoint)
                    {
                        var point = (XPoint)entity;
                        string layername = string.IsNullOrEmpty(point.layer) ? "0" : point.layer;

                        // an entity
                        netDxf.Entities.Point dxfEntity = new netDxf.Entities.Point { Position = new netDxf.Vector3(point.endPoint.x, point.endPoint.y, 0) };
                        dxfEntity.Color = new AciColor(point.color.r, point.color.g, point.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Ray)
                    {
                        var ray = (Ray)entity;
                        string layername = string.IsNullOrEmpty(ray.layer) ? "0" : ray.layer;

                        // an entity
                        netDxf.Entities.Ray dxfEntity = new netDxf.Entities.Ray { Origin = new netDxf.Vector3(ray.basePoint.x, ray.basePoint.y, 0), Direction = new netDxf.Vector3(ray.direction.x, ray.direction.y, 0) };
                        dxfEntity.Color = new AciColor(ray.color.r, ray.color.g, ray.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Xline)
                    {
                        var xline = (Xline)entity;
                        string layername = string.IsNullOrEmpty(xline.layer) ? "0" : xline.layer;

                        // an entity
                        netDxf.Entities.XLine dxfEntity = new netDxf.Entities.XLine { Origin = new netDxf.Vector3(xline.basePoint.x, xline.basePoint.y, 0), Direction = new netDxf.Vector3(xline.direction.x, xline.direction.y, 0) };
                        dxfEntity.Color = new AciColor(xline.color.r, xline.color.g, xline.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                    else if (entity is Text)
                    {
                        var text = (Text)entity;
                        string layername = string.IsNullOrEmpty(text.layer) ? "0" : text.layer;

                        // an entity
                        netDxf.Entities.Text dxfEntity = new netDxf.Entities.Text { Position  = new netDxf.Vector3(text.position.x, text.position.y, 0) };
                        dxfEntity.Color = new AciColor(text.color.r, text.color.g, text.color.b);
                        dxfEntity.Layer = doc.Layers.Where(o => o.Name == layername).SingleOrDefault();
                        dxfEntity.Lineweight = Lineweight.Default;
                        dxfEntity.Value = text.text;
                        dxfEntity.Height = text.height;

                        // add your entities here
                        doc.Entities.Add(dxfEntity);
                    }
                }
            }

            // save to file
            doc.Save(fileFullPath);
        }



        /// <summary>
        /// 写XML文件
        /// </summary>
        /// <param name="xmlFileFullPath">XML文件全路径</param>
        internal void XmlOut(string xmlFileFullPath)
        {
            Filer.XmlFilerImpl xmlFilerImpl = new Filer.XmlFilerImpl();

            //
            xmlFilerImpl.NewSubNodeAndInsert("Database");
            {
                // block table
                xmlFilerImpl.NewSubNodeAndInsert(_blockTable.className);
                _blockTable.XmlOut(xmlFilerImpl);
                xmlFilerImpl.Pop();

                // layer table
                xmlFilerImpl.NewSubNodeAndInsert(_layerTable.className);
                _layerTable.XmlOut(xmlFilerImpl);
                xmlFilerImpl.Pop();
            }
            xmlFilerImpl.Pop();

            //
            xmlFilerImpl.Save(xmlFileFullPath);
        }

        /// <summary>
        /// 读XML文件
        /// </summary>
        internal bool XmlIn(string xmlFileFullPath)
        {
            Filer.XmlFilerImpl xmlFilerImpl = new Filer.XmlFilerImpl();
            xmlFilerImpl.Load(xmlFileFullPath);

            //
            XmlDocument xmldoc = xmlFilerImpl.xmldoc;
            XmlNode dbNode = xmldoc.SelectSingleNode("Database");
            if (dbNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = dbNode;

            //
            ClearLayerTable();
            ClearBlockTable();

            // layer table
            XmlNode layerTblNode = dbNode.SelectSingleNode(_layerTable.className);
            if (layerTblNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = layerTblNode;
            _layerTable.XmlIn(xmlFilerImpl);

            // block table
            XmlNode blockTblNode = dbNode.SelectSingleNode(_blockTable.className);
            if (blockTblNode == null)
            {
                return false;
            }
            xmlFilerImpl.curXmlNode = blockTblNode;
            _blockTable.XmlIn(xmlFilerImpl);

            //
            _fileName = xmlFileFullPath;
            _idMgr.reset();
            return true;
        }

        /// <summary>
        /// 清空图层表
        /// </summary>
        private void ClearLayerTable()
        {
            List<Layer> allLayers = new List<Layer>();
            foreach (Layer layer in _layerTable)
            {
                allLayers.Add(layer);
            }
            _layerTable.Clear();

            foreach (Layer layer in allLayers)
            {
                layer.Erase();
            }
        }

        /// <summary>
        /// 清空块表
        /// </summary>
        private void ClearBlockTable()
        {
            Dictionary<Entity, Entity> allEnts = new Dictionary<Entity, Entity>();
            List<Block> allBlocks = new List<Block>();

            foreach (Block block in _blockTable)
            {
                foreach (Entity entity in block)
                {
                    allEnts[entity] = entity;
                }
                block.Clear();
                allBlocks.Add(block);
            }
            _blockTable.Clear();

            foreach (KeyValuePair<Entity, Entity> kvp in allEnts)
            {
                kvp.Key.Erase();
            }

            foreach (Block block in allBlocks)
            {
                block.Erase();
            }
        }

        public LitMath.Rectangle2 GetBoundingBox()
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            var allPoints = PointCloudsFromBlockTable();

            foreach (var point in allPoints)
            {
                if (point.x < minX)
                    minX = point.x;

                if (point.x > maxX)
                    maxX = point.x;

                if (point.y < minY)
                    minY = point.y;

                if (point.y > maxY)
                    maxY = point.y;
            }

            if (allPoints.Count > 0)
                return new LitMath.Rectangle2(new LitMath.Vector2(minX, maxY), new LitMath.Vector2(maxX, minY));
            else
                return new LitMath.Rectangle2(new LitMath.Vector2(0, 0), new LitMath.Vector2(0, 0));
        }

        private List<LitMath.Vector2> PointCloudsFromBlockTable()
        {
            List<LitMath.Vector2> allPoints = new List<LitMath.Vector2>();

            foreach (Block block in _blockTable)
            {
                foreach (Entity entity in block)
                {
                    if (entity is Polyline)
                    {
                        var poly = (Polyline)entity;

                        foreach (var vertice in poly.Vertices)
                        {
                            allPoints.Add(new LitMath.Vector2(vertice.x, vertice.y));
                        }
                    }
                    else if (entity is Circle)
                    {
                        var circle = (Circle)entity;

                        // Four quadrants 
                        allPoints.Add(new LitMath.Vector2 { x = circle.center.x + circle.radius, y = circle.center.y });
                        allPoints.Add(new LitMath.Vector2 { x = circle.center.x - circle.radius, y = circle.center.y });
                        allPoints.Add(new LitMath.Vector2 { x = circle.center.x, y = circle.center.y + circle.radius });
                        allPoints.Add(new LitMath.Vector2 { x = circle.center.x, y = circle.center.y - circle.radius });
                    }
                    else if (entity is Arc)
                    {
                        var arc = (Arc)entity;

                        // Four quadrants 
                        allPoints.Add(new LitMath.Vector2 { x = arc.center.x + arc.radius, y = arc.center.y });
                        allPoints.Add(new LitMath.Vector2 { x = arc.center.x - arc.radius, y = arc.center.y });
                        allPoints.Add(new LitMath.Vector2 { x = arc.center.x, y = arc.center.y + arc.radius });
                        allPoints.Add(new LitMath.Vector2 { x = arc.center.x, y = arc.center.y - arc.radius });
                    }
                    else if (entity is Line)
                    {
                        var line = (Line)entity;

                        allPoints.Add(new LitMath.Vector2 { x = line.startPoint.x, y = line.startPoint.y });
                        allPoints.Add(new LitMath.Vector2 { x = line.endPoint.x, y = line.endPoint.y });
                    }
                }
            }

            return allPoints;
        }


        #region IdentityObject
        private void IdentifyDBTable(DBTable table)
        {
            MapSingleObject(table);
        }

        internal void IdentifyObject(DBObject obj)
        {
            IdentifyObjectSingle(obj);
            if (obj is Block)
            {
                Block block = obj as Block;
                foreach (Entity entity in block)
                {
                    IdentifyObjectSingle(entity);
                }
            }
        }

        private void IdentifyObjectSingle(DBObject obj)
        {
            if (obj.id.isNull)
            {
                obj.SetId(_idMgr.NextId);
            }
            MapSingleObject(obj);
        }


        private netDxf.Tables.Linetype LineTypeFromEntityLineType(Entity entity)
        {
            if (entity.lineType == ZacCAD.DatabaseServices.LineType.ByBlock)
                return netDxf.Tables.Linetype.ByBlock;
            else if (entity.lineType == ZacCAD.DatabaseServices.LineType.Solid)
                return netDxf.Tables.Linetype.Continuous;
            else if (entity.lineType == ZacCAD.DatabaseServices.LineType.Dash)
                return netDxf.Tables.Linetype.Dashed;
            else if (entity.lineType == ZacCAD.DatabaseServices.LineType.Dot)
                return netDxf.Tables.Linetype.Dot;
            else if (entity.lineType == ZacCAD.DatabaseServices.LineType.DashDot)
                return netDxf.Tables.Linetype.DashDot;
            else if (entity.lineType == ZacCAD.DatabaseServices.LineType.DashDotDot)
                return netDxf.Tables.Linetype.DashDot;
            else if (entity.lineType == ZacCAD.DatabaseServices.LineType.Custom)
                return netDxf.Tables.Linetype.ByLayer;
            else
                return netDxf.Tables.Linetype.ByLayer;
        }

        #endregion

        #region MapObject
        private void MapObject(DBObject obj)
        {
            MapSingleObject(obj);
            if (obj is Block)
            {
                Block block = obj as Block;
                foreach (Entity entity in block)
                {
                    MapSingleObject(entity);
                }
            }
        }

        internal void UnmapObject(DBObject obj)
        {
            UnmapSingleObject(obj);
            if (obj is Block)
            {
                Block block = obj as Block;
                foreach (Entity entity in block)
                {
                    UnmapSingleObject(entity);
                }
            }
        }

        private void MapSingleObject(DBObject obj)
        {
            _dictId2Object[obj.id] = obj;
        }

        private void UnmapSingleObject(DBObject obj)
        {
            _dictId2Object.Remove(obj.id);
        }
        #endregion
    }
}
