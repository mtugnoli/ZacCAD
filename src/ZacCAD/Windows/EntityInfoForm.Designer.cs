using System.Diagnostics;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;
using PropertyGridEx;

namespace ZacCAD.Windows
{
    public partial class EntityInfoForm : System.Windows.Forms.Form
    {
        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //Required by the Windows Form Designer

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityInfoForm));
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ContextMenuSaveBooks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveBooksXml = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuSerialize = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UsingXmlSerializerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UsingBinaryFormatterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonSerialize = new System.Windows.Forms.ToolStripSplitButton();
            this.ContextMenuSaveItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveItems = new System.Windows.Forms.ToolStripMenuItem();
            this.TimerFadeout = new System.Windows.Forms.Timer(this.components);
            this.TimerFadein = new System.Windows.Forms.Timer(this.components);
            this.Properties = new PropertyGridEx.PropertyGridEx();
            this.ButtonExample1 = new System.Windows.Forms.ToolStripButton();
            this.ButtonExample2 = new System.Windows.Forms.ToolStripButton();
            this.ButtonExample3 = new System.Windows.Forms.ToolStripButton();
            this.ButtonExample4 = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonPrevious = new System.Windows.Forms.ToolStripButton();
            this.ButtonNext = new System.Windows.Forms.ToolStripButton();
            this.StatusStrip1.SuspendLayout();
            this.ContextMenuSaveBooks.SuspendLayout();
            this.ContextMenuSerialize.SuspendLayout();
            this.ContextMenuSaveItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Font = new System.Drawing.Font("Tahoma", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.StatusStrip1.Location = new System.Drawing.Point(0, 576);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(368, 22);
            this.StatusStrip1.TabIndex = 1;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoToolTip = true;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ContextMenuSaveBooks
            // 
            this.ContextMenuSaveBooks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveBooksXml});
            this.ContextMenuSaveBooks.Name = "ContextMenuStrip1";
            this.ContextMenuSaveBooks.Size = new System.Drawing.Size(167, 26);
            // 
            // SaveBooksXml
            // 
            this.SaveBooksXml.Image = ((System.Drawing.Image)(resources.GetObject("SaveBooksXml.Image")));
            this.SaveBooksXml.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SaveBooksXml.Name = "SaveBooksXml";
            this.SaveBooksXml.Size = new System.Drawing.Size(166, 22);
            this.SaveBooksXml.Text = "&Save books.xml";
            this.SaveBooksXml.Click += new System.EventHandler(this.SaveBooksXml_Click);
            // 
            // ContextMenuSerialize
            // 
            this.ContextMenuSerialize.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UsingXmlSerializerToolStripMenuItem,
            this.UsingBinaryFormatterToolStripMenuItem});
            this.ContextMenuSerialize.Name = "ContextMenuStrip2";
            this.ContextMenuSerialize.OwnerItem = this.ButtonSerialize;
            this.ContextMenuSerialize.Size = new System.Drawing.Size(192, 48);
            // 
            // UsingXmlSerializerToolStripMenuItem
            // 
            this.UsingXmlSerializerToolStripMenuItem.Name = "UsingXmlSerializerToolStripMenuItem";
            this.UsingXmlSerializerToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.UsingXmlSerializerToolStripMenuItem.Text = "using XmlSerializer";
            this.UsingXmlSerializerToolStripMenuItem.Click += new System.EventHandler(this.UsingXmlSerializerToolStripMenuItem_Click);
            // 
            // UsingBinaryFormatterToolStripMenuItem
            // 
            this.UsingBinaryFormatterToolStripMenuItem.Name = "UsingBinaryFormatterToolStripMenuItem";
            this.UsingBinaryFormatterToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.UsingBinaryFormatterToolStripMenuItem.Text = "using BinaryFormatter";
            this.UsingBinaryFormatterToolStripMenuItem.Click += new System.EventHandler(this.UsingBinaryFormatterToolStripMenuItem_Click);
            // 
            // ButtonSerialize
            // 
            this.ButtonSerialize.AutoToolTip = false;
            this.ButtonSerialize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSerialize.DropDown = this.ContextMenuSerialize;
            this.ButtonSerialize.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSerialize.Image")));
            this.ButtonSerialize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSerialize.Name = "ButtonSerialize";
            this.ButtonSerialize.Size = new System.Drawing.Size(32, 22);
            this.ButtonSerialize.Text = "Load Items";
            this.ButtonSerialize.ToolTipText = "Load Items...";
            this.ButtonSerialize.Click += new System.EventHandler(this.ButtonSerialize_Click);
            // 
            // ContextMenuSaveItems
            // 
            this.ContextMenuSaveItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveItems});
            this.ContextMenuSaveItems.Name = "ContextMenuStrip2";
            this.ContextMenuSaveItems.Size = new System.Drawing.Size(140, 26);
            // 
            // SaveItems
            // 
            this.SaveItems.AutoToolTip = true;
            this.SaveItems.Image = ((System.Drawing.Image)(resources.GetObject("SaveItems.Image")));
            this.SaveItems.Name = "SaveItems";
            this.SaveItems.Size = new System.Drawing.Size(139, 22);
            this.SaveItems.Text = "&Save Items...";
            this.SaveItems.Click += new System.EventHandler(this.SaveItems_Click);
            // 
            // Properties
            // 
            this.Properties.AutoSizeProperties = true;
            // 
            // 
            // 
            this.Properties.DocCommentDescription.AccessibleName = "";
            this.Properties.DocCommentDescription.AutoEllipsis = true;
            this.Properties.DocCommentDescription.BackColor = System.Drawing.Color.Transparent;
            this.Properties.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.Properties.DocCommentDescription.ForeColor = System.Drawing.Color.White;
            this.Properties.DocCommentDescription.Location = new System.Drawing.Point(3, 19);
            this.Properties.DocCommentDescription.Name = "";
            this.Properties.DocCommentDescription.Size = new System.Drawing.Size(362, 36);
            this.Properties.DocCommentDescription.TabIndex = 1;
            this.Properties.DocCommentImage = ((System.Drawing.Image)(resources.GetObject("Properties.DocCommentImage")));
            // 
            // 
            // 
            this.Properties.DocCommentTitle.AutoSize = true;
            this.Properties.DocCommentTitle.BackColor = System.Drawing.Color.Transparent;
            this.Properties.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.Properties.DocCommentTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Properties.DocCommentTitle.ForeColor = System.Drawing.Color.White;
            this.Properties.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.Properties.DocCommentTitle.Name = "";
            this.Properties.DocCommentTitle.Size = new System.Drawing.Size(0, 13);
            this.Properties.DocCommentTitle.TabIndex = 0;
            this.Properties.DocCommentTitle.UseMnemonic = false;
            this.Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Properties.Location = new System.Drawing.Point(0, 0);
            this.Properties.Name = "Properties";
            this.Properties.SelectedObject = this.Properties.Item;
            this.Properties.ShowCustomProperties = true;
            this.Properties.Size = new System.Drawing.Size(368, 576);
            this.Properties.TabIndex = 2;
            // 
            // 
            // 
            this.Properties.ToolStrip.AccessibleName = "ToolBar";
            this.Properties.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.Properties.ToolStrip.AllowMerge = false;
            this.Properties.ToolStrip.AutoSize = false;
            this.Properties.ToolStrip.CanOverflow = false;
            this.Properties.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.Properties.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Properties.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonExample1,
            this.ButtonExample2,
            this.ButtonExample3,
            this.ButtonSerialize,
            this.ButtonExample4,
            this.ToolStripSeparator1,
            this.ButtonPrevious,
            this.ButtonNext});
            this.Properties.ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.Properties.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.Properties.ToolStrip.Name = "";
            this.Properties.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.Properties.ToolStrip.Size = new System.Drawing.Size(368, 25);
            this.Properties.ToolStrip.TabIndex = 1;
            this.Properties.ToolStrip.TabStop = true;
            this.Properties.ToolStrip.Text = "PropertyGridToolBar";
            this.Properties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Properties_PropertyValueChanged);
            // 
            // ButtonExample1
            // 
            this.ButtonExample1.CheckOnClick = true;
            this.ButtonExample1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonExample1.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExample1.Image")));
            this.ButtonExample1.ImageTransparentColor = System.Drawing.Color.Black;
            this.ButtonExample1.Name = "ButtonExample1";
            this.ButtonExample1.Size = new System.Drawing.Size(23, 22);
            this.ButtonExample1.Text = "Single properties";
            this.ButtonExample1.Click += new System.EventHandler(this.ButtonExample1_Click);
            // 
            // ButtonExample2
            // 
            this.ButtonExample2.CheckOnClick = true;
            this.ButtonExample2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonExample2.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExample2.Image")));
            this.ButtonExample2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExample2.Name = "ButtonExample2";
            this.ButtonExample2.Size = new System.Drawing.Size(23, 22);
            this.ButtonExample2.Text = "Multiple properties";
            this.ButtonExample2.Click += new System.EventHandler(this.ButtonExample2_Click);
            // 
            // ButtonExample3
            // 
            this.ButtonExample3.CheckOnClick = true;
            this.ButtonExample3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonExample3.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExample3.Image")));
            this.ButtonExample3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExample3.Name = "ButtonExample3";
            this.ButtonExample3.Size = new System.Drawing.Size(23, 22);
            this.ButtonExample3.Text = "Databinding properties";
            this.ButtonExample3.Click += new System.EventHandler(this.ButtonExample3_Click);
            // 
            // ButtonExample4
            // 
            this.ButtonExample4.CheckOnClick = true;
            this.ButtonExample4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonExample4.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExample4.Image")));
            this.ButtonExample4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonExample4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExample4.Name = "ButtonExample4";
            this.ButtonExample4.Size = new System.Drawing.Size(30, 22);
            this.ButtonExample4.Text = "Datatable sample";
            this.ButtonExample4.Click += new System.EventHandler(this.ButtonExample4_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.ToolStripSeparator1.Visible = false;
            // 
            // ButtonPrevious
            // 
            this.ButtonPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonPrevious.Enabled = false;
            this.ButtonPrevious.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPrevious.Image")));
            this.ButtonPrevious.ImageTransparentColor = System.Drawing.Color.White;
            this.ButtonPrevious.Name = "ButtonPrevious";
            this.ButtonPrevious.Size = new System.Drawing.Size(23, 22);
            this.ButtonPrevious.Text = "Previous";
            this.ButtonPrevious.Visible = false;
            this.ButtonPrevious.Click += new System.EventHandler(this.ButtonPrevious_Click);
            // 
            // ButtonNext
            // 
            this.ButtonNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonNext.Image = ((System.Drawing.Image)(resources.GetObject("ButtonNext.Image")));
            this.ButtonNext.ImageTransparentColor = System.Drawing.Color.White;
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.Size = new System.Drawing.Size(23, 22);
            this.ButtonNext.Text = "Next";
            this.ButtonNext.Visible = false;
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // EntityInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 598);
            this.Controls.Add(this.Properties);
            this.Controls.Add(this.StatusStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EntityInfoForm";
            this.Text = "Entity Properties";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.ContextMenuSaveBooks.ResumeLayout(false);
            this.ContextMenuSerialize.ResumeLayout(false);
            this.ContextMenuSaveItems.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.ToolStripButton ButtonExample1;
        internal System.Windows.Forms.ToolStripButton ButtonExample2;
        internal System.Windows.Forms.ToolStripButton ButtonExample3;
        internal System.Windows.Forms.ToolStripButton ButtonNext;
        internal System.Windows.Forms.ToolStripButton ButtonPrevious;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        internal PropertyGridEx.PropertyGridEx Properties;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuSaveBooks;
        internal System.Windows.Forms.ToolStripMenuItem SaveBooksXml;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton ButtonExample4;
        internal System.Windows.Forms.ToolStripSplitButton ButtonSerialize;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuSerialize;
        internal System.Windows.Forms.ToolStripMenuItem UsingXmlSerializerToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem UsingBinaryFormatterToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuSaveItems;
        internal System.Windows.Forms.ToolStripMenuItem SaveItems;
        internal System.Windows.Forms.Timer TimerFadeout;
        internal System.Windows.Forms.Timer TimerFadein;
        private System.ComponentModel.IContainer components;
    }
}
