using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.DataCenter
{

    public class DataTableRef
    {
        public string Table;
        public string RefTable;
        public DataColumn Column;
        public DataColumn RefColumn;
        public DataTableRef Next;
        public DataTableRef Prov;
    }
    public class MappingConvert
    {
        public  MappingConvert()
        {
            
        }

        public static DataColumn DataPointToColumn(DataPoint data)
        {
            if (DataAccessCenter.DataPointMappings.ContainsKey(data.Name) && DataAccessCenter.DataColumnMappings.ContainsKey(data.Name))
            {
                return DataAccessCenter.DataColumnMappings[data.Name];
            }
            return null;
        }

        public static DataTableRef GetTableRef(string table, string RefTable)
        {
            if (!DataAccessCenter.DataTableRefs.ContainsKey(table)) return null;
            DataTableReference dtref = DataAccessCenter.DataTableRefs[table];
            for (int i = 0; i < dtref.References.Count; i++)
            {
                DataTableRef dtr = new DataTableRef();
                dtr.Table = table;
                dtr.RefTable = dtref.References[i].MainColumn.Table;
                dtr.Column = dtref.References[i].LientColumn;
                dtr.RefColumn = dtref.References[i].MainColumn;
                if (dtref.References[i].MainColumn.Table == RefTable)
                {

                    return dtr;
                }
                else
                {
                    dtr.Next = GetTableRef(dtr.RefColumn.Table, RefTable);
                    if (dtr.Next != null)
                    {
                        dtr.Next.Prov = dtr;
                        return dtr;
                    }
                }
            }
            return null;
            

            
        }
    }
    /// <summary>
    /// 视图数据处理类
    /// </summary>
    public class ViewDataPointProcess
    {
        DataRequest datareq;
        public ViewDataPointProcess(DataRequest req)
        {
            datareq = req;
        }
        Dictionary<string, List<DataPoint>> ViewDatapoints;
        //public Dictionary<string, List<DataPoint>> ViewDatapoints;
        public XmlNode GetDataPoint()
        {
            if (datareq == null) return null;
            Dictionary<string, List<DataPoint>> ret = new Dictionary<string, List<DataPoint>>();
            for (int i = 0; i < datareq.RequestItems.Count; i++)
            {
                DataColumn dc = MappingConvert.DataPointToColumn(datareq.RequestItems[i].DataPt);
                if (dc != null)
                {
                    if (ret.ContainsKey(dc.Table))
                    {
                        continue;
                    }
                    if(!DataAccessCenter.DataTabelMappings.ContainsKey(dc.Table))//如果表映射无此表，跳过
                    {
                        continue ;
                    }
                    List<DataColumn> cols = DataAccessCenter.DataTabelMappings[dc.Table];
                    List<DataPoint> dpts = new List<DataPoint>();
                    for (int c = 0; c < cols.Count; c++)
                    {
                        dpts.Add(new DataPoint( cols[c].DataPoint));
                    }
                    ret.Add(dc.Table, dpts);
                }
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<data/>");
            XmlNode root = xmldoc.SelectSingleNode("data");
            foreach(string strtab in ret.Keys)
            {
                XmlNode tabnode = XmlUtil.AddSubNode(root, "tab", true);
                XmlUtil.AddAttribute(tabnode, "name", strtab);
                List<DataPoint> dpts = ret[strtab];
                for (int i = 0; i < dpts.Count; i++)
                {
                    dpts[i].ToXml(tabnode);
                }
            }
            return root;
        }
    
        
    }
}
