using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;
using Microsoft;
using System.Reflection;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommCtrlLib
{

    public abstract class IFrameActionSwitch
    {
        protected static Assembly Appasmb;

       
        public static void FillTranData(IKeyForm currfrm, ITranslateableInterFace ifrm, CMenuItem mnu, ref UpdateData data)
        {
            if (data == null) data = new UpdateData();
            ifrm.NeedUpdateData = data.Clone() as UpdateData;//传入
            ifrm.TranData = mnu.TranDataMapping;
            #region
            if (mnu.TranDataMapping != null)
            {
                for (int i = 0; i < mnu.TranDataMapping.Count; i++)
                {
                    DataTranMapping dtm = mnu.TranDataMapping[i];
                    if (currfrm != null && currfrm.strKey == dtm.FromDataPoint)//如果匹配到关键字，传送到下一个界面
                    {

                        //break;
                    }
                    else
                    {
                        if (GlobalShare.DataPointMappings.ContainsKey(dtm.FromDataPoint))//如果是数据点，传数据
                        {
                            //暂时无法处理
                            if (currfrm is ITranslateableInterFace)//获得该界面的所有数据
                            {
                                UpdateData currframedata = (currfrm as ITranslateableInterFace).GetCurrFrameData();
                                if (currframedata.Items.ContainsKey(dtm.FromDataPoint))//如果数据中包括要传送的数据点
                                {
                                    if (data.Items.ContainsKey(dtm.ToDataPoint))
                                    {
                                        data.Items[dtm.ToDataPoint].value = currframedata.Items[dtm.FromDataPoint].value;
                                    }
                                    else
                                    {
                                        data.Items.Add(dtm.ToDataPoint, new UpdateItem(dtm.ToDataPoint, currframedata.Items[dtm.FromDataPoint].value));
                                    }
                                }
                            }

                        }
                        else//传常量
                        {
                            if (data.Items.ContainsKey(dtm.ToDataPoint))
                            {
                                data.Items[dtm.ToDataPoint].value = dtm.FromDataPoint;
                            }
                            else
                            {
                                data.Items.Add(dtm.ToDataPoint, new UpdateItem(dtm.ToDataPoint, dtm.FromDataPoint));
                            }
                        }
                    }
                }
            }
            #endregion
            //关键字下传
            if (data == null)
                data = new UpdateData();
            if (data.Items.ContainsKey(currfrm.strKey) && currfrm.strRowId != "")
            {
                data.Items[currfrm.strKey].value = currfrm.strRowId;
            }
            else
            {
                data.Items.Add(currfrm.strKey, new UpdateItem(currfrm.strKey, currfrm.strRowId));
            }
        }

        public bool GetFrameHandle(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg,ref BaseFormHandle retHandle)
        {
            msg = null;
            if (mnu == null) return false;
            if (mnu.LinkValue == null || mnu.LinkValue.Trim().Length == 0)
            {
                msg = "目标为空！";
                return false;
            }
            Assembly ass = this.GetAssembly();
            if(ass == null)
            {
                msg = "无法获得程序集！";
                return false;
            }
            string strFrmB = string.Format(mnu.LinkValue, GlobalShare.SystemAppInfo.AssemName)+"B";
            Type tFrm = ass.GetType(strFrmB);
            if (mnu.linkType != LinkType.WebPage)
            {

                if (tFrm == null)
                {
                    msg = string.Format("未找到类{0}!", mnu.LinkValue);
                    return false;
                }
            }
            retHandle = null;
            switch (mnu.linkType)
            {
                case LinkType.WebPage:
                    {
                        return CreateWebPage(Container, CurrFormHandle, mnu, ref msg);

                        break;
                    }
                case LinkType.Form:
                default:
                    {
                        BaseFormHandle objInst;
                        if (mnu.Params == null || mnu.Params.Trim().Length == 0)
                        {
                            objInst = Activator.CreateInstance(tFrm, null) as BaseFormHandle;
                        }
                        else
                        {
                            objInst = Activator.CreateInstance(tFrm, mnu.Params) as BaseFormHandle;
                        }
                        if (objInst == null)
                        {
                            msg = "无法建立实例！";
                            return false;
                        }
                        objInst.strUid = mnu.strUid;
                        objInst.Tag = mnu.MnuName;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        if (mnu.Module.Trim().Length > 0)
                            objInst.strModule = mnu.Module;
                        if (mnu.Screen.Trim().Length > 0)
                            objInst.strScreen = mnu.Screen;
                        if (mnu.Target.Trim().Length > 0)
                            objInst.strTarget = mnu.Target;
                        objInst.strKey = mnu.Key;
                        objInst.Link = CurrFormHandle;
                        objInst.TranData = mnu.TranDataMapping;
                        if (objInst is ITranslateableInterFace && CurrFormHandle is IKeyForm)
                        {
                            FillTranData(CurrFormHandle, objInst as ITranslateableInterFace, mnu, ref data);
                        }
                        if (!this.CreateFrame(Container, CurrFormHandle, mnu, ref data, ref msg))
                        {
                            return false;
                        }
                        retHandle = objInst;
                        break;

                    }
            }
            msg = null;
            return true;
        }

        public bool ShowDialoger(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            msg = null;
            if (mnu == null) return false;
            if (mnu.LinkValue == null || mnu.LinkValue.Trim().Length == 0)
            {
                msg = "目标为空！";
                return false;
            }
            Assembly ass = this.GetAssembly();
            if (ass == null)
            {
                msg = "无法获得程序集！";
                return false;
            }
            Type tFrm = ass.GetType(string.Format(mnu.LinkValue, GlobalShare.SystemAppInfo.AssemName));
            if (mnu.linkType != LinkType.WebPage)
            {

                if (tFrm == null)
                {
                    msg = string.Format("未找到类{0}!", mnu.LinkValue);
                    return false;
                }
            }
            switch (mnu.linkType)
            {
                case LinkType.WebPage:
                    {
                        return CreateWebPage(Container, CurrFormHandle, mnu, ref msg);
                        break;
                    }
                case LinkType.Form:
                default:
                    {
                        BaseFormHandle objInst;
                        if (mnu.Params == null || mnu.Params.Trim().Length == 0)
                        {
                            objInst = Activator.CreateInstance(tFrm, null) as BaseFormHandle;
                        }
                        else
                        {
                            objInst = Activator.CreateInstance(tFrm, mnu.Params) as BaseFormHandle;
                        }
                        if (objInst == null)
                        {
                            msg = "无法建立实例！";
                            return false;
                        }
                        objInst.Tag = mnu.MnuName;
                        objInst.strUid = mnu.strUid;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        if (mnu.Module.Trim().Length > 0)
                            objInst.strModule = mnu.Module;
                        if (mnu.Screen.Trim().Length > 0)
                            objInst.strScreen = mnu.Screen;
                        if (mnu.Target.Trim().Length > 0)
                            objInst.strTarget = mnu.Target;
                        objInst.strKey = mnu.Key;
                        objInst.Link = CurrFormHandle;
                        if (objInst is ITranslateableInterFace && CurrFormHandle is IKeyForm)
                        {
                            FillTranData(CurrFormHandle, objInst as ITranslateableInterFace, mnu, ref data);
                        }
                        if (!this.CreateFrame(Container, CurrFormHandle, mnu, ref data, ref msg))
                        {
                            return false;
                        }

                        break;

                    }
            }
            msg = null;
            return true;
        }

        public abstract bool CreateFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg);
        public abstract bool CreateWebPage(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref string msg);
        public abstract bool CreateDialoger(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg);
        public abstract bool CreateSelect(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg);
        public abstract bool CreateTagFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg);
        
        protected abstract Assembly GetAssembly();
    }

    public class ExtraRequest
    {
        string _Rnd;
        public string Rnd { get { return _Rnd; } set { _Rnd = value; } }
        public BaseFormHandle Handle;
        public CMenuItem RequestSource;
        public string RequestURL;

        public ExtraRequest()
        {
            _Rnd = new Random().NextDouble().ToString();
        }

        public ExtraRequest(string id)
        {
            string _Rnd = id;
        }
    }


    class ActionHandle
    {
  
    }
}
