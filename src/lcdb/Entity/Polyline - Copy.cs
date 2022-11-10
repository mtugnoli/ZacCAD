using System;
using System.Collections.Generic;
using LitMath;

namespace ZacCAD.DatabaseServices
{
    /// <summary>
    /// 多段线
    /// </summary>
    public class Polyline : Entity
    {
        /// <summary>
        /// 类名
        /// </summary>
        public override string className
        {
            get { return "Polyline"; }
        }

        //private List<LitMath.Vector2> _vertices = new List<LitMath.Vector2>();
        private List<LitMath.Vertex2> _vertices = new List<LitMath.Vertex2>();
        private bool _closed = false;

        public int NumberOfVertices
        {
            get
            {
                return _vertices.Count;
            }
        }

        public List<LitMath.Vertex2> Vertices
        {
            get
            {
                return _vertices;
            }
        }

        /// <summary>
        /// 是否闭合
        /// </summary>
        public bool closed
        {
            get { return _closed; }
            set { _closed = value; }
        }

        /// <summary>
        /// 绘制函数
        /// </summary>
        public override void Draw(IGraphicsDraw gd)
        {
            int numOfVertices = NumberOfVertices;


            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                if (GetBulgeAt(i) == 0)
                    gd.DrawLine(GetPointAt(i), GetPointAt(i + 1));
                else
                {
                    double length = Vector2.Distance(GetPointAt(i), GetPointAt(i + 1));
                    double alpha = 4 * System.Math.Atan(GetBulgeAt(i));
                    double radius = length / (2d * System.Math.Abs(System.Math.Sin(alpha * 0.5d)));
                    double bulgeSign = System.Math.Sign(GetBulgeAt(i));
                    LitMath.Vector2 delta = GetPointAt(i + 1) - GetPointAt(i);
                    length = delta.length;
                    LitMath.Vector2 lnormalized = delta;
                    lnormalized.Normalize();
                    LitMath.Vector2 lnormal = new LitMath.Vector2(-lnormalized.y, lnormalized.x) * bulgeSign;
                    LitMath.Vector2 arcCenter = ((GetPointAt(i + 1) + GetPointAt(i)) * 0.5d) + lnormal * System.Math.Cos(alpha * 0.5d) * radius;

                    double startAngle = LitMath.Vector2.Angle(arcCenter, GetPointAt(i));
                    startAngle = MathUtils.NormalizeRadianAngle(startAngle);

                    double endAngle = LitMath.Vector2.Angle(arcCenter, GetPointAt(i + 1));
                    endAngle = MathUtils.NormalizeRadianAngle(endAngle);

                    if (bulgeSign == 1)
                        gd.DrawArc(arcCenter, radius, startAngle, endAngle);
                    else
                        gd.DrawArc(arcCenter, radius, endAngle, startAngle);
                }
            }

            if (closed && numOfVertices > 2)
            {
                gd.DrawLine(GetPointAt(numOfVertices - 1), GetPointAt(0));
            }
        }

        public void AddVertexAt(int index, LitMath.Vector2 point, double bulge)
        {
            // _vertices.Insert(index, point);
            _vertices.Insert(index, new LitMath.Vertex2(point, bulge));
        }

        public void RemoveVertexAt(int index)
        {
            _vertices.RemoveAt(index);
        }

        public LitMath.Vector2 GetPointAt(int index)
        {
            return _vertices[index].Position;
        }

        public void SetPointAt(int index, LitMath.Vector2 point)
        {
            _vertices[index].Position = point;
        }

        public double GetBulgeAt(int index)
        {
            return _vertices[index].Bulge;
        }

        /// <summary>
        /// 外围边框
        /// </summary>
        public override Bounding bounding
        {
            get
            {
                if (_vertices.Count > 0)
                {
                    double minX = double.MaxValue;
                    double minY = double.MaxValue;
                    double maxX = double.MinValue;
                    double maxY = double.MinValue;

                    foreach (var item in _vertices)
                    {
                        var point = item.Position;

                        minX = point.x < minX ? point.x : minX;
                        minY = point.y < minY ? point.y : minY;

                        maxX = point.x > maxX ? point.x : maxX;
                        maxY = point.y > maxY ? point.y : maxY;
                    }

                    return new Bounding(new LitMath.Vector2(minX, minY), new LitMath.Vector2(maxX, maxY));
                }
                else
                {
                    return new Bounding();
                }
            }
        }

        /// <summary>
        /// 克隆函数
        /// </summary>
        public override object Clone()
        {
            Polyline polyline = base.Clone() as Polyline;
            polyline._vertices.AddRange(_vertices);
            polyline._closed = _closed;

            return polyline;
        }

        protected override DBObject CreateInstance()
        {
            return new Polyline();
        }

