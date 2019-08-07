using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfInv.Com.ExcelIOLib
{
    public class StringProcessClass
    {

        public static string SearchMaxSubStr(List<string> list)
        {
            List<string> ret = new List<string>();
            int minLen = list.Min(a => a.Length);
            string strMin = list.Where(a => a.Length == minLen).First();//找到第一个最小长度字符串
            for(int i=minLen;i>=1;i--)
            {
                for(int j=0;j<=minLen-i;j++)
                {
                    string selStr = strMin.Substring(j, i);//选取要比较的字符串
                    int MatchCnt = 0;
                    list.ForEach(a =>
                    {
                        if(a.IndexOf(selStr)>=0)//如果包含该串计数器+1
                        {
                            MatchCnt++;
                        }
                    });//遍历比较串列表
                    if(MatchCnt==list.Count)
                    {
                        if(selStr.Length > 1)//单个字符不考虑
                        {
                            return selStr;
                        }
                    }
                }
            }
            return null;

        }
    
    }

}

