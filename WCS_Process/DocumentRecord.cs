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
        /// ����Xml����
        /// </summary>
        /// <returns></returns>
        XmlNode ToXml();

        /// <summary>
        /// ����xml���ɶ���
        /// </summary>
        /// <param name="xmldoc">XmlԴ</param>
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

        #region IXmlObject ��Ա

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
    /// �ļ��ͼ������ϼ�¼
    /// </summary>
    public class CFileDocumentRecord : CDocumentRecord
    {
        public int FileRecId;
        
        public List<CAttachment> Attachments;
        public Dictionary<DateTime, CFileDocumentRecord> History;
        public string UpdateComment;
    }

    /// <summary>
    /// ��¼�ͼ������ϼ�¼
    /// </summary>
    public class CDataDocumentRecord : CDocumentRecord
    {

    }


    public class DocumentType
    {
        #region ���Լ���
        //****************************************************************************					
        //*�����Զ���������					
        //*�����ˣ�	zhouys				
        //*����ʱ�䣺2010-09-09 16:31:48					
        //****************************************************************************					
        #region �Զ������


        int t_TypeId;			   //����id
        string t_TypeName;			   //������
        int t_DataType;			   //��������	  
        int t_FileCount;			   //�ļ�����	  
        #endregion

        #region ����:ҳ���
        ///<summary>			
        ///ҳ���menu id			
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

        #region ����:ҳȨ��
        ///<summary>			
        ///ҳȨ��0���ޣ�1��view��2��insert 4��update 8��delete 16��print 32:tree 64:export			
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

        #region ����:��������
        ///<summary>			
        ///��������			
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

        #region ����:�ļ�����
        ///<summary>			
        ///�ļ�����			
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
