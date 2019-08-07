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
        static FrameSwitch()
        {
            Appasmb = Assembly.LoadFrom(string.Format("{0}.exe",GlobalShare.SystemAppInfo.AssemName));

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
            

        public static bool switchToView(IXPanel container, CMenuItem mnu)
        {
            return switchToView(container, null, mnu);
        }

        public static bool switchToView(IXPanel container, IKeyForm Link, CMenuItem mnu)
        {
            UpdateData data = null;
            return switchToView(container, Link, mnu, ref data);
        }
        public static bool switchToView(IXPanel container, IKeyForm Link, CMenuItem mnu, ref UpdateData data)
        {
            return switchToView(container, Link, mnu, ref data, null);
        }
        public static bool switchToView(IXPanel container, IKeyForm Link, CMenuItem mnu, ref UpdateData data, List<UpdateData> injectedatas)
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
                case LinkType.WebPage://winform专用
                    {
                        //container.Controls.Clear();//清除所有控件
                        if (container != null)//support the tab frame
                        {
                            ////WebForm wb = new WebForm();
                            ////wb.Url = new Uri(mnu.LinkValue);
                            //WebForm wb = new WebForm(mnu.MnuId,mnu.LinkValue);
                            //container.Controls_Add(wb);
                            //wb.Dock = DockStyle.Fill;
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
                        //wbfrm.MdiParent = ParentForm;
                        wbfrm.TopMost = true;
                        wbfrm.Focus();
                        wbfrm.Show();

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
                        objInst.strUid = mnu.strUid;
                        if(objInst.ControlType == PlatformControlType.WinForm )
                        {
                            (objInst as System.Windows.Forms.Control).Dock = DockStyle.Fill;
                        }
                        else
                        {

                        }
                        
                        objInst.lb_Title.Text = mnu.MnuName;
                        objInst.GridSource = mnu.GridSource;
                        objInst.DetailSource = mnu.DetailSrouce;
                        objInst.SetDock(XPlatformDockStyle.Fill);
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
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
                            (objInst as ITranslateableInterFace).NeedUpdateData = data;
                        }
                        IXForm frmobj = CreateForm(objInst.ControlType);

                        frmobj.InitForm(mnu, SystemIcon);
                        frmobj.Controls_Add(objInst);
                        frmobj.ShowIXDialog();
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
                        objInst.strUid = mnu.strUid;
                        objInst.SetDock(XPlatformDockStyle.Fill);
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
                        SubForm frm = new SubForm();
                        if (mnu.WHeight == 0 || mnu.WWidth == 0)
                        {
                            frm.Size = new Size(800, 600);
                        }
                        else
                        {
                            frm.Size = new Size(mnu.WWidth, mnu.WHeight);
                        }
                        if (container != null)
                            frm.Icon = SystemIcon;
                        frm.Text = mnu.Title;
                        frm.MaximizeBox = false;
                        frm.MinimizeBox = false;
                        frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                        frm.WindowState = FormWindowState.Maximized;
                        if (objInst is IMutliDataInterface)
                        {
                            (objInst as IMutliDataInterface).InjectedDatas = injectedatas;
                        }
                        if (objInst is ITranslateableInterFace && (Link is IKeyForm || Link == null))
                        {
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
                        }
                        container.Controls_Add(objInst);
                        //frm.StartPosition = FormStartPosition.CenterParent;
                        // objInst.AllowClose = false;
                        //frm.Controls.Add(objInst);

                        ////Form fm = null;
                        ////if (container != null)
                        ////{
                        ////    fm = (container.TopLevelControl as Form);
                        ////    if (fm.IsMdiContainer)
                        ////    {
                        ////        //frm.MdiParent = fm;
                                
                        ////    }
                        ////    else
                        ////    {
                        ////        fm = fm.MdiParent;
                        ////    }
                        ////    //container.Controls.Clear();
                        ////    //container.Controls.Add(objInst);
                        ////}
                        objInst.ToTopLevel();
                        //frm.
                        //frm.TopLevel = true ;
                        //frm.MdiParent = ParentForm;
                        ////frm.Dock = DockStyle.Fill;
                        ////frm.Show();
                        //fm.ActiveMdiChild = frm;
                        break;

                    }
                case LinkType.UserControl:
                    {
                        if (container == null)
                            break;
                        //container.Controls.Clear();
                        
                        //container.Controls.Clear();//清除所有控件
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
                        objInst.strUid = mnu.strUid;
                        objInst.SetDock(XPlatformDockStyle.Fill);
                        if(objInst.lb_Title != null)
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
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
                        }
                        objInst.TranData = mnu.TranDataMapping;//菜单数据下传
                        if (container != null)
                        {
                            container.Controls_Add(objInst);
                            objInst.ToTopLevel();
                            break;
                        }
                        
                        //container.Controls.Add(objInst);
                        ////////////SubForm frm = new SubForm();
                        ////////////frm.Controls.Add(objInst);
                        //////////////frm.MdiParent = ParentForm;
                        ////////////frm.Width = 800;
                        ////////////frm.Height = 600;
                        ////////////frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                        ////////////frm.Dock = DockStyle.Fill;
                        ////////////frm.StartPosition = FormStartPosition.CenterParent;
                        ////////////frm.Opacity = 0.5;
                        ////////////frm.Show();
                        //////////////container.Controls.Add(objInst);

                        //objInst.Focus();
                        break;
                    }
                case LinkType.PrintToPDF:
                    {
                        if (container == null)
                        {
                            container = null;
                            break;
                        }
                        //container.Controls.Clear();
                        //container.Controls.Clear();//清除所有控件
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
                        objInst.strUid = mnu.strUid;
                        objInst.SetDock( XPlatformDockStyle.Fill);
                        if(objInst.lb_Title !=null)
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
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
                        }
                        objInst.TranData = mnu.TranDataMapping;//菜单数据下传
                        objInst.ToTopLevel();
                        container.Controls_Add(objInst);



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

                        objInst.SetDock( XPlatformDockStyle.Fill);
                        objInst.strUid = mnu.strUid;
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
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
                        }
                        frm.Controls_Add(objInst);
                        frm.ShowIXDialog();
                        if (objInst is ISaveableInterFace)
                            data = (objInst as ISaveableInterFace).NeedUpdateData;//传出

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
                        if (mnu.WHeight == 0 || mnu.WWidth == 0)
                        {
                            objInst.Size = new Size(800, 600);
                        }
                        else
                        {
                            objInst.Size = new Size(mnu.WWidth, mnu.WHeight);
                        }
                        objInst.MaximizeBox = false;
                        objInst.MinimizeBox = false;
                        objInst.FormBorderStyle = FormBorderStyle.FixedDialog;
                        objInst.StartPosition = FormStartPosition.CenterParent;
                        //objInst.AllowClose = false;
                        //(objInst as Dlg_CommModel).ReturnData = data;//传入
                        if (objInst is ITranslateableInterFace)
                        {
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data, false);
                        }
                        //if (objInst.ShowDialog(container) != DialogResult.Yes)
                        //{
                        //    return false;
                        //}
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
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
                            (objInst as ITranslateableInterFace).NeedUpdateData = data;
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
                            FillTranData(Link, objInst as ITranslateableInterFace, mnu, ref data);
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

        public static void FillTranData(IKeyForm currfrm, ITranslateableInterFace ifrm, CMenuItem mnu, ref UpdateData data)
        {
            FillTranData(currfrm, ifrm, mnu, ref data, true);
        }

        public static void FillTranData(IKeyForm currfrm, ITranslateableInterFace ifrm, CMenuItem mnu, ref UpdateData data, bool TranData)
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
                if (ifrm.TranData != null)
                {
                    List<DataTranMapping> prevdata = (currfrm as ITranslateableInterFace).TranData;
                    for (int i = 0; i < prevdata.Count; i++)
                    {
                        DataTranMapping inputdata = prevdata[i];
                        for (int t = 0; t < ifrm.TranData.Count; t++)
                        {
                            if (ifrm.TranData[t].FromDataPoint == inputdata.ToDataPoint)
                            {
                                ifrm.TranData[t].FromDataPoint = inputdata.FromDataPoint;
                            }
                        }
                    }
                }
            }
            #region 传输数据不为空
            if (mnu.TranDataMapping != null)
            {
                for (int i = 0; i < mnu.TranDataMapping.Count; i++)
                {
                    DataTranMapping dtm = mnu.TranDataMapping[i];
                    if (currfrm != null && currfrm is IKeyForm && currfrm.strKey == dtm.FromDataPoint)//如果匹配到关键字，传送到下一个界面
                    {

                        //break;
                    }
                    else
                    {
                        if (GlobalShare.DataPointMappings.ContainsKey(dtm.FromDataPoint))//如果是数据点，传数据
                        {
                            if (!TranData)
                            {
                                continue;
                            }
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
                                        //dtm.FromDataPoint  = currframedata.Items[dtm.FromDataPoint].value;
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