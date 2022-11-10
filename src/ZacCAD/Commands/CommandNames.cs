using System;

namespace ZacCAD.Commands
{
    internal class CommandNames
    {
        // draw
        public static string Draw_Point     = "point";
        public static string Draw_Line      = "line";
        public static string Draw_Xline     = "xline";
        public static string Draw_Ray       = "ray";
        public static string Draw_Polyline  = "polyline";
        public static string Draw_Polygon   = "polygon";
        public static string Draw_Rectangle = "rectangle";
        public static string Draw_Circle    = "circle";
        public static string Draw_Ellipse   = "ellipse";
        public static string Draw_Arc       = "arc";
        public static string Draw_Text      = "text";

        // edit
        public static string Edit_Redo = "redo";
        public static string Edit_Undo = "undo";

        // Revise
        public static string Modify_Delete = "delete";
        public static string Modify_Copy   = "copy";
        public static string Modify_Rotate = "rotate";
        public static string Modify_Mirror = "mirror";
        public static string Modify_Offset = "offset";
        public static string Modify_Move   = "move";
    }
}