        /// <summary>
        /// 平移
        /// </summary>
        public override void Translate(LitMath.Vector2 translation)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {
                _vertices[i].Position += translation;
            }
        }

        public override void Rotate(LitMath.Vector2 center, double angle)
        {


        }

        /// <summary>
        /// Transform
        /// </summary>
        public override void TransformBy(LitMath.Matrix3 transform)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {
                _vertices[i].Position = transform * _vertices[i].Position;
            }
        }

        /// <summary>
        /// 对象捕捉点
        /// </summary>
        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            int numOfVertices = this.NumberOfVertices;
            for (int i = 0; i < numOfVertices; ++i)
            {
                snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, GetPointAt(i)));

                if (i < numOfVertices - 1)
                {
                    snapPnts.Add(
                        new ObjectSnapPoint(ObjectSnapMode.Mid, (GetPointAt(i) + GetPointAt(i + 1)) / 2));
                }


            }

            if (_closed && numOfVertices > 1)
            {
                snapPnts.Add(
                    new ObjectSnapPoint(ObjectSnapMode.Mid, (GetPointAt(0) + GetPointAt(numOfVertices - 1)) / 2));
            }

            return snapPnts;
        }

        /// <summary>
        /// 获取夹点
        /// </summary>
        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            int numOfVertices = NumberOfVertices;
            for (int i = 0; i < numOfVertices; ++i)
            {
                gripPnts.Add(new GripPoint(GripPointType.End, _vertices[i].Position));
            }
            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (_vertices[i].Position + _vertices[i + 1].Position) / 2);
                midGripPnt.xData1 = _vertices[i];
                midGripPnt.xData2 = _vertices[i + 1];
                gripPnts.Add(midGripPnt);
            }
            if (_closed && numOfVertices > 2)
            {
                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (_vertices[0].Position + _vertices[numOfVertices - 1].Position) / 2);
                midGripPnt.xData1 = _vertices[0];
                midGripPnt.xData2 = _vertices[numOfVertices - 1];
                gripPnts.Add(midGripPnt);
            }

            return gripPnts;
        }

        /// <summary>
        /// 设置夹点
        /// </summary>
        public override void SetGripPointAt(int index, GripPoint gripPoint, LitMath.Vector2 newPosition)
        {
            switch (gripPoint.type)
            {
                case GripPointType.End:
                    {
                        this.SetPointAt(index, newPosition);
                    }
                    break;

                case GripPointType.Mid:
                    {
                        int numOfVertices = NumberOfVertices;
                        int i = index - numOfVertices;
                        if (i >= 0 && i <= numOfVertices - 1)
                        {
                            int vIndex1st = i;
                            int vIndex2nd = i + 1;
                            if (vIndex2nd == numOfVertices)
                            {
                                vIndex2nd = 0;
                            }
                            LitMath.Vector2 t = newPosition - gripPoint.position;
                            this.SetPointAt(vIndex1st, (LitMath.Vector2)gripPoint.xData1 + t);
                            this.SetPointAt(vIndex2nd, (LitMath.Vector2)gripPoint.xData2 + t);
                        }
                    }
                    break;

                default:
                    break;
            }
        }



        // Adds an arc (fillet) at the specified vertex. Returns 1 if the operation succeeded, 0 if it failed.
        public int FilletAt(int index, double radius)
        {
            int prev = index == 0 && this.closed ? this.NumberOfVertices - 1 : index - 1;

            //if (this.GetSegmentType(prev) != _AcDb.SegmentType.Line ||
            //  pline.GetSegmentType(index) != _AcDb.SegmentType.Line)
            //    return 0;

            //LineSegment2d seg1 = this.GetLineSegment2dAt(prev);
            //LineSegment2d seg2 = this.GetLineSegment2dAt(index);

            //Vector2d vec1 = seg1.StartPoint - seg1.EndPoint;
            //Vector2d vec2 = seg2.EndPoint - seg2.StartPoint;

            //double angle = (Math.PI - vec1.GetAngleTo(vec2)) / 2.0;
            //double dist = radius * Math.Tan(angle);
            //if (dist > seg1.Length || dist > seg2.Length)
            //    return 0;

            //Point2d pt1 = seg1.EndPoint + vec1.GetNormal() * dist;
            //Point2d pt2 = seg2.StartPoint + vec2.GetNormal() * dist;

            //double bulge = Math.Tan(angle / 2.0);
            //if (Clockwise(seg1.StartPoint, seg1.EndPoint, seg2.EndPoint))
            //    bulge = -bulge;

            //pline.AddVertexAt(index, pt1, bulge, 0.0, 0.0);
            //pline.SetPointAt(index + 1, pt2);

            return 1;
        }


        /// <summary>
        /// 写XML
        /// </summary>
        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            //
            filer.Write("closed", _closed);
            //
            string strVertices = "";
            int i = 0;
            foreach (var vertex in _vertices)
            {
                if (++i > 1)
                {
                    strVertices += "|";
                }
                strVertices += vertex.Position.x.ToString() + ";" + vertex.Position.y.ToString() + ";" + vertex.Bulge.ToString();
            }
            filer.Write("vertices", strVertices);
        }

        /// <summary>
        /// 读XML
        /// </summary>
        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            //
            filer.Read("closed", out _closed);
            //
            string strVertices;
            filer.Read("vertices", out strVertices);
            string[] vts = strVertices.Split('|');
            double x = 0;
            double y = 0;
            double b = 0;
            string[] xy = null;
            foreach (string vtx in vts)
            {
                xy = vtx.Split(';');
                if (xy.Length != 3)
                {
                    continue;
                }
                if (!double.TryParse(xy[0], out x))
                {
                    continue;
                }
                if (!double.TryParse(xy[1], out y))
                {
                    continue;
                }
                if (!double.TryParse(xy[2], out b))
                {
                    continue;
                }

                _vertices.Add(new LitMath.Vertex2(new LitMath.Vector2(x, y), b));
            }
        }
    }
}
