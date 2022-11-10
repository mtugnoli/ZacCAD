using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ZacCAD
{
    public class Language
    {
        #region 界面UI字段
        public string Menu_File = "";
        public string MenuItem_New = "";
        public string MenuItem_Open = "";
        public string MenuItem_Save = "";
        public string MenuItem_SaveAs = "";

        public string Menu_Edit = "";
        public string MenuItem_Undo = "";
        public string MenuItem_Redo = "";

        public string Menu_Format = "";
        public string MenuItem_Layer = "";

        public string Menu_Draw = "";
        public string MenuItem_Point = "";
        public string MenuItem_Line = "";
        public string MenuItem_Ray = "";
        public string MenuItem_XLine = "";
        public string MenuItem_Polyline = "";
        public string MenuItem_Polygon = "";
        public string MenuItem_Rectangle = "";
        public string MenuItem_Circle = "";
        public string MenuItem_Ellipse = "";
        public string MenuItem_Arc = "";
        public string MenuItem_Text = "";

        public string Menu_Modify = "";
        public string MenuItem_Erase = "";
        public string MenuItem_Copy = "";
        public string MenuItem_Mirror = "";
        public string MenuItem_Offset = "";
        public string MenuItem_Move = "";
        public string MenuItem_Rotate = "";
        public string MenuItem_Info = "";

        public string Document_New = "";
        public string Document_LayerManger = "";
        public string Document_LayerName = "";
        public string Document_LayerDesc = "";
        public string Document_LayerColor = "";
        public string Document_LayerLineType = "";
        public string Document_LayerLock = "";
        public string Document_LayerItemTitle = "";
        public string Document_LayerItemLayerName = "";
        public string Document_LayerItemLayerDesc = "";
        public string Document_LayerItemLayerColor = "";
        public string Document_SaveFilter = "";
        public string Document_SaveAsFilter = "";

        public string Color_Red = "";
        public string Color_Yellow = "";
        public string Color_Green = "";
        public string Color_Cyan = "";
        public string Color_Blue = "";
        public string Color_Magenta = "";
        public string Color_White = "";
        public string Color_Choose = "";
        public string Color_ByLayer = "";
        public string Color_ByBlock = "";
        public string Color_None = "";

        public string LineType_ByLayer = "";
        public string LineType_ByBlock = "";
        public string LineType_Solid = "";
        public string LineType_Dash = "";
        public string LineType_Dot = "";
        public string LineType_DashDot = "";
        public string LineType_DashDotDot = "";
        public string LineType_Custom = "";

        public string Button_Modify = "";
        public string Button_Add = "";
        public string Button_Delete = "";
        public string Button_Ok = "";
        public string Button_Cancel = "";

        public string Menu_Tool = "";
        public string Menu_Help = "";

        public string Menu_Zoom = "";
        public string MenuZoom_Plus = "";
        public string MenuZoom_Minus = "";
        public string MenuZoom_Extend = "";

        public string Command_PointCenter = "";
        public string Command_PointFirst = "";
        public string Command_PointSecond = "";
        public string Command_PointNext = "";
        public string Command_PointSpecified = "";
        public string Command_PointPass = "";
        public string Command_PointAxisX = "";
        public string Command_PointAxisY = "";
        public string Command_PointStart = "";
        public string Command_ValueRadius = "";
        public string Command_ValueAngle = "";
        public string Command_ValueText = "";
        public string Command_ValueEdges = "";
        public string Command_ValuePolygonOptions = "";
        public string Command_PointPolilyneOptions = "";
        public string Command_ValueOffsetDist = "";
        public string Command_SelectObjects = "";
        public string Command_Command = "";
        public string Command_Cancel = "";

        public string Status_OrthoOn = "";
        public string Status_OrthoOff = "";

        #endregion


        protected Dictionary<string, string> DicLanguage = new Dictionary<string, string>();
        public Language()
        {
            XmlLoad(GlobalData.SystemLanguage);
            BindLanguageText();
        }

        /// <summary>
        /// Read XML and put it in memory
        /// </summary>
        /// <param name="language"></param>
        protected void XmlLoad(string language)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string address = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Languages\" + language + ".xml");
                doc.Load(address);
                XmlElement root = doc.DocumentElement;

                XmlNodeList nodeLst1 = root.ChildNodes;
                foreach (XmlNode item in nodeLst1)
                {
                    DicLanguage.Add(item.Name, item.InnerText);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        public void BindLanguageText()
        {
            Menu_File = DicLanguage["Menu_File"];
            MenuItem_New = DicLanguage["MenuItem_New"];
            MenuItem_Open = DicLanguage["MenuItem_Open"];
            MenuItem_Save = DicLanguage["MenuItem_Save"];
            MenuItem_SaveAs = DicLanguage["MenuItem_SaveAs"];

            Menu_Edit = DicLanguage["Menu_Edit"];
            MenuItem_Undo = DicLanguage["MenuItem_Undo"];
            MenuItem_Redo = DicLanguage["MenuItem_Redo"];

            Menu_Format = DicLanguage["Menu_Format"];
            MenuItem_Layer = DicLanguage["MenuItem_Layer"];

            Menu_Draw = DicLanguage["Menu_Draw"];
            MenuItem_Point = DicLanguage["MenuItem_Point"];
            MenuItem_Line = DicLanguage["MenuItem_Line"];
            MenuItem_Ray = DicLanguage["MenuItem_Ray"];
            MenuItem_XLine = DicLanguage["MenuItem_XLine"];
            MenuItem_Polyline = DicLanguage["MenuItem_Polyline"];
            MenuItem_Polygon = DicLanguage["MenuItem_Polygon"];
            MenuItem_Rectangle = DicLanguage["MenuItem_Rectangle"];
            MenuItem_Circle = DicLanguage["MenuItem_Circle"];
            MenuItem_Ellipse = DicLanguage["MenuItem_Ellipse"];
            MenuItem_Arc = DicLanguage["MenuItem_Arc"];
            MenuItem_Text = DicLanguage["MenuItem_Text"];

            Menu_Modify = DicLanguage["Menu_Modify"];
            MenuItem_Erase = DicLanguage["MenuItem_Erase"];
            MenuItem_Copy = DicLanguage["MenuItem_Copy"];
            MenuItem_Mirror = DicLanguage["MenuItem_Mirror"];
            MenuItem_Offset = DicLanguage["MenuItem_Offset"];
            MenuItem_Move = DicLanguage["MenuItem_Move"];
            MenuItem_Rotate = DicLanguage["MenuItem_Rotate"];
            MenuItem_Info = DicLanguage["MenuItem_Info"];

            Document_New = DicLanguage["Document_New"];
            Document_LayerManger = DicLanguage["Document_LayerManger"];
            Document_LayerName = DicLanguage["Document_LayerName"];
            Document_LayerDesc = DicLanguage["Document_LayerDesc"];
            Document_LayerColor = DicLanguage["Document_LayerColor"];
            Document_LayerLineType = DicLanguage["Document_LayerLineType"];
            Document_LayerLock = DicLanguage["Document_LayerLock"];
            Document_LayerItemTitle = DicLanguage["Document_LayerItemTitle"];
            Document_LayerItemLayerName = DicLanguage["Document_LayerItemLayerName"];
            Document_LayerItemLayerDesc = DicLanguage["Document_LayerItemLayerDesc"];
            Document_LayerItemLayerColor = DicLanguage["Document_LayerItemLayerColor"];
            Document_SaveFilter = DicLanguage["Document_SaveFilter"];
            Document_SaveAsFilter = DicLanguage["Document_SaveAsFilter"];

            Color_Red = DicLanguage["Color_Red"];
            Color_Yellow = DicLanguage["Color_Yellow"];
            Color_Green = DicLanguage["Color_Green"];
            Color_Cyan = DicLanguage["Color_Cyan"];
            Color_Blue = DicLanguage["Color_Blue"];
            Color_Magenta = DicLanguage["Color_Magenta"];
            Color_White = DicLanguage["Color_White"];
            Color_Choose = DicLanguage["Color_Choose"];
            Color_ByBlock = DicLanguage["Color_ByBlock"];
            Color_ByLayer = DicLanguage["Color_ByLayer"];
            Color_None = DicLanguage["Color_None"];

            LineType_ByLayer = DicLanguage["LineType_ByLayer"];
            LineType_ByBlock = DicLanguage["LineType_ByBlock"];
            LineType_Solid = DicLanguage["LineType_Solid"];
            LineType_Dash = DicLanguage["LineType_Dash"];
            LineType_Dot = DicLanguage["LineType_Dot"];
            LineType_DashDot = DicLanguage["LineType_DashDot"];
            LineType_DashDotDot = DicLanguage["LineType_DashDotDot"];
            LineType_Custom = DicLanguage["LineType_Custom"];

            Button_Modify = DicLanguage["Button_Modify"];
            Button_Add = DicLanguage["Button_Add"];
            Button_Delete = DicLanguage["Button_Delete"];
            Button_Ok = DicLanguage["Button_Ok"];
            Button_Cancel = DicLanguage["Button_Cancel"];

            Menu_Tool = DicLanguage["Menu_Tool"];
            Menu_Help = DicLanguage["Menu_Help"];

            Menu_Zoom = DicLanguage["Menu_Zoom"];
            MenuZoom_Plus = DicLanguage["MenuZoom_Plus"];
            MenuZoom_Minus = DicLanguage["MenuZoom_Minus"];
            MenuZoom_Extend = DicLanguage["MenuZoom_Extend"];

            Command_PointCenter = DicLanguage["Command_PointCenter"];
            Command_PointFirst = DicLanguage["Command_PointFirst"];
            Command_PointSecond = DicLanguage["Command_PointSecond"];
            Command_PointNext = DicLanguage["Command_PointNext"];
            Command_PointSpecified = DicLanguage["Command_PointSpecified"];
            Command_PointAxisX = DicLanguage["Command_PointAxisX"];
            Command_PointAxisY = DicLanguage["Command_PointAxisY"];
            Command_PointStart = DicLanguage["Command_PointStart"];
            Command_PointPass = DicLanguage["Command_PointPass"];
            Command_ValueRadius = DicLanguage["Command_ValueRadius"];
            Command_ValueAngle = DicLanguage["Command_ValueAngle"];
            Command_ValueText = DicLanguage["Command_ValueText"];
            Command_ValueEdges = DicLanguage["Command_ValueEdges"];
            Command_ValuePolygonOptions = DicLanguage["Command_ValuePolygonOptions"];
            Command_PointPolilyneOptions = DicLanguage["Command_PointPolilyneOptions"];
            Command_ValueOffsetDist = DicLanguage["Command_ValueOffsetDist"];
            Command_SelectObjects = DicLanguage["Command_SelectObjects"];
            Command_Command = DicLanguage["Command_Command"];
            Command_Cancel = DicLanguage["Command_Cancel"];

            Status_OrthoOn = DicLanguage["Status_OrthoOn"];
            Status_OrthoOff = DicLanguage["Status_OrthoOff"];
        }
    }
}
