using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
// Updated: Replaced System.Data.SqlClient with Microsoft.Data.SqlClient for .NET 8 compatibility
using Microsoft.Data.SqlClient;

namespace SchoolManagementApplciation
{
    class SqlControl
    {

        private SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
        private SqlCommand cmd = new SqlCommand();

        public SqlDataAdapter adapter = new SqlDataAdapter();
        public DataSet data = new DataSet();

        public List<SqlParameter> prams = new List<SqlParameter>();

        public int count;
        public string exep;

        public void ExecSql(string query)
        {
            exep = "";
            count = 0;
            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                prams.ForEach(x => cmd.Parameters.Add(x));
                prams.Clear();
                data = new DataSet();
                adapter = new SqlDataAdapter(cmd);
                count = adapter.Fill(data, query);
                con.Close();
            }
            catch (Exception ex)
            {
                exep = ex.Message;
            }
            if (con.State == ConnectionState.Open)
                con.Close();
        }
        public void ExecProc(string query)
        {
            exep = "";
            count = 0;
            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                prams.ForEach(x => cmd.Parameters.Add(x));
                prams.Clear();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                exep = ex.Message;
            }
            if (con.State == ConnectionState.Open)
                con.Close();
        }
        public void addprams(string name, object value)
        {
            SqlParameter para = new SqlParameter(name, value);
            prams.Add(para);
        }
    }
}
