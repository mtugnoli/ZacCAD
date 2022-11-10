using System.Diagnostics;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using PropertyGridEx;
using ZacCAD.DatabaseServices;
using ZacCAD.ApplicationServices;
using ZacCAD.Commands;
using ZacCAD.UI;

namespace ZacCAD.Windows
{
    public partial class EntityInfoForm
    {
        protected XmlDataDocument document = null;
        protected int iSelectedRow = 0;
        protected int iCountRow = 0;
        private DBObject _object;
        private Document _document;
        private List<string> layers = new List<string>();

        public EntityInfoForm(Document document, DBObject entity)
        {
            InitializeComponent();

            _object = entity;
            _document = document;

            foreach (var layer in document.database.layerTable)
            {
                layers.Add(layer.name);
            }
        }

        private void frmMain_Load(System.Object sender, System.EventArgs e)
        {
            // Remove the Property Pages button
            Properties.ToolStrip.Items.RemoveAt(4);     // 1)

            this.Show();

            // Run the first example
            //ButtonExample1_Click(this, null);
        }

        private void frmMain_Shown(object sender, System.EventArgs e)
        {
            PopulatePropertyGrid();

            // Apply style to DocComment object
            ApplyCommentsStyle();       //3)
        }

        private void ApplyCommentsStyle()
        {
            // Apply new style to DocComment
            Properties.DocCommentTitle.Font = new Font("Tahoma", 14, FontStyle.Bold);
            Properties.DocCommentDescription.Location = new Point(3, (5 + Properties.DocCommentTitle.Font.Height));
        }

