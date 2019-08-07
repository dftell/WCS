using System;
using System.Collections.Generic;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.WCS_Process;
using System.Windows.Forms;
using System.Reflection;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommCtrlLib
{
    public class ObjectSwith
    {
        public static void FillTranData(IKeyForm currfrm, ITranslateableInterFace ifrm, CMenuItem mnu, ref UpdateData data)
        {
            if (data == null) data = new UpdateData();
            ifrm.NeedUpdateData = data;//传入
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

    }


}
