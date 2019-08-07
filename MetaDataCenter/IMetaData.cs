using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace WolfInv.Com.MetaDataCenter
{
    public interface IXml
    {
        XmlNode ToXml(XmlNode parent);
        void LoadXml(XmlNode node);
    }

    public interface IUserData
    {
        string strUid { get;set;}
    }
}
