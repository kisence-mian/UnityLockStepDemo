using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatabase.Cursor
{
    class MySqlCursor : ICursor
    {
        private MySqlDataReader reader = null;

        public MySqlCursor(MySqlDataReader reader)
        {
            this.reader = reader;
        }

        public void Close()
        {
            if(!reader.IsClosed)
            {
                reader.Close();
            }
            reader.Dispose();
        }

        public int FieldCount()
        {
            return reader.FieldCount;
        }

        public string GetDataTypeName(int i)
        {
            return reader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(string column)
        {
            return reader.GetDateTime(column);
        }

        public DateTime GetDateTime(int i)
        {
            return reader.GetDateTime(i);
        }

        public double GetDouble(string column)
        {
            return reader.GetDouble(column);
        }

        public double GetDouble(int i)
        {
            return reader.GetDouble(i);
        }

        public int GetFieldIndex(string field)
        {
            return reader.GetOrdinal(field);
        }

        public int GetInt(string column)
        {
            return reader.GetInt32(column);
        }

        public int GetInt(int i)
        {
            return reader.GetInt32(i);
        }

        public string GetName(int i)
        {
            return reader.GetName(i);
        }

        public string GetString(string column)
        {
            return reader.GetString(column);
        }

        public string GetString(int i)
        {
            return reader.GetString(i);
        }

        public bool IsDBNull(int i)
        {
            return reader.IsDBNull(i);
        }

        public bool MoveToNext()
        {
            return reader.Read();
        }
    }
}
