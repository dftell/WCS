using System.Collections.Generic;
namespace WolfInv.Com.DataCenter
{
    public class DataTableReference
    {
        public string TableName;
        public List<DataColumnReference> References;
        Dictionary<string, DataColumnReference> m_maps;
        public Dictionary<string, DataColumnReference> GetMaps()
        {
            if (References == null) return null;
                if (m_maps == null)
                {
                    m_maps = new Dictionary<string, DataColumnReference>();
                    for (int i = 0; i < References.Count; i++)
                    {
                        DataColumnReference dcr = References[i];
                        string key = null;
                        key = dcr.MainColumn.DataPoint;
                        
                        DataColumn dc = DataCenter.DataAccessCenter.DataColumnMappings[key];
                        if (!m_maps.ContainsKey(dc.Table))
                        {
                            m_maps.Add(dc.Table, dcr);
                        }
                    }
                }
                return m_maps;
           
        }
    }
}
