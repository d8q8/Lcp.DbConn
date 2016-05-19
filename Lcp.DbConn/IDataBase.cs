using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Lcp.DbConn
{
    public interface IDataBase
    {
        string Name { get; }
        void Close();
        void Dispose();
        IDataReader GetDataReader(string sql, CommandType ctype);
        IDataReader GetDataReader(string sql, CommandType ctype, params IDataParameter[] param);
        DataSet GetDataSet(string sql, CommandType ctype, string dataname, params IDataParameter[] param);
        DataSet GetDataSet(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param);
        DataSet GetDataSet(string sql, CommandType ctype, int startindex, int pagesize, string dataname);
        DataSet GetDataSet(string sql, CommandType ctype, string dataname);
        int GetExecuteNonQuery(string sql, CommandType ctype, params IDataParameter[] param);
        int GetExecuteNonQuery(string sql, CommandType ctype);
        object GetExecuteScalar(string sql, CommandType ctype, params IDataParameter[] param);
        object GetExecuteScalar(string sql, CommandType ctype);
        void Open();
        void CacheParameters(string cacheKey, params IDataParameter[] commandParameters);
        IDataParameter[] GetCachedParameters(string cacheKey);
        IDataParameter MyParams(string name, object value);
    }
}
