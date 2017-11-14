using CDatabase.Common;
using CDatabase.Cursor;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase
{
    class MySqlDatabase : IDatabase
    {
        private static MySqlDatabase instance = null;
        private DbConfig config = null;

        private ConnectionPool connectPool;
        //private MySqlConnection connection = null;

        private MySqlDatabase(DbConfig config)
        {
            this.config = config;

            string server = config.Server;
            string port = Convert.ToString(config.Port);
            string user = config.User;
            string password = config.Password;

            string connectionStr = String.Format(
                "server={0};port={1};user={2};password={3}",
                server, port, user, password);

            ConnectionPool.Init(connectionStr, config.Database);

            connectPool = ConnectionPool.getPool();

            MySqlConnection  conn = connectPool.getConnection();

            connectPool.closeConnection(conn);

            //connection = new MySqlConnection(connectionStr);
        }

        public static MySqlDatabase GetInstance(DbConfig config)
        {
            if(instance == null)
            {
                instance = new MySqlDatabase(config);
            }

            return instance;
        }

        public void ChangeDatabase(string database)
        {
            ConnectionState state = connectPool.getConnection().State;

            if(state.HasFlag(ConnectionState.Open))
            {
                try
                {
                    connectPool.getConnection().ChangeDatabase(database);
                    config.Database = database;
                }
                catch (MySqlException e)
                {
                    throw new DatabaseException(DbConfig.DbType.MYSQL, e.Number);
                }
            }
        }

        public bool IsOpen()
        {
            return true;
        }

        public void Open()
        {
            //ConnectionState state = connectPool.getConnection().State;

            //if (!state.HasFlag(ConnectionState.Open))
            //{
            //    try
            //    {
            //        connectPool.getConnection().Open();
            //        connectPool.getConnection().ChangeDatabase(config.Database);
            //    }
            //    catch (MySqlException e)
            //    {
            //        throw new DatabaseException(DbConfig.DbType.MYSQL, e.Number);
            //    }
            //}
        }

        public void Close()
        {
            //ConnectionState state = connection.State;

            //if (!state.HasFlag(ConnectionState.Closed))
            //{
            //    try
            //    {
            //        connection.Close();
            //    }
            //    catch (MySqlException e)
            //    {
            //        throw new DatabaseException(DbConfig.DbType.MYSQL, e.Number);
            //    }
            //}
        }

        public int ExecSQL(string sql, string[] bindArgs)
        {
            int effect = 0;

            if (!IsOpen())
            {
                Open();
            }

            if (bindArgs != null)
            {
                sql = Function.CompleteArgsToSql(sql, bindArgs);
            }

            MySqlConnection connect = connectPool.getConnection();

            MySqlCommand command = new MySqlCommand(sql, connect);
            effect = command.ExecuteNonQuery();
            command.Dispose();
            connectPool.closeConnection(connect);
            return effect;
        }

        public int ExecSQL(FileInfo info)
        {
            if(!info.Exists || !info.Extension.Equals(Database.SQL_FILE_SUFFIX))
            {
                return 0;
            }
            string sql = File.ReadAllText(info.FullName);

            return ExecSQL(sql, null);
        }

        public long Insert(string table, string nullColumnHack, Dictionary<string, string> values)
        {
            long insertedId = 0;

            if (!IsOpen())
            {
                Open();
            }

            string fieldParams = "";
            string valueParams = "";
            foreach (KeyValuePair<string, string> item in values)
            {
                fieldParams += item.Key + ",";
                valueParams += "'" + item.Value + "'" + ",";
            }
            fieldParams = fieldParams.Substring(0, fieldParams.Length - 1);
            valueParams = valueParams.Substring(0, valueParams.Length - 1);

            MySqlConnection connect = connectPool.getConnection();

            string cmd = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", table, fieldParams, valueParams);
            MySqlCommand command = new MySqlCommand(cmd, connect);

            if (command.ExecuteNonQuery() > 0)
            {
                insertedId = command.LastInsertedId;
            }
            command.Dispose();
            connectPool.closeConnection(connect);
            return insertedId;
        }

        public int Delete(string table, string whereClause, string[] whereArgs)
        {
            if (!IsOpen())
            {
                Open();
            }

            if (whereArgs != null)
            {
                whereClause = Function.CompleteArgsToSql(whereClause, whereArgs);
            }

            MySqlConnection connect = connectPool.getConnection();

            string cmd = String.Format("DELETE FROM {0} WHERE {1}", table, whereClause);
            MySqlCommand command = new MySqlCommand(cmd, connect);
            int effect = command.ExecuteNonQuery();
            command.Dispose();

            connectPool.closeConnection(connect);

            return effect;
        }

        public int Update(string table, Dictionary<string, string> values, string whereClause, string[] whereArgs)
        {


            if (!IsOpen())
            {
                Open();
            }

            string valueParams = "";
            foreach (KeyValuePair<string, string> item in values)
            {
                valueParams += item.Key + "='" + item.Value + "',";
            }
            valueParams = valueParams.Substring(0, valueParams.Length - 1);

            if (whereArgs != null)
            {
                whereClause = Function.CompleteArgsToSql(whereClause, whereArgs);
            }
            MySqlConnection connect = connectPool.getConnection();

            string cmd = String.Format("UPDATE {0} SET {1} WHERE {2}", table, valueParams, whereClause);
            MySqlCommand command = new MySqlCommand(cmd, connect);

            int effect = -1;

            try
            {
                effect = command.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                command.Dispose();
                connectPool.closeConnection(connect);
            }

            return effect;
        }

        public ICursor Query(string table, string[] columns, string whereClause, string[] whereArgs, string groupBy, string having, string orderBy)
        {
            return Query(false, table, columns, whereClause, whereArgs, groupBy, having, orderBy, null);
        }

        public ICursor Query(string table, string[] columns, string whereClause, string[] whereArgs, string groupBy, string having, string orderBy, string limit)
        {
            return Query(false, table, columns, whereClause, whereArgs, groupBy, having, orderBy, limit);
        }

        public ICursor Query(bool distinct, string table, string[] columns, string whereClause, string[] whereArgs, string groupBy, string having, string orderBy, string limit)
        {
            string cmd = "";

            string headArgs = "";
            if (distinct)
            {
                headArgs = "SELECT DISTINCT" + headArgs;
            }
            else
            {
                headArgs = "SELECT" + headArgs;
            }

            string columnArgs = "";
            if (columns == null)
            {
                columnArgs = "*";
            }
            else
            {
                foreach (string column in columns)
                {
                    columnArgs += column + ",";
                }
                columnArgs = columnArgs.Substring(0, columnArgs.Length - 1);
            }

            cmd += String.Format("{0} {1} FROM {2} ", headArgs, columnArgs, table);

            if (whereArgs != null)
            {
                whereClause = Function.CompleteArgsToSql(whereClause, whereArgs);
                cmd += String.Format("WHERE {0} ", whereClause);
            }
            else if (whereClause != null && !whereClause.Equals(""))
            {
                cmd += String.Format("WHERE {0} ", whereClause);
            }

            if (groupBy != null && !groupBy.Equals(""))
            {
                cmd += String.Format("GROUP BY {0} ", groupBy);
            }

            if (having != null && !having.Equals(""))
            {
                cmd += String.Format("HAVING {0} ", having);
            }

            if (orderBy != null && !orderBy.Equals(""))
            {
                cmd += String.Format("ORDER BY {0} ", orderBy);
            }

            if (limit != null && !limit.Equals(""))
            {
                cmd += String.Format("LIMIT {0} ", limit);
            }

            if (!IsOpen())
            {
                Open();
            }

            MySqlConnection connect = connectPool.getConnection();

            MySqlCommand command = new MySqlCommand(cmd, connect);
            MySqlDataReader reader = command.ExecuteReader();

            return new MySqlCursor(reader, connect);
        }
    }
}
