namespace ZacCAD.Windows
{
    partial class LayerItemForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textboxName = new System.Windows.Forms.TextBox();
            this.lblLayerItemLayerName = new System.Windows.Forms.Label();
            this.lblLayerItemLayerColor = new System.Windows.Forms.Label();
            this.comboColor = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLayerItemLayerDesc = new System.Windows.Forms.Label();
            this.textboxDescription = new System.Windows.Forms.TextBox();
            this.comboLineType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textboxName
            // 
            this.textboxName.Location = new System.Drawing.Point(79, 15);
            this.textboxName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textboxName.Name = "textboxName";
            this.textboxName.Size = new System.Drawing.Size(140, 20);
            this.textboxName.TabIndex = 0;
            // 
            // lblLayerItemLayerName
            // 
            this.lblLayerItemLayerName.AutoSize = true;
            this.lblLayerItemLayerName.Location = new System.Drawing.Point(8, 19);
            this.lblLayerItemLayerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLayerItemLayerName.Name = "lblLayerItemLayerName";
            this.lblLayerItemLayerName.Size = new System.Drawing.Size(62, 13);
            this.lblLayerItemLayerName.TabIndex = 1;
            this.lblLayerItemLayerName.Text = "Layer name";
            this.lblLayerItemLayerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLayerItemLayerColor
            // 
            this.lblLayerItemLayerColor.AutoSize = true;
            this.lblLayerItemLayerColor.Location = new System.Drawing.Point(8, 79);
            this.lblLayerItemLayerColor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLayerItemLayerColor.Name = "lblLayerItemLayerColor";
            this.lblLayerItemLayerColor.Size = new System.Drawing.Size(31, 13);
            this.lblLayerItemLayerColor.TabIndex = 3;
            this.lblLayerItemLayerColor.Text = "Color";
            this.lblLayerItemLayerColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboColor
            // 
            this.comboColor.FormattingEnabled = true;
            this.comboColor.Location = new System.Drawing.Point(79, 75);
            this.comboColor.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboColor.Name = "comboColor";
            this.comboColor.Size = new System.Drawing.Size(140, 21);
            this.comboColor.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(47, 144);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(69, 29);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(126, 144);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 29);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblLayerItemLayerDesc
            // 
            this.lblLayerItemLayerDesc.AutoSize = true;
            this.lblLayerItemLayerDesc.Location = new System.Drawing.Point(8, 50);
            this.lblLayerItemLayerDesc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLayerItemLayerDesc.Name = "lblLayerItemLayerDesc";
            this.lblLayerItemLayerDesc.Size = new System.Drawing.Size(60, 13);
            this.lblLayerItemLayerDesc.TabIndex = 8;
            this.lblLayerItemLayerDesc.Text = "Description";
            this.lblLayerItemLayerDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textboxDescription
            // 
            this.textboxDescription.Location = new System.Drawing.Point(79, 46);
            this.textboxDescription.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textboxDescription.Name = "textboxDescription";
            this.textboxDescription.Size = new System.Drawing.Size(140, 20);
            this.textboxDescription.TabIndex = 7;
            // 
            // comboLineType
            // 
            this.comboLineType.FormattingEnabled = true;
            this.comboLineType.Location = new System.Drawing.Point(79, 105);
            this.comboLineType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboLineType.Name = "comboLineType";
            this.comboLineType.Size = new System.Drawing.Size(140, 21);
            this.comboLineType.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 109);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Line Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LayerItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 184);
            this.Controls.Add(this.comboLineType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLayerItemLayerDesc);
            this.Controls.Add(this.textboxDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comboColor);
            this.Controls.Add(this.lblLayerItemLayerColor);
            this.Controls.Add(this.lblLayerItemLayerName);
            this.Controls.Add(this.textboxName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayerItemForm";
            this.Text = "Layer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textboxName;
        private System.Windows.Forms.Label lblLayerItemLayerName;
        private System.Windows.Forms.Label lblLayerItemLayerColor;
        private System.Windows.Forms.ComboBox comboColor;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLayerItemLayerDesc;
        private System.Windows.Forms.TextBox textboxDescription;
        private System.Windows.Forms.ComboBox comboLineType;
        private System.Windows.Forms.Label label1;
    }
}