using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace WolfInv.Com.CommCtrlLib
{
    public class ExcelExporter
    {
        public void Export(Grid gridobj, string path)
        {
            Export(gridobj, path, 1, 0);
        }

        public void Export(Grid gridobj,string path,int StartRow,int StartColumn)
        {
            Microsoft.Office.Interop.Excel.Application excel;// = new Application(); 
            int rowIndex = StartRow;
            int colIndex = StartColumn;

            _Workbook xBk;
            _Worksheet xSt;

            excel = new ApplicationClass();

            xBk = excel.Workbooks.Add(true);

            xSt = (_Worksheet)xBk.ActiveSheet;

            // 
            //取得标题 
            // 
            ////foreach (DataGridColumn col in gridobj.Columns)
            ////{
            ////    colIndex++;
            ////    excel.Cells[4, colIndex] = col.Text;
            ////    xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[4, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置标题格式为居中对齐 
            ////}
            for (int i = 0; i < gridobj.Columns.Count; i++)
            {
                DataGridColumn col = gridobj.Columns[i];
                if (gridobj.Columns[i].Hide || gridobj.Columns[i].Width == 0 || !gridobj.Columns[i].Visable)
                {
                    continue;
                }
                colIndex++;
                excel.Cells[rowIndex, colIndex] = col.Text;
                xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置标题格式为居中对齐 

                
            }

            // 
            //取得表格中的数据 
            // 
            foreach (ListGridItem  row in (gridobj.listViewObj as ListGrid).Items)
            {
                rowIndex++;
                colIndex = StartColumn;
                for (int i = 0; i < gridobj.Columns.Count;i++ )
                {
                    if (gridobj.Columns[i].Hide || gridobj.Columns[i].Width == 0 || !gridobj.Columns[i].Visable)
                    {
                        continue;
                    }
                    colIndex++;
                    DataGridColumn col = gridobj.Columns[i];
                    if (col.DataType == "date")
                    {
                        excel.Cells[rowIndex, colIndex] = row.SubItems[i].Text;
                        xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置日期型的字段格式为居中对齐 
                    }
                    else
                        if (col.DataType == "text")
                        {
                            excel.Cells[rowIndex, colIndex] = row.SubItems[i].Text;
                            xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置字符型的字段格式为居中对齐 
                        }
                        else
                        {
                            excel.Cells[rowIndex, colIndex] = row.SubItems[i].Text;
                        }
                }
            }
            // 
            //加载一个合计行 
            // 
            ////int rowSum = rowIndex + 1;
            ////int colSum = StartColumn+1;
            ////excel.Cells[rowSum, colSum] = "合计";
            ////xSt.get_Range(excel.Cells[rowSum, 2], excel.Cells[rowSum, 2]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //////////////// 
            ////////////////设置选中的部分的颜色 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]).Select();
            //////////////xSt.get_Range(excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]).Interior.ColorIndex = 19;//设置为浅黄色，共计有56种 
            //////////////// 
            ////////////////取得整个报表的标题 
            //////////////// 
            ////////////////excel.Cells[2, 2] = str;
            //////////////// 
            ////////////////设置整个报表的标题格式 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, 2]).Font.Bold = true;
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, 2]).Font.Size = 22;
            //////////////// 
            ////////////////设置报表表格为最适应宽度 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Select();
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Columns.AutoFit();
            //////////////// 
            ////////////////设置整个报表的标题为跨列居中 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, colIndex]).Select();
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, colIndex]).HorizontalAlignment = XlHAlign.xlHAlignCenterAcrossSelection;
            //////////////// 
            ////////////////绘制边框 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Borders.LineStyle = 1;
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, 2]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;//设置左边线加粗 
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[4, colIndex]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;//设置上边线加粗 
            //////////////xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;//设置右边线加粗 
            //////////////xSt.get_Range(excel.Cells[rowSum, 2], excel.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;//设置下边线加粗 
            //////////////// 
            ////////////////显示效果 
            //////////////// 
            excel.Visible = true;

            //xSt.Export(Server.MapPath(".")+"\\"+this.xlfile.Text+".xls",SheetExportActionEnum.ssExportActionNone,Microsoft.Office.Interop.OWC.SheetExportFormat.ssExportHTML); 
            try
            {
                xBk.SaveCopyAs(path);

                // ds = null;
                xBk.Close(false, null, null);

                excel.Quit();
                MessageBox.Show("导出成功！");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
                
                
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                xSt = null;
                xBk = null;
                excel = null;
                
                GC.Collect();
            }

        }
    }
}