        public void PopulatePropertyGrid()
        {
            // 2)
            try
            {

                // hide all buttons
                ButtonSelection(6);

                // Load the propertygrid
                if (_object is Polyline)
                {
                    var poly = (Polyline)_object;

                    FillPropertyByPolyline(poly, layers);

                    //foreach (var vertice in poly.Vertices)
                    //{
                    //    allPoints.Add(vertice);
                    //}
                }
                else if (_object is Circle)
                {
                    var circle = (Circle)_object;

                    FillPropertyByCircle(circle, layers);
                }
                else if (_object is Arc)
                {
                    var arc = (Arc)_object;

                    FillPropertyByArc(arc, layers);
                }
                else if (_object is Ellipse)
                {
                    var ellipse = (Ellipse)_object;

                    FillPropertyByEllipse(ellipse, layers);
                }
                else if (_object is Line)
                {
                    var line = (Line)_object;

                    FillPropertyByLine(line, layers);
                }
                else if (_object is XPoint)
                {
                    var point = (XPoint)_object;

                    FillPropertyByPoint(point, layers);
                }
                else if (_object is Ray)
                {
                    var ray = (Ray)_object;

                    FillPropertyByRay(ray, layers);
                }
                else if (_object is Xline)
                {
                    var xline = (Xline)_object;

                    FillPropertyByXline(xline, layers);
                }

                // Update the status bar
                StatusLabel.Text = ButtonExample1.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = ButtonExample1.Text + " [ FAILED ]";
                string text = ex.Message.ToString() + "\n" + ex.StackTrace.ToString();
                MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillPropertyByPolyline(Polyline poly, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", poly.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("Closed", poly.closed, false, "Data", "Polyline is closed", true);

            //Properties.Item.Add("Point", new PointF((float)point.endPoint.x, (float)point.endPoint.y), false, "Data", "Point", true);
            //Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            //Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", poly.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(poly.color.r, poly.color.g, poly.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(poly.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);


            Properties.Item.Add("Layer", poly.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByEllipse(Ellipse ellipse, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", ellipse.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("RadiusX", ellipse.radiusX, false, "Data", "Entity Radius X", true);
            Properties.Item.Add("RadiusY", ellipse.radiusY, false, "Data", "Entity Radius Y", true);

            Properties.Item.Add("CenterPoint", new PointF((float)ellipse.center.x, (float)ellipse.center.y), false, "Data", "Start Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", ellipse.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(ellipse.color.r, ellipse.color.g, ellipse.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(ellipse.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);


            Properties.Item.Add("Layer", ellipse.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByCircle(Circle circle, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", circle.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("Radius", circle.radius, false, "Data", "Entity Radius", true);

            Properties.Item.Add("CenterPoint", new PointF((float)circle.center.x, (float)circle.center.y), false, "Data", "Start Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", circle.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(circle.color.r, circle.color.g, circle.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(circle.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);


            Properties.Item.Add("Layer", circle.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByArc(Arc arc, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", arc.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("Radius", arc.radius, false, "Data", "Entity Radius", true);
            Properties.Item.Add("StartAngle", arc.startAngle, false, "Data", "Entity Start Angle", true);
            Properties.Item.Add("EndAngle", arc.endAngle, false, "Data", "Entity End Angle", true);

            Properties.Item.Add("CenterPoint", new PointF((float)arc.center.x, (float)arc.center.y), false, "Data", "Start Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", arc.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(arc.color.r, arc.color.g, arc.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(arc.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);


            Properties.Item.Add("Layer", arc.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByLine(Line line, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", line.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("StartPoint", new PointF((float)line.startPoint.x, (float)line.startPoint.y), false, "Data", "Start Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("EndPoint", new PointF((float)line.endPoint.x, (float)line.endPoint.y), false, "Data", "Start Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", line.lineWeight, false, "Data", "Line Weight", true);

            //Properties.Item.Add("Color", Color.FromArgb(line.color.r, line.color.g, line.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(line.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);


            Properties.Item.Add("Layer", line.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByPoint(XPoint point, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", point.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("Point", new PointF((float)point.endPoint.x, (float)point.endPoint.y), false, "Data", "Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", point.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(point.color.r, point.color.g, point.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(point.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);

            Properties.Item.Add("Layer", point.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByRay(Ray ray, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", ray.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("BasePoint", new PointF((float)ray.basePoint.x, (float)ray.basePoint.y), false, "Data", "Base Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("Direction", new PointF((float)ray.direction.x, (float)ray.direction.y), false, "Data", "Direction Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", ray.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(ray.color.r, ray.color.g, ray.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(ray.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);


            Properties.Item.Add("Layer", ray.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }

        private void FillPropertyByXline(Xline xline, List<string> layers)
        {
            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Id", xline.id, true, "Data", "Entity Id", true);

            Properties.Item.Add("BasePoint", new PointF((float)xline.basePoint.x, (float)xline.basePoint.y), false, "Data", "Base Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("Direction", new PointF((float)xline.direction.x, (float)xline.direction.y), false, "Data", "Direction Point", true);
            Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
            Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsPointF;

            Properties.Item.Add("LineWeight", xline.lineWeight, false, "Data", "Line Weight", true);
            //Properties.Item.Add("Color", Color.FromArgb(xline.color.r, xline.color.g, xline.color.b), false, "Data", "Entity Color", true);
            ColorType enumColor = GetColorTypeFromRgb(xline.color);
            Properties.Item.Add("Color", enumColor, false, "Data", "Entity Color", true);

            Properties.Item.Add("Layer", xline.layer, false, "Data", "Entity Layer", true);
            Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(layers.ToArray(), true);

            Properties.Refresh();
        }



        #region "PropertyGrid ToolStripButton Events"

        private void ButtonExample1_Click(object sender, EventArgs e)
        {
            try
            {
                // Select the first example page
                ButtonSelection(1);

                // Load the propertygrid
                //FillPropertyGrid1(FilterPropertyType.None);

                // Update the status bar
                StatusLabel.Text = ButtonExample1.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = ButtonExample1.Text + " [ FAILED ]";
                string text = ex.Message.ToString() + "\n" + ex.StackTrace.ToString();
                // Interaction.MsgBox(text, MsgBoxStyle.Critical, null);
            }
        }

        private void ButtonExample2_Click(object sender, EventArgs e)
        {
            try
            {
                // Select the second example page
                ButtonSelection(2);

                // Load the propertygrid
                FillPropertyGrid2();

                // Update the status bar
                StatusLabel.Text = ButtonExample2.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = ButtonExample2.Text + " [ FAILED ]";
                // Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical, null);
            }
        }

        private void ButtonExample3_Click(object sender, EventArgs e)
        {
            try
            {
                // Select the third example page
                ButtonSelection(3);

                // Load the propertygrid
                FillPropertyGrid3();

                // Update the status bar
                StatusLabel.Text = ButtonExample3.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = ButtonExample3.Text + " [ FAILED ]";
                // Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical, null);
            }
        }

        private void ButtonExample4_Click(object sender, EventArgs e)
        {
            try
            {
                // Select the third example page
                ButtonSelection(4);

                // Load the propertygrid
                iSelectedRow = 0;
                FillPropertyGrid4();

                // Update the status bar
                StatusLabel.Text = ButtonExample4.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = ButtonExample4.Text + " [ FAILED ]";
                // Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical, null);
            }
        }

        private void ButtonSerialize_Click(System.Object sender, System.EventArgs e)
        {
            ButtonSerialize.ShowDropDown();
        }

        private void UsingXmlSerializerToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Items.xml");

                // Select the third example page
                ButtonSerialize.Text = UsingXmlSerializerToolStripMenuItem.Text;
                ButtonSelection(5);

                Properties.Item.Clear();
                if (!Properties.Item.LoadXml(filename))
                {
                    //FillPropertyGrid1(FilterPropertyType.FilterXmlSerializer);
                    Properties.Item.SaveXml(filename);
                }
                Properties.Refresh();

                // Update the status bar
                StatusLabel.Text = "Load Items " + ButtonSerialize.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = "Load Items " + ButtonSerialize.Text + " [ FAILED ]";
                // Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical, null);
            }
        }

        private void UsingBinaryFormatterToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Items.dat");

                // Select the third example page
                ButtonSerialize.Text = UsingBinaryFormatterToolStripMenuItem.Text;
                ButtonSelection(5);

                //// Load the propertygrid
                //Properties.Item.Clear();
                //if (!Properties.Item.LoadBinary(filename))
                //{
                //    FillPropertyGrid1(FilterPropertyType.FilterBinaryFormatter);
                //    Properties.Item.SaveBinary(filename);
                //}
                //Properties.Refresh();

                // Update the status bar
                StatusLabel.Text = "Load Items " + ButtonSerialize.Text + " [ OK ]";
            }
            catch (Exception ex)
            {
                StatusLabel.Image = SystemIcons.Error.ToBitmap();
                StatusLabel.Text = "Load Items " + ButtonSerialize.Text + " [ FAILED ]";
                // Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical, null);
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {

            DataRow Row = null;

            if (document != null)
            {
                DataRowCollection Rows;
                Rows = document.DataSet.Tables[1].Rows;

                iSelectedRow++;
                if (iSelectedRow == Rows.Count - 1)
                {
                    ButtonNext.Enabled = false;
                }
                if (ButtonPrevious.Enabled == false)
                {
                    ButtonPrevious.Enabled = true;
                }

                Row = Rows[iSelectedRow];

                foreach (CustomProperty CustomProp in new ArrayList(Properties.Item))
                {
                    if (CustomProp.Category == "Dynamic view of a DataTable")
                    {
                        Properties.Item.Remove(CustomProp.Name);
                    }
                }

                foreach (DataColumn Column in document.DataSet.Tables[1].Columns)
                {
                    object oRow = Row;
                    Properties.Item.Add(Column.ColumnName.ToString(), ref oRow, Column.ColumnName.ToString(), false, "Dynamic view of a DataTable", "", true);
                }
                Properties.Refresh();
            }

        }

        private void ButtonPrevious_Click(object sender, EventArgs e)
        {
            DataRow Row = null;

            if (document != null)
            {
                DataRowCollection Rows;
                Rows = document.DataSet.Tables[1].Rows;
                iSelectedRow--;
                if (iSelectedRow == 0)
                {
                    ButtonPrevious.Enabled = false;
                }
                if (ButtonNext.Enabled == false)
                {
                    ButtonNext.Enabled = true;
                }

                Row = Rows[iSelectedRow];

                foreach (CustomProperty CustomProp in new ArrayList(Properties.Item))
                {
                    if (CustomProp.Category == "Dynamic view of a DataTable")
                    {
                        Properties.Item.Remove(CustomProp.Name);
                    }
                }

                foreach (DataColumn Column in document.DataSet.Tables[1].Columns)
                {
                    object oRow = Row;
                    Properties.Item.Add(Column.ColumnName.ToString(), ref oRow, Column.ColumnName.ToString(), false, "Dynamic view of a DataTable", "", true);
                }
                Properties.Refresh();
            }

        }

        #endregion

        private void ButtonSelection(int iState)
        {

            // This routine plays with the Toolbar of the PropertyGrid
            switch (iState)
            {
                case 1: // Single property Page

                    ButtonExample1.Checked = true;
                    ButtonExample1.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    Properties.ContextMenuStrip = null;
                    ButtonExample2.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample2.Checked = false;
                    ButtonExample3.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample3.Checked = false;
                    ButtonExample4.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample4.Checked = false;
                    ToolStripSeparator1.Visible = false;
                    ButtonNext.Visible = false;
                    ButtonPrevious.Visible = false;
                    ButtonSerialize.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    break;

                case 2: // Multi object property Page

                    ButtonExample2.Checked = true;
                    ButtonExample2.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    Properties.ContextMenuStrip = null;
                    ButtonExample1.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample1.Checked = false;
                    ButtonExample3.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample3.Checked = false;
                    ButtonExample4.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample4.Checked = false;
                    ToolStripSeparator1.Visible = false;
                    ButtonNext.Visible = false;
                    ButtonPrevious.Visible = false;
                    ButtonSerialize.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    break;


                case 3: // Databinding

                    ButtonExample3.Checked = true;
                    ButtonExample3.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    Properties.ContextMenuStrip = null;
                    ButtonExample1.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample1.Checked = false;
                    ButtonExample2.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample2.Checked = false;
                    ButtonExample4.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample4.Checked = false;
                    ToolStripSeparator1.Visible = false;
                    ButtonNext.Visible = false;
                    ButtonPrevious.Visible = false;
                    ButtonSerialize.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    break;


                case 4: // XML Sample

                    ButtonExample4.Checked = true;
                    ButtonExample4.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    ToolStripSeparator1.Visible = true;
                    ButtonNext.Visible = true;
                    ButtonPrevious.Visible = true;
                    ButtonPrevious.Enabled = false;
                    ButtonNext.Enabled = true;
                    Properties.ContextMenuStrip = ContextMenuSaveBooks;

                    ButtonExample1.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample1.Checked = false;
                    ButtonExample2.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample2.Checked = false;
                    ButtonExample3.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample3.Checked = false;
                    ButtonSerialize.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    break;

                case 5:

                    ButtonSerialize.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    Properties.ContextMenuStrip = ContextMenuSaveItems;
                    ButtonExample1.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample1.Checked = false;
                    ButtonExample2.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample2.Checked = false;
                    ButtonExample3.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample3.Checked = false;
                    ButtonExample4.DisplayStyle = ToolStripItemDisplayStyle.Image;
                    ButtonExample4.Checked = false;
                    ToolStripSeparator1.Visible = false;
                    ButtonNext.Visible = false;
                    ButtonPrevious.Visible = false;
                    break;

                case 6:

                    ButtonSerialize.Visible = false;
                    ButtonExample1.Visible = false;
                    ButtonExample2.Visible = false;
                    ButtonExample3.Visible = false;
                    ButtonExample4.Visible = false;
                    ToolStripSeparator1.Visible = false;
                    ButtonNext.Visible = false;
                    ButtonPrevious.Visible = false;
                    break;


            }
        }

        public ColorType GetColorTypeFromRgb(ZacCAD.Colors.Color color)
        {
            ColorType ret = ColorType.ByLayer;

            if (color.Name.ToLower() == "bylayer")
            {
                ret = ColorType.ByLayer;
            }
            else if (color.Name.ToLower() == "byblock")
            {
                ret = ColorType.ByBlock;
            }
            else
            {
                if (color == ZacCAD.Colors.Color.FromRGB(255, 0, 0))
                    ret = ColorType.Red;
                else if (color == ZacCAD.Colors.Color.FromRGB(255, 255, 0))
                    ret = ColorType.Yellow;
                else if (color == ZacCAD.Colors.Color.FromRGB(0, 255, 0))
                    ret = ColorType.Green;
                else if (color == ZacCAD.Colors.Color.FromRGB(0, 255, 255))
                    ret = ColorType.Cyan;
                else if (color == ZacCAD.Colors.Color.FromRGB(0, 0, 255))
                    ret = ColorType.Blue;
                else if (color == ZacCAD.Colors.Color.FromRGB(255, 0, 255))
                    ret = ColorType.Magenta;
                else if (color == ZacCAD.Colors.Color.FromRGB(255, 255, 255))
                    ret = ColorType.White;
            }

            return ret;
        }

        public ZacCAD.Colors.Color GetColorFromColorType(ColorType color)
        {
            ZacCAD.Colors.Color ret = ZacCAD.Colors.Color.ByLayer;

            if (color == ColorType.ByLayer)
                ret = ZacCAD.Colors.Color.ByLayer;
            else if (color == ColorType.ByBlock)
                ret = ZacCAD.Colors.Color.ByBlock;
            else if (color == ColorType.Red)
                ret = ZacCAD.Colors.Color.FromRGB(255, 0, 0);
            else if (color == ColorType.Yellow)
                ret = ZacCAD.Colors.Color.FromRGB(255, 255, 0);
            else if (color == ColorType.Green)
                ret = ZacCAD.Colors.Color.FromRGB(0, 255, 0);
            else if (color == ColorType.Cyan)
                ret = ZacCAD.Colors.Color.FromRGB(0, 255, 255);
            else if (color == ColorType.Blue)
                ret = ZacCAD.Colors.Color.FromRGB(0, 0, 255);
            else if (color == ColorType.Magenta)
                ret = ZacCAD.Colors.Color.FromRGB(255, 0, 255);
            else if (color == ColorType.White)
                ret = ZacCAD.Colors.Color.FromRGB(255, 255, 255);


            return ret;
        }

        //private void FillPropertyGrid1(FilterPropertyType filter)
        //{
        //    string[] Languages = new string[] { "English", "Italian", "Spanish", "Dutch" };
        //    MyOwnClass[] ListValues = new MyOwnClass[] { new MyOwnClass("English", 0), new MyOwnClass("Italian", 1), new MyOwnClass("Spanish", 2), new MyOwnClass("Dutch", 3) };
        //    int[] Values = new int[] { 1, 2, 3, 4 };
        //    MyOwnClass oInstance = new MyOwnClass("String value", 0);

        //    // The variable filter is used in the "Serialization Example"
        //    // The filter remove from the grid the properties not correctly supported
        //    // or not supported at all.

        //    Properties.ShowCustomProperties = true;
        //    Properties.Item.Clear();

        //    // Simple properties
        //    Properties.Item.Add("My Integer", 100, false, "Simple properties", "This is an integer", true);
        //    Properties.Item.Add("My Double", 10.4, false, "Simple properties", "This is a double", true);
        //    Properties.Item.Add("My String", "My Value", false, "Simple properties", "This is a string", true);
        //    if (filter != FilterPropertyType.FilterXmlSerializer)
        //    {
        //        Properties.Item.Add("My Font", new Font("Arial", 9), false, "Simple properties", "This is a font class", true);
        //        Properties.Item.Add("My Color", new Color(), false, "Simple properties", "This is a color class", true);
        //        Properties.Item.Add("My Point", new Point(10, 10), false, "Simple properties", "This is point class", true);
        //    }
        //    Properties.Item.Add("My Date", new DateTime(DateTime.Today.Ticks), true, "Simple properties", "This is date class", true);
        //    Properties.Item.Add("My Enum", MyEnum.FirstEntry, false, "Simple properties", "Work with Enum too!", true);

        //    // IsPassword attribute
        //    Properties.Item.Add("My Password", "password", false, ".NET v2.0 only", "This is a masked string." + "\r\n" + "(This feature is available only under .NET v2.0)", true);
        //    Properties.Item[Properties.Item.Count - 1].IsPassword = true;

        //    // Filename editor
        //    Properties.Item.Add("Filename", "", false, "Properties with custom UITypeEditor", "This property is a filename path. It define a custom UITypeConverter that show a OpenFileDialog or a SaveFileDialog when the user press the 3 dots button to edit the value.", true);
        //    Properties.Item[Properties.Item.Count - 1].UseFileNameEditor = true;
        //    Properties.Item[Properties.Item.Count - 1].FileNameDialogType = UIFilenameEditor.FileDialogType.LoadFileDialog;
        //    Properties.Item[Properties.Item.Count - 1].FileNameFilter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

        //    if (filter != FilterPropertyType.FilterBinaryFormatter && filter != FilterPropertyType.FilterXmlSerializer)
        //    {
        //        // Custom Editor
        //        Properties.Item.Add("My Custom Editor", "", false, "Properties with custom UITypeEditor", "The component accept custom UITypeEditor.", true);
        //        Properties.Item[Properties.Item.Count - 1].CustomEditor = new MyEditor();

        //        // Custom Event Editor
        //        Properties.Item.Add("My Custom Event", "Click me", false, "Properties with custom UITypeEditor", "The component accept custom UITypeEditor.", true);
        //        Properties.Item[Properties.Item.Count - 1].OnClick += this.CustomEventItem_OnClick;

        //        // Custom TypeConverter
        //        Properties.Item.Add("Integer", 1, false, "Properties with custom TypeConverter", "This property have a custom type converter that show a custom error message.", true);
        //        Properties.Item[Properties.Item.Count - 1].CustomTypeConverter = new MyTypeConverter();
        //    }

        //    // Custom Choices Type Converter
        //    if (filter != FilterPropertyType.FilterXmlSerializer)
        //    {
        //        Properties.Item.Add("Language", "", false, "Properties with custom TypeConverter", "This property uses a TypeConverter to dropdown a list of values.", true);
        //        Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(Languages, true);

        //        Properties.Item.Add("Values", 1, false, "Properties with custom TypeConverter", "This property uses a TypeConverter to dropdown a list of values.", true);
        //        Properties.Item[Properties.Item.Count - 1].Choices = new CustomChoices(Values, false);
        //    }

        //    if (filter != FilterPropertyType.FilterBinaryFormatter && filter != FilterPropertyType.FilterXmlSerializer)
        //    {
        //        // Expandable Type Converter			
        //        Properties.Item.Add("My object", oInstance, false, "Properties with custom TypeConverter", "This property make a \'MyOwnClass\' instance browsable.", true);
        //        Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
        //        Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = BrowsableTypeConverter.LabelStyle.lsEllipsis;
        //    }

        //    // Dynamic properties
        //    if (filter != FilterPropertyType.FilterBinaryFormatter && filter != FilterPropertyType.FilterXmlSerializer)
        //    {
        //        object grid = Properties;
        //        Properties.Item.Add("Autosize properties", ref grid, "AutoSizeProperties", false, "Dynamic Properties", "This is a dynamic bound property. It changes the autosize property of this grid. Try it!", true);
        //        Properties.Item.Add("Draw flat toolbar", ref grid, "DrawFlatToolbar", false, "Dynamic Properties", "This is a dynamic bound property. It changes the DrawFlatToolbar property of this grid. Try it!", true);

        //        object form = this;
        //        Properties.Item.Add("Form opacity", ref form, "Opacity", false, "Dynamic Properties", "This is a dynamic bound property. It changes the Opacity property of this form. Try it!", true);
        //        Properties.Item[Properties.Item.Count - 1].IsPercentage = true;

        //        // PropertyGridEx
        //        Properties.Item.Add("Item", ref grid, "Item", false, "PropertyGridEx", "Represent the PropertyGridEx Item collection.", true);
        //        Properties.Item[Properties.Item.Count - 1].Parenthesize = true;

        //        Properties.Item.Add("DocComment", ref grid, "DocComment", false, "PropertyGridEx", "Represent the DocComment usercontrol of the PropertyGrid.", true);
        //        Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;

        //        Properties.Item.Add("Image", ref grid, "DocCommentImage", false, "PropertyGridEx", "Represent the DocComment usercontrol of the PropertyGrid.", true);
        //        Properties.Item[Properties.Item.Count - 1].DefaultValue = null;
        //        Properties.Item[Properties.Item.Count - 1].DefaultType = typeof(Image);

        //        Properties.Item.Add("Toolstrip", ref grid, "Toolstrip", false, "PropertyGridEx", "Represent the toolstrip of the PropertyGrid.", true);
        //        Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;

        //    }
        //    if (filter == FilterPropertyType.FilterBinaryFormatter)
        //    {

        //        // Databinding works with serialization
        //        Properties.Item.Add("Array of objects", ListValues[2].Text, false, "Databinding", "This is a UITypeEditor that implement a listbox", true);
        //        Properties.Item[Properties.Item.Count - 1].ValueMember = "Value";
        //        Properties.Item[Properties.Item.Count - 1].DisplayMember = "Text";
        //        Properties.Item[Properties.Item.Count - 1].Datasource = ListValues;
        //    }

        //    Properties.Refresh();

        //}

        private object CustomEventItem_OnClick(object sender, EventArgs e)
        {
            CustomProperty prop = (CustomProperty)((CustomProperty.CustomPropertyDescriptor)sender).CustomProperty;
            // Interaction.MsgBox("You clicked on property \'" + prop.Name + "\'", MsgBoxStyle.Information, "Custom Events as UITypeEditor");
            return "Click me again";
        }

        private void FillPropertyGrid2()
        {

            Properties.ShowCustomPropertiesSet = true;
            Properties.ItemSet.Clear();

            Properties.ItemSet.Add();
            Properties.ItemSet[0].Add("My Point", new Point(10, 10), false, "Appearance", "This is a font class", true);
            Properties.ItemSet[0].Add("My Date", new DateTime(2006, 1, 1), false, "Appearance", "This is a datetime", true);

            Properties.ItemSet.Add();
            Properties.ItemSet[1].Add("My Point", new Point(10, 10), false, "Appearance", "This is a font class", true);
            Properties.ItemSet[1].Add("My Date", new DateTime(2007, 1, 1), false, "Appearance", "This is a datetime", true);
            Properties.ItemSet[1].Add("My Color", new Color(), false, "Appearance", "This is a color class", true);

            Properties.Refresh();
        }

        private void FillPropertyGrid3()
        {
            string[] Languages = new string[] { "English", "Italian", "Spanish", "Dutch" };
            MyOwnClass[] ListValues = new MyOwnClass[] { new MyOwnClass("English", 0), new MyOwnClass("Italian", 1), new MyOwnClass("Spanish", 2), new MyOwnClass("Dutch", 3) };

            DataTable LookupTable = null;
            bool IsXmlSampleLoaded = false;
            document = new XmlDataDocument();

            // Load a DataTable from XML
            ParseSchema(ref document, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.xsd"));
            try
            {
                document.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.xml"));
                if (document.DataSet != null)
                {
                    LookupTable = document.DataSet.Tables[1];
                }
                if (LookupTable != null)
                {
                    iCountRow = LookupTable.Rows.Count;
                    IsXmlSampleLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during XML Load: " + ex.Message);
                IsXmlSampleLoaded = false;
            }

            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            Properties.Item.Add("Array of objects", ListValues[2].Text, false, "Databinding", "This is a UITypeEditor that implement a listbox", true);
            Properties.Item[Properties.Item.Count - 1].ValueMember = "Value";
            Properties.Item[Properties.Item.Count - 1].DisplayMember = "Text";
            Properties.Item[Properties.Item.Count - 1].Datasource = ListValues;

            Properties.Item.Add("Array of strings", Languages[1], false, "Databinding", "This is a UITypeEditor that implement a listbox", true);
            Properties.Item[Properties.Item.Count - 1].Datasource = Languages;
            Properties.Item[Properties.Item.Count - 1].IsDropdownResizable = true;

            // If the XML Samples was loaded
            if (IsXmlSampleLoaded)
            {

                // Bind a property to a DataTable
                Properties.Item.Add("Datatable", "", false, "Databinding", "This is a UITypeEditor that implement a listbox", true);
                Properties.Item[Properties.Item.Count - 1].ValueMember = "book_Id";
                Properties.Item[Properties.Item.Count - 1].DisplayMember = "title";
                Properties.Item[Properties.Item.Count - 1].Datasource = LookupTable;
            }

            Properties.MoveSplitterTo(120);
            Properties.Refresh();
        }

        private void FillPropertyGrid4()
        {
            DataTable LookupTable = null;
            DataRow Row = null;
            bool IsXmlSampleLoaded = false;
            document = new XmlDataDocument();

            // Load a DataTable from XML
            ParseSchema(ref document, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.xsd"));
            try
            {
                document.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.xml"));
                if (document.DataSet != null)
                {
                    LookupTable = document.DataSet.Tables[1];
                }
                if (LookupTable != null)
                {
                    IsXmlSampleLoaded = true;
                    iCountRow = LookupTable.Rows.Count;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during XML Load: " + ex.Message);
                IsXmlSampleLoaded = false;
            }

            Properties.ShowCustomProperties = true;
            Properties.Item.Clear();

            // If the XML Samples was loaded
            if (IsXmlSampleLoaded)
            {
                // Get a row for demo purposes
                Row = LookupTable.Rows[iSelectedRow];

                // Bind the row to the grid (create a property for each column)
                foreach (DataColumn Column in LookupTable.Columns)
                {
                    object oRow = Row;
                    Properties.Item.Add(Column.ColumnName.ToString(), ref oRow, Column.ColumnName.ToString(), false, "Dynamic view of a DataTable", "", true);
                }
            }

            Properties.MoveSplitterTo(120);
            Properties.Refresh();

        }

        public void ParseSchema(ref XmlDataDocument document, string schema)
        {
            StreamReader myStreamReader = null;
            try
            {
                myStreamReader = new StreamReader(schema);
                document.DataSet.ReadXmlSchema(myStreamReader);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during XSD Parsing: " + e.Message);
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                }
            }
        }

        private void Properties_PropertyValueChanged(System.Object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {

            string message;
            StatusLabel.Image = SystemIcons.Information.ToBitmap();

            switch (e.ChangedItem.PropertyDescriptor.GetType().Name)
            {
                case "CustomPropertyDescriptor":

                    CustomProperty.CustomPropertyDescriptor cpd = e.ChangedItem.PropertyDescriptor as CustomProperty.CustomPropertyDescriptor;
                    if (cpd != null)
                    {
                        CustomProperty cp = (PropertyGridEx.CustomProperty)cpd.CustomProperty;
                        if (cp == null)
                        {
                            return;
                        }
                        if (cp.Value != null)
                        {
                            message = " Value: " + cp.Value.ToString();
                            if (e.OldValue != null)
                            {
                                message = message + "; Previous: " + e.OldValue.ToString();
                            }
                            if (cp.SelectedItem != null)
                            {
                                message = message + "; SelectedItem: " + cp.SelectedItem.ToString();
                            }
                            if (cp.SelectedValue != null)
                            {
                                message = message + "; SelectedValue: " + cp.SelectedValue.ToString();
                            }
                            StatusLabel.Text = message;


                            Database db = (_document as Document).database;


                            if (_object is Polyline)
                            {
                                Polyline entity = db.GetObject(_object.id) as Polyline;

                                if (cp.Name.ToLower() == "color")
                                    entity.color =  GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();
                            }
                            else if (_object is Circle)
                            {
                                Circle entity = db.GetObject(_object.id) as Circle;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();

                                if (cp.Name.ToLower() == "radius")
                                    entity.radius = Convert.ToDouble(cp.Value.ToString());
                            }
                            else if (_object is Ellipse)
                            {
                                Ellipse entity = db.GetObject(_object.id) as Ellipse;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();

                                if (cp.Name.ToLower() == "radiusx")
                                    entity.radiusX = Convert.ToDouble(cp.Value.ToString());

                                if (cp.Name.ToLower() == "radiusy")
                                    entity.radiusY = Convert.ToDouble(cp.Value.ToString());

                            }
                            else if (_object is Arc)
                            {
                                Arc entity = db.GetObject(_object.id) as Arc;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();

                                if (cp.Name.ToLower() == "radius")
                                    entity.radius = Convert.ToDouble(cp.Value.ToString());

                                if (cp.Name.ToLower() == "startangle")
                                    entity.startAngle = Convert.ToDouble(cp.Value.ToString());

                                if (cp.Name.ToLower() == "endangle")
                                    entity.endAngle = Convert.ToDouble(cp.Value.ToString());
                            }
                            else if (_object is Line)
                            {
                                Line entity = db.GetObject(_object.id) as Line;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();

                            }
                            else if (_object is XPoint)
                            {
                                XPoint entity = db.GetObject(_object.id) as XPoint;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();
                            }
                            else if (_object is Ray)
                            {
                                Ray entity = db.GetObject(_object.id) as Ray;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();
                            }
                            else if (_object is Xline)
                            {
                                Xline entity = db.GetObject(_object.id) as Xline;

                                if (cp.Name.ToLower() == "color")
                                    entity.color = GetColorFromColorType((ColorType)cp.Value);

                                if (cp.Name.ToLower() == "lineweight")
                                    entity.lineWeight = (LineWeight)cp.Value;

                                if (cp.Name.ToLower() == "layer")
                                    entity.layer = cp.Value.ToString();
                            }
                        }
                    }
                    break;

                case "MergePropertyDescriptor":


                    message = " {MultiProperty [" + e.ChangedItem.Label + "]} " + e.ChangedItem.Value.ToString();
                    if (e.OldValue == null)
                    {
                        message = message + "; Nothing";
                    }
                    else
                    {

                        message = message + "; " + e.OldValue.ToString();
                    }
                    StatusLabel.Text = message;
                    break;

                case "ReflectPropertyDescriptor":

                    message = " {NestedProperty [" + e.ChangedItem.Label + "]} " + e.ChangedItem.Value.ToString();
                    if (e.OldValue == null)
                    {
                        message = message + "; Nothing";
                    }
                    else
                    {

                        message = message + "; " + e.OldValue.ToString();
                    }
                    StatusLabel.Text = message;



                    Database db2 = (_document as Document).database;


                    if (_object is Polyline)
                    {
                        Polyline entity = db2.GetObject(_object.id) as Polyline;

                    }
                    else if (_object is Circle)
                    {
                        Circle entity = db2.GetObject(_object.id) as Circle;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "center")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.center = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.center.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.center = new LitMath.Vector2(entity.center.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }
                    }
                    else if (_object is Ellipse)
                    {
                        Ellipse entity = db2.GetObject(_object.id) as Ellipse;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "center")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.center = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.center.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.center = new LitMath.Vector2(entity.center.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }
                    }
                    else if (_object is Arc)
                    {
                        Arc entity = db2.GetObject(_object.id) as Arc;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "center")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.center = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.center.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.center = new LitMath.Vector2(entity.center.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }
                    }
                    else if (_object is Line)
                    {
                        Line entity = db2.GetObject(_object.id) as Line;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "startpoint")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.startPoint = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.startPoint.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.startPoint = new LitMath.Vector2(entity.startPoint.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "endpoint")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.endPoint = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.endPoint.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.endPoint = new LitMath.Vector2(entity.endPoint.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }

                    }
                    else if (_object is XPoint)
                    {
                        XPoint entity = db2.GetObject(_object.id) as XPoint;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "endpoint")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.endPoint = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.endPoint.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.endPoint = new LitMath.Vector2(entity.endPoint.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }
                    }
                    else if (_object is Ray)
                    {
                        Ray entity = db2.GetObject(_object.id) as Ray;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "basePoint")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.basePoint = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.basePoint.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.basePoint = new LitMath.Vector2(entity.basePoint.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "direction")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.direction = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.direction.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.direction = new LitMath.Vector2(entity.direction.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }
                    }
                    else if (_object is Xline)
                    {
                        Xline entity = db2.GetObject(_object.id) as Xline;

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "basePoint")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.basePoint = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.basePoint.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.basePoint = new LitMath.Vector2(entity.basePoint.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }

                        if (e.ChangedItem.Parent.PropertyDescriptor.Name.ToLower() == "direction")
                        {
                            if (e.ChangedItem.Label.ToLower() == "x")
                                entity.direction = new LitMath.Vector2(Convert.ToDouble(e.ChangedItem.Value.ToString()), entity.direction.y);

                            if (e.ChangedItem.Label.ToLower() == "y")
                                entity.direction = new LitMath.Vector2(entity.direction.x, Convert.ToDouble(e.ChangedItem.Value.ToString()));
                        }
                    }


                    break;


                default:

                    StatusLabel.Image = SystemIcons.Error.ToBitmap();
                    StatusLabel.Text = " {Unknown PropertyDescriptor}";
                    break;
            }
        }

        private void SaveBooksXml_Click(System.Object sender, System.EventArgs e)
        {
            document.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.xml"));
        }

        private void SaveItems_Click(System.Object sender, System.EventArgs e)
        {
            if (ButtonSerialize.Text == UsingXmlSerializerToolStripMenuItem.Text)
            {
                string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Items.xml");
                Properties.Item.SaveXml(filename);
            }
            if (ButtonSerialize.Text == UsingBinaryFormatterToolStripMenuItem.Text)
            {
                string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Items.dat");
                Properties.Item.SaveBinary(filename);
            }
        }

    }

}
