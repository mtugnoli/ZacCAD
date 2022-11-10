﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZacCAD.DatabaseServices
{
    /// <summary>
    /// 线段
    /// </summary>
    public class Line : Entity
    {
        /// <summary>
        /// 类名
        /// </summary>
        public override string className
        {
            get { return "Line"; }
        }

        /// <summary>
        /// 起点
        /// </summary>
        private LitMath.Vector2 _startPoint = new LitMath.Vector2();

        [Description("Defines the Start Point")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        [TypeConverter(typeof(PointFConverter))]
        public LitMath.Vector2 startPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        /// <summary>
        /// 终点
        /// </summary>
        private LitMath.Vector2 _endPoint = new LitMath.Vector2();
        public LitMath.Vector2 endPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        /// <summary>
        /// 外围边框
        /// </summary>
        [Browsable(false)]
        public override Bounding bounding
        {
            get
            {
                return new Bounding(_startPoint, _endPoint);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Line()
        {
        }

        public Line(LitMath.Vector2 startPnt, LitMath.Vector2 endPnt)
        {
            _startPoint = startPnt;
            _endPoint = endPnt;
        }

        /// <summary>
        /// 绘制函数
        /// </summary>
        public override void Draw(IGraphicsDraw gd)
        {
            gd.DrawLine(_startPoint, _endPoint);
        }

        /// <summary>
        /// 克隆函数
        /// </summary>
        public override object Clone()
        {
            Line line = base.Clone() as Line;
            line._startPoint = _startPoint;
            line._endPoint = _endPoint;
            return line;
        }

        protected override DBObject CreateInstance()
        {
            return new Line();
        }

        /// <summary>
        /// 平移
        /// </summary>
        public override void Translate(LitMath.Vector2 translation)
        {
            _startPoint += translation;
            _endPoint += translation;
        }

        public override void Rotate(LitMath.Vector2 center, double angle)
        {
            _startPoint =  LitMath.Vector2.RotateInRadian(_startPoint, center, angle);
            _endPoint = LitMath.Vector2.RotateInRadian(_endPoint, center, angle);
        }

        /// <summary>
        /// Transform
        /// </summary>
        public override void TransformBy(LitMath.Matrix3 transform)
        {
            _startPoint = transform * _startPoint;
            _endPoint = transform * _endPoint;
        }

        /// <summary>
        /// 对象捕捉点
        /// </summary>
        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, _startPoint));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.End, _endPoint));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Mid, (_startPoint + _endPoint) / 2));

            return snapPnts;
        }

        /// <summary>
        /// 获取夹点
        /// </summary>
        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            gripPnts.Add(new GripPoint(GripPointType.End, _startPoint));
            gripPnts.Add(new GripPoint(GripPointType.End, _endPoint));

            return gripPnts;
        }

        /// <summary>
        /// 设置夹点
        /// </summary>
        public override void SetGripPointAt(int index, GripPoint gripPoint, LitMath.Vector2 newPosition)
        {
            if (index == 0)
            {
                _startPoint = newPosition;
            }
            else if (index == 1)
            {
                _endPoint = newPosition;
            }
        }

        /// <summary>
        /// 写XML
        /// </summary>
        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("startPoint", _startPoint);
            filer.Write("endPoint", _endPoint);
        }

        /// <summary>
        /// 读XML
        /// </summary>
        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("startPoint", out _startPoint);
            filer.Read("endPoint", out _endPoint);
        }
    }
}
