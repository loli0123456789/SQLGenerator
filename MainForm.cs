using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SQLGenerator
{
    public partial class MainForm : Form
    {
        public static string DBConString;
        public static string DBInfo;

        public float X;
        public float Y;
        public float y;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnGeneral_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DBConString))
            {
                MessageBox.Show("请先连接数据库");
                return;
            }

            string querySql = tbQuerySQL.Text.Trim();
            if (string.IsNullOrEmpty(querySql))
            {
                MessageBox.Show("请先输入查询SQL");
                return;
            }

            try
            {
                StringBuilder sbSql = CommonHelper.GetInsertSQL(querySql);

                tbResultSQL.Text = sbSql.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询SQL有误，请重新输入");
                tbResultSQL.Text = ex.Message;
            }
        }

        private void 连接数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDB();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenDB();

            this.Resize += new EventHandler(MainForm_Resize);

            X = this.Width;
            Y = this.Height;
            //y = this.statusStrip1.Height;
            SetTag(this);
        }

        protected void OpenDB()
        {
            DBConnection dBConnection = new DBConnection();
            dBConnection.ShowDialog();
            if (dBConnection.DialogResult == DialogResult.OK)
            {
                MessageBox.Show("连接成功");
                lbDBInfo.Text = DBInfo;
            }
        }

        private void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;

                if (con.Controls.Count > 0)
                {
                    SetTag(con);
                }
            }
        }

        private void SetControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {

                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);

                Single currentSize = Convert.ToSingle(mytag[4]) * newy;
                //改变字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);

                if (con.Controls.Count > 0)
                {
                    SetControls(newx, newy, con);
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;

            SetControls(newx, newy, this);
        }
    }
}
