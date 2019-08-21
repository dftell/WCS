using System;
using System.Collections.Generic;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using System.Data;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    /// <summary>
    /// �������ݽӿ�
    /// </summary>
    public interface ITranslateableInterFace
    {

        UpdateData NeedUpdateData { get;set;}//���������
        List<DataTranMapping> TranData { get;set;}//���������
        UpdateData GetCurrFrameData();

        
    }

    
    /// <summary>
    /// �޸����ݽӿ�
    /// </summary>
    public interface ISaveableInterFace : ITranslateableInterFace
    {
        bool SaveData(DataRequestType type= DataRequestType.Update);
    }

   
    
    public interface IKeyForm
    {
        
        string strRowId{get;set;}
        string strKey{get;set;}

        
        
    }

    public interface ILink
    {
        CMenuItem FromMenu { get; set; }
        IKeyForm Link { get; set; }
        
    }

    public interface IKeyTransable
    {
        List<DataCondition> InitBaseConditions();
    }

    

    public interface IModuleControl
    {
        string strModule { get;set;}
        string strScreen { get;set;}
        string strTarget { get;set;}
    }

    public interface IDataSouceControl
    {
        string GridSource{get;set;}
        string DetailSource { get;set;}
        DataSet InitDataSource(string sGridSource, out string msg);
    }

    public interface IMutliDataInterface
    {
        List<UpdateData> InjectedDatas { get;set;}
        List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem);
        List<UpdateData> GetDataList(List<UpdateData> OrgList);
        List<UpdateData> GetDataList(bool OnlyCheckedItem);
    }
 }
