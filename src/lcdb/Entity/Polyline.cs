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

        private List<LitMath.Vector2b> _vertices = new List<LitMath.Vector2b>();
        private bool _closed = false;

        public int NumberOfVertices
        {
            get
            {
                return _vertices.Count;
            }
        }

        public List<LitMath.Vector2b> Vertices
        {
            get { return _vertices; }
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
        //public override void Draw(IGraphicsDraw gd)
        //{
        //    int numOfVertices = NumberOfVertices;
        //    for (int i = 0; i < numOfVertices - 1; ++i)
        //    {
        //        gd.DrawLine(GetPointAt(i), GetPointAt(i + 1));
        //    }

        //    if (closed
        //        && numOfVertices > 2)
        //    {
        //        gd.DrawLine(GetPointAt(numOfVertices - 1), GetPointAt(0));
        //    }
        //}
        public override void Draw(IGraphicsDraw gd)
        {
            int numOfVertices = NumberOfVertices;


            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                if (GetBulgeAt(i) == 0)
                {
                    LitMath.Vector2b p1 = GetPointAt(i);
                    LitMath.Vector2b p2 = GetPointAt(i + 1);

                    gd.DrawLine(new Vector2(p1.x, p1.y), new Vector2(p2.x, p2.y));
                }
                else
                {
                    LitMath.Vector2b p1 = GetPointAt(i);
                    LitMath.Vector2b p2 = GetPointAt(i + 1);

                    double length = Vector2b.Distance(GetPointAt(i), GetPointAt(i + 1));
                    double alpha = 4 * System.Math.Atan(GetBulgeAt(i));
                    double radius = length / (2d * System.Math.Abs(System.Math.Sin(alpha * 0.5d)));
                    double bulgeSign = System.Math.Sign(GetBulgeAt(i));
                    LitMath.Vector2b delta = new Vector2b(p2.x, p2.y, p2.b) - new Vector2b(p1.x, p1.y, p1.b);
                    length = delta.length;
                    LitMath.Vector2b lnormalized = delta;
                    lnormalized.Normalize();
                    LitMath.Vector2b lnormal = new LitMath.Vector2b(-lnormalized.y, lnormalized.x, lnormalized.b) * bulgeSign;
                    LitMath.Vector2b arcCenter = ((GetPointAt(i + 1) + GetPointAt(i)) * 0.5d) + lnormal * System.Math.Cos(alpha * 0.5d) * radius;

                    double startAngle = LitMath.Vector2b.Angle(arcCenter, GetPointAt(i));
                    startAngle = MathUtils.NormalizeRadianAngle(startAngle);

                    double endAngle = LitMath.Vector2b.Angle(arcCenter, GetPointAt(i + 1));
                    endAngle = MathUtils.NormalizeRadianAngle(endAngle);

                    if (bulgeSign == 1)
                        gd.DrawArc(new Vector2(arcCenter.x, arcCenter.y), radius, startAngle, endAngle);
                    else
                        gd.DrawArc(new Vector2(arcCenter.x, arcCenter.y), radius, endAngle, startAngle);
                }
            }

            if (closed && numOfVertices > 2)
            {
                LitMath.Vector2b p1 = GetPointAt(numOfVertices - 1);
                LitMath.Vector2b p2 = GetPointAt(0);

                gd.DrawLine(new Vector2(p1.x, p1.y), new Vector2(p2.x, p2.y));
            }
        }


        public void AddVertexAt(int index, LitMath.Vector2b point)
        {
            _vertices.Insert(index, point);
        }

        public void AddVertexAt(int index, LitMath.Vector2 point)
        {
            _vertices.Insert(index, new Vector2b(point.x, point.y, 0));
        }

        public void RemoveVertexAt(int index)
        {
            _vertices.RemoveAt(index);
        }

        public LitMath.Vector2b GetPointAt(int index)
        {
            return _vertices[index];
        }

        public void SetPointAt(int index, LitMath.Vector2b point)
        {
            _vertices[index] = point;
        }

        public void SetPointAt(int index, LitMath.Vector2 point)
        {
            _vertices[index] = new Vector2b(point.x, point.y, 0);
        }

        public void SetBulgeAt(int index, double bulge)
        {
            _vertices[index] = new Vector2b(_vertices[index].x, _vertices[index].y, bulge);
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

                    foreach (LitMath.Vector2b point in _vertices)
                    {
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
                LitMath.Vector2b p1 = new LitMath.Vector2b(_vertices[i].x + translation.x, _vertices[i].y + translation.y, _vertices[i].b);
                _vertices[i] = p1;
            }
        }

        public override void Rotate(LitMath.Vector2 center, double angle)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {
                LitMath.Vector2b p1 = LitMath.Vector2b.RotateInRadian(_vertices[i], center, angle);
                p1.b = _vertices[i].b;

                _vertices[i] = p1;
            }



        }

        /// <summary>
        /// Transform
        /// </summary>
        public override void TransformBy(LitMath.Matrix3 transform)
        {
            for (int i = 0; i < this.NumberOfVertices; ++i)
            {
                LitMath.Vector2 p1 = new LitMath.Vector2(_vertices[i].x, _vertices[i].y);
                LitMath.Vector2 p2 = transform * p1;
                LitMath.Vector2b p3 = new LitMath.Vector2b(p2.x, p2.y, _vertices[i].b);
                _vertices[i] = p3;

                //_vertices[i] = transform * _vertices[i];
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
                LitMath.Vector2b p1 = GetPointAt(i);

                snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, new Vector2(p1.x, p1.y)));

                if (i != numOfVertices - 1)
                {
                    LitMath.Vector2b p2 = GetPointAt(i + 1);

                    snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Mid, (new Vector2(p1.x, p1.y) + new Vector2(p2.x, p2.y)) / 2));
                }
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
                LitMath.Vector2b p1 = _vertices[i];

                gripPnts.Add(new GripPoint(GripPointType.End, new Vector2(p1.x, p1.y)));
            }

            for (int i = 0; i < numOfVertices - 1; ++i)
            {
                LitMath.Vector2b p1 = _vertices[i];
                LitMath.Vector2b p2 = _vertices[i + 1];

                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (new Vector2(p1.x, p1.y) + new Vector2(p2.x, p2.y)) / 2);
                midGripPnt.xData1 = _vertices[i];
                midGripPnt.xData2 = _vertices[i + 1];
                gripPnts.Add(midGripPnt);
            }

            if (_closed && numOfVertices > 2)
            {
                LitMath.Vector2b p1 = _vertices[0];
                LitMath.Vector2b p2 = _vertices[numOfVertices - 1];

                GripPoint midGripPnt = new GripPoint(GripPointType.Mid, (new Vector2(p1.x, p1.y) + new Vector2(p2.x, p2.y)) / 2);
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
                        this.SetPointAt(index, new Vector2b(newPosition.x, newPosition.y, 0));
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
                            LitMath.Vector2b t = new Vector2b(newPosition.x, newPosition.y, 0) - new Vector2b(gripPoint.position.x, gripPoint.position.y, 0);
                            this.SetPointAt(vIndex1st, (LitMath.Vector2b)gripPoint.xData1 + t);
                            this.SetPointAt(vIndex2nd, (LitMath.Vector2b)gripPoint.xData2 + t);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public double GetBulgeAt(int index)
        {
            return _vertices[index].b;
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
            foreach (LitMath.Vector2b vertex in _vertices)
            {
                if (++i > 1)
                {
                    strVertices += "|";
                }
                strVertices += vertex.x.ToString() + ";" + vertex.y.ToString() + ";" + vertex.b.ToString();
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
                if (xy.Length < 3)
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

                _vertices.Add(new LitMath.Vector2b(x, y, b));
            }
        }
    }
}
