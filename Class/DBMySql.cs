using MySql.Data.MySqlClient;

using System;
using System.Data;
using System.Windows;

namespace WalkyDog
{
    public class DBMySql
    {
        /* public void Initialize()
         {
             Debug.WriteLine("DataBase Initialize");
             string connectionPath = $"SERVER=localhost;DATABASE=profile;UID=root;PASSWORD=201933043";
             App.connection = new MySqlConnection(connectionPath);
         }*/

        MySqlConnection conn = new MySqlConnection("SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!");
        MySqlDataAdapter adpt;
        MySqlCommand cmd;

        public void Connection()
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            conn.Close();
        }

        public DataSet SelectAll(string table)
        {
            try
            {
                DataSet ds = new DataSet();

                string sql = $"SELECT * FROM {table}";
                adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, table);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public DataSet SelectDetail(string table, string condition, string where = "")
        {
            try
            {
                DataSet ds = new DataSet();

                string sql = $"SELECT {condition} FROM {table} {where}";
                adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, table);
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public void Insert(string table, string value)
        {
            try
            {
                conn.Open();
                string sql = $"INSERT INTO {table} VALUES ({value})";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public void Update(string table, string setvalue, string wherevalue)
        {
            try
            {
                conn.Open();
                string sql = $"UPDATE {table} SET {setvalue} WHERE {wherevalue}";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public void DeleteAll(string table)
        {
            try
            {
                conn.Open();
                string sql = $"DELETE FROM {table}";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        public void DeleteDetail(string table, string wherecol, string wherevalue)
        {
            try
            {
                conn.Open();
                string sql = $"DELETE FROM {table} WHERE {wherecol}='{wherevalue}'";
                cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }
    }
}