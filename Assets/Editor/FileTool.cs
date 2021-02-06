using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

public class FileTool
{
    /// <summary>
    /// 删除指定文件目录下的所有文件
    /// </summary>
    /// <param name="fullPath">文件路径</param>
    public static void DeleteAllFile(string fullPath)
    {

        foreach (string d in Directory.GetFileSystemEntries(fullPath))
        {

            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);//直接删除其中的文件  
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);
                if (d1.GetFiles().Length != 0)
                {
                    DeleteAllFile(d1.FullName); //递归删除子文件夹
                }
                Directory.Delete(d);
            }
        }

    }

}
