using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web.Script.Serialization;
using System.ComponentModel;
using System;
using WolfInv.Com.JsLib;

namespace WolfInv.Com.ExcelIOLib
{
    public class ExcelSheetDefineClass:JsonableClass<ExcelSheetDefineClass>
    {
        #region 全局设置
        [DescriptionAttribute("满足正则表达式的主表"),
        DisplayName("主表名规则"),
        CategoryAttribute("全局设置")]
        public string SheetNameRule { get; set; }

        [DescriptionAttribute("表名"),
        DisplayName("表名"),
        CategoryAttribute("全局设置")]
        public string SheetName { get; set; }

        [DescriptionAttribute("数据类型"),
        DisplayName("数据类型"),
        CategoryAttribute("全局设置")]
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataMode DataType { get; set; }

        [DescriptionAttribute("数据排列方向"),
        DisplayName("数据排列方向"),
        CategoryAttribute("全局设置")]
        /// <summary>
        /// 数据排列方向
        /// </summary>
        public DataDirection DataDirect { get; set; }

        [DescriptionAttribute("全局数据点"),
        DisplayName("全局数据点"),
        CategoryAttribute("全局设置")]
        /// <summary>
        /// 全局数据点
        /// </summary>
        public string GlobalDataPoint { get; set; }

        [DescriptionAttribute("全局数据点值"),
        DisplayName("全局数据点值"),
        CategoryAttribute("全局设置")]
        /// <summary>
        /// 全局数据点值
        /// </summary>
        public string GlobalDataPointValue { get; set; }
        #endregion

        #region 子表设置
        [DescriptionAttribute("满足正则表达式的主表"),
        DisplayName("子表名规则"),
        CategoryAttribute("子表设置")]
        public string SubSheetNameRule { get; set; }
        /// <summary>
        /// 子表名列表
        /// 在出从或者汇总，明细表中使用
        /// </summary>
        [DescriptionAttribute("子表列表"),
        DisplayName("子表列表"),
        CategoryAttribute("子表设置")]
        public List<string> SubSheetName { get; set; }

        /// <summary>
        /// 子表设置
        /// </summary>
        [DescriptionAttribute("子表定义"),
        DisplayName("子表定义"),
        CategoryAttribute("子表设置")]
        public ExcelSheetDefineClass SubSheetDefine { get; set; }
        #endregion

        #region 标题设置
        [DescriptionAttribute("标题开始位置索引"),
        DisplayName("标题开始位置索引"),
        CategoryAttribute("标题设置")]
        /// <summary>
        /// 标题开始位置索引
        /// </summary>
        public int TitleBaseIndex { get; set; }

        [DescriptionAttribute("标题占用单位数"),
        DisplayName("标题占用单位数"),
        CategoryAttribute("标题设置")]
        /// <summary>
        /// 标题占用单位数
        /// </summary>
        public int TitleUseUnits { get; set; }


        /// <summary>
        /// 标题项定义列表
        /// </summary>
        [DescriptionAttribute("快速标题项"),
        DisplayName("快速标题项"),
        CategoryAttribute("标题设置")]
        public string QuickTitleList { get; set; }

        /// <summary>
        /// 标题项定义列表
        /// </summary>
        [DescriptionAttribute("快速标题对应数据项"),
        DisplayName("快速标题对应数据项"),
        CategoryAttribute("标题设置")]
        public string QuickTitleRefList { get; set; }






        /// <summary>
        /// 标题项定义列表
        /// </summary>
        [DescriptionAttribute("标题项定义列表"),
        DisplayName("标题项定义列表"),
        CategoryAttribute("标题设置")]
        public List<ExcelItemDefineClass> TitleList { get; set; }
        #endregion

        #region 数据项设置
        [DescriptionAttribute("数据项开始位置"),
        DisplayName("数据项开始位置"),
        CategoryAttribute("数据项设置")]
        /// <summary>
        /// 数据项开始位置
        /// </summary>
        public int ItemsBaseIndex { get; set; }

        [DescriptionAttribute("单项记录使用行/列数量"),
        DisplayName("单项记录使用单位数量"),
        CategoryAttribute("数据项设置")]
        /// <summary>
        /// 单项记录使用行/列数量
        /// </summary>
        public int ItemUseUnits { get; set; }
        

        #endregion

        #region 交叉表设置
        [DescriptionAttribute("如果是交叉表，交叉数据开始位置"),
        DisplayName("交叉开始位置"),
        CategoryAttribute("交叉数据项设置")]
        /// <summary>
        /// 交叉开始位置
        /// </summary>
        public int CrossBaseIndex { get; set; }

