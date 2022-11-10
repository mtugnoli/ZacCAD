﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using ZacCAD.DatabaseServices;
using ZacCAD.ApplicationServices;
using ZacCAD.Commands;
using ZacCAD.UI;
using ZacCAD.Windows;
using ZacCAD.Windows.Controls;

namespace ZacCAD
{
    /// <summary>
    /// 图形绘制
    /// </summary>
    internal class WorldDraw : IGraphicsDraw
    {
        /// <summary>
        /// 绘图图面
        /// </summary>
        private Graphics _g = null;
        public Graphics graphics
        {
            get { return _g; }
            set { _g = value; }
        }

        /// <summary>
        /// 绘图笔
        /// </summary>
        private Pen _pen = null;
        public Pen pen
        {
            get { return _pen; }
            set
            {
                _pen = value;
            }
        }

        /// <summary>
        /// 绘图画刷
        /// </summary>
        private Brush _brush = null;
        public Brush brush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        /// <summary>
        /// Presenter
        /// </summary>
        private Presenter _presenter = null;
        public Presenter presenter
        {
            get { return _presenter; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorldDraw(Presenter presenter)
        {
            _presenter = presenter;
        }

        public void DrawPoint(LitMath.Vector2 endPoint)
        {
            LitMath.Vector2 endInCanvas = _presenter.ModelToCanvas(endPoint);
            graphics.FillEllipse(_pen.Brush, (float)endInCanvas.x, (float)endInCanvas.y, 5, 5);
        }

        public void DrawLine(LitMath.Vector2 startPoint, LitMath.Vector2 endPoint)
        {
            LitMath.Vector2 startInCanvas = _presenter.ModelToCanvas(startPoint);
            LitMath.Vector2 endInCanvas = _presenter.ModelToCanvas(endPoint);
            graphics.DrawLine(_pen, (float)startInCanvas.x, (float)startInCanvas.y, (float)endInCanvas.x, (float)endInCanvas.y);
        }

        public void DrawXLine(LitMath.Vector2 basePoint, LitMath.Vector2 direction)
        {
            LitMath.Vector2 basePnt = _presenter.ModelToCanvas(basePoint);
            LitMath.Vector2 otherPnt = _presenter.ModelToCanvas(basePoint + direction);
            LitMath.Vector2 dir = (otherPnt - basePnt).normalized;

            double xk = double.MinValue;
            double yk = double.MinValue;
            if (dir.y != 0)
            {
                double k = basePnt.y / dir.y;
                xk = basePnt.x - k * dir.x;
            }
            if (dir.x != 0)
            {
                double k = basePnt.x / dir.x;
                yk = basePnt.y - k * dir.y;
            }

            if (xk > 0 || (xk == 0 && dir.x * dir.y >= 0))
            {
                LitMath.Vector2 spnt = new LitMath.Vector2(xk, 0);
                if (dir.y < 0)
                {
                    dir = -dir;
                }
                LitMath.Vector2 epnt = spnt + 10000 * dir;

                graphics.DrawLine(_pen, (float)spnt.x, (float)spnt.y, (float)epnt.x, (float)epnt.y);
            }
            else if (yk > 0 || (yk == 0 && dir.x * dir.y >= 0))
            {
                LitMath.Vector2 spnt = new LitMath.Vector2(0, yk);
                if (dir.x < 0)
                {
                    dir = -dir;
                }
                LitMath.Vector2 epnt = spnt + 10000 * dir;

                graphics.DrawLine(_pen, (float)spnt.x, (float)spnt.y, (float)epnt.x, (float)epnt.y);
            }
        }

        public void DrawRay(LitMath.Vector2 basePoint, LitMath.Vector2 direction)
        {
            LitMath.Vector2 basePnt = _presenter.ModelToCanvas(basePoint);
            LitMath.Vector2 otherPnt = _presenter.ModelToCanvas(basePoint + direction);
            LitMath.Vector2 dir = (otherPnt - basePnt).normalized;

            double xk = double.MinValue;
            double yk = double.MinValue;
            if (basePnt.x > 0 && basePnt.x < 10000 && basePnt.y > 0 && basePnt.y < 10000)
            {
                xk = 1;
                yk = 1;
            }
            else
            {
                if (dir.y != 0)
                {
                    double k = -basePnt.y / dir.y;
                    if (k >= 0)
                    {
                        xk = basePnt.x + k * dir.x;
                    }
                }
                if (dir.x != 0)
                {
                    double k = -basePnt.x / dir.x;
                    if (k >= 0)
                    {
                        yk = basePnt.y + k * dir.y;
                    }
                }

            }

            if (xk > 0 || (xk == 0 && dir.x * dir.y >= 0) || yk > 0 || (yk == 0 && dir.x * dir.y >= 0))
            {
                LitMath.Vector2 epnt = basePnt + 10000 * dir;

                graphics.DrawLine(_pen, (float)basePnt.x, (float)basePnt.y, (float)epnt.x, (float)epnt.y);
            }
        }

        public void DrawCircle(LitMath.Vector2 center, double radius)
        {
            LitMath.Vector2 centerInCanvas = _presenter.ModelToCanvas(center);
            double radiusInCanvas = _presenter.ModelToCanvas(radius);
            graphics.DrawEllipse(_pen, (float)(centerInCanvas.x - radiusInCanvas), (float)(centerInCanvas.y - radiusInCanvas), (float)radiusInCanvas * 2, (float)radiusInCanvas * 2);
        }

        public void DrawEllipse(LitMath.Vector2 center, double radiusX, double radiusY)
        {
            LitMath.Vector2 centerInCanvas = _presenter.ModelToCanvas(center);
            double radiusXInCanvas = _presenter.ModelToCanvas(radiusX);
            double radiusYInCanvas = _presenter.ModelToCanvas(radiusY);
            graphics.DrawEllipse(_pen, (float)(centerInCanvas.x - radiusXInCanvas), (float)(centerInCanvas.y - radiusYInCanvas), (float)radiusXInCanvas * 2, (float)radiusYInCanvas * 2);
        }

        /// <summary>
        /// 绘制圆弧,逆时针从startAngle到endAngle
        /// </summary>
        public void DrawArc(LitMath.Vector2 center, double radius, double startAngle, double endAngle)
        {
            //
            LitMath.Vector2 centerInCanvas = _presenter.ModelToCanvas(center);
            double radiusInCanvas = _presenter.ModelToCanvas(radius);

            // GDI为顺时针绘制圆弧,而当前函数为逆时针绘制圆弧
            double startAngleInCanvas = MathUtils.NormalizeRadianAngle(-startAngle);
            double endAngleInCanvas = MathUtils.NormalizeRadianAngle(-endAngle);

            //
            double angle = endAngle - startAngle;
            if (endAngle < startAngle)
            {
                angle += LitMath.Utils.PI * 2;
            }

            //
            if (radiusInCanvas > 0)
            {
                graphics.DrawArc(pen, (float)(centerInCanvas.x - radiusInCanvas), (float)(centerInCanvas.y - radiusInCanvas), (float)radiusInCanvas * 2, (float)radiusInCanvas * 2, (float)(startAngleInCanvas * 180.0 / LitMath.Utils.PI), -(float)(angle * 180.0 / LitMath.Utils.PI));
            }
        }

        public void DrawRectangle(LitMath.Vector2 position, double width, double height)
        {
            double widthInCanvas = _presenter.ModelToCanvas(width);
            double heightInCanvas = _presenter.ModelToCanvas(height);
            LitMath.Vector2 posInCanvas = _presenter.ModelToCanvas(position);
            posInCanvas.y -= heightInCanvas;

            graphics.DrawRectangle(pen, (float)posInCanvas.x, (float)posInCanvas.y, (float)widthInCanvas, (float)heightInCanvas);
        }

        public LitMath.Vector2 DrawText(LitMath.Vector2 position, string text, double height, string fontName, TextAlignment textAlign)
        {
            int fontHeight = (int)_presenter.ModelToCanvas(height);
            if (fontHeight <= 0)
            {
                return new LitMath.Vector2(0, 0);
            }
            position = _presenter.ModelToCanvas(position);
            string fontFamily = fontName == "" ? "Arial" : fontName;

            FontStyle fontStyle = FontStyle.Regular;
            Font font = new Font(fontFamily, (int)fontHeight, fontStyle);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Far;
            switch (textAlign)
            {
                case TextAlignment.LeftBottom:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.LeftMiddle:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.LeftTop:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;

                case TextAlignment.CenterBottom:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.CenterMiddle:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.CenterTop:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;

                case TextAlignment.RightBottom:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.RightMiddle:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.RightTop:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
            }
            PointF pos = new PointF((float)position.x, (float)position.y);
            graphics.DrawString(text, font, _brush, pos, format);
            SizeF size = graphics.MeasureString(text, font, pos, format);
            double w = _presenter.CanvasToModel(size.Width);
            double h = _presenter.CanvasToModel(size.Height);
            return new LitMath.Vector2(w, h);
        }
    }

    internal class CanvasDraw : IGraphicsDraw
    {
        /// <summary>
        /// 绘图图面
        /// </summary>
        private Graphics _g = null;
        public Graphics graphics
        {
            get { return _g; }
            set { _g = value; }
        }

        /// <summary>
        /// 绘图笔
        /// </summary>
        private Pen _pen = null;
        public Pen pen
        {
            get { return _pen; }
            set { _pen = value; }
        }

        /// <summary>
        /// 绘图画刷
        /// </summary>
        private Brush _brush = null;
        public Brush brush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        /// <summary>
        /// Presenter
        /// </summary>
        private Presenter _presenter = null;
        public Presenter presenter
        {
            get { return _presenter; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CanvasDraw(Presenter presenter)
        {
            _presenter = presenter;
        }

        public void DrawPoint(LitMath.Vector2 endPoint)
        {
            graphics.FillEllipse(_pen.Brush, (float)endPoint.x, (float)endPoint.y, 5, 5);
        }

        public void DrawLine(LitMath.Vector2 startPoint, LitMath.Vector2 endPoint)
        {
            graphics.DrawLine(_pen, (float)startPoint.x, (float)startPoint.y, (float)endPoint.x, (float)endPoint.y);
        }

        public void DrawXLine(LitMath.Vector2 basePoint, LitMath.Vector2 direction)
        {
            direction.Normalize();

            double xk = double.MinValue;
            double yk = double.MinValue;
            if (direction.y != 0)
            {
                double k = basePoint.y / direction.y;
                xk = basePoint.x - k * direction.x;
            }
            if (direction.x != 0)
            {
                double k = basePoint.x / direction.x;
                yk = basePoint.y - k * direction.y;
            }

            if (xk > 0
                || (xk == 0 && direction.x * direction.y >= 0))
            {
                LitMath.Vector2 spnt = new LitMath.Vector2(xk, 0);
                if (direction.y < 0)
                {
                    direction = -direction;
                }
                LitMath.Vector2 epnt = spnt + 10000 * direction;

                graphics.DrawLine(_pen, (float)spnt.x, (float)spnt.y, (float)epnt.x, (float)epnt.y);
            }
            else if (yk > 0
                || (yk == 0 && direction.x * direction.y >= 0))
            {
                LitMath.Vector2 spnt = new LitMath.Vector2(0, yk);
                if (direction.x < 0)
                {
                    direction = -direction;
                }
                LitMath.Vector2 epnt = spnt + 10000 * direction;

                graphics.DrawLine(_pen, (float)spnt.x, (float)spnt.y, (float)epnt.x, (float)epnt.y);
            }
        }

        public void DrawRay(LitMath.Vector2 basePnt, LitMath.Vector2 dir)
        {
            dir.Normalize();

            double xk = double.MinValue;
            double yk = double.MinValue;
            if (basePnt.x > 0 && basePnt.x < 10000
                && basePnt.y > 0 && basePnt.y < 10000)
            {
                xk = 1;
                yk = 1;
            }
            else
            {
                if (dir.y != 0)
                {
                    double k = -basePnt.y / dir.y;
                    if (k >= 0)
                    {
                        xk = basePnt.x + k * dir.x;
                    }
                }
                if (dir.x != 0)
                {
                    double k = -basePnt.x / dir.x;
                    if (k >= 0)
                    {
                        yk = basePnt.y + k * dir.y;
                    }
                }

            }

            if (xk > 0
                || (xk == 0 && dir.x * dir.y >= 0)
                || yk > 0
                || (yk == 0 && dir.x * dir.y >= 0))
            {

                LitMath.Vector2 epnt = basePnt + 10000 * dir;

                graphics.DrawLine(_pen, (float)basePnt.x, (float)basePnt.y, (float)epnt.x, (float)epnt.y);
            }
        }

        public void DrawCircle(LitMath.Vector2 center, double radius)
        {
            graphics.DrawEllipse(_pen, (float)(center.x - radius), (float)(center.y - radius), (float)radius * 2, (float)radius * 2);
        }

        public void DrawEllipse(LitMath.Vector2 center, double radiusX, double radiusY)
        {
            graphics.DrawEllipse(_pen, (float)(center.x - radiusX), (float)(center.y - radiusY), (float)radiusX * 2, (float)radiusY * 2);
        }

        /// <summary>
        /// 绘制圆弧,逆时针从startAngle到endAngle
        /// </summary>
        public void DrawArc(LitMath.Vector2 center, double radius, double startAngle, double endAngle)
        {
            // GDI为顺时针绘制圆弧,而当前函数为逆时针绘制圆弧
            double startAngleInCanvas = MathUtils.NormalizeRadianAngle(-startAngle);
            double endAngleInCanvas = MathUtils.NormalizeRadianAngle(-endAngle);

            //
            double angle = endAngle - startAngle;
            if (endAngle < startAngle)
            {
                angle += LitMath.Utils.PI * 2;
            }

            graphics.DrawArc(pen, (float)(center.x - radius), (float)(center.y - radius), (float)radius * 2, (float)radius * 2, (float)(startAngleInCanvas * 180.0 / LitMath.Utils.PI), -(float)(angle * 180.0 / LitMath.Utils.PI));
        }

        public void DrawRectangle(LitMath.Vector2 position, double width, double height)
        {
            graphics.DrawRectangle(pen, (float)position.x, (float)position.y, (float)width, (float)height);
        }

        public LitMath.Vector2 DrawText(LitMath.Vector2 position, string text, double height, string fontName, TextAlignment textAlign)
        {
            int fontHeight = (int)height;
            if (fontHeight <= 0)
            {
                return new LitMath.Vector2(0, 0);
            }
            string fontFamily = fontName == "" ? "Arial" : fontName;

            FontStyle fontStyle = FontStyle.Regular;
            Font font = new Font(fontFamily, (int)fontHeight, fontStyle);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Far;
            switch (textAlign)
            {
                case TextAlignment.LeftBottom:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.LeftMiddle:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.LeftTop:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;

                case TextAlignment.CenterBottom:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.CenterMiddle:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.CenterTop:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;

                case TextAlignment.RightBottom:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.RightMiddle:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.RightTop:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
            }
            PointF pos = new PointF((float)position.x, (float)position.y);
            graphics.DrawString(text, font, _brush, pos, format);

            SizeF size = graphics.MeasureString(text, font, pos, format);
            return new LitMath.Vector2(size.Width, size.Height);
        }
    }

    internal class Presenter : IPresenter
    {
        private ICanvas _canvas = null;
        private Document _document = null;

        //
        private CommandsMgr _cmdsMgr = null;

        private Pointer _pointer = null;
        internal Pointer pointer
        {
            get { return _pointer; }
        }

        private DynamicInputer _dynamicInputer = null;
        //internal DynamicInputer dynamicInputer
        //{
        //    get { return _dynamicInputer; }
        //}

        // 原点
        private Origin _origin;

        //
        public IDocument document
        {
            get { return _document; }
        }

        public Selections selections
        {
            get { return _document.selections; }
        }

        public ICanvas canvas
        {
            get { return _canvas; }
        }

        internal bool canUndo
        {
            get { return _cmdsMgr.canUndo; }
        }

        internal bool canRedo
        {
            get { return _cmdsMgr.canRedo; }
        }

        //
        private Bitmap _bufferBitmap = null;
        private bool _bufferBitmapToRedraw = true;

        //
        private LitMath.Vector2 _screenPan = new LitMath.Vector2();
        private LitMath.Vector2 _screenDrag = new LitMath.Vector2();

        public LitMath.Vector2 screenPan
        {
            get { return _screenPan + _screenDrag; }
            set { _screenPan = value; }
        }

        private float _resolution = 96.0f;
        private double _zoom = 1.0f;
        private double _zoomMin = 1e-4;
        private double _zoomMax = 1e4;
        private bool _isOrtho = false;

        //
        public WorldDraw worldDraw
        {
            get { return _worldDraw; }
        }
        private WorldDraw _worldDraw = null;

        public CanvasDraw canvasDraw
        {
            get { return _canvasDraw; }
        }
        private CanvasDraw _canvasDraw = null;

        public double Zoom
        {
            get { return _zoom; }
            set { _zoom = value >= _zoomMin && value <= _zoomMax ? value : _zoom; }
        }

        public float Resolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }

        public bool IsOrtho
        {
            get { return _isOrtho; }
            set { _isOrtho = value; }
        }

        /// <summary>
        /// Current block
        /// </summary>
        public Block currentBlock
        {
            get
            {
                return _document.database.blockTable[_document.currentBlockName] as Block;
            }
        }

        private StatusStripMgr _statusStripMgr;
        internal StatusStripMgr statusStripMgr
        {
            get { return _statusStripMgr; }
        }

        public delegate void CurrentMouseMove(MouseEventArgs e);
        public event CurrentMouseMove docMouseMove;

        public Presenter(ICanvas canvas, Document doc, StatusStripMgr statusStripMgr)
        {
            _canvas = canvas;
            _document = (Document)doc;
            _canvas.SetPresenter(this);

            _statusStripMgr = statusStripMgr;

            doc.selections.changed += this.OnSelectionChanged;

            _cmdsMgr = new CommandsMgr(this);
            _cmdsMgr.commandFinished += this.OnCommandFinished;
            _cmdsMgr.commandCanceled += this.OnCommandCanceled;

            _pointer = new Pointer(this);

            StatusStrip statusStrip = statusStripMgr.GetStatusStrip();

            _dynamicInputer = new DynamicInputer(this);
            _dynamicInputer.cmdInput.finish += this.OnCmdInputResult;
            _dynamicInputer.cmdInput.cancel += this.OnCmdInputResult;

            _origin = new Origin(this);

            _worldDraw = new WorldDraw(this);
            _canvasDraw = new CanvasDraw(this);

            //TestData();
        }

        private void TestData()
        {
            Block modelSpace = _document.database.blockTable["ModelSpace"] as Block;

            Line line = new Line();
            line.startPoint = new LitMath.Vector2(0, 0);
            line.endPoint = new LitMath.Vector2(100, 100);
            line.color = ZacCAD.Colors.Color.FromColor(Color.Green);
            modelSpace.AppendEntity(line);

            Circle circle = new Circle();
            circle.center = new LitMath.Vector2(0, 0);
            circle.radius = 20;
            circle.color = ZacCAD.Colors.Color.FromColor(Color.Blue);
            modelSpace.AppendEntity(circle);

            Polyline polyline = new Polyline();
            polyline.color = ZacCAD.Colors.Color.FromColor(Color.Yellow);
            polyline.AddVertexAt(polyline.NumberOfVertices, new LitMath.Vector2b(0, 0, 0));
            polyline.AddVertexAt(polyline.NumberOfVertices, new LitMath.Vector2b(10, 20, 0));
            polyline.AddVertexAt(polyline.NumberOfVertices, new LitMath.Vector2b(20, 5, 0));
            polyline.AddVertexAt(polyline.NumberOfVertices, new LitMath.Vector2b(30, 25, 0));
            modelSpace.AppendEntity(polyline);

            Arc arc = new Arc();
            arc.color = ZacCAD.Colors.Color.FromColor(Color.Blue);
            arc.center = new LitMath.Vector2(20, 20);
            arc.radius = 6;
            arc.startAngle = Math.PI / 4;
            arc.endAngle = Math.PI * 1.4f;
            modelSpace.AppendEntity(arc);

            Xline xline = new Xline();
            xline.basePoint = new LitMath.Vector2(0, 50);
            xline.direction = new LitMath.Vector2(-1, -1);
            modelSpace.AppendEntity(xline);

            Ray ray = new Ray();
            ray.basePoint = new LitMath.Vector2(-10, 20);
            ray.direction = new LitMath.Vector2(2, -5);
            modelSpace.AppendEntity(ray);

            //Text text = new Text();
            //text.color = Colors.Color.FromRGB(255, 0, 0);
            //text.text = "gabc 北京\n efg";
            //text.height = 5;
            //text.font = "Arial";
            //text.position = new LitMath.Vector2(0, 0);
            //text.alignment = ZacCAD.DatabaseServices.TextAlignment.RightBottom;
            //modelSpace.AppendEntity(text);

            //Text text2 = new Text();
            //text2.text = "我爱北京天安门\nhello kitty";
            //text2.height = 5;
            //text2.font = "1234bbb";
            //text2.position = new LitMath.Vector2(100, 100);
            //modelSpace.AppendEntity(text2);
        }

        public void AppendCommandLine(string text)
        {
            _statusStripMgr.AppendCommandLine(text);
        }


        public void DrawEntity(Graphics graphics, Entity entity, Pen pen = null)
        {
            _worldDraw.graphics = graphics;

            if (pen == null)
                _worldDraw.pen = GetPen(entity);
            else
                _worldDraw.pen = pen;


            if (entity is Text)
            {
                _worldDraw.brush = GetBrush(entity);
            }

            entity.Draw(_worldDraw);
        }


        protected Pen GetPen(Entity entity)
        {
            DashStyle linetype = (System.Drawing.Drawing2D.DashStyle)entity.lineType;
            Color color = entity.colorValue;


            if (entity.database == null)
            {
                if (entity.color.colorMethod == Colors.ColorMethod.ByLayer)
                {
                    Database db = (this.document as Document).database;
                    Layer layer = db.GetObject(entity.layerId) as Layer;
                    if (layer != null)
                    {
                        color = layer.colorValue;
                    }
                }

                if (entity.lineType == ZacCAD.DatabaseServices.LineType.ByLayer)
                {
                    Database db = (this.document as Document).database;
                    Layer layer = db.GetObject(entity.layerId) as Layer;
                    if (layer != null)
                    {
                        linetype = (DashStyle)layer.lineType;
                    }
                }
            }

            if ((int)linetype == -1)
                linetype = DashStyle.Solid;

            return GDIResMgr.Instance.GetPen(color, linetype, 1);
        }


        protected Brush GetBrush(Entity entity)
        {
            if (entity.database != null)
            {
                return GDIResMgr.Instance.GetBrush(entity.colorValue);
            }
            else
            {
                if (entity.color.colorMethod == Colors.ColorMethod.ByLayer)
                {
                    Database db = (this.document as Document).database;
                    Layer layer = db.GetObject(entity.layerId) as Layer;
                    if (layer != null)
                    {
                        return GDIResMgr.Instance.GetBrush(layer.colorValue);
                    }
                    else
                    {
                        return GDIResMgr.Instance.GetBrush(entity.colorValue);
                    }
                }
                else
                {
                    return GDIResMgr.Instance.GetBrush(entity.colorValue);
                }
            }
        }

        public void AppendEntity(Entity entity)
        {
            Block modelSpace = _document.database.blockTable["ModelSpace"] as Block;
            modelSpace.AppendEntity(entity);
        }

        /// <summary>
        /// 绘制画布
        /// </summary>
        public void OnPaintCanvas(PaintEventArgs e)
        {
            int canvasWidth = (int)_canvas.width;
            int canvasHeight = (int)_canvas.height;

            if (canvasWidth <= 0 || canvasHeight <= 0)
                return;


            Rectangle clipRectangle = e.ClipRectangle;
            Rectangle canvasRectangle = new Rectangle(0, 0, canvasWidth, canvasHeight);

            if (_bufferBitmap == null)
            {
                clipRectangle = canvasRectangle;
                _bufferBitmap = new Bitmap(canvasWidth, canvasHeight);
                _bufferBitmapToRedraw = true;
            }

            if (_bufferBitmapToRedraw)
            {
                _bufferBitmapToRedraw = false;

                Graphics graphics = Graphics.FromImage(_bufferBitmap);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw background
                graphics.Clear(Color.FromArgb(33, 40, 48));

                // 
                _worldDraw.graphics = graphics;
                _canvasDraw.graphics = graphics;

                // Draw the database graphic object
                Block modelSpace = _document.database.blockTable[_document.currentBlockName] as Block;
                foreach (Entity entity in modelSpace)
                {
                    if (_document.selections.IsObjectSelected(entity.id))
                    {
                        _worldDraw.pen = GDIResMgr.Instance.GetEntitySelectedPen(entity);
                        if (entity is Text)
                        {
                            _worldDraw.brush = GDIResMgr.Instance.GetEntitySelectedBrush(entity);
                        }
                        entity.Draw(_worldDraw);
                    }
                    else
                    {
                        _worldDraw.pen = GetPen(entity);
                        if (entity is Text)
                        {
                            _worldDraw.brush = GetBrush(entity);
                        }
                        entity.Draw(_worldDraw);
                    }
                }
            }
            _worldDraw.graphics = e.Graphics;
            _canvasDraw.graphics = e.Graphics;

            // Double buffer: Draw the picture to the canvas
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.DrawImage(_bufferBitmap, clipRectangle, clipRectangle, GraphicsUnit.Pixel);

            // Command manager drawing
            _cmdsMgr.OnPaint(e.Graphics);

            // origin
            _origin.OnPaint(e.Graphics);

            // Draw Pointer
            _pointer.OnPaint(e.Graphics);

            return;

            LitMath.Vector2 origin = this.ModelToCanvas(new LitMath.Vector2(0, 0));
            e.Graphics.DrawArc(GDIResMgr.Instance.GetPen(Color.Green, DashStyle.Solid, 0), (float)origin.x - 50, (float)origin.y - 50, 100, 100, 45, -45);

            string text = "1234567890\n北京\nabcdefg";
            FontStyle fontStyle = FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout | FontStyle.Underline;
            Font font = new Font("Arial", 16, fontStyle);
            SolidBrush brush = new SolidBrush(Color.White);
            PointF position = new PointF(500, 500);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;
            //format.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            e.Graphics.DrawString(text, font, brush, position, format);

            brush = new SolidBrush(Color.Green);
            RectangleF rect = new RectangleF(position, new SizeF(200, 65));
            e.Graphics.DrawString(text, font, brush, rect, format);
            e.Graphics.DrawRectangle(new Pen(Color.White, 1), position.X, position.Y, rect.Width, rect.Height);

            SizeF size = e.Graphics.MeasureString(text, font, position, format);

            PointF pos1 = position;
            PointF pos2 = new PointF(pos1.X + size.Width, pos1.Y);
            PointF pos3 = new PointF(pos2.X, pos2.Y + size.Height);
            PointF pos4 = new PointF(pos3.X - size.Width, pos3.Y);
            e.Graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, DashStyle.Solid, 0), pos1, pos2);
            e.Graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, DashStyle.Solid, 0), pos2, pos3);
            e.Graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, DashStyle.Solid, 0), pos3, pos4);
            e.Graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, DashStyle.Solid, 0), pos4, pos1);

            e.Graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Green, DashStyle.Solid, 0), position, new PointF(1000, 0));
        }

        internal void RepaintCanvas(bool bufferBitmapToRedraw = false)
        {
            if (bufferBitmapToRedraw)
                _bufferBitmapToRedraw = true;
            _canvas.Repaint();
        }

        public void OnResize(EventArgs e)
        {
            _bufferBitmap = null;
            RepaintCanvas(true);
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            Commands.Command cmd = _pointer.OnMouseDown(e);

            _mouseDownPoint.x = e.X;
            _mouseDownPoint.y = e.Y;

            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnMouseDown(e);
                RepaintCanvas(true);
            }
            else
            {
                if (cmd != null)
                {
                    _cmdsMgr.DoCommand(cmd);
                    RepaintCanvas();
                }
            }
        }

        private LitMath.Vector2 _mouseDownPoint = new LitMath.Vector2();
        public void OnMouseUp(MouseEventArgs e)
        {
            _pointer.OnMouseUp(e);

            if (e.Button == MouseButtons.Middle)
            {
                _screenPan += _screenDrag;
                _screenDrag.x = 0;
                _screenDrag.y = 0;

                RepaintCanvas(true);

                return;
            }

            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnMouseUp(e);
                RepaintCanvas();
            }
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            _pointer.OnMouseMove(e);

            if (e.Button == MouseButtons.Middle)
            {
                LitMath.Vector2 ePoint = new LitMath.Vector2(e.X, e.Y);
                _screenDrag = ePoint - _mouseDownPoint;
                RepaintCanvas(true);
                return;
            }

            if (_cmdsMgr.CurrentCmd != null)
            {
                _cmdsMgr.OnMouseMove(e);
                RepaintCanvas();
            }

            if (docMouseMove != null)
            {
                docMouseMove.Invoke(e);
            }
        }

        public void OnMouseDoubleClick(MouseEventArgs e)
        {
            _pointer.OnMouseDoubleClick(e);
        }

        public void OnMouseWheel(MouseEventArgs e)
        {
            LitMath.Vector2 mousePosInCanvas = _canvas.GetMousePosition();
            LitMath.Vector2 mousePosInModel = this.CanvasToModel(mousePosInCanvas);

            float zoomDelta = 1.25f * (float)Math.Abs(e.Delta) / 120.0f;
            if (e.Delta < 0)
            {
                if (_zoom >= _zoomMin)
                {
                    _zoom = _zoom / zoomDelta;
                }
            }
            else
            {
                if (_zoom <= _zoomMax)
                {
                    _zoom = _zoom * zoomDelta;
                }
            }

            MoveModelPositionToCanvasPosition(mousePosInModel, mousePosInCanvas);

            RepaintCanvas(true);
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            if (_cmdsMgr.CurrentCmd != null)
            {
                if (e.KeyCode == Keys.Escape)
                    _cmdsMgr.DoParameter(e.ToString());
                else
                    _cmdsMgr.OnKeyDown(e);
            }
            else
            {
                //if (_dynamicInputer.StartCmd(e))
                //{
                //}
                //else if (e.KeyCode == Keys.Escape)
                if (e.KeyCode == Keys.Escape)
                {
                    _document.selections.Clear();
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if ((document as Document).selections.Count > 0)
                    {
                        Commands.Modify.DeleteCmd cmd = new Commands.Modify.DeleteCmd();
                        this.OnCommand(cmd);
                    }
                }
                else
                {
                    _cmdsMgr.OnKeyDown(e);
                }
            }
        }

        private CommandsFactory _cmdsFactory = new CommandsFactory();
        private void OnCmdInputResult(DynInputToolStripCtrl sender, DynInputResult result)
        {
            switch (result.status)
            {
                case DynInputStatus.OK:
                    {
                        DynInputResult<string> cmdInputRet = result as DynInputResult<string>;

                        if (cmdInputRet.value != "" && cmdInputRet.value.Substring(0, 1) == "_")
                        {
                            string cmdStr = cmdInputRet.value.Substring(1);

                            Command cmd = _cmdsFactory.NewCommand(cmdStr.ToLower());
                            if (cmd != null)
                            {
                                this.OnCommand(cmd);
                            }
                        }
                        else
                        {
                            this.OnParameter(cmdInputRet.value);
                        }
                    }
                    break;

                case DynInputStatus.Cancel:
                    break;

                case DynInputStatus.Error:
                    break;

                default:
                    break;
            }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            if (_cmdsMgr != null)
            {
                _cmdsMgr.OnKeyUp(e);
            }
        }

        /// <summary>
        /// Excuting an order
        /// </summary>
        public void OnParameter(string parameter)
        {
            _cmdsMgr.DoParameter(parameter);
        }

        /// <summary>
        /// Excuting an order
        /// </summary>
        public void OnCommand(ICommand cmd)
        {
            _cmdsMgr.DoCommand(cmd as Commands.Command);
        }

        /// <summary>
        /// 命令完成
        /// </summary>
        public void OnCommandFinished(Commands.Command cmd)
        {
            this.RepaintCanvas(true);
        }

        /// <summary>
        /// 命令取消
        /// </summary>
        public void OnCommandCanceled(Commands.Command cmd)
        {
            this.RepaintCanvas(false);
        }

        /// <summary>
        /// 选择集变更
        /// </summary>
        public void OnSelectionChanged()
        {
            _pointer.OnSelectionChanged();
            this.RepaintCanvas(true);
        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        public void DrawLine(Graphics graphics, Pen pen, LitMath.Vector2 p1, LitMath.Vector2 p2, CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                LitMath.Vector2 startInCanvas = ModelToCanvas(p1);
                LitMath.Vector2 endInCanvas = ModelToCanvas(p2);
                graphics.DrawLine(pen, (float)startInCanvas.x, (float)startInCanvas.y, (float)endInCanvas.x, (float)endInCanvas.y);
            }
            else
            {
                graphics.DrawLine(pen, (float)p1.x, (float)p1.y, (float)p2.x, (float)p2.y);
            }
        }

        /// <summary>
        /// 绘制圆
        /// </summary>
        public void DrawCircle(Graphics graphics, Pen pen, LitMath.Vector2 center, double radius, CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                LitMath.Vector2 centerInCanvas = ModelToCanvas(center);
                double radiusInCanvas = ModelToCanvas(radius);
                graphics.DrawEllipse(pen, (float)(centerInCanvas.x - radiusInCanvas), (float)(centerInCanvas.y - radiusInCanvas), (float)radiusInCanvas * 2, (float)radiusInCanvas * 2);
            }
            else
            {
                graphics.DrawEllipse(pen, (float)(center.x - radius), (float)(center.y - radius), (float)radius * 2, (float)radius * 2);
            }
        }

        /// <summary>
        /// 绘制圆弧
        /// </summary>
        public void DrawArc(Graphics graphics, Pen pen, LitMath.Vector2 center, double radius, double startAngle, double endAngle, CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                LitMath.Vector2 centerInCanvas = ModelToCanvas(center);
                double radiusInCanvas = ModelToCanvas(radius);
                double startAngleInCanvas = 360 - endAngle;
                double endAngleInCanvas = 360 - startAngle;

                DrawArcInCanvas(graphics, pen,
                    centerInCanvas, radiusInCanvas, startAngleInCanvas, endAngleInCanvas);
            }
            else
            {
                DrawArcInCanvas(graphics, pen,
                    center, radius, startAngle, startAngle);
            }
        }

        private void DrawArcInCanvas(Graphics graphics, Pen pen, LitMath.Vector2 center, double radius, double startAngle, double endAngle)
        {
            if (endAngle < startAngle)
                endAngle += 360;

            graphics.DrawArc(pen, (float)(center.x - radius), (float)(center.y - radius), (float)radius * 2, (float)radius * 2, (float)startAngle, (float)(endAngle - startAngle));
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public void DrawRectangle(Graphics graphics, Pen pen, LitMath.Vector2 position, double width, double height, CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                double widthInCanvas = this.ModelToCanvas(width);
                double heightInCanvas = this.ModelToCanvas(height);
                LitMath.Vector2 posInCanvas = this.ModelToCanvas(position);
                posInCanvas.y -= heightInCanvas;

                graphics.DrawRectangle(pen, (float)posInCanvas.x, (float)posInCanvas.y, (float)widthInCanvas, (float)heightInCanvas);
            }
            else
            {
                graphics.DrawRectangle(pen, (float)position.x, (float)position.y, (float)width, (float)height);
            }
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public void DrawRectangle(Graphics graphics, Pen pen, LitMath.Rectangle2 rectangle, CSYS csys = CSYS.Model)
        {
            this.DrawRectangle(graphics, pen, rectangle.location, rectangle.width, rectangle.height, csys);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public void DrawRectangle(Graphics graphics, Pen pen, double x, double y, double width, double height, CSYS csys = CSYS.Model)
        {
            this.DrawRectangle(graphics, pen, new LitMath.Vector2(x, y), width, height, csys);
        }

        /// <summary>
        /// 绘制点
        /// </summary>
        public void DrawPoint(Graphics graphics, Brush brush, LitMath.Vector2 point)
        {
            graphics.FillEllipse(brush, (float)point.x, (float)point.y, 5, 5);
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        public LitMath.Vector2 DrawString(Graphics graphics, Brush brush, string text, string fontName, double fontHeight, TextAlignment textAlign, LitMath.Vector2 position, CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                fontHeight = (int)this.ModelToCanvas(fontHeight);
                position = this.ModelToCanvas(position);
            }

            SizeF textSizeInCanvas = this.DrawStringInCanvas(
                graphics, brush, text, fontName, (int)fontHeight, textAlign,
                (float)position.x, (float)position.y);

            double w = textSizeInCanvas.Width;
            double h = textSizeInCanvas.Height;
            if (csys == CSYS.Model)
            {
                w = this.CanvasToModel(w);
                h = this.CanvasToModel(h);
            }
            return new LitMath.Vector2(w, h);
        }

        public SizeF DrawStringInCanvas(Graphics graphics, Brush brush, string text, string fontName, int fontHeight, TextAlignment textAlign, float x, float y)
        {
            if (fontHeight <= 0)
            {
                return new SizeF(0, 0);
            }

            FontStyle fontStyle = FontStyle.Regular;
            Font font = new Font(fontName, (int)fontHeight, fontStyle);
            StringFormat format = new StringFormat();
            switch (textAlign)
            {
                case TextAlignment.LeftBottom:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.LeftMiddle:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.LeftTop:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;

                case TextAlignment.CenterBottom:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.CenterMiddle:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.CenterTop:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;

                case TextAlignment.RightBottom:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;

                case TextAlignment.RightMiddle:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;

                case TextAlignment.RightTop:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
            }

            PointF pos = new PointF(x, y);
            graphics.DrawString(text, font, brush, pos, format);

            SizeF size = graphics.MeasureString(text, font, pos, format);
            return size;

            //PointF pos1 = new PointF((float)posInCanvas.x, (float)posInCanvas.y - size.Height);
            //PointF pos2 = new PointF(pos1.X + size.Width, pos1.Y);
            //PointF pos3 = new PointF(pos2.X, pos2.Y + size.Height);
            //PointF pos4 = new PointF(pos3.X - size.Width, pos3.Y);
            //graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, 0),
            //    pos1, pos2);
            //graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, 0),
            //    pos2, pos3);
            //graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, 0),
            //    pos3, pos4);
            //graphics.DrawLine(GDIResMgr.Instance.GetPen(Color.Red, 0),
            //    pos4, pos1);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        public void FillRectangle(Graphics graphics, Brush brush, LitMath.Vector2 position, double width, double height, CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                double widthInCanvas = this.ModelToCanvas(width);
                double heightInCanvas = this.ModelToCanvas(height);
                LitMath.Vector2 posInCanvas = this.ModelToCanvas(position);
                posInCanvas.y -= heightInCanvas;

                graphics.FillRectangle(brush,
                    (float)posInCanvas.x, (float)posInCanvas.y,
                    (float)widthInCanvas, (float)heightInCanvas);
            }
            else
            {
                graphics.FillRectangle(brush,
                    (float)position.x, (float)position.y,
                    (float)width, (float)height);
            }
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        public void FillRectangle(
            Graphics graphics, Brush brush,
            LitMath.Rectangle2 rectangle,
            CSYS csys = CSYS.Model)
        {
            this.FillRectangle(graphics, brush, rectangle.location, rectangle.width, rectangle.height, csys);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        public void FillRectangle(
            Graphics graphics, Brush brush,
            double x, double y, double width, double height,
            CSYS csys = CSYS.Model)
        {
            this.FillRectangle(graphics, brush, new LitMath.Vector2(x, y), width, height, csys);
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        public void FillEllipse(
            Graphics graphics, Brush brush,
            LitMath.Vector2 center, double width, double height,
            CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                LitMath.Vector2 centerInCanvas = ModelToCanvas(center);
                double widthInCanvas = ModelToCanvas(width);
                double heightInCanvas = ModelToCanvas(height);

                graphics.FillEllipse(brush,
                    (float)centerInCanvas.x, (float)centerInCanvas.y,
                    (float)widthInCanvas, (float)heightInCanvas);
            }
            else
            {
                graphics.FillEllipse(brush,
                    (float)center.x, (float)center.y,
                    (float)width, (float)height);
            }
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        public void FillPolygon(
            Graphics graphics, Brush brush,
            List<LitMath.Vector2> points,
            CSYS csys = CSYS.Model)
        {
            if (csys == CSYS.Model)
            {
                List<LitMath.Vector2> pointsInCanvas = new List<LitMath.Vector2>(points.Count);
                for (int i = 0; i < points.Count; ++i)
                {
                    pointsInCanvas.Add(ModelToCanvas(points[i]));
                }

                PointF[] pnts = new PointF[points.Count];
                for (int i = 0; i < points.Count; ++i)
                {
                    pnts[0] = new PointF((float)pointsInCanvas[i].x, (float)pointsInCanvas[i].y);
                }

                graphics.FillPolygon(brush, pnts);
            }
            else
            {
                PointF[] pnts = new PointF[points.Count];
                for (int i = 0; i < points.Count; ++i)
                {
                    pnts[i] = new PointF((float)points[i].x, (float)points[i].y);
                }
                graphics.FillPolygon(brush, pnts);
            }
        }

        /// <summary>
        /// 绘制选择框
        /// </summary>
        public void DrawSelectRect(Graphics g, SelectRectangle selRect)
        {
            Color color = Color.White;
            if (selRect.selectMode == SelectRectangle.SelectMode.Window)
            {
                color = Color.Green;
            }
            else
            {
                color = Color.Blue;
            }

            XorGDI.DrawRectangle(g, XorGDI.PenStyles.PS_DOT, color, 1,
                selRect.startPoint, selRect.endPoint);
        }

        /// <summary>
        /// 将Model下的坐标移动到和Canvas下的坐标对应
        /// </summary>
        private void MoveModelPositionToCanvasPosition(
            LitMath.Vector2 modelPos,
            LitMath.Vector2 canvasPos)
        {
            LitMath.Vector2 modelPosInCanvas = this.ModelToCanvas(modelPos);
            _screenPan += canvasPos - modelPosInCanvas;
        }

        public LitMath.Vector2 ModelToCanvasSec(LitMath.Vector2 pointInModel)
        {
            LitMath.Vector2 pan = this.screenPan;
            double x = pointInModel.x * _resolution * _zoom + pan.x;
            double y = pointInModel.y * _resolution * _zoom + pan.y;

            return new LitMath.Vector2(x, y);
        }

        /// <summary>
        /// Model <> Screen 坐标变换
        /// </summary>
        #region Model<>Screen
        public double ModelToCanvas(double value)
        {
            return value * _resolution * _zoom;
        }

        public LitMath.Vector2 ModelToCanvas(LitMath.Vector2 pointInModel)
        {
            LitMath.Vector2 pan = this.screenPan;
            double x = pointInModel.x * _resolution * _zoom + pan.x;
            double y = _canvas.height - pointInModel.y * _resolution * _zoom + pan.y;

            return new LitMath.Vector2(x, y);
        }

        public double CanvasToModel(double value)
        {
            return value / (_resolution * _zoom);
        }

        public LitMath.Vector2 CanvasToModel(LitMath.Vector2 pointInCanvas)
        {
            LitMath.Vector2 pan = this.screenPan;
            double x = pointInCanvas.x - pan.x;
            double y = _canvas.height - (pointInCanvas.y - pan.y);

            return new LitMath.Vector2(x, y) / (_resolution * _zoom);
        }
        #endregion
    }
}
