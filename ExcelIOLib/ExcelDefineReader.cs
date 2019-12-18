using Microsoft.Office.Interop.Excel;
using System;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Script.Services;
using WolfInv.Com.JsLib;

namespace WolfInv.Com.ExcelIOLib
{
    public class ExcelDefineReader
    {
        ExcelSheetDefineClass gbdef;
        Dictionary<string, ExcelItemDefineClass> AllPoints = new Dictionary<string, ExcelItemDefineClass>();

        public ExcelDefineReader(ExcelSheetDefineClass define)
        {
            gbdef = define;
            if(define.QuickTitleList!=null&& define.QuickTitleList.Trim().Length>0)
            {
                gbdef = new ExcelSheetDefineClass(define.QuickTitleList, define.QuickTitleRefList, define.TitleBaseIndex, define.ItemsBaseIndex, define.DataDirect);
                gbdef.SheetName = define.SheetName;
            }
        }

        public ReadResult GetResult(string strFileName)
        {
            
            ReadResult ret = new ReadResult();
            XmlDocument res = new XmlDocument();
            DataSet ds = new DataSet();
            res.LoadXml("<?xml version=\"1.0\" encoding=\"UTF - 8\"?><root><MainData/><SubData/></root>");
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Application excel;// = new Application(); 
            _Workbook xBk;
            excel = new ApplicationClass();
            excel.DisplayAlerts = false;
            excel.Visible = false;
            excel.UserControl = true;
            Workbook wb = null;
            // 以只读的形式打开EXCEL文件  
            try
            {
                wb = excel.Application.Workbooks.Open(strFileName, missing, true, missing, missing, missing,
                                       missing, missing, missing, true, missing, missing, missing, missing, missing);
            }
            catch(Exception ce)
            {
                
                excel = null;
                ret.Message = ce.Message;
                return ret;
            }
            //取得第一个工作薄  
            Worksheet MainWs = null;// (Worksheet)wb.Worksheets.get_Item(1);//默认是第一个
            if(wb.Worksheets.Count == 0)
            {
                excel.Quit();
                ExcelCommLib.Kill(excel);
                excel = null;
                ret.Message = "Excel内容为空！";
                return ret;
            }
            if (gbdef.SheetName != null && gbdef.SheetName.Trim().Length > 0)
            {
                for (int i = 0; i < wb.Worksheets.Count; i++)
                {
                    Worksheet st = (Worksheet)wb.Worksheets[i + 1];
                    if (st.Name == gbdef.SheetName)
                    {
                        MainWs = st;
                        break;
                    }
                }
            }
            else
            {
                if (gbdef.SheetNameRule != null && gbdef.SheetNameRule.Trim().Length > 0)//如果指定了规则，
                {
                    Regex regTr = new Regex(gbdef.SheetNameRule);
                    int MatchCnt = 0;
                    for (int i = 0; i < wb.Worksheets.Count; i++)
                    {
                        Worksheet st = (Worksheet)wb.Worksheets[i + 1];
                        MatchCollection mcs = regTr.Matches(st.Name);
                        if (mcs.Count == 0)
                            continue;
                        MatchCnt++;
                        MainWs = st;
                        gbdef.SheetName = st.Name;
                        break;//默认第一个匹配的
                    }
                    if (MatchCnt == 0)
                    {
                        excel.Quit();
                        excel = null;
                        ret.Message = "未找到满足条件的Excel表单名";
                        return ret;
                    }

                }
                else
                {
                    MainWs = wb.Worksheets[1] as Worksheet;
                    gbdef.SheetName = MainWs.Name;
                }
            }
            XmlNode node = res.SelectSingleNode("root/MainData");
            string msg = null;
            this.AllPoints.Clear();
            if(!GetTitleInfo(MainWs, gbdef,out msg))
            {
                
                ret.Message = string.Format("{0}获取数据错误:{1}", MainWs.Name, msg);
                excel.Quit();
                ExcelCommLib.Kill(excel);
                excel = null;
                return ret;
            }
            if(!GetDataInfo(MainWs, gbdef, node,ref ret.ReData,out msg))
            {
               
                ret.Message = string.Format("{0}获取数据错误:{1}", MainWs.Name, msg);
                excel.Quit();
                ExcelCommLib.Kill(excel);
                excel = null;
                return ret;
            }
            List<string> msglist = new List<string>();
            if(msg!=null)
            {
                msglist.Add(string.Format("表格{0}提示:{1};",MainWs.Name, msg));
            }
            if(gbdef.DataType == DataMode.GroupAndDetail || gbdef.DataType == DataMode.MainAndSubList)
            {
                XmlNode subNode = res.SelectSingleNode("root/SubData");
                if (gbdef.SubSheetNameRule != null && gbdef.SubSheetNameRule.Trim().Length > 0)//如果子表名指定了规则，
                {
                    if(gbdef.SubSheetDefine == null)
                    {
                        excel.Quit();
                        ExcelCommLib.Kill(excel);
                        excel = null;
                        ret.Message = "未设置子表定义";
                        return ret;
                    }
                    Regex regTr = new Regex(gbdef.SubSheetNameRule);
                    int MatchCnt = 0;
                    for (int i = 0; i < wb.Worksheets.Count; i++)
                    {
                        Worksheet st = (Worksheet)wb.Worksheets[i+1];
                        MatchCollection mcs = regTr.Matches(st.Name);
                        if (mcs.Count == 0)
                            continue;
                        if(gbdef.SubSheetDefine.GlobalDataPoint!= null&& gbdef.SubSheetDefine.GlobalDataPoint.Trim().Length>0)
                        {
                            gbdef.SubSheetDefine.GlobalDataPointValue = st.Name;
                            gbdef.SheetName = st.Name;
                        }
                        if( !GetTitleInfo(st, gbdef.SubSheetDefine,out msg))
                        {
                            excel.Quit();
                            ExcelCommLib.Kill(excel);
                            excel = null;
                            ret.Message = string.Format("{0}获取数据错误:{1}", st.Name, msg);
                            return ret;
                        }
                        if(!GetDataInfo(st, gbdef.SubSheetDefine, node,ref ret.ReData,out msg))
                        {
                            excel.Quit();
                            ExcelCommLib.Kill(excel);
                            excel = null;
                            ret.Message = string.Format("{0}获取数据错误:{1}",st.Name,msg);
                            return ret;
                        }
                        if(msg != null)
                        {
                            msglist.Add(string.Format("表格{0}提示:{1};", st.Name, msg));
                        }
                        MatchCnt++;
                        
                        
                    }
                    if (MatchCnt == 0)
                    {
                        excel.Quit();
                        ExcelCommLib.Kill(excel);
                        excel = null;
                        ret.Message = "未找到满足匹配条件的Excel子表单名";

                        return ret;
                    }
                }
                else
                {
                    MainWs = wb.Worksheets[1] as Worksheet;
                }
            }
            if(msglist.Count==0)
            {
                ret.Message = null;
            }
            else
            {
                ret.Message = string.Join(",", msglist);
            }
            ret.Succ = true;
            
            excel.Quit();
            ExcelCommLib.Kill(excel);
            excel = null;
            return ret;
        }

