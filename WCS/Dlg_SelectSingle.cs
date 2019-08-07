using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
namespace WCS
{
    public partial class Dlg_SelectSingle :Dlg_CommModel 
    {
        public Dlg_SelectSingle()
        {
            InitializeComponent();
        }
        public Dlg_SelectSingle(string param)
        {
            InitializeComponent();
            this.strRowId = param;
        }
    }
}