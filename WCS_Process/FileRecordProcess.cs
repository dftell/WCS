using System;
using System.Collections.Generic;
using System.Text;
using DBAccess;
using System.Data;
using System.Xml;
using XmlProcess;

namespace WolfInv.Com.WCS_Process
{
    interface IDocumentProcess
    {
        string Validate(bool OnlyComment);
        void Update();
        void New();
        void Audit();
        void ListView();
        void ListView(int Dep);
        void ListView(int Dep, int DocType);
    }

    public class FileRecordProcess : IDocumentProcess
    {


        DBAccess.DBAccessClass db;
        public bool SupportMutliFiles ;
        public CFileDocumentRecord FileRec;
        int mDep;
        public FileRecordProcess(string connstr,int dep)
        {
            mDep = dep;
            db = new DBAccessClass(connstr);
        }


        #region IDocumentProcess 成员

        public string Validate(bool OnlyComment)
        {
            if (FileRec == null) return "02_001";
            if (FileRec.FileRecId > 0) //update 
            {
                if(FileRec.UpdateComment.Trim().Length == 0)//更新说明不能为空
                {
                    return "02_003";
                }
                if (!OnlyComment &&　FileRec.Attachments != null && FileRec.Attachments.Count == 0)//如果需要更新附件,附件不能为空
                {
                    return "02_002";
                }

            }
            return null;
        }

        public void Update()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void New()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Audit()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDocumentProcess 成员

        

        public void ListView()
        {
            
            
            throw new Exception("The method or operation is not implemented.");
        }

        public void ListView(int Dep)
        {
            LParamate[] paramArr = new LParamate[1];
            paramArr[0] = new LParamate("@OwnDepId", "int", Dep.ToString());
            DataSet ds = db.ExecuteDataSet("sp_ITMS_DocumentUpdateSummary", paramArr);

        }

        public void ListView(int Dep, int DocType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
