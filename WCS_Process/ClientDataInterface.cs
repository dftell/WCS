using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using WolfInv.Com.DataCenter;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{

    public class ClientDataInterface : DataInterface
    {
        //AccessWebService aws;
        DataAccessCenter dac;
        public ClientDataInterface(string apppath,Dictionary<string, ConnectObject> objs)
            : base(apppath,objs)
        {
            if (connobj == null) return;
            XmlDocument xmlmap = GlobalShare.GetXmlFile("\\xml\\dataidmapping.xml");
            XmlDocument xmlcol = GlobalShare.GetXmlFile("\\xml\\sdataidmapping.xml");
            if (GlobalShare.DataConnectObjs == null) return;
            dac = new DataAccessCenter(GlobalShare.AppPath, GlobalShare.DataConnectObjs ,xmlmap,xmlcol);
        }
        public override DataSet GetDataSet(XmlNode xml, out string msg)
        {

            return DataAccessCenter.GetDataList(xml,out msg);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string UpdateData(XmlNode xml)
        {
            return DataAccessCenter.UpdateDataList(xml);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override XmlNode GetViewSettings(XmlNode xml)
        {
            string msg = null;
            return DataAccessCenter.GetViewSource(xml);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string DisConnect()
        {
            return DataAccessCenter.DisConnect();
            //throw new Exception("The method or operation is not implemented.");
        }

        public override XmlDocument GetConfigXmlFile(string filepath,string module, string screen, string target, string flag)
        {
            XmlDocument xmldoc = GlobalShare.GetXmlFileString(filepath,module, screen, target, flag);

            return xmldoc;   
        }
    }

}
