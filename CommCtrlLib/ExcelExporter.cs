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
            //ȡ�ñ��� 
            // 
            ////foreach (DataGridColumn col in gridobj.Columns)
            ////{
            ////    colIndex++;
            ////    excel.Cells[4, colIndex] = col.Text;
            ////    xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[4, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//���ñ����ʽΪ���ж��� 
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
                xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//���ñ����ʽΪ���ж��� 

                
            }

            // 
            //ȡ�ñ���е����� 
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
                        xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//���������͵��ֶθ�ʽΪ���ж��� 
                    }
                    else
                        if (col.DataType == "text")
                        {
                            excel.Cells[rowIndex, colIndex] = row.SubItems[i].Text;
                            xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//�����ַ��͵��ֶθ�ʽΪ���ж��� 
                        }
                        else
                        {
                            excel.Cells[rowIndex, colIndex] = row.SubItems[i].Text;
                        }
                }
            }
            // 
            //����һ���ϼ��� 
            // 
            ////int rowSum = rowIndex + 1;
            ////int colSum = StartColumn+1;
            ////excel.Cells[rowSum, colSum] = "�ϼ�";
            ////xSt.get_Range(excel.Cells[rowSum, 2], excel.Cells[rowSum, 2]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //////////////// 
            ////////////////����ѡ�еĲ��ֵ���ɫ 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]).Select();
            //////////////xSt.get_Range(excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]).Interior.ColorIndex = 19;//����Ϊǳ��ɫ��������56�� 
            //////////////// 
            ////////////////ȡ����������ı��� 
            //////////////// 
            ////////////////excel.Cells[2, 2] = str;
            //////////////// 
            ////////////////������������ı����ʽ 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, 2]).Font.Bold = true;
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, 2]).Font.Size = 22;
            //////////////// 
            ////////////////���ñ�����Ϊ����Ӧ��� 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Select();
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Columns.AutoFit();
            //////////////// 
            ////////////////������������ı���Ϊ���о��� 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, colIndex]).Select();
            //////////////xSt.get_Range(excel.Cells[2, 2], excel.Cells[2, colIndex]).HorizontalAlignment = XlHAlign.xlHAlignCenterAcrossSelection;
            //////////////// 
            ////////////////���Ʊ߿� 
            //////////////// 
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, colIndex]).Borders.LineStyle = 1;
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[rowSum, 2]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;//��������߼Ӵ� 
            //////////////xSt.get_Range(excel.Cells[4, 2], excel.Cells[4, colIndex]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;//�����ϱ��߼Ӵ� 
            //////////////xSt.get_Range(excel.Cells[4, colIndex], excel.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;//�����ұ��߼Ӵ� 
            //////////////xSt.get_Range(excel.Cells[rowSum, 2], excel.Cells[rowSum, colIndex]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;//�����±��߼Ӵ� 
            //////////////// 
            ////////////////��ʾЧ�� 
            //////////////// 
            excel.Visible = true;

            //xSt.Export(Server.MapPath(".")+"\\"+this.xlfile.Text+".xls",SheetExportActionEnum.ssExportActionNone,Microsoft.Office.Interop.OWC.SheetExportFormat.ssExportHTML); 
            try
            {
                xBk.SaveCopyAs(path);

                // ds = null;
                xBk.Close(false, null, null);

                excel.Quit();
                MessageBox.Show("�����ɹ���");
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