        bool GetTitleInfo(Worksheet ws, ExcelSheetDefineClass def,out string msg)
        {
            msg = null;
            AllPoints.Clear();
            if (def.TitleList.Count == 0)
            {
                msg = "标题列表不能为空";
                return false;
            }
            for(int i=0;i<def.TitleList.Count;i++)
            {
                string key = def.TitleList[i].PointName;
                if(key == null || key.Trim().Length==0)
                {
                    continue;
                }
                int pos = def.TitleList[i].Position;
                string title = def.TitleList[i].TitleName;
                if(pos<=0 && (title == null || title.Trim().Length==0))//位置和标题均未定义
                {
                    continue;
                }
                if(!AllPoints.ContainsKey(key))
                {
                    AllPoints.Add(key, def.TitleList[i]);
                }
            }
            int rows = ws.UsedRange.Rows.Count;
            int cols = ws.UsedRange.Columns.Count;
            int UseRow = 0;
            int UseCol = 0;
            int UsePos = 0;
            int MaxPos = 0;
            int absMaxPos = 0;
            int TitleStart = def.TitleBaseIndex;
            UsePos = TitleStart;
            bool isVer = false;
            if (def.DataDirect == DataDirection.Vertical)//竖排
            {
                UseRow = TitleStart;
                MaxPos = cols;
                isVer = true;
               
            }
            else//横排
            {
                UseCol = TitleStart;
                MaxPos = cols;
            }
            absMaxPos = MaxPos;
            if (def.DataType == DataMode.CrossList)//如果是交叉表
            {
                MaxPos = def.CrossBaseIndex - 1;//最大值不能超过交叉点
            }
            Dictionary<string, int> AllTitles = new Dictionary<string, int>();
            for (int i = 1; i <= MaxPos; i++)//首先按位置填好所有的Title
            {
                object obj = (ws.Cells[isVer?UseRow:i, isVer?i:UseCol] as Range).Value;
                if (obj == null)
                    continue;
                string val = obj.ToString().Trim();
                if (val.Length == 0)//标题长度为0
                {
                    continue;
                }
                var vals = AllPoints.Where(a => a.Value.Position == i);
                foreach (var item in vals)
                {
                    AllPoints[item.Key].TitleName = val;
                }
                if (!AllTitles.ContainsKey(val))
                    AllTitles.Add(val, i);
            }
            var NoPosItems = AllPoints.Where(a => a.Value.Position == 0);//再处理未设置位置的标题
            foreach (var item in NoPosItems)
            {
                ExcelItemDefineClass edc = AllPoints[item.Key];
                if (edc.TitleName == null || edc.TitleName.Trim().Length == 0)
                {
                    continue;
                }
                if (AllTitles.ContainsKey(edc.TitleName))
                {
                    edc.Position = AllTitles[edc.TitleName];
                }
            }
            if (def.DataType == DataMode.CrossList)//如果是交叉表,标题增加2列
            {
                if(def.CrossItemNames.Length == 0)
                {
                    msg = "交叉表模式交叉列数据点不能为空";
                    return false;
                }
                for(int i=0;i<def.CrossItemNames.Length;i++)
                {
                    string strPoint = def.CrossItemNames[i];
                    ExcelItemDefineClass edc = new ExcelItemDefineClass(false);//增加非标题类型
                    edc.PointName = strPoint;
                    edc.Position = i;//交叉列的位置为相对位置
                    edc.TitleName = strPoint;
                    if(!AllPoints.ContainsKey(strPoint))
                    {
                        AllPoints.Add(strPoint, edc);
                    }
                }
         
            }
            if(def.TitleList.Where(a=>a.Position>0).Count()==0)
            {
                msg = "所有列均未能识别";
                return false;
            }
            return true;
        }

