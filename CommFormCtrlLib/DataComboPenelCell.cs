using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;
using System;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    public class DataComboPenelCell : PanelCell,IDataSourceable
    {

        public string DataSourceName { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string ComboItemsSplitString { get; set; }
        public DataComboPenelCell(string uid)
            : base(uid)
        {

        }
        public DataComboPenelCell(XmlNode node, string uid)
            : base(node, uid)
        {

        }

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            DataSourceName = XmlUtil.GetSubNodeText(node, "@datasource");
            ValueField = XmlUtil.GetSubNodeText(node, "@valmember");
            TextField = XmlUtil.GetSubNodeText(node, "@txtmember");
            ComboItemsSplitString = XmlUtil.GetSubNodeText(node, "@membersplitor");
            mnu = MenuProcess.GetMenu(null, node, strUid);
        }

        protected override Control GetControl()
        {
            DataComboBox ctrl = new DataComboBox(DataSourceName, strUid);
            ctrl.TextField = TextField;
            ctrl.ValueField = ValueField;
            ctrl.ComboItemsSplitString = ComboItemsSplitString;
            ctrl.Name = this.Field;
            UpdateData data = new UpdateData();
            FrameSwitch.FillTranData(this.OwnRow.OwnPanel.OwnerForm, ctrl,ref mnu, ref data);
            ctrl.NeedUpdateData = data;
            ctrl.ChangeDataSource();
            ctrl.SelectedIndexChanged += OnControlValueChanged;
            ctrl.Width = this.Width;
            ctrl.Enabled = !this.ReadOnly;
            return ctrl;
        }



        protected override void OnControlValueChanged(object sender, EventArgs e)
        {
            
            base.OnControlValueChanged(sender, e);

            List<DataChoiceItem> datasource = (this.ItemControl as DataComboBox).DataSource as List<DataChoiceItem>;
            if (datasource == null)
                return;
            DataComboBox dcb = this.ItemControl as DataComboBox;
            DataChoiceItem drci = null;
            if (dcb.SelectedIndex < 0)
            {
                if (dcb.Items.Count > 0)
                {
                    drci = datasource[0] ;
                }
                else
                {
                    return;
                }
            }
            else
            {
                drci = datasource[dcb.SelectedIndex] ;
            }
            foreach (string dpt in drci.Data.Items.Keys)
            {
                if (dpt == this.Field)
                    continue;
                if (this.OwnRow.OwnPanel.ControlList.ContainsKey(dpt) && dcb.SelectedIndex >= 0) //如果存在相同项目，以值填充
                {
                    this.OwnRow.OwnPanel.ControlList[dpt].ChangeValue(drci.Data.Items[dpt].value, drci.Data.Items[dpt].text);
                }
            }

        }


        public void RefreshItems()
        {
            DataComboBox ctrl = ItemControl as DataComboBox;
            ctrl.ChangeDataSource();
        }
    }


}

