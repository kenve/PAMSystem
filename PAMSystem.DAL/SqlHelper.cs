using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAMSystem.DAL
{
    /// <summary>
    /// 
    /// </summary>
    ///  
    public static class SqlHelper
    {
       
        //获取配置文件中的连接字符串
        private static readonly string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
       
        // 执行insert、delete、update的方法
        public static int ExecuteNonQuery(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    //如果pms为null,则直接调用cmd.Parameters.AddRange(pms)会报错。
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }

            }
        }
        
        // 执行sql语句，返回单个值。
 
        public static object ExecuteScalar(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    //如果pms为null,则直接调用cmd.Parameters.AddRange(pms)会报错。
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }

            }
        }
        /// <summary>
        /// 执行sql语句返回一个DataReader
        /// 当返回DataReader的时候，注意：
        /// 1.Connection不能关闭
        /// 2.DataReader不能关闭
        /// 3.command对象执行ExecuteReader()的时候需要传递一个参数CommandBehavior.CloseConnection
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(string sql, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(constr);
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    //当调用ExecuteReader()方法的时候，如果传递一个CommandBehavior.CloseConnection参数，则表示将来当用户关闭reader的时候，系统会自动将Connection也关闭掉。

                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                }
                throw;
            }
        }
        
        // 封装一个返回DataTable的方法。
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, constr))
            {
                if (pms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(pms);
                }
                sda.Fill(dt);
            }
            return dt;
        }

    }
}