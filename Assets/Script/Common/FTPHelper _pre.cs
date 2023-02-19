using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Net;

public class FTPHelper_
{
    public string ftpURI;
    public string ftpUserID;
    public string ftpPassword;

    public void Init(string ftpURI, string ftpUserID, string ftpPassword)
    {
        this.ftpURI = ftpURI;
        this.ftpUserID = ftpUserID;
        this.ftpPassword = ftpPassword;
    }

    /// <summary>
    /// 新建目录
    /// </summary>
    /// <param name="ftpPath"></param>
    /// <param name="dirName"></param>
    public void MakeDir(string ftpPath, string dirName)
    {
        try
        {
            var resultURI = ftpPath + dirName;
            //实例化FTP连接
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(resultURI));
            request.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
            //指定FTP操作类型为创建目录
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            //获取FTP服务器的响应
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }
        catch (Exception ex)
        {
            Logx.LogError(ex.Message);
        }
    }

    /// <summary>
    /// 检查目录是否存在
    /// </summary>
    /// <param name="ftpPath">要检查的目录的上一级目录</param>
    /// <param name="dirName">要检查的目录名</param>
    /// <returns>存在返回true，否则false</returns>
    public bool CheckDirectoryExist(string ftpPath, string dirName)
    {
        bool result = false;
        try
        {
            //实例化FTP连接
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
            //指定FTP操作类型为获得目录
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
            //获取FTP服务器的响应
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

            StringBuilder str = new StringBuilder();
            string line = sr.ReadLine();
            while (line != null)
            {
                str.Append(line);
                str.Append("|");
                line = sr.ReadLine();
            }

            Debug.Log("---str : " + str);

            string[] datas = str.ToString().Split('|');


            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].Contains("<DIR>"))
                {
                    int index = datas[i].IndexOf("<DIR>");
                    string name = datas[i].Substring(index + 5).Trim();
                    if (name == dirName)
                    {
                        result = true;
                        break;
                    }
                }
            }

            sr.Close();
            sr.Dispose();
            response.Close();
        }
        catch (Exception ex)
        {
            Logx.LogError(ex.Message);
        }
        return result;
    }

    #region 上传文件
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="localFile">要上传到FTP服务器的文件</param>
    /// <param name="ftpPath"></param>
    public void UpLoadFile(string localFile, string ftpPath)
    {
        var ftpUser = this.ftpUserID;
        var ftpPassword = this.ftpPassword;
        //ftpPath = this.ftpURI + "/" + ftpPath;
     

        //检测本地文件
        if (!File.Exists(localFile))
        {
            Logx.Log("文件：“" + localFile + "” 不存在！");
            return;
        }

        ////检测远端目录
        //var dirName = Path.GetDirectoryName(ftpPath);
        //if (!CheckDirectoryExist(this.ftpURI, dirName))
        //{
        //    MakeDir(dirName);
        //}


        FtpWebRequest ftpWebRequest = null;
        FileStream localFileStream = null;
        Stream requestStream = null;
        try
        {
            ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
            ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.ContentLength = localFile.Length;
            int buffLength = 4096;
            byte[] buff = new byte[buffLength];
            int contentLen;
            localFileStream = new FileInfo(localFile).OpenRead();
            requestStream = ftpWebRequest.GetRequestStream();
            contentLen = localFileStream.Read(buff, 0, buffLength);
            while (contentLen != 0)
            {
                requestStream.Write(buff, 0, contentLen);
                contentLen = localFileStream.Read(buff, 0, buffLength);
            }
        }
        catch (Exception ex)
        {
            Logx.LogError(ex.Message);
        }
        finally
        {
            if (requestStream != null)
            {
                requestStream.Close();
            }
            if (localFileStream != null)
            {
                localFileStream.Close();
            }
        }
    }
    #endregion


    #region 获取目录下的详细信息
    /// <summary>
    /// 获取目录下的详细信息
    /// </summary>
    /// <param name="localDir">本机目录</param>
    /// <returns></returns>
    private List<List<string>> GetDirDetails(string localDir)
    {
        List<List<string>> infos = new List<List<string>>();
        try
        {
            infos.Add(Directory.GetFiles(localDir).ToList()); //获取当前目录的文件

            infos.Add(Directory.GetDirectories(localDir).ToList()); //获取当前目录的目录

            for (int i = 0; i < infos[0].Count; i++)
            {
                int index = infos[0][i].LastIndexOf(@"\");
                infos[0][i] = infos[0][i].Substring(index + 1);
            }
            for (int i = 0; i < infos[1].Count; i++)
            {
                int index = infos[1][i].LastIndexOf(@"\");
                infos[1][i] = infos[1][i].Substring(index + 1);
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
        return infos;
    }
    #endregion


    /// <summary>
    /// 上传整个目录
    /// </summary>
    /// <param name="localDir">要上传的目录的上一级目录</param>
    /// <param name="ftpPath">FTP路径</param>
    /// <param name="dirName">要上传的目录名</param>
    /// <param name="ftpUser">FTP用户名（匿名为空）</param>
    /// <param name="ftpPassword">FTP登录密码（匿名为空）</param>
    public void UploadDirectory(string localDir, string ftpPath, string dirName)
    {
        var ftpUser = this.ftpUserID;
        var ftpPassword = this.ftpPassword;

        string dir = localDir + dirName + @"\"; //获取当前目录（父目录在目录名）

        if (!Directory.Exists(dir))
        {
            Logx.Log("目录：“" + dir + "” 不存在！");
            return;
        }

        var isExist = CheckDirectoryExist(ftpPath, dirName);
        Debug.Log("isExist : " + isExist + " " + ftpPath + " " + dirName);
        if (!isExist)
        {
            MakeDir(ftpPath, dirName);
        }



        List<List<string>> infos = GetDirDetails(dir); //获取当前目录下的所有文件和文件夹

        //先上传文件
        //MyLog.ShowMessage(dir + "下的文件数：" + infos[0].Count.ToString());
        for (int i = 0; i < infos[0].Count; i++)
        {
            Console.WriteLine(infos[0][i]);
            Debug.Log("开始上传文件 : " + infos[0][i]);
            UpLoadFile(dir + infos[0][i], ftpPath + dirName + @"/" + infos[0][i]);
            Debug.Log("完成上传文件 : " + infos[0][i]);
        }
        //再处理文件夹
        //MyLog.ShowMessage(dir + "下的目录数：" + infos[1].Count.ToString());
        for (int i = 0; i < infos[1].Count; i++)
        {
            UploadDirectory(dir, ftpPath + dirName + @"/", infos[1][i]);
        }
    }

    //public void Upload(string localfile, string ftpfile)//, System.Windows.Forms.ProgressBar pb
    //{
    //    FileInfo fileInf = new FileInfo(localfile);
    //    FtpWebRequest reqFTP;
    //    var resultURI = ftpURI + "/" + ftpfile;
    //    Logx.Log("resultURI : " + resultURI);


    //    if (!CheckDirectoryExist(ftpPath, dirName))
    //    {
    //        MakeDir(ftpPath, dirName);
    //    }

    //    //Logx.Log("upload uri : " + resultURI);
    //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(resultURI));
    //    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
    //    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
    //    reqFTP.KeepAlive = false;
    //    reqFTP.UseBinary = true;
    //    reqFTP.ContentLength = fileInf.Length;
    //    //if (pb != null)
    //    //{
    //    //    pb.Maximum = Convert.ToInt32(reqFTP.ContentLength / 2048);
    //    //    pb.Maximum = pb.Maximum + 1;
    //    //    pb.Minimum = 0;
    //    //    pb.Value = 0;
    //    //}
    //    int buffLength = 2048;
    //    byte[] buff = new byte[buffLength];
    //    int contentLen;
    //    FileStream fs = fileInf.OpenRead();
    //    try
    //    {
    //        Stream strm = reqFTP.GetRequestStream();
    //        contentLen = fs.Read(buff, 0, buffLength);
    //        while (contentLen != 0)
    //        {
    //            strm.Write(buff, 0, contentLen);
    //            //if (pb != null)
    //            //{
    //            //    if (pb.Value != pb.Maximum)
    //            //        pb.Value = pb.Value + 1;
    //            //}
    //            contentLen = fs.Read(buff, 0, buffLength);
    //            //System.Windows.Forms.Application.DoEvents();
    //        }
    //        //if (pb != null)
    //        //    pb.Value = pb.Maximum;
    //        //System.Windows.Forms.Application.DoEvents();
    //        strm.Close();
    //        fs.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }

    //}
}