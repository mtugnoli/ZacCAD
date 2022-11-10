﻿using System;
using System.Collections.Generic;

namespace ZacCAD.DatabaseServices
{
    public class Circle : Entity
    {
        /// <summary>
        /// 类名
        /// </summary>
        public override string className
        {
            get { return "Circle"; }
        }

        /// <summary>
        /// 圆心
        /// </summary>
        private LitMath.Vector2 _center = new LitMath.Vector2(0, 0);
        public LitMath.Vector2 center
        {
            get { return _center; }
            set { _center = value; }
        }

        /// <summary>
        /// 半径
        /// </summary>
        private double _radius = 0.0;
        public double radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// 直径
        /// </summary>
        public double diameter
        {
            get { return _radius * 2; }
        }

        /// <summary>
        /// 外围边框
        /// </summary>
        public override Bounding bounding
        {
            get
            {
                return new Bounding(_center, this.diameter, this.diameter);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Circle()
        {
        }

        public Circle(LitMath.Vector2 center, double radius)
        {
            _center = center;
            _radius = radius;
        }

        /// <summary>
        /// 绘制函数
        /// </summary>
        public override void Draw(IGraphicsDraw gd)
        {
            gd.DrawCircle(_center, _radius);
        }

        /// <summary>
        /// 克隆函数
        /// </summary>
        public override object Clone()
        {
            Circle circle = base.Clone() as Circle;
            circle._center = _center;
            circle._radius = _radius;
            return circle;
        }

        /// <summary>
        /// 创建圆实例
        /// </summary>
        protected override DBObject CreateInstance()
        {
            return new Circle();
        }

        /// <summary>
        /// 平移
        /// </summary>
        public override void Translate(LitMath.Vector2 translation)
        {
            _center += translation;
        }

        public override void Rotate(LitMath.Vector2 center, double angle)
        {
            _center = LitMath.Vector2.RotateInRadian(_center, center, angle);
        }

        /// <summary>
        /// Transform
        /// </summary>
        public override void TransformBy(LitMath.Matrix3 transform)
        {
            LitMath.Vector2 pnt = _center + new LitMath.Vector2(_radius, 0);

            _center = transform * _center;
            _radius = (transform * pnt - _center).length;
        }

        /// <summary>
        /// 对象捕捉点
        /// </summary>
        public override List<ObjectSnapPoint> GetSnapPoints()
        {
            List<ObjectSnapPoint> snapPnts = new List<ObjectSnapPoint>();
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Center, _center));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Quad, _center + new LitMath.Vector2(_radius, 0)));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Quad, _center + new LitMath.Vector2(0, _radius)));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Quad, _center + new LitMath.Vector2(-_radius, 0)));
            snapPnts.Add(new ObjectSnapPoint(ObjectSnapMode.Quad, _center + new LitMath.Vector2(0, -_radius)));
            return snapPnts;
        }

        /// <summary>
        /// 获取夹点
        /// </summary>
        public override List<GripPoint> GetGripPoints()
        {
            List<GripPoint> gripPnts = new List<GripPoint>();
            gripPnts.Add(new GripPoint(GripPointType.Center, _center));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new LitMath.Vector2(_radius, 0)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new LitMath.Vector2(0, _radius)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new LitMath.Vector2(-_radius, 0)));
            gripPnts.Add(new GripPoint(GripPointType.Quad, _center + new LitMath.Vector2(0, -_radius)));
            
            return gripPnts;
        }

        /// <summary>
        /// 设置夹点
        /// </summary>
        public override void SetGripPointAt(int index, GripPoint gripPoint, LitMath.Vector2 newPosition)
        {
            if (index == 0)
            {
                _center = newPosition;
            }
            else if (index >= 1 && index <= 4)
            {
                _radius = (newPosition - _center).length;
            }
        }

        /// <summary>
        /// 写XML
        /// </summary>
        public override void XmlOut(Filer.XmlFiler filer)
        {
            base.XmlOut(filer);

            filer.Write("center", _center);
            filer.Write("radius", _radius);
        }

        /// <summary>
        /// 读XML
        /// </summary>
        public override void XmlIn(Filer.XmlFiler filer)
        {
            base.XmlIn(filer);

            filer.Read("center", out _center);
            filer.Read("radius", out _radius);
        }
    }
}
