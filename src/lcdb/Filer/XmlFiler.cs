﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace ZacCAD.DatabaseServices.Filer
{
    /// <summary>
    /// ZacCAD XML 文件读写接口类
    /// </summary>
    public abstract class XmlFiler
    {
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="value">键值</param>
        /// <returns>
        /// 成功则返回true
        /// 失败则返回false
        /// </returns>
        public abstract bool Write(string name, string value);
        public abstract bool Write(string name, bool value);
        public abstract bool Write(string name, byte value);
        public abstract bool Write(string name, uint value);
        public abstract bool Write(string name, int value);
        public abstract bool Write(string name, double value);
        public abstract bool Write(string name, LitMath.Vector2 value);
        public abstract bool Write(string name, ZacCAD.Colors.Color color);
        public abstract bool Write(string name, ObjectId value);
        public abstract bool Write(string name, ZacCAD.DatabaseServices.LineWeight value);
        public abstract bool Write(string name, ZacCAD.DatabaseServices.LineType value);

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="name">键名称</param>
        /// <param name="value">读取的键值</param>
        /// <returns>
        /// 成功则返回true
        /// 失败则返回false
        /// </returns>
        public abstract bool Read(string name, out string value);
        public abstract bool Read(string name, out bool value);
        public abstract bool Read(string name, out byte value);
        public abstract bool Read(string name, out uint value);
        public abstract bool Read(string name, out int value);
        public abstract bool Read(string name, out double value);
        public abstract bool Read(string name, out LitMath.Vector2 value);
        public abstract bool Read(string name, out ZacCAD.Colors.Color color);
        public abstract bool Read(string name, out ObjectId value);
        public abstract bool Read(string name, out ZacCAD.DatabaseServices.LineWeight value);
        public abstract bool Read(string name, out ZacCAD.DatabaseServices.LineType value);
    }
}
