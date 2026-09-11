using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
// Updated: Replaced Microsoft.Data.SqlClient with Npgsql for PostgreSQL compatibility
using Npgsql;

namespace SchoolManagementApplciation
{
    class SqlControl
    {

        private NpgsqlConnection con = new NpgsqlConnection(ConfigurationManager.AppSettings["connectionString"]);
        private NpgsqlCommand cmd = new NpgsqlCommand();

        public NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
        public DataSet data = new DataSet();

        public List<NpgsqlParameter> prams = new List<NpgsqlParameter>();

        public int count;
        public string exep;

        public void ExecSql(string query)
        {
            exep = "";
            count = 0;
            try
            {
                con.Open();
                cmd = new NpgsqlCommand(query, con);
                prams.ForEach(x => cmd.Parameters.Add(x));
                prams.Clear();
                data = new DataSet();
                adapter = new NpgsqlDataAdapter(cmd);
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
                cmd = new NpgsqlCommand(query, con);
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
            NpgsqlParameter para = new NpgsqlParameter(name, value);
            prams.Add(para);
        }
    }
}
