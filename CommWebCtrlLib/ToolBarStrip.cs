using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WolfInv.Com.CommCtrlLib;
namespace WolfInv.Com.CommWebCtrlLib
{
    public class ToolBarStrip:Panel,ITag
    {
        object _Tag;
        public object Tag { get { return _Tag; } set { _Tag = value; } }
        
        public ToolBarStrip():base()
        {
            _Items = new ToolBarStripItemCollection(this);
            this.Height = new Unit(25);
            
            
        }


        ToolBarStripItemCollection _Items;
        public ToolBarStripItemCollection Items
        {
            get
            {
                return _Items;
            }
        }

     

        

        

    }

    public class ToolBarStripItemCollection :ControlCollection
    {
        protected ToolBarStrip Container;
        protected ControlCollection _List
        {
            get
            {
                return Container.Controls;
            }
        }
        public ToolBarStripItemCollection(Control owner):base(owner)
        {
            Container = owner as ToolBarStrip;
            //Container.Controls.Clear();
        }

        public void Add(ToolBarStripItem child)
        {
            _List.Add(child.ActiveControl);
        }

        public void Remove(ToolBarStripItem child)
        {
            _List.Remove(child.ActiveControl);
        }

        public void Clear()
        {
            _List.Clear();
        }

        public int Count
        {
            get { return _List.Count; }
        }
  }

    public class ToolBarStripItem :Control,ITag
    {
        object _Tag;
        public object Tag { get { return _Tag; } set { _Tag = value; } }
        public System.Web.UI.WebControls.WebControl ActiveControl;
        public string Text;
        public string Name;
        public bool Enabled
        {
            get
            {
                return ActiveControl.Visible;
            }
            set
            {
                ActiveControl.Visible = value;
            }
        }
        public ToolBarStripItem()
        {
            
        }

        public ToolBarStripItem(string name)
        {
            this.Text = name;
        }
        public event EventHandler Click;

        public Unit Width { get { return this.ActiveControl.Width; } set { this.ActiveControl.Width = value; } }
        public Unit Height { get { return this.ActiveControl.Height; } set { this.ActiveControl.Height = value; } }
    }

    public class ToolBarStripButton : ToolBarStripItem
    {

        public ToolBarStripButton() : base() {
            ActiveControl = new Button();
        }
        public ToolBarStripButton(string name):base(name)

        {
            ActiveControl = new Button();
            (ActiveControl as Button).Text = name;
        }

        

        public new string Text { get { return (ActiveControl as Button).Text; } set { (ActiveControl as Button).Text = value; } }
    }

    public class ToolBarStripSeparator : ToolBarStripItem
    {
        const string ImgPath = "ITMS/imgs/Separator.gif";
        public ToolBarStripSeparator() : base() {
            
            Image img = new Image();
            img.ImageUrl =  ImgPath;
            ActiveControl = img;
            
        }
    }

    public class ToolBarStripLabel : ToolBarStripItem
    {
        public ToolBarStripLabel() : base() {
            ActiveControl = new Label();
        }

        public ToolBarStripLabel(string name):base(name)
        {
            ActiveControl = new Label();
            (ActiveControl as Label).Text = name;
        }
        public new  string Text { get { return (ActiveControl as Label).Text; } set { (ActiveControl as Label).Text = value; } }
    }
    public class ToolStripTextBoxD : ToolBarStripItem
    {
        public ToolStripTextBoxD() : base() {
            ActiveControl = new TextBox();
            //KeyUp = (ActiveControl as TextBox).
        }
        public event EventHandler KeyUp;
        public string Text { get { return (ActiveControl as TextBox).Text; } set { (ActiveControl as TextBox).Text = value; } }

        
    }
    public class ToolBarStripComobox : ToolBarStripItem
    {
        public event EventHandler SelectedIndexChanged;
        public ToolBarStripComobox() : base() {
            ActiveControl = new DropDownList();
        }
        public int SelectedIndex
        {
            get
            {
                return (ActiveControl as DropDownList).SelectedIndex;
            }
            set
            {
                (ActiveControl as DropDownList).SelectedIndex = value;
            }
            
        }

        public ListItemCollection Items
        {
            get
            {
                return (ActiveControl as DropDownList).Items;
            }
            

        }
    }
}
