using System.Collections.Generic;
using System.Xml;
using XmlProcess;
namespace WolfInv.Com.MetaDataCenter
{
    public class DataPoint:IXml
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Text { get; set; }
        public string ComboName { get; set; }
        public int Width { get; set; }
        public DataPoint()
        {
        }

        public DataPoint(string pointname)
        {
            this.Name = pointname;
        }

        public static Dictionary<string, DataPoint> InitMapping(XmlDocument xmldoc)
        {
            if (xmldoc == null) return null;
            Dictionary<string, DataPoint> DataPointMappings = new Dictionary<string, DataPoint>();

            //string strDpmPath = string.Format("{0}\\xml\\dataidmapping.xml", strAppPath);

            //    XmlDocument xmldoc = new XmlDocument();
            
                ////try
                ////{
                ////    xmldoc.Load(strDpmPath);
                ////}
                ////catch (Exception ce)
                ////{
                ////    throw new Exception("can't init the DataIdMapping File!");
                ////}
                XmlNodeList nodes = xmldoc.SelectNodes("/flds/f");
                DataPointMappings = new Dictionary<string, DataPoint>();
                foreach (XmlNode node in nodes)
                {
                    DataPoint dp = new DataPoint();
                    dp.LoadXml(node);
                    ////dp.Name = XmlUtil.GetSubNodeText(node, "@i");
                    ////dp.DataType = XmlUtil.GetSubNodeText(node, "@type");
                    if (!DataPointMappings.ContainsKey(dp.Name))
                    {
                        DataPointMappings.Add(dp.Name, dp);
                    }
                }
                return DataPointMappings;
        }

        public virtual void LoadXml(XmlNode node)
        {
            Name = XmlUtil.GetSubNodeText(node, "@i");
            DataType = XmlUtil.GetSubNodeText(node, "@type");
            Text = XmlUtil.GetSubNodeText(node, "@udlbl");
            string strWidth = XmlUtil.GetSubNodeText(node, "@width");
            int twidth = 0;
            int.TryParse(strWidth, out twidth);
            Width = twidth;
            ComboName = XmlUtil.GetSubNodeText(node, "@combo");
            
        }

         public virtual XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.Load("root");
                parent = xmldoc.SelectSingleNode("root");
            }
            xmldoc = parent.OwnerDocument;
            XmlNode node = XmlUtil.AddSubNode(parent, "f", true);
            XmlUtil.AddAttribute(node, "i", Name);
             if(Text != null && Text !="")
                XmlUtil.AddAttribute(node, "udlbl", Text);
            if(ComboName != null && ComboName != "")
                XmlUtil.AddAttribute(node, "combo", ComboName);
            if(Width > 0)
                XmlUtil.AddAttribute(node, "width", Width.ToString());
            XmlUtil.AddAttribute(node, "type", DataType);
            return node;
        }

         
    }
}
