using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfInv.Com.JsLib;
namespace jdyInterfaceLib
{

    public abstract class JDYSCM_Class: JdyRequestClass
    {
        public string dbId { get; set; } //账套Id
        public string code { get; set; }
        public string msg { get; set; }
        public int page { get; set; }   //当前页码
        public int totalsize { get; set; }  //当前返回总记录数
        public int records { get; set; }    //总记录数
        public int totalPages { get; set; }	//总页数
        //public List<JDYSCM_Item_Class> items { get; set; }

        public class JDYSCM_Item_Class:JdyJsonClass
        {

        }

        
    }

    public abstract class JDYSCM_Bussiness_Class: JDYSCM_Class
    {

        public bool InitRequestJson()
        {
            base.InitRequestJson();
            ReqJson = string.Format("access_token={0}&dbId={1}", jdy_GlbObject.Access_token, jdy_GlbObject.dbId);

            return true;

        }

        public string getUrl()
        {
            string url = string.Format("{0}?{1}", ReqUrl, ReqJson);
            return url;
        }

        public new string PostRequest()
        {
            if (ReqJson == null)
            {
                if (!this.InitRequestJson())
                {
                    //return null;
                }
            }
            
            return AccessWebServerClass.PostData(getUrl(), this.Req_PostData??"", Encoding.UTF8);
        }
    }

    public abstract class JDYSCM_Bussiness_List_Class : JDYSCM_Bussiness_Class
    {
        public JDYSCM_Bussiness_Filter_Class filter;
        public class JDYSCM_Bussiness_Filter_Class:JsonableClass<JDYSCM_Bussiness_Filter_Class>
        {
            //public string updTimeBegin { get; set; }//": "2019-01-08",
            //public string updTimeEnd { get; set; }//":"2019-10-09",
            public int pageSize { get; set; }//":20,
            public int page { get; set; }//":1
        }
    }


    public class JDYSCM_Service_List_Class: JDYSCM_Class
    {
        public new List<JDYSCM_Service_List_Item_Class> items { get; set; }
        public class JDYSCM_Service_List_Item_Class : JDYSCM_Class.JDYSCM_Item_Class
        {
            public string dbId { get; set; }
            public bool isFree {get;set;}
            public string name { get; set; }
            public string endDate { get; set; }
            public string beginDate { get; set; }
            //// "dbId": 7950938951,
            ////"isFree": false,
            ////"name": "在线进销存3.0（标准版）",
            ////"endDate": "2022-02-24",
            ////"beginDate": "2017-02-24"

        }

        public bool InitRequestJson()
        {
            base.InitRequestJson();
            ReqJson = string.Format("access_token={0}", jdy_GlbObject.Access_token);

            return true;

        }
    }

    
    public class JDYSCM_Product_List_Class: JDYSCM_Bussiness_List_Class
    {
        public JDYSCM_Product_List_Class()
        {

        }

        public List<JDYSCM_Bussiness_Item_Product_List_Class> items { get; set; }

        public class JDYSCM_Bussiness_Filter_Product_Class : JDYSCM_Bussiness_List_Class.JDYSCM_Bussiness_Filter_Class
        {
            public string productNumber { get; set; }
	         public string productName { get; set; }//": "iphone7",
	         public string spec { get; set; }//": "64G",
	         public int status { get; set; }//": 0,
	         public string categoryId { get; set; }//":""
	         
        }

