using System;
using System.IO;
using System.Windows.Forms;

namespace SQLGenerator
{
    public partial class DBConnection : Form
    {
        string TxtFilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache.txt");
            }
        }

        public DBConnection()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string conStr = tbConStr.Text.Trim();
            SQLHelper helper = new SQLHelper(conStr);
            try
            {
                helper.OpenDB();

                MainForm.DBConString = conStr;
                MainForm.DBInfo = $"当前数据库信息：服务器【{helper.SqlConnection.DataSource}】，数据库【{helper.SqlConnection.Database}】";

                CommonHelper.SaveTxt(TxtFilePath, conStr);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接失败，请重试");
            }
            finally
            {
                helper.CloseDB();
            }
        }

        private void DBConnection_Load(object sender, EventArgs e)
        {
            tbConStr.Text = CommonHelper.ReadTxt(TxtFilePath);
        }
    }
}
