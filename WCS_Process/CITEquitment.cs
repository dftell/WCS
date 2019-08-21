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
    public class CITEquitment
    {

    }

    public class ITType
    {
        #region ���Լ���
        //****************************************************************************					
        //*�����Զ���������					
        //*�����ˣ�					
        //*����ʱ�䣺2010-10-27 12:37:40					
        //****************************************************************************					
        #region �Զ������


        string t_TypeName;			   //��˳��	  
        int t_TypeId;			   //��������	  
        int t_ParentId;			   //���ñ�	  
        string t_ParentType;			   //�Ƿ�����	  
        #endregion

        #region ����:��˳��
        ///<summary>					
        ///��˳��					
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

        #region ����:���ñ�
        ///<summary>			
        ///���ñ�			
        ///</summary>			
        public int ParentId
        {
            get
            {
                return t_ParentId;
            }
            set
            {

                t_ParentId = value;
            }
        }
        #endregion

        #region ����:�Ƿ�����
        ///<summary>			
        ///�Ƿ�����			
        ///</summary>			
        public string ParentType
        {
            get
            {
                return t_ParentType;
            }
            set
            {

                t_ParentType = value;
            }
        }
        #endregion


        #endregion			

    }

    public class ITTypeProcess
    {
        static DBAccessClass db;
        public ITTypeProcess(string connectstr)
        {
            if(db == null)
                db = new DBAccessClass(connectstr);
            db.Connect();
            
        }

        public List<ITType> GetITTypes()
        {
            List<ITType> retList = new List<ITType>();
            DataSet ds = db.ExecuteDataSet("Select a.TypeId,a.TypeName,a.ParentType as ParentId,b.TypeName as ParentName from ITMS_CommTypes a inner join ITMS_CommTypes b on a.parentType = b.TypeId where b.parentType = 12");
            if (ds == null || ds.Tables.Count != 1)
            {
                return null;
            }
            for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
            {
                ITType dt = new ITType();
                dt.TypeId = int.Parse(ds.Tables[0].Rows[n][0].ToString());
                dt.TypeName = ds.Tables[0].Rows[n][1].ToString();
                dt.ParentId = int.Parse(ds.Tables[0].Rows[n][2].ToString());
                dt.ParentType = ds.Tables[0].Rows[n][3].ToString();
                retList.Add(dt);
            }
            db.Close();
            return retList;
        }
    }
}
