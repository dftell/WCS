using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.MetaDataCenter;
using System.Windows.Forms;
using System.Drawing;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    public class FormActionHandle:IFrameActionSwitch 
    {
        public override bool CreateFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            BaseFormHandle bfh = null;
            msg = null;
            bool ret = this.GetFrameHandle(Container, CurrFormHandle, mnu, ref data, ref msg, ref bfh);
            if (!ret || msg != null || bfh == null)
            {
                string msgsub = "建立页面处理失败！错误:[{0}]";
                if (msg == null)
                {
                    msg = string.Format(msgsub, "建立页面处理出现错误或者返回为空处理");
                }
                else
                    msg = string.Format(msgsub, msg);

                return false;
            }
            Form frm = new Form();
            frm.Size = new Size(800, 600);
            //frm.Icon = SystemIcon;
            frm.Text = mnu.Title;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            frm.Dock = DockStyle.Fill;
            //frm.WindowState = FormWindowState.Maximized;
            //frm.StartPosition = FormStartPosition.CenterParent;
            //objInst.AllowClose = false;
            frm_Model fmodel = new frm_Model();
            fmodel.BehHandleObject = bfh;
            bfh.DataFrm = fmodel;
            WinFormHandle objInst = bfh as WinFormHandle;
            objInst.InitToolBar(fmodel.toolStrip1);
            frm.Controls.Add(fmodel);
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
            ////if (frm.ShowDialog(Container) != DialogResult.Yes)
            ////{
            ////    return false;
            ////}
            return true;
        }

        public override bool CreateWebPage(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool CreateDialoger(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            BaseFormHandle bfh = null;
            msg = null;
            bool ret = this.GetFrameHandle(Container, CurrFormHandle, mnu, ref data, ref msg, ref bfh);
            if (!ret || msg != null || bfh == null)
            {
                string msgsub = "建立页面处理失败！错误:[{0}]";
                if (msg == null)
                {
                    msg = string.Format(msgsub, "建立页面处理出现错误或者返回为空处理");
                }
                else
                    msg = string.Format(msgsub, msg);

                return false;
            }
            Form frm = new Form();
            frm.Size = new Size(800, 600);
            //frm.Icon = SystemIcon;
            frm.Text = mnu.Title;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            frm_Model fmodel = new frm_Model();
            fmodel.BehHandleObject = bfh;
            bfh.DataFrm = fmodel;
            WinFormHandle objInst = bfh as WinFormHandle;
            objInst.InitToolBar(fmodel.toolStrip1);
            frm.Controls.Add(fmodel);
            //frm.Show();
            if (frm.ShowDialog(Container as Form) != DialogResult.Yes)
            {
                return false;
            }
            return true;
        }

        public override bool CreateSelect(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override System.Reflection.Assembly GetAssembly()
        {
            if (Appasmb == null)
            {
                Appasmb = System.Reflection.Assembly.LoadFrom(string.Format("{0}", GlobalShare.SystemAppInfo.AssemName));
            }
            return Appasmb;
        }

        //无用的类型处理
        public override bool CreateTagFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            BaseFormHandle bfh = null;
            msg = null;
            bool ret = this.GetFrameHandle(Container, CurrFormHandle, mnu, ref data, ref msg, ref bfh);
            if (!ret || msg != null || bfh == null)
            {
                string msgsub = "建立页面处理失败！错误:[{0}]";
                if (msg == null)
                {
                    msg = string.Format(msgsub, "建立页面处理出现错误或者返回为空处理");
                }
                else
                    msg = string.Format(msgsub, msg);

                return false;
            }
            Form frm = new Form();
            frm.Size = new Size(800, 600);
            //frm.Icon = SystemIcon;
            frm.Text = mnu.Title;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            //frm.WindowState = FormWindowState.Maximized;
            //frm.StartPosition = FormStartPosition.CenterParent;
            //objInst.AllowClose = false;
            frm_Model fmodel = new frm_Model();
            fmodel.BehHandleObject = bfh;
            bfh.DataFrm = fmodel;
            WinFormHandle objInst = bfh as WinFormHandle;
            objInst.InitToolBar(fmodel.toolStrip1);
            frm.Controls.Add(fmodel);
            frm.Show();
            ////if (frm.ShowDialog(Container) != DialogResult.Yes)
            ////{
            ////    return false;
            ////}
            return true;
        }

    }
}
