using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace WolfInv.Com.HtmlExp
{
    public partial class frm_UpdateXml : Form
    {
        public string _Xml;
        public string Xml
        {
            get
            {
                return _Xml;
            }
        }
        public frm_UpdateXml(string xml)
        {
            InitializeComponent();
            _Xml = xml;
            this.txt_xml.Text = xml;
        }



        private void button_ok_Click(object sender, EventArgs e)
        {
            _Xml = this.txt_xml.Text;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(_Xml);
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
                return;
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btn_skip_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}