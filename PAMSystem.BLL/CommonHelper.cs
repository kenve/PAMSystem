using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace PAMSystem.BLL
{
    public class CommonHelper
    {

        //字符串转换为MD5
        public static string GetMD5(string sDataIn)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear(); //清除MD5
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }
        //把一些可能会变的值写入app.config
        //读取密码盐
        public static string GetPasswordSalt()
        {
            //从配置文件中（AppSettings节点中）取盐
            string salt = ConfigurationManager.AppSettings["passwordSalt"];
            return salt;
        }
    }
}