        bool GetDataInfo(Worksheet ws, ExcelSheetDefineClass def, XmlNode doc,ref DataSet ds, out string msg)
        {
            Dictionary<string,List<string>> SkipRec = new Dictionary<string, List<string>>();
            List<string> ErrorRec = new List<string>();
            if (ds == null)
                ds = new DataSet();
            msg = null;
            int rows = ws.UsedRange.Rows.Count;
            int cols = ws.UsedRange.Columns.Count;
            int UseRow = 0;
            int UseCol = 0;
            int UsePos = 0;
            int MaxPos = 0;
            int absMaxPos = 0;
            int DataStart = def.ItemsBaseIndex;
            UsePos = DataStart;
            bool isVer = false;
            
            if (def.DataDirect == DataDirection.Vertical)//竖排
            {
                UseRow = UsePos;
                MaxPos = rows;
                absMaxPos = cols;
                isVer = true;
                
            }
            else//横排
            {
                UseCol = UsePos;
                MaxPos = cols;
                absMaxPos = rows;
            }

            
            System.Data.DataTable dt = new System.Data.DataTable(ws.Name);
            List<DataPoint> FixPoint = new List<DataPoint>();
            List<DataPoint> DgPoint = new List<DataPoint>();
            var Allkeys = AllPoints.Where(a => a.Value.IsKey == true);
            int ncnt = 0;
            string strKeyModel = "";
            foreach(var key in Allkeys) //col1_col2_col3 
            {
                strKeyModel += string.Format("{0}[{1}]", ncnt == 0 ? "" : "_", key.Key);
                ncnt++;
            }
            
            foreach (string pt in AllPoints.Keys)
            {
                dt.Columns.Add(pt);
                ExcelItemDefineClass eidc = AllPoints[pt];
                DataPoint dp = new DataPoint();
                dp.pos = eidc.Position;
                dp.name = eidc.PointName;
                dp.expr = eidc.CalcRule;
                if(eidc.IsTitleItem)
                {
                    FixPoint.Add(dp);
                }
                else
                {
                    DgPoint.Add(dp);
                }
            }
            bool HasGlobalData = false;
            if (def.GlobalDataPoint != null && def.GlobalDataPoint.Trim().Length > 0)
            {
                HasGlobalData = true;
                dt.Columns.Add(def.GlobalDataPoint);
            }
            HashSet<string> keys = new HashSet<string>();
            for (int i=UsePos;i<=MaxPos;i++)
            {
                DataRow dr = dt.NewRow();
                bool NeedSkip = false;
                string SkipReason = null;
                string keyval = strKeyModel;
                for (int j=0;j<FixPoint.Count;j++)
                {
                    object val = null;
                    try
                    {
                        object range = ws.Cells[isVer ? i : FixPoint[j].pos, isVer ? FixPoint[j].pos : i];
                        if (range == null)
                            continue;
                        val = (range as Range).Value;
                    }
                    catch(Exception ce)
                    {
                        msg = string.Format("固定列第{0}行/列，第{1}个数据点出现错误[{2}]", i, j,ce.Message);
                        return false;
                    }
                    dr[FixPoint[j].name] = val;
                    if (val == null)
                    {
                        if(!AllPoints[FixPoint[j].name].AllowNull||AllPoints[FixPoint[j].name].IsKey)
                        {
                            NeedSkip = true;
                            SkipReason=string.Format("定义的非空字段{0}出现空值，无法导入，跳过！","");
                            continue;
                        }
                    }
                    
                    if (val!= null)
                    {
                        val = val.ToString().Trim();//去除空格
                        if (AllPoints[FixPoint[j].name].SkipValue == val.ToString().Trim())
                        {
                            NeedSkip = true;
                            SkipReason = string.Format("定义的字段{0}违反非{1}约束，无法导入，跳过！", AllPoints[FixPoint[j].name], AllPoints[FixPoint[j].name].SkipValue);
                            continue;
                        }
                    }
                    if(AllPoints[FixPoint[j].name].IsKey)
                        keyval = keyval.Replace(string.Format("[{0}]", FixPoint[j].name), val?.ToString().Trim());
                    
                    if (FixPoint[j].expr != null && FixPoint[j].expr.Trim().Length > 0)
                    {
                        string js = string.Format(FixPoint[j].expr,val);
                        dr[FixPoint[j].name] = JavaScriptClass.getEval(js);
                    }
                    else
                    {
                        dr[FixPoint[j].name] = val;
                    }
                }
                if (keyval.Trim().Length>0 && keys.Contains(keyval))
                {
                    msg = string.Format("存在重复的关键字,第{2}条记录关键字[{0}]值为[{1}]", strKeyModel, keyval, i + 1);
                    return false;
                }
                
                if (HasGlobalData)
                    dr[def.GlobalDataPoint] = def.GlobalDataPointValue;
                if (!NeedSkip)
                {
                    keys.Add(keyval);
                    dt.Rows.Add(dr);
                }
                else
                {
                    if(!SkipRec.ContainsKey(SkipReason))
                    {
                        SkipRec.Add(SkipReason, new List<string>());
                    }
                    SkipRec[SkipReason].Add(string.Join(",",dr.ItemArray));
                }
            }
            System.Data.DataTable retDt = dt.Clone();
            if (def.DataType == DataMode.CrossList)//如果是交叉表
            {
                
                //MaxPos = def.CrossBaseIndex - 1;//最大值不能超过交叉点
                
                List<int> dgPoss = new List<int>();
                //寻找开始位置
                int dgStartPos = def.CrossBaseIndex;
                List<string> AlldgTitles = new List<string>();
                while (dgStartPos <= absMaxPos)//遍历从交叉开始位置到结束的所有Title值
                {
                    
                    List<string> dgTitles = new List<string>();
                    
                    for (int k = 0; k < def.CrossDataUseUnits; k++)
                    {
                        int row = isVer ? def.ItemsBaseIndex - 1 : dgStartPos + k;
                        int col = isVer ? dgStartPos + k : def.ItemsBaseIndex - 1;
                        object val = (ws.Cells[row, col] as Range).Value;
                        if (val == null)
                        {
                            msg = "动态列未找到标题";
                            return false;
                        }
                        if(val!=null)
                            val = val.ToString().Trim();//去除空格
                        if((def.CrossSkipTitle!=null && def.CrossSkipTitle.Trim().Length>0)&&val.ToString().IndexOf(def.CrossSkipTitle)>=0)//如果找到跳过关键字
                        {
                            dgStartPos++;
                            goto wcontinue;
                        }
                        dgTitles.Add(val.ToString());
                    }
                    string strCommTitle = StringProcessClass.SearchMaxSubStr(dgTitles);//标题公共内容
                    if(strCommTitle == null)
                    {
                        msg = "无法找到公共标题";
                        return false;
                    }
                    AlldgTitles.Add(strCommTitle);
                    dgPoss.Add(dgStartPos);
                    dgStartPos += def.CrossDataUseUnits;
                    wcontinue:;
                }
                for (int j=0;j<dgPoss.Count;j++)
                {
                  
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = retDt.NewRow();
                        dr.ItemArray = dt.Rows[i].ItemArray;
                        bool NeedSkip = false;
                        string SkipReason = null;
                        for (int k = 0; k < def.CrossDataUseUnits; k++)
                        {
                            int row = isVer ? def.ItemsBaseIndex + i : dgPoss[j] + k;
                            int col = isVer ? dgPoss[j] + k : def.ItemsBaseIndex + i;
                            object val = (ws.Cells[row, col] as Range).Value;
                            string strPoint = def.CrossItemNames[k];
                            if(def.CrossIfNullSkip)
                            {
                                if (val == null)
                                {
                                    NeedSkip = true;
                                    SkipReason = "交叉值为空跳过";
                                    //continue;
                                }
                            }
                            if(def.CrossIfZeroSkip)
                            {
                                if (val!= null && val.ToString() == "0")
                                {
                                    NeedSkip = true;
                                    SkipReason = "交叉值为0跳过";
                                }
                            }
                            dr[strPoint] = val;
                        }
                        //公共标题所对应的数据点必须放在最后一个字符串。
                        dr[def.CrossItemNames[def.CrossDataUseUnits]] = AlldgTitles[j];
                        if (!NeedSkip)
                        {
                            retDt.Rows.Add(dr);
                        }
                        else
                        {
                            if (!SkipRec.ContainsKey(SkipReason))
                            {
                                SkipRec.Add(SkipReason, new List<string>());
                            }
                            SkipRec[SkipReason].Add(string.Join(",", dr.ItemArray));
                        }
                    }
                }
            }
            else
            {
                retDt = dt;
            }
            if (SkipRec.Count == 0)
            {
                msg = null;
            }
            else
            {
                msg = string.Join(";", SkipRec.Select(a => {
                    return string.Format("[{0}:{1}]", a.Key, string.Join(";", a.Value));
                }).ToArray());
            }
            ds.Tables.Add(retDt);
           
