using System;
using System.Data;
using System.IO;
using System.Text;

namespace SQLGenerator
{
    public class CommonHelper
    {
        /// <summary>
        /// 读取txt内容
        /// </summary>
        /// <param name="txtFilePath"></param>
        /// <returns></returns>
        public static string ReadTxt(string txtFilePath)
        {
            using (StreamReader st = new StreamReader(txtFilePath, System.Text.Encoding.UTF8))//UTF8为编码
            {
                return st.ReadToEnd();
            }
        }

        /// <summary>
        /// 保存txt文件内容
        /// </summary>
        /// <param name="txtFilePath"></param>
        /// <param name="txtContent"></param>
        public static void SaveTxt(string txtFilePath, string txtContent)
        {
            using (StreamWriter sw = new StreamWriter(txtFilePath))
            {
                sw.Write(txtContent);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 获取INSERT语句
        /// </summary>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public static StringBuilder GetInsertSQL(string querySql)
        {
            SQLHelper helper = new SQLHelper(MainForm.DBConString);
            DataTable dt = helper.GetDataTable(querySql);
            string tableName = GetTableNameFromQuerySQL(querySql);
            StringBuilder sbSql = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sbSql.Append($"INSERT INTO {tableName} (");
                foreach (DataColumn col in dt.Columns)
                {
                    string columnName = col.ColumnName;
                    sbSql.Append($"{columnName},");
                }
                //去掉最后一个逗号,
                sbSql.Remove(sbSql.Length - 1, 1);
                sbSql.Append(")");
                sbSql.AppendLine();
                sbSql.Append("VALUES(");

                foreach (DataColumn col in dt.Columns)
                {
                    string columnName = col.ColumnName;
                    string field = string.Empty;
                    //NULL特殊处理，避免全部为''导致类型转换错误
                    if (row[col] == DBNull.Value)
                    {
                        field = "NULL";
                    }
                    else
                    {
                        field = Convert.ToString(row[col]);
                        //替换单引号'为两个单引号''
                        field = field.Replace("'", "''");
                        field = $"'{field}'";
                    }

                    sbSql.Append($"{field},");
                }
                sbSql.Remove(sbSql.Length - 1, 1);
                sbSql.Append(")");
                sbSql.AppendLine();
            }

            return sbSql;
        }

        static readonly string FromString = "FROM";
        static readonly string WhereString = "WHERE";
        /// <summary>
        /// 根据查询语句获取表名（单表）
        /// </summary>
        /// <param name="querySql"></param>
        /// <returns></returns>
        private static string GetTableNameFromQuerySQL(string querySql)
        {
            string tableName = string.Empty;
            int index1 = querySql.IndexOf(FromString, StringComparison.CurrentCultureIgnoreCase);
            int index2 = querySql.IndexOf(WhereString, StringComparison.CurrentCultureIgnoreCase);
            if (index2 > -1)
            {
                tableName = querySql.Substring(index1 + FromString.Length, index2 - index1 - FromString.Length);
            }
            else
            {
                tableName = querySql.Substring(index1 + FromString.Length);
            }

            tableName = tableName.Trim();
            return tableName;
        }
    }
}
