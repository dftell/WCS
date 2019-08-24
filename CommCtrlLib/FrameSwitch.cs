using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.MetaDataCenter;
using System;
using System.Windows.Forms;
using System.Web;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

using WolfInv.Com.WCS_Process;
using System.Xml;
using System.IO;
using System.Linq;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    public class FrameSwitch
    {
        public static Form ParentForm;
        public static Icon SystemIcon;
        public static string SystemText;
        protected static Assembly Appasmb;
        public static Dictionary<string, WebForm> ShowWebForms = new Dictionary<string, WebForm>();
        public static Dictionary<string, Ifrm_Model> AllModules = new Dictionary<string, Ifrm_Model>();
        static FrameSwitch()
        {
            //ExtraReqMappings = new Dictionary<string, ExtraRequest>();
            Appasmb = GlobalShare.MainAssem; //Assembly.LoadFrom(string.Format("{0}.exe",GlobalShare.SystemAppInfo.AssemName));

        }

        public static IXForm CreateForm(PlatformControlType pct)
        {
            IXForm ret = null;
            switch(pct)
            {
                case PlatformControlType.Web:
                    {
                        
                        break;
                    }
                case PlatformControlType.WinForm:
                default:
                    {
                        ret = new XWinForm_Form();
                        break;
                    }
                    
            }
            return ret;
        }
            

        public static bool switchToView(IXContainerControl container, CMenuItem mnu)
        {
            return switchToView(container, null, mnu);
        }

        public static bool switchToView(IXContainerControl container, IKeyForm Link, CMenuItem mnu)
        {
            UpdateData data = null;
            return switchToView(container, Link, mnu, ref data);
        }
        public static bool switchToView(IXContainerControl container, IKeyForm Link, CMenuItem mnu, ref UpdateData data)
        {
            return switchToView(container, Link, mnu, ref data, null);
        }
        public static bool switchToView(IXContainerControl container, IKeyForm Link, CMenuItem mnu, ref UpdateData data, List<UpdateData> injectedatas)
        {
            if (mnu == null) return false;
            if (mnu.LinkValue == null || mnu.LinkValue.Trim().Length == 0)
            {
                return false;
            }

            Type tFrm = Appasmb.GetType(string.Format(mnu.LinkValue, GlobalShare.SystemAppInfo.AssemName));
            if (true)
            {

                if (tFrm == null)
                {
                    MessageBox.Show(string.Format("未找到类{0}!", mnu.LinkValue));
                    return false;
                }
            }
            if (mnu.linkType == LinkType.WebPage)
            {
                if (AllModules.ContainsKey(mnu.MnuId))
                {
                    container.CurrMainControl = AllModules[mnu.MnuId];
                    return true;
                }
            }
            Ifrm_Model objInst;
            if (container == null)
                return false;
            //container.Controls.Clear();

            //container.Controls.Clear();//清除所有控件

            if (mnu.Params == null || mnu.Params.Trim().Length == 0)
            {
                object obj = Activator.CreateInstance(tFrm, null);
                objInst = obj as Ifrm_Model;
            }
            else
            {
                objInst = Activator.CreateInstance(tFrm, mnu.Params) as Ifrm_Model;
            }
            if (objInst == null)
            {
                return false;
            }
            objInst.FromMenu = mnu;
            objInst.strUid = mnu.strUid;
            objInst.SetDock(XPlatformDockStyle.Fill);
            if (objInst.lb_Title != null)
                objInst.lb_Title.Text = mnu.MnuName;
            objInst.GridSource = mnu.GridSource;
            objInst.DetailSource = mnu.DetailSrouce;
            objInst.strKey = mnu.Key;
            if (mnu.Module.Trim().Length > 0)
                objInst.strModule = mnu.Module;
            if (mnu.Screen.Trim().Length > 0)
                objInst.strScreen = mnu.Screen;
            if (mnu.Target.Trim().Length > 0)
                objInst.strTarget = mnu.Target;
            objInst.strKey = mnu.Key;
            objInst.Link = Link;
            if (objInst is IMutliDataInterface)
            {
                (objInst as IMutliDataInterface).InjectedDatas = injectedatas;
            }
            if (objInst is ITranslateableInterFace && (Link is IKeyForm || Link == null))
            {
                FillTranData(Link, objInst as ITranslateableInterFace, ref mnu, ref data);
                (objInst as ITranslateableInterFace).NeedUpdateData = data;
            }
            objInst.TranData = mnu.TranDataMapping;
            //菜单数据下传
                                                   //if (objInst.InjectedDatas == null)
                                                   //{
                                                   //    objInst.InjectedDatas = new List<UpdateData>();
                                                   //}
                                                   //if(data != null)
                                                   //{
                                                   //    objInst.InjectedDatas.Add(data);
                                                   //}
            if (container != null)
            {
                container.Controls_Add(objInst);
                objInst.ToTopLevel();

            }
            if (!AllModules.ContainsKey(mnu.MnuId))
            {
                AllModules.Add(mnu.MnuId, objInst);
            }
            container.CurrMainControl = objInst;
            switch (mnu.linkType)
            {
                case LinkType.WebPage://winform专用
                    {
                        
                        break;
                    }
                case LinkType.Dialog:
                    {
                        if(container is IXPanel) //容器是ixpanel 并且
                        {
                            if((container as IXPanel).InForm)//容器是在最外层main/mdi_main
                            {
                               
                            }
                            else
                            {
                                break;
                            }
                        }
                        else//如果是form
                        {
                            //自加载
                            break;
                        }
                        
                        IXForm frmobj = CreateForm(objInst.ControlType);
                        frmobj.InitForm(mnu, SystemIcon);
                        frmobj.Controls_Add(objInst);
                        frmobj.ShowIXDialog();
                        break;

                    }
                case LinkType.Form:
                    {

                        break;

                    }
                case LinkType.UserControl:
                    {

                        break;
                    }
                case LinkType.PrintToPDF:
                    {
                        if (container is IXPanel) //容器是ixpanel 并且
                        {
                            if ((container as IXPanel).InForm)//容器是在最外层main/mdi_main
                            {
                                break;
                            }
                            else
                            {

                            }
                        }
                        else//如果是form
                        {
                            //自加载
                            break;
                        }

                        IXForm frmobj = CreateForm(objInst.ControlType);
                        frmobj.InitForm(mnu, SystemIcon);
                        frmobj.Controls_Add(objInst);
                        frmobj.ShowIXDialog();
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            return true;
        }

        public static bool ShowDialoger(IXPanel container, IKeyForm Link, CMenuItem mnu, ref UpdateData data)
        {
            return ShowDialoger(container, Link, mnu, ref data, false);
        }
        public static bool ShowDialoger(IXPanel container, IKeyForm Link, CMenuItem mnu, ref UpdateData data, bool MultiSelected)
        {
            if (mnu == null) return false;
            if (mnu.LinkValue == null || mnu.LinkValue.Trim().Length == 0)
            {
                return false;
            }
            Type tFrm = Appasmb.GetType(string.Format(mnu.LinkValue, GlobalShare.SystemAppInfo.AssemName));
            if (mnu.linkType != LinkType.WebPage)
            {

                if (tFrm == null)
                {
                    MessageBox.Show(string.Format("未找到类{0}!", mnu.LinkValue));
                    return false;
                }
            }

            switch (mnu.linkType)
            {
                case LinkType.WebPage:
                    {
                        if (container != null)//support the tab frame
                        {
                            ////WebBrowser wb = new WebBrowser();
                            ////wb.Url = new Uri(mnu.LinkValue);
                            ////container.ControlsAdd(wb);
                            ////wb.Dock = DockStyle.Fill;
                            break;
                        }
                        WebForm wbfrm = null;
                        if (ShowWebForms.ContainsKey(mnu.MnuId))
                        {
                            wbfrm = ShowWebForms[mnu.MnuId];
                        }
                        else
                        {
                            wbfrm = new WebForm(mnu.MnuId, mnu.LinkValue);
                        }
                        wbfrm.ShowDialog(ParentForm);
                        break;
                    }
                case LinkType.Dialog:
                    {
                        Ifrm_Model objInst;
                        if (mnu.Params == null || mnu.Params.Trim().Length == 0)
                        {
                            objInst = Activator.CreateInstance(tFrm, null) as Ifrm_Model;
                        }
                        else
                        {
                            objInst = Activator.CreateInstance(tFrm, mnu.Params) as Ifrm_Model;
                        }
                        if (objInst == null)
                        {
                            return false;
                        }
                        objInst.FromMenu = mnu;
                        objInst.SetDock( XPlatformDockStyle.Fill);
                        objInst.strUid = mnu.strUid;
                        objInst.FromMenu = mnu;
                        objInst.lb_Title.Text = mnu.MnuName;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        if (mnu.Module.Trim().Length > 0)
                            objInst.strModule = mnu.Module;
                        if (mnu.Screen.Trim().Length > 0)
                            objInst.strScreen = mnu.Screen;
                        if (mnu.Target.Trim().Length > 0)
                            objInst.strTarget = mnu.Target;
                        objInst.strKey = mnu.Key;
                        objInst.Link = Link;
                        IXForm frm = CreateForm(objInst.ControlType);
                        frm.InitForm(mnu, SystemIcon);
                        //objInst.AllowClose = false;
                        if (objInst is ITranslateableInterFace && (Link is IKeyForm || Link == null))
                        {
                            FillTranData(Link, objInst as ITranslateableInterFace,ref mnu, ref data);
                        }
                        objInst.TranData = mnu.TranDataMapping;
                        objInst.FromMenu = mnu;
                        if (objInst is ITranslateableInterFace)
                        {
                            data.ReqType = DataRequestType.Update;
                            if(mnu.Params == null ||mnu.Params.Trim().Length == 0)
                            {
                                data.ReqType = DataRequestType.Add;
                                data.Updated = true;
                            }
                            (objInst as ITranslateableInterFace).NeedUpdateData = data;
                        }
                        frm.Controls_Add(objInst);
                        if (!frm.ShowIXDialog())
                            return false;
                        if (objInst is ITranslateableInterFace)
                            data = (objInst as ITranslateableInterFace).NeedUpdateData;//传出

                        break;
                    }
                case LinkType.Select:
                    {
                        Dlg_CommModel objInst;
                        if (mnu.Params == null || mnu.Params.Trim().Length == 0)
                        {
                            objInst = Activator.CreateInstance(tFrm, null) as Dlg_CommModel;
                        }
                        else
                        {
                            objInst = Activator.CreateInstance(tFrm, mnu.Params) as Dlg_CommModel;
                        }
                        if (objInst == null)
                        {
                            return false;
                        }
                        objInst.FromMenu = mnu;
                        objInst.strUid = mnu.strUid;
                        //objInst.Dock = DockStyle.Fill;
                        //objInst.lb_Title.Text = mnu.MnuName;
                        objInst.MultiSelect = MultiSelected;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        if (mnu.Module.Trim().Length > 0)
                            objInst.strModule = mnu.Module;
                        if (mnu.Screen.Trim().Length > 0)
                            objInst.strScreen = mnu.Screen;
                        if (mnu.Target.Trim().Length > 0)
                            objInst.strTarget = mnu.Target;
                        objInst.strKey = mnu.Key;
                        objInst.Link = Link;

                        if (container != null)
                            objInst.Icon = SystemIcon;
                        objInst.Text = mnu.Title;
                        
                        objInst.MaximizeBox = false;
                        objInst.MinimizeBox = false;
                        objInst.FormBorderStyle = FormBorderStyle.FixedDialog;
                        objInst.StartPosition = FormStartPosition.CenterParent;
                        //objInst.AllowClose = false;
                        //(objInst as Dlg_CommModel).ReturnData = data;//传入
                        if (objInst is ITranslateableInterFace)
                        {
                            FillTranData(Link, objInst as ITranslateableInterFace,ref mnu, ref data, false);
                        }
                        if (objInst.ShowDialog() != DialogResult.Yes)
                        {
                            return false;
                        }
                        if (objInst is ITranslateableInterFace && Link is IKeyForm)
                        {
                            data = (objInst as ITranslateableInterFace).NeedUpdateData;//传出
                        }

                        break;
                    }
                case LinkType.Form:
                    {
                        Ifrm_Model objInst;
                        if (mnu.Params == null || mnu.Params.Trim().Length == 0)
                        {
                            objInst = Activator.CreateInstance(tFrm, null) as Ifrm_Model;
                        }
                        else
                        {
                            objInst = Activator.CreateInstance(tFrm, mnu.Params) as Ifrm_Model;
                        }
                        if (objInst == null)
                        {
                            return false;
                        }
                        objInst.FromMenu = mnu;
                        objInst.strUid = mnu.strUid;
                        objInst.SetDock( XPlatformDockStyle.Fill);
                        objInst.lb_Title.Text = mnu.MnuName;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        if (mnu.Module.Trim().Length > 0)
                            objInst.strModule = mnu.Module;
                        if (mnu.Screen.Trim().Length > 0)
                            objInst.strScreen = mnu.Screen;
                        if (mnu.Target.Trim().Length > 0)
                            objInst.strTarget = mnu.Target;
                        objInst.strKey = mnu.Key;
                        objInst.Link = Link;
                        IXForm frm = CreateForm(objInst.ControlType);
                        frm.InitForm(mnu, SystemIcon);

                        //objInst.AllowClose = false;
                        if (objInst is ITranslateableInterFace && (Link is IKeyForm || Link == null))
                        {
                            FillTranData(Link, objInst as ITranslateableInterFace, ref mnu, ref data);
                            //(objInst as ITranslateableInterFace).NeedUpdateData = data;
                        }
                        frm.Controls_Add(objInst);
                        frm.ShowIXDialog();
                        if (objInst is ISaveableInterFace)
                            data = (objInst as ISaveableInterFace).NeedUpdateData;//传出

                        break;

                    }
                case LinkType.UserControl:
                    {
                        container.Controls_Clear();//清除所有控件
                        Ifrm_Model objInst;
                        if (mnu.Params == null || mnu.Params.Trim().Length == 0)
                        {
                            objInst = Activator.CreateInstance(tFrm, null) as Ifrm_Model;
                        }
                        else
                        {
                            objInst = Activator.CreateInstance(tFrm, mnu.Params) as Ifrm_Model;
                        }
                        if (objInst == null)
                        {
                            return false;
                        }
                        objInst.FromMenu = mnu;
                        objInst.strUid = mnu.strUid;
                        objInst.SetDock( XPlatformDockStyle.Fill);
                        objInst.lb_Title.Text = mnu.MnuName;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        objInst.strKey = mnu.Key;
                        if (mnu.Module.Trim().Length > 0)
                            objInst.strModule = mnu.Module;
                        if (mnu.Screen.Trim().Length > 0)
                            objInst.strScreen = mnu.Screen;
                        if (mnu.Target.Trim().Length > 0)
                            objInst.strTarget = mnu.Target;
                        objInst.strKey = mnu.Key;
                        objInst.Link = Link;
                        if (objInst is ITranslateableInterFace && (Link is IKeyForm || Link == null))
                        {
                            FillTranData(Link, objInst as ITranslateableInterFace,ref mnu, ref data);
                        }
                        container.Controls_Add(objInst);
                        objInst.ToTopLevel();
                        //objInst.Focus();
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            return true;
        }

        public static void FillTranData(IKeyForm currfrm, ITranslateableInterFace ifrm, ref CMenuItem mnu, ref UpdateData data)
        {
            FillTranData(currfrm, ifrm, ref mnu, ref data, true);
        }

        public static void FillTranData(IKeyForm currfrm,ITranslateableInterFace ifrm,ref CMenuItem mnu, ref UpdateData data, bool TranData)
        {
            if (data == null) data = new UpdateData();


            if (TranData)
            {
                ifrm.NeedUpdateData = data.Clone() as UpdateData;//传入
            }
            ifrm.TranData = mnu.CopyTranDatas();

            //上个页面查询条件下传
            if (currfrm != null && currfrm is ITranslateableInterFace && (currfrm as ITranslateableInterFace).TranData != null)
            {
                if(ifrm.TranData == null)
                {
                    ifrm.TranData = new List<DataTranMapping>();

                }
                if (ifrm.TranData != null)
                {
                    List<DataTranMapping> prevdata = (currfrm as ITranslateableInterFace).TranData;
                    for (int i = 0; i < prevdata.Count; i++)
                    {
                        DataTranMapping inputdata = prevdata[i];
                        var mp = ifrm.TranData.Where(a => a.FromDataPoint.Name == inputdata.ToDataPoint);
                        if (mp.Count()==0)
                        {
                            ifrm.TranData.Add(inputdata);
                                
                        }
                        else
                        {
                            mp.First().FromDataPoint.Text = inputdata.FromDataPoint.Text;
                        }
                        if (!data.Items.ContainsKey(inputdata.FromDataPoint.Name))
                        {
                            data.Items.Add(inputdata.FromDataPoint.Name, new UpdateItem(inputdata.FromDataPoint.Name, inputdata.FromDataPoint.Text));
                        }
                        data.Items[inputdata.FromDataPoint.Name].text = inputdata.FromDataPoint.Text;

                    }
                }
            }
            #region 传输数据不为空
            if (mnu.TranDataMapping != null)
            {
                for (int i = 0; i < mnu.TranDataMapping.Count; i++)
                {
                    DataTranMapping dtm = mnu.TranDataMapping[i];
                    if (currfrm != null && currfrm is IKeyForm && currfrm.strKey == dtm.FromDataPoint.Name)//如果匹配到关键字，传送到下一个界面
                    {
                        string val = currfrm.strRowId;
                        if(mnu.Params!= null && mnu.Params.Trim().Length>0)
                        {
                            if(val.Trim().Length == 0)
                            {
                                val = mnu.Params;
                            }
                        }
                        if(data.Items.ContainsKey(dtm.FromDataPoint.Name))
                        {
                            data.Items[dtm.FromDataPoint.Name].value = val;
                        }
                        else
                        {
                            data.Items.Add(dtm.FromDataPoint.Name, new UpdateItem(dtm.FromDataPoint.Name, val));
                        }
                        //break;
                    }
                    else
                    {
                        string strPoint = dtm.FromDataPoint.Name.Replace("{", "").Replace("}", "");
                        if (GlobalShare.DataPointMappings.ContainsKey(strPoint))//如果是数据点，传数据
                        {
                            if (!TranData)
                            {
                                continue;
                            }
                            //暂时无法处理
                            if (currfrm is ITranslateableInterFace)//获得该界面的所有数据
                            {
                                UpdateData currframedata = (currfrm as ITranslateableInterFace).GetCurrFrameData();
                                
                                if (currframedata.Items.ContainsKey(strPoint))//如果数据中包括要传送的数据点
                                {
                                    if (data.Items.ContainsKey(dtm.ToDataPoint))
                                    {
                                        data.Items[dtm.ToDataPoint].value = currframedata.Items[strPoint].value;
                                    }
                                    else
                                    {
                                        data.Items.Add(dtm.ToDataPoint, new UpdateItem(dtm.ToDataPoint, currframedata.Items[strPoint].value));
                                        //dtm.FromDataPoint  = currframedata.Items[dtm.FromDataPoint.Name].value;
                                    }
                                    dtm.FromDataPoint.Text = currframedata.Items[strPoint].value;
                                }
                            }
                            else//如果是顶层菜单传入，并且是系统的用户常量
                            {
                                

                                if(GlobalShare.UserAppInfos.First().Value.appinfo.UserInfo.Items.ContainsKey(strPoint))
                                {
                                    UpdateData ud = GlobalShare.UserAppInfos.First().Value.appinfo.UserInfo;
                                    if (data.Items.ContainsKey(strPoint))
                                    {
                                        data.Items[dtm.ToDataPoint].value = ud.Items[strPoint].value;
                                    }
                                    else
                                    {
                                        data.Items.Add(dtm.ToDataPoint, new UpdateItem(strPoint, ud.Items[strPoint].value));
                                        //dtm.FromDataPoint  = currframedata.Items[dtm.FromDataPoint.Name].value;
                                    }
                                    dtm.FromDataPoint.Text = ud.Items[strPoint].value;
                                }
                            }

                        }
                        else//传常量
                        {
                            if (data.Items.ContainsKey(dtm.ToDataPoint))
                            {
                                data.Items[dtm.ToDataPoint].value = dtm.FromDataPoint.Text;
                            }
                            else
                            {
                                data.Items.Add(dtm.ToDataPoint, new UpdateItem(dtm.ToDataPoint, dtm.FromDataPoint.Text));
                            }
                        }
                    }
                }
            }
            #endregion
            if (!TranData)
            {
                return;
            }
            //关键字下传
            if (currfrm == null)
            {
                return;
            }
            if (data == null)
                data = new UpdateData();
            if (data.Items.ContainsKey(currfrm.strKey) && currfrm.strRowId != "")
            {
                data.Items[currfrm.strKey].value = currfrm.strRowId;
            }
            else
            {
                if (!data.Items.ContainsKey(currfrm.strKey))
                    data.Items.Add(currfrm.strKey, new UpdateItem(currfrm.strKey, currfrm.strRowId));
            }
        }

        

    }

}