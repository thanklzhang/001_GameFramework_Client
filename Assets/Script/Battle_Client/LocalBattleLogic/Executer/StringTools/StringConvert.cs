using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//简单的 文本 to int 类
public class StringConvert
{

    public static List<int> ToIntList(string str, char splitChar = ',')
    {

        if (string.IsNullOrEmpty(str))
        {
            return new List<int>();
        }

        var strs = str.Split(splitChar);
        List<int> list = strs.Select((v) =>
        {
            return int.Parse(v);
        }).ToList();
        return list;
    }
    
    public static List<string> ToStringList(string str, char splitChar = ',')
    {

        if (string.IsNullOrEmpty(str))
        {
            return new List<string>();
        }

        var strs = str.Split(splitChar).ToList();
        return strs;
    }

}