            ExcelDataTable edt = new ExcelDataTable(retDt);
            edt.ToXml(doc);
            return true;

        }

        struct DataPoint
        {
            public int pos;
            public string name;
            public string expr;
        }

    }

    public class ReadResult:WriteResult
    {
        public DataSet ReData;
        public XmlDocument Result;

    }

    public class WriteResult
    {
        public bool Succ;
        public string Message;
    }

    public class ExcelDataTable
    {
        System.Data.DataTable dt;
        public ExcelDataTable(System.Data.DataTable _dt)
        {
            dt = _dt;
        }
        public XmlNode ToXml(XmlNode Parent)
        {
            XmlNode node = Parent.OwnerDocument.CreateElement("Table");
            XmlAttribute att = Parent.OwnerDocument.CreateAttribute("Name");
            att.Value = dt.TableName;
            node.Attributes.Append(att);
            for(int i=0;i<dt.Rows.Count;i++)
            {
                XmlNode NodeRow = node.OwnerDocument.CreateElement("Row");
                for(int j=0;j<dt.Columns.Count;j++)
                {
                    XmlNode NodeCell = node.OwnerDocument.CreateElement(dt.Columns[j].ColumnName);
                    NodeCell.InnerText = dt.Rows[i][j]?.ToString();
                    NodeRow.AppendChild(NodeCell);
                }
                node.AppendChild(NodeRow);
            }
            Parent.AppendChild(node);
            return node;
        }
    }
}
