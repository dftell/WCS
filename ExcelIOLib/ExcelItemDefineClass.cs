using System;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel;
//using WolfInv.Com.JsLib;
using WolfInv.com.BaseObjectsLib;
namespace WolfInv.Com.ExcelIOLib
{
    /// <summary>
    /// 标题/数据项定义
    /// 标题中数据点/位置必选1项，优先使用位置查找。
    /// </summary>
    [Serializable]
    [DisplayName("Excel项设置")]
    //Editor(typeof(SerialObjectEdit<ExcelItemDefineClass>), typeof(UITypeEditor))]
    public class ExcelItemDefineClass: JsonableClass<ExcelItemDefineClass>
    {
        /// <summary>
        /// 是否是标题
        /// </summary>
        [DescriptionAttribute("是否是标题"),
        DisplayName("是否是标题"),
        CategoryAttribute("设置")]
        
        public bool IsTitleItem { get; set; }

        /// <summary>
        /// 数据点
        /// </summary>
        [DescriptionAttribute("对应的数据点"),
        DisplayName("数据点"),
        CategoryAttribute("设置"),
            ]
        public string PointName { get; set; }

        /// <summary>
        /// 标题名
        /// </summary>
        [DescriptionAttribute("标题名，如未提供位置，可根据此标题查找"),
        DisplayName("标题名"),
        CategoryAttribute("设置")]
        public string TitleName { get; set; }

        /// <summary>
        /// 数据点
        /// </summary>
        [DescriptionAttribute("数据点位置"),
        DisplayName("数据点位置"),
        CategoryAttribute("设置")]
        public int Position { get; set; }

        /// <summary>
        /// 数据点
        /// </summary>
        [DescriptionAttribute("数据点序号"),
        DisplayName("数据点序号"),
        CategoryAttribute("设置")]
        public int Index { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        [DescriptionAttribute("标题为数据点，数据项为本身值"),
        DisplayName("数据值"),
        CategoryAttribute("设置")]
        public string Value { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        [DescriptionAttribute("js计算规则"),
        DisplayName("计算规则"),
        CategoryAttribute("设置")]
        public string CalcRule { get; set; }

        /// <summary>
        /// 可否为空
        /// </summary>
        [DescriptionAttribute("可否为空"),
        DisplayName("可否为空"),
        CategoryAttribute("设置")]
        public bool AllowNull { get; set; }

        /// <summary>
        /// 是否为关键字
        /// </summary>
        [DescriptionAttribute("是否为关键字，关键字不能为空，遇到关键字为空"),
        DisplayName("是否为Key"),
        CategoryAttribute("设置")]
        public bool IsKey { get; set; }

        /// <summary>
        /// 跳过值
        /// </summary>
        [DescriptionAttribute("跳过值"),
        DisplayName("跳过值"),
        CategoryAttribute("设置")]
        public string SkipValue { get; set; }

        public bool Skip(Func<bool> CheckResult)
        {
            return CheckResult.Invoke();
        }

        public ExcelItemDefineClass()
        {

        }

        public ExcelItemDefineClass(bool isTitle)
        {

        }

        public ExcelItemDefineClass(XmlDocument xmldefine)
        {

        }

        

        public XmlDocument ToXml()
        {

            return null;
        }
    }
}
