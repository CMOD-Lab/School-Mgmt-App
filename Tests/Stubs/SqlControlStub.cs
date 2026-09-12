// Testable re-implementation of SqlControl for unit testing.
// This mirrors the production SqlControl exactly but lives in the test project
// so it compiles without the full WinForms application context.

using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using Npgsql;

namespace SchoolManagementApplciation
{
    /// <summary>
    /// Testable copy of SqlControl – identical logic to the production class.
    /// </summary>
    public class SqlControl
    {
        private NpgsqlConnection con;
        private NpgsqlCommand cmd = new NpgsqlCommand();

        public NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
        public DataSet data = new DataSet();
        public List<NpgsqlParameter> prams = new List<NpgsqlParameter>();

        public int count;
        public string exep = string.Empty;

        public SqlControl()
        {
            // Use connection string from config if available; otherwise empty string
            string? connStr = null;
            try { connStr = ConfigurationManager.AppSettings["connectionString"]; } catch { }
            con = new NpgsqlConnection(connStr ?? string.Empty);
        }

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