        public class JDYSCM_Bussiness_Item_Product_List_Class
        {
            public string id { get; set; }
            public string productNumber { get; set; }
            public string productName { get; set; }
            public string spec { get; set; }
            public string categoryId { get; set; }
            public string categoryName { get; set; }
            public string unitType { get; set; }
            public string unitTypeName { get; set; }
            public string unit { get; set; }
            public string unitName { get; set; }
            public string barcode { get; set; }
            public string primaryStock { get; set; }
            public string retailPrice { get; set; }
            public string wholeSalePrice { get; set; }
            public string vipPrice { get; set; }
            public string discount { get; set; }
            public string discount2 { get; set; }
            public string elsPurPrice { get; set; }
            public bool isDeleted { get; set; }
            public DateTime createTime { get; set; }
            public DateTime updateTime { get; set; }
            public bool isWarranty { get; set; }
            public int safeDays { get; set; }
            public int advanceDays { get; set; }
        }
    }
    public class JDYSCM_Product_Add_Class : JDYSCM_Bussiness_Class
    {
        public List<JDYSCM_Bussiness_Item_Product_Add_Class> items;
        public class JDYSCM_Bussiness_Item_Product_Add_Class:JsonableClass<JDYSCM_Bussiness_Item_Product_Add_Class>
        {
            public string productNumber { get; set; }
            public string productName { get; set; }
            public string spec { get; set; }
            public string category { get; set; }
            public string unitType { get; set; }
            public string unit { get; set; }
            public string barcode { get; set; }
            public string primaryStock { get; set; }
            public int retailPrice { get; set; }
            public int wholeSalePrice { get; set; }
            public int vipPrice { get; set; }
            public int discount { get; set; }
            public int discount2 { get; set; }
            public int elsPurPrice { get; set; }
        }
    }
    public class JDYSCM_SaleOrder_List_Class : JDYSCM_Bussiness_List_Class
    {
        }
    public class JDYSCM_SaleOrder_Add_Class : JDYSCM_Bussiness_Class
    {
        public List<JDYSCM_SaleOrder_List_Item_Class> items { get; set; }
        public class JDYSCM_SaleOrder_List_Item_Class:JsonableClass<JDYSCM_SaleOrder_List_Item_Class>
        {
            public string number { get; set; }
            public string date { get; set; }
            public string customerNumber { get; set; }
            public string employeeNumber { get; set; }
            public string discRate { get; set; }
            public string discAmt { get; set; }
            public string deliveryDate { get; set; }
            public string remark { get; set; }
            public List<SaleOrderProduct> entries { get; set; }

        }

        public class SaleOrderProduct
        {
            public string productNumber { get; set; }
            public string skuCode { get; set; }
            public string skuId { get; set; }
            public string unit { get; set; }
            public string location { get; set; }
            public int qty { get; set; }
            public string price { get; set; }
            public int discRate { get; set; }
            public int discAmt { get; set; }
            public string remark { get; set; }

        }

    }

    public class JDYSCM_Product_Unit_List_Class: JDYSCM_Bussiness_List_Class
    {
        public List<Product_Unit_Class> items { get; set; }
        public class Product_Unit_Class
        {
            public string name { get; set; }
            public string ID { get; set; }
            public List<Product_Unit_Item> entries { get; set; }
        }

        public class Product_Unit_Item
        {
            public string name { get; set; }
            public string ID { get; set; }
        }
    }

    public class JDYSCM_WareHouse_List_Class: JDYSCM_Bussiness_List_Class
    {
        public List<WareHouse_List_Item> items { get; set; }
        //https://api.kingdee.com/jdyscm/warehouse/list
        public class WareHouse_List_Item: SimleListItem
        {
            public bool isDeleted { get; set; }
        }
    }

    public class JDYSCM_Supplier_List_Class: JDYSCM_Bussiness_List_Class
    {
        public List<Supplier_Litem_Item> items{ get; set; }
        //https://api.kingdee.com/jdyscm/supplier/list
        public class Supplier_Litem_Item: SimleListItem
        {
            
            public string contact { get; set; }
        }
    }

    public class JDYSCM_Customer_List_Class : JDYSCM_Bussiness_List_Class
    {
        public List<Customer_Litem_Item> items { get; set; }
        //https://api.kingdee.com/jdyscm/supplier/list
        public class Customer_Litem_Item : SimleListItem
        {
            public string id { get; set; }


            public string category { get; set; }
            public string level { get; set; }
            public string employeeName { get; set; }
            public string difMoney { get; set; }
            public string taxPayerNo { get; set; }
            public string bank { get; set; }
            public string cardNo { get; set; }
            public string beginDate { get; set; }
            public long amount { get; set; }
            public long periodMoney { get; set; }
            public string remark { get; set; }
            public bool isDeleted { get; set; }
            public string createTime { get; set; }
            public string updateTime { get; set; }
            public List<Customer_Contactors_Item> contacts { get; set; }
        }

        public class Customer_Contactors_Item
        {
            public string name { get; set; }
            public string im { get; set; }
            public string mobile { get; set; }
            public string id { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public bool isPrimary { get; set; }
        }
    }

    public class SimleListItem
    {
        public string number { get; set; }
        public string name { get; set; }
    }
}
