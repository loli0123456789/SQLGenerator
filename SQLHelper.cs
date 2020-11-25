using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLGenerator
{
    public class SQLHelper
    {
        public string ConnectionString { set; get; }

        public SqlConnection SqlConnection;

        public SQLHelper(string conString)
        {
            ConnectionString = conString;
        }

        //连接数据库
        public void OpenDB()
        {

            if (SqlConnection == null)
                SqlConnection = new SqlConnection(ConnectionString);
            if (SqlConnection.State == ConnectionState.Open)
                SqlConnection.Close();
            SqlConnection.Open();
        }

        //关闭数据库
        public void CloseDB()
        {
            try
            {
                SqlConnection.Close();
            }
            catch { }
        }

        public int ExecuteSQL(String sql)
        {
            //int cmd = 0;
            try
            {
                OpenDB();
                SqlCommand cmm = new SqlCommand(sql, SqlConnection);
                int cmd = cmm.ExecuteNonQuery();
                CloseDB();
                return cmd;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //执行Select，返回DataReader
        public SqlDataReader GetDataReader(String sql)
        {
            try
            {
                SqlDataReader dr;
                OpenDB();
                SqlCommand cmm = new SqlCommand(sql, SqlConnection);
                dr = cmm.ExecuteReader();
                //CloseDB();
                return dr;

            }
            catch (Exception e)
            {

                throw e;

            }
            finally
            {
                CloseDB();
            }
        }

        //获取DataTable
        public DataTable GetDataTable(string CmdString)
        {
            try
            {
                OpenDB();
                SqlDataAdapter myDa = new SqlDataAdapter();
                myDa.SelectCommand = new SqlCommand(CmdString, SqlConnection);
                DataTable myDt = new DataTable();
                myDa.Fill(myDt);

                return myDt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseDB();
            }
        }
    }
}
