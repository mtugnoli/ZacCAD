using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZacCAD.Windows.Controls;

namespace ZacCAD.Windows
{
    public partial class MainWin : Form
    {
        private ToolStripMenuItem menuTool;
        private ToolStripMenuItem menuHelp;

        private const int EM_SETCUEBANNER = 0x1501;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);


        private MainWin()
        {
            InitializeComponent();

            SetupToolStripUI();

            StatusStrip statusStrip = _statusStripMgr.GetStatusStrip();


            statusStrip.Items["toolStripStatusLabelOrtho"].Text = GlobalData.GlobalLanguage.Status_OrthoOff;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                OpenFile(args[1]);
            }
            else
            {
                NewFile();
            }

            Application.Idle += this.OnIdle;
        }

        /// <summary>
        /// 单例
        /// </summary>
        private static MainWin _instance = null;
        public static MainWin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainWin();
                }
                return _instance;
            }
        }


        private ToolStripMgr _toolStripMgr = new ToolStripMgr();
        private StatusStripMgr _statusStripMgr = new StatusStripMgr();

        public StatusStripMgr StatusStripMgr
        {
            get { return _statusStripMgr; }
        }

        /// <summary>
        /// Set the menu, toolbar, status bar
        /// </summary>
        private void SetupToolStripUI()
        {
            // main menu
            MenuStrip menuMain = SetupMainMenu();
            this.MainMenuStrip = menuMain;


            // Toolbar
            List<ToolStrip> toolStripList = SetupToolbar();

            // Top Panel
            ToolStripPanel topPanel = _toolStripMgr.GetToolStripPanle(DockStyle.Top, true);
            this.Controls.Add(topPanel);

            // Add main menu and toolbar to Top Panel
            for (int i = toolStripList.Count - 1; i >= 0; --i)
            {
                topPanel.Join(toolStripList[i]);
            }
            topPanel.Join(menuMain);

            /* STATUSBAR */


            // Bottom 
            Panel panelStrip = _statusStripMgr.GetPanel(DockStyle.Bottom, true);
            this.Controls.Add(panelStrip);

            StatusStrip statusStrip = _statusStripMgr.GetStatusStrip(DockStyle.Bottom, true);


            ToolStripDropDownButton _opencloseButton = new ToolStripDropDownButton();
            _opencloseButton.Name = "opencloseButton";
            _opencloseButton.ShowDropDownArrow = false;
            _opencloseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            _opencloseButton.Image = global::ZacCAD.Resource1.up_arrow;
            _opencloseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _opencloseButton.Size = new System.Drawing.Size(29, 20);
            _opencloseButton.Click += new System.EventHandler(OpenCloseButton_Click);

            statusStrip.Items.Add(_opencloseButton);


            ToolStripStatusLabel _coordinates = new ToolStripStatusLabel();
            _coordinates.ToolTipText = _coordinates.Text;
            _coordinates.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            _coordinates.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            _coordinates.Name = "toolStripStatusLabelInfo";
            _coordinates.Size = new System.Drawing.Size(122, 19);
            _coordinates.Text = "-";
            statusStrip.Items.Add(_coordinates);


            ToolStripStatusLabel _ortho = new ToolStripStatusLabel();
            _ortho.AutoSize = false;
            _ortho.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            _ortho.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            _ortho.Name = "toolStripStatusLabelOrtho";
            _ortho.Size = new System.Drawing.Size(118, 19);
            _ortho.Tag = "OFF";
            _ortho.Text = "-";
            _ortho.Click += new System.EventHandler(this.toolStripStatusLabelOrtho_Click);
            statusStrip.Items.Add(_ortho);

            ToolStripLabel _commandLabel = new ToolStripLabel();
            _commandLabel.Name = "toolStripLabelCommand";
            _commandLabel.AutoSize = true;
            _commandLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _commandLabel.TextAlign = ContentAlignment.MiddleCenter;
            _commandLabel.Text = "";
            _commandLabel.Tag = "CMD";
            _commandLabel.BackColor = System.Drawing.Color.White;
            _commandLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            statusStrip.Items.Add(_commandLabel);

            ToolStripStatusLabelCmds _commandInfoLabel = new ToolStripStatusLabelCmds();
            _commandInfoLabel.Name = "toolStripLabelCommandInfo";
            _commandInfoLabel.AutoSize = true;
            _commandInfoLabel.BackColor = System.Drawing.Color.White;
            _commandInfoLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            _commandInfoLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            _commandInfoLabel.TextAlign = ContentAlignment.MiddleCenter;
            _commandInfoLabel.Text = "";
            _commandInfoLabel.Tag = "CMD";
            statusStrip.Items.Add(_commandInfoLabel);

            DynamicInputToolStripTextBox statusTextBox = new DynamicInputToolStripTextBox();
            statusTextBox.BorderStyle = BorderStyle.None;
            statusTextBox.Name = "toolStripTextBoxCommands";
            statusTextBox.BackColor = System.Drawing.Color.White;
            statusTextBox.Size = new System.Drawing.Size(400, 19);
            SendMessage(statusTextBox.Control.Handle, EM_SETCUEBANNER, 0, "Digitare un comando");
            statusTextBox.keyEnterDown += this.OnEnterDown;
            statusTextBox.keyEscDown += this.OnEscDown;

            statusStrip.Items.Add(statusTextBox);

            this.Controls.Add(statusStrip);
        }

        private void OnEnterDown(object sender)
        {
            DocumentForm activeDocForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocForm == null)
            {
                return;
            }

            DynamicInputToolStripTextBox input = (DynamicInputToolStripTextBox)sender;

            Presenter presenter = activeDocForm.presenter;

            presenter.OnParameter(input.Text);

            input.Text = "";
        }

        private void OnEscDown(object sender)
        {
            DocumentForm activeDocForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocForm == null)
            {
                return;
            }

            Presenter presenter = activeDocForm.presenter;

            presenter.OnParameter(Keys.Escape.ToString());

            presenter.statusStripMgr.CommandFinish();
        }



        /// <summary>
        /// Set the main menu
        /// </summary>
        private MenuStrip SetupMainMenu()
        {
            MenuStrip menuMain = _toolStripMgr.GetMenuStrip("Main", true);

            SetupMainMenu_File(menuMain);

            return menuMain;
        }

        /// <summary>
        /// 文件菜单
        /// </summary>
        private void SetupMainMenu_File(MenuStrip menuMain)
        {
            ToolStripMenuItem menuFile = new ToolStripMenuItem();
            menuFile.Text = GlobalData.GlobalLanguage.Menu_File;
            menuMain.Items.Add(menuFile);

            // 新建
            ToolStripMenuItem _new = _toolStripMgr.NewMenuItem("file_new", GlobalData.GlobalLanguage.MenuItem_New, Resource1.file_new.ToBitmap(), this.OnFileNew);
            menuFile.DropDownItems.Add(_new);

            // 打开
            ToolStripMenuItem open = _toolStripMgr.NewMenuItem("file_open", GlobalData.GlobalLanguage.MenuItem_Open, Resource1.file_open.ToBitmap(), this.OnFileOpen);
            menuFile.DropDownItems.Add(open);

            // 保存
            ToolStripMenuItem save = _toolStripMgr.NewMenuItem("file_save", GlobalData.GlobalLanguage.MenuItem_Save, Resource1.file_save.ToBitmap(), this.OnFileSave);
            menuFile.DropDownItems.Add(save);

            // 另存为
            ToolStripMenuItem saveas = _toolStripMgr.NewMenuItem("file_saveas", GlobalData.GlobalLanguage.MenuItem_SaveAs, Resource1.file_saveas.ToBitmap(), this.OnFileSaveAs);
            menuFile.DropDownItems.Add(saveas);
        }

        /// <summary>
        /// 工具菜单
        /// </summary>
        private void SetupMainMenu_Tool(MenuStrip menuMain)
        {
            menuTool = new ToolStripMenuItem();
            menuTool.Text = GlobalData.GlobalLanguage.Menu_Tool;
            menuMain.Items.Add(menuTool);

            // 新建
            ToolStripMenuItem _new = _toolStripMgr.NewMenuItem("file_new", GlobalData.GlobalLanguage.MenuItem_New, Resource1.file_new.ToBitmap(), this.OnFileNew);
            menuTool.DropDownItems.Add(_new);
        }

        /// <summary>
        /// 工具菜单
        /// </summary>
        private void SetupMainMenu_Help(MenuStrip menuMain)
        {
            menuHelp = new ToolStripMenuItem();
            menuHelp.Text = GlobalData.GlobalLanguage.Menu_Help;
            menuMain.Items.Add(menuHelp);

            // 新建
            ToolStripMenuItem _new = _toolStripMgr.NewMenuItem("file_new", GlobalData.GlobalLanguage.MenuItem_New, Resource1.file_new.ToBitmap(), this.OnFileNew);
            menuHelp.DropDownItems.Add(_new);
        }

        /// <summary>
        /// 设置工具条
        /// </summary>
        private List<ToolStrip> SetupToolbar()
        {
            List<ToolStrip> toolStripList = new List<ToolStrip>();

            // 文件
            ToolStrip fileToolstrip = SetupToolbar_File();
            toolStripList.Add(fileToolstrip);

            // 编辑
            ToolStrip editToolstrip = SetupToolbar_Edit();
            toolStripList.Add(editToolstrip);

            // 绘制
            ToolStrip drawToolstrip = SetupToolbar_Draw();
            toolStripList.Add(drawToolstrip);

            // 修改
            ToolStrip modifyToolstrip = SetupToolbar_Modify();
            toolStripList.Add(modifyToolstrip);

            ToolStrip zoomToolstrip = SetupToolbar_Zoom();
            toolStripList.Add(zoomToolstrip);

            // 图层
            ToolStrip layerToolstrip = SetupToolbar_Layer();
            toolStripList.Add(layerToolstrip);

            // 特性
            ToolStrip propertyToolstrip = SetupToolbar_Property();
            toolStripList.Add(propertyToolstrip);

            ToolStrip linetypeToolstrip = SetupToolbar_LineType();
            toolStripList.Add(linetypeToolstrip);

            return toolStripList;
        }

        /// <summary>
        /// 工具条: 文件
        /// </summary>
        private ToolStrip SetupToolbar_File()
        {
            ToolStrip fileToolstrip = _toolStripMgr.GetToolStrip("File", true);

            // 新建
            ToolStripButton _new = _toolStripMgr.NewToolStripButton("file_new");
            _new.ToolTipText = _new.Text;
            _new.Text = "";
            fileToolstrip.Items.Add(_new);

            // 打开
            ToolStripButton open = _toolStripMgr.NewToolStripButton("file_open");
            open.ToolTipText = open.Text;
            open.Text = "";
            fileToolstrip.Items.Add(open);

            // 保存
            ToolStripButton save = _toolStripMgr.NewToolStripButton("file_save");
            save.ToolTipText = save.Text;
            save.Text = "";
            fileToolstrip.Items.Add(save);

            // 另存为
            ToolStripButton saveas = _toolStripMgr.NewToolStripButton("file_saveas");
            saveas.ToolTipText = saveas.Text;
            saveas.Text = "";
            fileToolstrip.Items.Add(saveas);


            return fileToolstrip;
        }

        /// <summary>
        /// 工具条: 编辑
        /// </summary>
        private ToolStrip SetupToolbar_Edit()
        {
            ToolStrip editToolstrip = _toolStripMgr.GetToolStrip("Edit", true);

            return editToolstrip;
        }

        /// <summary>
        /// 工具条: 绘制
        /// </summary>
        private ToolStrip SetupToolbar_Draw()
        {
            ToolStrip drawToolstrip = _toolStripMgr.GetToolStrip("Draw", true);

            return drawToolstrip;
        }

        /// <summary>
        /// 工具条: 修改
        /// </summary>
        private ToolStrip SetupToolbar_Modify()
        {
            ToolStrip modifyToolstrip = _toolStripMgr.GetToolStrip("Modify", true);

            return modifyToolstrip;
        }

        /// <summary>
        /// 工具条: 图层
        /// </summary>
        private ToolStrip SetupToolbar_Layer()
        {
            ToolStrip layerToolstrip = _toolStripMgr.GetToolStrip("Layer", true);

            return layerToolstrip;
        }

        /// <summary>
        /// 工具条: 特性
        /// </summary>
        private ToolStrip SetupToolbar_Property()
        {
            ToolStrip propertyToolstrip = _toolStripMgr.GetToolStrip("Property", true);

            return propertyToolstrip;
        }

        private ToolStrip SetupToolbar_LineType()
        {
            ToolStrip linetypeToolstrip = _toolStripMgr.GetToolStrip("LineType", true);

            return linetypeToolstrip;
        }

        private ToolStrip SetupToolbar_Zoom()
        {
            ToolStrip zoomToolstrip = _toolStripMgr.GetToolStrip("Zoom", true);

            return zoomToolstrip;
        }

        /// <summary>
        /// 新建菜单项
        /// </summary>
        private ToolStripMenuItem NewMenuItem(string text, Image image, EventHandler eventHandler)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = text;
            menuItem.Image = image;
            menuItem.Click += eventHandler;
            return menuItem;
        }

        /// <summary>
        /// 新建文件
        /// </summary>
        private void OnFileNew(object sender, EventArgs e)
        {
            NewFile();
        }

        private void NewFile()
        {
            DocumentForm docForm = new DocumentForm(_statusStripMgr);
            docForm.Text = GetNextNewFileName();
            docForm.MdiParent = this;
            docForm.WindowState = FormWindowState.Maximized;
            docForm.Show();

            removeMenu();
        }

        private string GetNextNewFileName()
        {
            string strBase = GlobalData.GlobalLanguage.Document_New;
            uint id = 1;

            foreach (Form form in this.MdiChildren)
            {
                DocumentForm docForm = form as DocumentForm;
                if (docForm == null)
                {
                    continue;
                }

                string fileName = "";
                ZacCAD.DatabaseServices.Database db = docForm.document.database;
                if (db.fileName != null)
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(db.fileName);
                }
                else
                {
                    fileName = docForm.Text;
                }
                fileName = fileName.ToLower();

                if (fileName.IndexOf(strBase) == 0)
                {
                    fileName = fileName.Substring(strBase.Length);
                    uint number = 0;
                    if (uint.TryParse(fileName, out number))
                    {
                        if (number >= id)
                        {
                            id = number + 1;
                        }
                    }
                }
            }

            return string.Format("{0}{1}", strBase, id);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        private void OnFileOpen(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = GlobalData.GlobalLanguage.MenuItem_Open;
            ofd.Filter = GlobalData.GlobalLanguage.Document_SaveAsFilter;
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string strFileFullPath = ofd.FileName;
                OpenFile(strFileFullPath);
            }
        }

        private void OpenFile(string fileFullPath)
        {
            // 检查是否已经打开
            string fileFullPathLower = fileFullPath.ToLower();
            foreach (Form form in this.MdiChildren)
            {
                DocumentForm childDocForm = form as DocumentForm;
                if (childDocForm == null)
                {
                    continue;
                }

                string strDocPath = childDocForm.fileFullPath.ToLower();
                if (fileFullPathLower == strDocPath)
                {
                    childDocForm.Activate();
                    return;
                }
            }

            // 打开文件
            DocumentForm docForm = new DocumentForm(_statusStripMgr);
            if (fileFullPath != null
                && System.IO.File.Exists(fileFullPath))
            {
                docForm.Open(fileFullPath);
            }
            docForm.MdiParent = this;
            docForm.WindowState = FormWindowState.Maximized;
            docForm.Show();

            removeMenu();
        }

        private void removeMenu()
        {
            MenuStrip menuMain = _toolStripMgr.GetMenuStrip("Main", true);
            if (menuMain.Items.Contains(menuTool))
            {
                menuMain.Items.Remove(menuTool);
            }

            if (menuMain.Items.Contains(menuHelp))
            {
                menuMain.Items.Remove(menuHelp);
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private void OnFileSave(object sender, EventArgs e)
        {
            DocumentForm activeDocForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocForm == null)
            {
                return;
            }

            ZacCAD.DatabaseServices.Database db = activeDocForm.document.database;
            if (db.fileName == null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = GlobalData.GlobalLanguage.MenuItem_Save;
                savedialog.Filter = GlobalData.GlobalLanguage.Document_SaveFilter;
                savedialog.FilterIndex = 0;
                savedialog.RestoreDirectory = true;
                savedialog.CheckPathExists = true;
                savedialog.FileName = activeDocForm.Text;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    string fileFullPath = savedialog.FileName;
                    activeDocForm.SaveAs(fileFullPath, true);
                }
            }
            else
            {
                activeDocForm.Save();
            }
        }

        /// <summary>
        /// 文件另存为
        /// </summary>
        private void OnFileSaveAs(object sender, EventArgs e)
        {
            DocumentForm activeDocForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocForm == null)
            {
                return;
            }

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = GlobalData.GlobalLanguage.MenuItem_SaveAs;
            savedialog.Filter = GlobalData.GlobalLanguage.Document_SaveAsFilter;
            savedialog.FilterIndex = 0;
            savedialog.RestoreDirectory = true;
            savedialog.CheckPathExists = true;
            savedialog.FileName = "";
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                string fileFullPath = savedialog.FileName;
                activeDocForm.SaveAs(fileFullPath);
            }
        }

        /// <summary>
        /// 子窗体激活事件
        /// </summary>
        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);

            DocumentForm activeDocForm = this.ActiveMdiChild as DocumentForm;
            foreach (Control ctrl in Controls)
            {
                if (ctrl is ToolStripPanel)
                    ((ToolStripPanel)ctrl).SuspendLayout();
            }

            if (activeDocForm != null)
            {
                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("Edit"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("Edit"), _toolStripMgr.GetToolStrip("Edit"));

                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("Draw"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("Draw"), _toolStripMgr.GetToolStrip("Draw"));

                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("Modify"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("Modify"), _toolStripMgr.GetToolStrip("Modify"));

                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("Layer"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("Layer"), _toolStripMgr.GetToolStrip("Layer"));

                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("Property"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("Property"), _toolStripMgr.GetToolStrip("Property"));

                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("LineType"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("LineType"), _toolStripMgr.GetToolStrip("LineType"));

                ToolStripManager.RevertMerge(_toolStripMgr.GetToolStrip("Zoom"));
                ToolStripManager.Merge(activeDocForm.toolstripMgr.GetToolStrip("Zoom"), _toolStripMgr.GetToolStrip("Zoom"));
            }
            else
            {
                MenuStrip menuMain = _toolStripMgr.GetMenuStrip("Main", true);
                SetupMainMenu_Tool(menuMain);
                SetupMainMenu_Help(menuMain);
                this.MainMenuStrip = menuMain;
            }


            foreach (Control ctrl in Controls)
            {
                if (ctrl is ToolStripPanel)
                    ((ToolStripPanel)ctrl).ResumeLayout();
            }
        }

        /// <summary>
        /// 空闲事件
        /// </summary>
        private void OnIdle(object sender, EventArgs e)
        {
            DocumentForm currActiveDocForm = this.ActiveMdiChild as DocumentForm;
            if (currActiveDocForm != null)
            {
                currActiveDocForm.UpdateUI();
            }
        }

        public void SetPosition(string text)
        {
            StatusStrip statusStrip = _statusStripMgr.GetStatusStrip();
            statusStrip.Items["toolStripStatusLabelInfo"].Text = text;
        }

        private void toolStripStatusLabelOrtho_Click(object sender, EventArgs e)
        {
            DocumentForm activeDocForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocForm == null)
            {
                return;
            }

            StatusStrip statusStrip = _statusStripMgr.GetStatusStrip();

            if (statusStrip.Items["toolStripStatusLabelOrtho"].Tag.ToString() == "ON")
            {
                statusStrip.Items["toolStripStatusLabelOrtho"].Text = GlobalData.GlobalLanguage.Status_OrthoOff;
                statusStrip.Items["toolStripStatusLabelOrtho"].Tag = "OFF";
                statusStrip.Items["toolStripStatusLabelOrtho"].BackColor = Color.FromArgb(255, 240, 240, 240);
                statusStrip.Items["toolStripStatusLabelOrtho"].ForeColor = Color.Black;
                activeDocForm.SetOrtho(false);
            }
            else
            {
                statusStrip.Items["toolStripStatusLabelOrtho"].Text = GlobalData.GlobalLanguage.Status_OrthoOn;
                statusStrip.Items["toolStripStatusLabelOrtho"].Tag = "ON";
                statusStrip.Items["toolStripStatusLabelOrtho"].BackColor = Color.Red;
                statusStrip.Items["toolStripStatusLabelOrtho"].ForeColor = Color.White;
                activeDocForm.SetOrtho(true);
            }
        }

        private void OpenCloseButton_Click(object sender, EventArgs e)
        {
            Panel panelStrip = _statusStripMgr.GetPanel(DockStyle.Bottom, true);

            panelStrip.Visible = !panelStrip.Visible;

            ToolStripDropDownButton button = (ToolStripDropDownButton)sender;
            button.Image = panelStrip.Visible ? global::ZacCAD.Resource1.down_arrow : global::ZacCAD.Resource1.up_arrow;
        }
    }
}
