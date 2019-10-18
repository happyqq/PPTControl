using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class Encrypt
{
    public static string GetMD5(string sDataIn)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bytValue, bytHash;
        bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
        bytHash = md5.ComputeHash(bytValue);
        md5.Clear();
        string sTemp = "";
        for (int i = 0; i < bytHash.Length; i++)
        {
            sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
        }
        return sTemp.ToLower();
    }

    #region TripleDES加密
    /// <summary>
    /// TripleDES加密
    /// </summary>
    public static string TripleDESEncrypting(string strSource)
    {
        try
        {
            byte[] bytIn = Encoding.Default.GetBytes(strSource);
            byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 11, 167, 44, 80, 26, 20, 145, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
            byte[] IV = { 55, 103, 245, 79, 36, 99, 167, 8 };  //定义偏移量
            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.IV = IV;
            TripleDES.Key = key;
            ICryptoTransform encrypto = TripleDES.CreateEncryptor();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            byte[] bytOut = ms.ToArray();
            return System.Convert.ToBase64String(bytOut);
        }
        catch (Exception ex)
        {
            throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
        }
    }
    #endregion

    #region TripleDES解密
    /// <summary>
    /// TripleDES解密
    /// </summary>
    public static string TripleDESDecrypting(string Source)
    {
        try
        {
            byte[] bytIn = System.Convert.FromBase64String(Source);
            byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 11, 167, 44, 80, 26, 20, 145, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
            byte[] IV = { 55, 103, 245, 79, 36, 99, 167, 8 };   //定义偏移量
            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.IV = IV;
            TripleDES.Key = key;
            ICryptoTransform encrypto = TripleDES.CreateDecryptor();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader strd = new StreamReader(cs, Encoding.Default);
            return strd.ReadToEnd();
        }
        catch (Exception ex)
        {
            throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
        }
    }
    #endregion
}