        /// <summary>
        /// 交叉数据项占用单元数
        /// </summary>
        [DescriptionAttribute("如果是交叉表，交叉数据项占用单元数"),
        DisplayName("交叉数据项占用单元数"),
        CategoryAttribute("交叉数据项设置")]
        public int CrossDataUseUnits { get; set; }


        /// <summary>
        /// 交叉数据名称数组
        /// </summary>
        [DescriptionAttribute("如果是交叉表，交叉数据名称数组"),
        DisplayName("交叉数据名称数组"),
        CategoryAttribute("交叉数据项设置")]
        public string[] CrossItemNames { get; set; }

        /// <summary>
        /// 标题跳过关键字
        /// </summary>
        [DescriptionAttribute("交叉表标题跳过关键字"),
        DisplayName("交叉表标题跳过关键字"),
        CategoryAttribute("交叉数据项设置")]
        public string CrossSkipTitle { get; set; }

        /// <summary>
        /// 交叉值为空是否跳过
        /// </summary>
        [DescriptionAttribute("交叉值为空是否跳过"),
        DisplayName("交叉值为空是否跳过"),
        CategoryAttribute("交叉值为空是否跳过")]
        public bool CrossIfNullSkip { get; set; }
        #endregion




        /// <summary>
        /// 
        /// </summary>
        public ExcelSheetDefineClass()
        {
            Init();
        }

        public ExcelSheetDefineClass(DataMode type)
        {
            DataType = type;
            Init();
            ////if(type == DataMode.CrossList)
            ////{
            ////    CrossNames = new List<string>();
            ////}
            if(type == DataMode.GroupAndDetail || type == DataMode.MainAndSubList)
            {
                SubSheetDefine = new ExcelSheetDefineClass();
                SubSheetName = new List<string>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">默认数据竖排</param>
        /// <param name="Titles">例子：id,data1,data2</param>
        /// <param name="Datas">例子：outid,outdata1,outdata2</param>
        /// <param name="TitleStartPos">默认1</param>
        /// <param name="DataStartPos">默认2</param>
        public ExcelSheetDefineClass(string Titles,string Datas,int TitleStartPos = 1,int DataStartPos = 2, DataDirection dir=DataDirection.Vertical)
        {
            this.DataDirect = dir;
            this.DataType = DataMode.List;
            this.TitleList = new List<ExcelItemDefineClass>();
            this.TitleBaseIndex = TitleStartPos;
            this.ItemsBaseIndex = DataStartPos;
            string[] TitleArr = Titles.Split(',');
            string[] DataArr = Datas.Split(',');
            int cols = Math.Min(TitleArr.Length, DataArr.Length);
            for(int i=0;i<cols;i++)
            {
                ExcelItemDefineClass eidc = new ExcelItemDefineClass();
                eidc.IsTitleItem = true;
                eidc.TitleName = TitleArr[i];
                string strItem = DataArr[i];
                string[] subitms = strItem.Split(':');

                eidc.PointName = subitms[0];
                eidc.IsTitleItem = true;

                eidc.AllowNull = true;
                
                eidc.IsKey = false;
                if (subitms.Length > 1)
                {
                    if(subitms[1].ToLower().IndexOf("allownull=0")>=0)
                    {
                        eidc.AllowNull = false;
                    }
                    if (subitms[1].ToLower().IndexOf("iskey=1") >= 0)
                    {
                        eidc.IsKey = true;
                    }
                }
                this.TitleList.Add(eidc);
            }
        }
        void Init()
        {
            TitleList = new List<ExcelItemDefineClass>();
            //ItemList = new List<ExcelItemDefineClass>();
            //SubSheetDefine = new List<ExcelSheetDefineClass>();
        }

        public ExcelSheetDefineClass(XmlDocument xmldefine)
        {
            Init();
        }

        

        public XmlDocument ToXml()
        {
            return null;
        }
    }

    





    /// <summary>
    /// Excel 数据模式
    /// </summary>
    public enum DataMode
    {
        /// <summary>
        /// 单sheet列表模式
        /// </summary>
        List,
        /// <summary>
        /// 多sheet主从模式
        /// </summary>
        MainAndSubList,
        /// <summary>
        /// 汇总多Sheet明细模式
        /// </summary>
        GroupAndDetail,
        /// <summary>
        /// 混合模式
        /// </summary>
        CrossList

    }
    /// <summary>
    /// 数据列表排列方向
    /// </summary>
    public enum DataDirection
    {
        /// <summary>
        /// 竖排
        /// </summary>
        Vertical,
        /// <summary>
        /// 横排
        /// </summary>
        Horizontal
    }

    
}
