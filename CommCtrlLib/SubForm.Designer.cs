using System.Runtime.InteropServices;
using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    partial class SubForm//:XWinForm_Form
    {
        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("User32.dll ")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("Kernel32.dll ")]
        private static extern int GetLastError();

        //标题栏按钮的矩形区域。 

        Rectangle m_rect = new Rectangle(205, 6, 20, 20);


        protected override void WndProc(ref   Message m)
        {

            base.WndProc(ref   m);
            return;
            switch (m.Msg)
            {

                case 0x86://WM_NCACTIVATE 
                    goto case 0x85;
                case 0x85://WM_NCPAINT 
                    {
                        IntPtr hDC = GetWindowDC(m.HWnd);
                        //把DC转换为.NET的Graphics就可以很方便地使用Framework提供的绘图功能了 
                        Graphics gs = Graphics.FromHdc(hDC);
                        gs.FillRectangle(new LinearGradientBrush(m_rect, Color.Pink, Color.Purple, LinearGradientMode.BackwardDiagonal), m_rect);
                        StringFormat strFmt = new StringFormat();
                        strFmt.Alignment = StringAlignment.Center;
                        strFmt.LineAlignment = StringAlignment.Center;
                        //gs.DrawString("√ ", this.Font, Brushes.BlanchedAlmond, m_rect, strFmt);
                        gs.Dispose();
                        //释放GDI资源 
                        ReleaseDC(m.HWnd, hDC);
                        break;
                    }
                case 0xA1://WM_NCLBUTTONDOWN 
                    {
                        Point mousePoint = new Point((int)m.LParam);
                        mousePoint.Offset(-this.Left, -this.Top);
                        if (m_rect.Contains(mousePoint))
                        {
                            MessageBox.Show("hello ");
                        }
                        break;
                    }
            }
        }

        //在窗口大小改变时及时更新按钮的区域。 

        private void Form1_SizeChanged(object sender, System.EventArgs e)
        {
            
            return;
            m_rect.X = this.Bounds.Width - 95;
            m_rect.Y = 6;
            m_rect.Width = m_rect.Height = 20;
        } 

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.Text = FrameSwitch.SystemText ;
            //this.Icon = FrameSwitch.SystemIcon;
        }

        #endregion
    }
}