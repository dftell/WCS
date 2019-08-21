using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WolfInv.Com.AccessDataBase;
using System.Data;
using System.Xml;
using XmlProcess;
namespace WolfInv.Com.WCS_Process
{
    interface IXmlObject
    {
        /// <summary>
        /// 生成Xml对象
        /// </summary>
        /// <returns></returns>
        XmlNode ToXml();

        /// <summary>
        /// 根据xml生成对象
        /// </summary>
        /// <param name="xmldoc">Xml源</param>
        void GetFromXml(XmlNode xmldoc);
    }

    interface IDocumentRecord:IXmlObject 
    {

    }

    interface IFileDocument
    {
        void AttachFiles();
    }

    

    public class CDocumentRecord:IDocumentRecord 
    {

        public DateTime UpdateTime;
        public CITMSUser UpdateMan;
        public CDepartment OwnerDepartment;
        public DocumentType DocumentType;

        #region IXmlObject 成员

        public XmlNode ToXml()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetFromXml(XmlNode xmldoc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
  
    public class CAttachment
    {
        string FileTitle;
        Stream FileContent;
        string FileUrl;
        string UpdateComment;
    }

    /// <summary>
    /// 文件型技术资料记录
    /// </summary>
    public class CFileDocumentRecord : CDocumentRecord
    {
        public int FileRecId;
        
        public List<CAttachment> Attachments;
        public Dictionary<DateTime, CFileDocumentRecord> History;
        public string UpdateComment;
    }

    /// <summary>
    /// 记录型技术资料记录
    /// </summary>
    public class CDataDocumentRecord : CDocumentRecord
    {

    }


    public class DocumentType
    {
        #region 属性集合
        //****************************************************************************					
        //*程序自动生成属性					
        //*建立人：	zhouys				
        //*生成时间：2010-09-09 16:31:48					
        //****************************************************************************					
        #region 自定义变量


        int t_TypeId;			   //类型id
        string t_TypeName;			   //类型名
        int t_DataType;			   //数据类型	  
        int t_FileCount;			   //文件数量	  
        #endregion

        #region 属性:页编号
        ///<summary>			
        ///页编号menu id			
        ///</summary>			
        public int TypeId
        {
            get
            {
                return t_TypeId;
            }
            set
            {

                t_TypeId = value;
            }
        }
        #endregion

        #region 属性:页权限
        ///<summary>			
        ///页权限0：无：1：view；2：insert 4：update 8：delete 16：print 32:tree 64:export			
        ///</summary>			
        public string TypeName
        {
            get
            {
                return t_TypeName;
            }
            set
            {

                t_TypeName = value;
            }
        }
        #endregion

        #region 属性:数据类型
        ///<summary>			
        ///数据类型			
        ///</summary>			
        public int DataType
        {
            get
            {
                return t_DataType;
            }
            set
            {

                t_DataType = value;
            }
        }
        #endregion

        #region 属性:文件数量
        ///<summary>			
        ///文件数量			
        ///</summary>			
        public int FileCount
        {
            get
            {
                return t_FileCount;
            }
            set
            {

                t_FileCount = value;
            }
        }
        #endregion


        #endregion			
    }

    public class DocumentTypeProcess
    {
        static DBAccessClass db;
        public DocumentTypeProcess(string connectstr)
        {
            db = new DBAccessClass(connectstr);
        }

        public List<DocumentType>  GetDocumentTyes()
        {
            List<DocumentType> retList = new List<DocumentType>();
            db.Connect();
            DataSet ds = null;
            db.GetResult("Select TypeId,TypeName from ITMS_CommTypes where parentType = 1",ref ds);
            if (ds == null || ds.Tables.Count != 1)
            {
                return null;
            }
            for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
            {
                DocumentType dt = new DocumentType();
                dt.TypeId = int.Parse(ds.Tables[0].Rows[n][0].ToString());
                dt.TypeName = ds.Tables[0].Rows[n][1].ToString();
                retList.Add(dt);
            }
            db.Close();
            return retList;
        }
    }
}
