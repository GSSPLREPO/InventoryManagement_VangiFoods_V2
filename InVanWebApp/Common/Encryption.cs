using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace InVanWebApp.Common
{
    public class Encryption
    {
        public string Encrypt(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string Decrypt(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        #region Decrypt Encrypted String
        /// <summary>
        /// 13/12/2022 Bhandavi
        /// code to decrypt given string
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public static string Decrypt_Static(string conStr)
        {
            string decryptStr = string.Empty;
            UTF8Encoding encodeStr = new UTF8Encoding();
            Decoder Decode = encodeStr.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(conStr);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptStr = new String(decoded_char);
            return decryptStr;
        }
        #endregion
        //public static string ConnectionString
        //{
        //    get
        //    {
        //        SqlConnectionStringBuilder conStr = new SqlConnectionStringBuilder();
        //        conStr.ConnectionString = Decrypt_Static(ConfigurationManager.ConnectionStrings["Db_DeconEntities"].ToString());
        //        EntityConnectionStringBuilder esb = new EntityConnectionStringBuilder();
        //        esb.Provider = "System.Data.SqlClient";
        //        esb.ProviderConnectionString = conStr.ToString();
        //        esb.Metadata = string.Format("res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl",
        //            typeof(Entities).Assembly.FullName);
        //        return esb.ToString();
        //    }
        //}
    }
}