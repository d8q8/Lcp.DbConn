﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcp.DbConn
{
    public class Oracle: IDataBase, IDisposable
    {
        public string Name { get { return "Oracle"; } }
        private OracleConnection _connSql;
        private readonly string _dataSql;
        private bool _disposed;
        /// <summary>
        /// 初始化MSSQL数据库
        /// </summary>
        /// <param name="connstr"></param>
        public Oracle(string connstr)
        {
            _dataSql = connstr;
        }
        /// <summary>
        /// 传参转化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDataParameter MyParams(string name, object value)
        {
            return new OracleParameter(name, value);
        }

        #region 基本数据库操作
        /// <summary>
        /// 打开数据库
        /// </summary>
        public void Open()
        {
            var connstr = _dataSql;
            _connSql = new OracleConnection(connstr);
            if (_connSql.State == ConnectionState.Open) return;
            try
            {
                _connSql.Open();
            }
            catch (Exception)
            {
                _connSql.Dispose();
                throw new Exception("打开数据库失败！");
            }
        }
        /// <summary>
        /// 关闭数据库并释放资源
        /// </summary>
        public void Close()
        {
            if (_connSql?.State != ConnectionState.Open) return;
            try
            {
                _connSql.Close();
                _connSql.Dispose();
                _connSql = null;
            }
            catch (Exception)
            {
                _connSql?.Dispose();
                _connSql = null;
                throw new Exception("关闭数据库失败！");
            }
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~Oracle()
        {
            Dispose(false);
        }
        /// <summary>
        /// 系统回收
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 判断是否释放
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    Close();
                }
            }
            _disposed = true;
        }
        #endregion

        #region 不带参数封装类

        /// <summary>
        /// 获取DataSet数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataSet GetDataSet(string sql, CommandType ctype, int startindex, int pagesize, string dataname)
        {
            return GetDataSet(sql, ctype, startindex, pagesize, dataname, null);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet GetDataSet(string sql, CommandType ctype, string dataname)
        {
            return GetDataSet(sql, ctype, dataname, null);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        public int GetExecuteNonQuery(string sql, CommandType ctype)
        {
            return GetExecuteNonQuery(sql, ctype, null);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回object对象</returns>
        public object GetExecuteScalar(string sql, CommandType ctype)
        {
            return GetExecuteScalar(sql, ctype, null);
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回记录集列表</returns>
        public IDataReader GetDataReader(string sql, CommandType ctype)
        {
            return GetDataReader(sql, ctype, null);
        }

        #endregion

        #region 带参数封装类

        /// <summary>
        /// 获取DataSet数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataSet GetDataSet(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            Open();
            var cmd = new OracleCommand();
            PrepareCommand(cmd, _connSql, null, ctype, sql, param);
            using (var dap = new OracleDataAdapter(cmd))
            {
                var ds = new DataSet();
                try
                {
                    dap.Fill(ds, (startindex - 1) * pagesize, pagesize, dataname);
                    dap.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return ds;
                }
                catch (OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet GetDataSet(string sql, CommandType ctype, string dataname, params IDataParameter[] param)
        {
            Open();
            var cmd = new OracleCommand();
            PrepareCommand(cmd, _connSql, null, ctype, sql, param);
            using (var dap = new OracleDataAdapter(cmd))
            {
                var ds = new DataSet();
                try
                {
                    dap.Fill(ds, dataname);
                    dap.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    return ds;
                }
                catch (OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        public int GetExecuteNonQuery(string sql, CommandType ctype, params IDataParameter[] param)
        {
            Open();
            int i;
            var cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, _connSql, null, ctype, sql, param);
                i = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
            finally
            {
                Close();
            }
            return i;
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>查询结果（object）</returns>
        public object GetExecuteScalar(string sql, CommandType ctype, params IDataParameter[] param)
        {
            Open();
            var cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, _connSql, null, ctype, sql, param);
                var obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
                return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回记录集列表</returns>
        public IDataReader GetDataReader(string sql, CommandType ctype, params IDataParameter[] param)
        {
            Open();
            var cmd = new OracleCommand();
            PrepareCommand(cmd, _connSql, null, ctype, sql, param);
            var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return dr;
        }

        /// <summary>
        /// 核心类
        /// </summary>
        /// <param name="cmd">表示要对数据源执行的SQL或存储过程</param>
        /// <param name="conn">表示是数据源的连接是打开的</param>
        /// <param name="trans">表示是数据源的SQL事务,不能被继承</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="cmdText">字符串</param>
        /// <param name="cmdParms">参数</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, params IDataParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事务处理
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;

            if (cmdParms == null) return;
            foreach (var parameter in cmdParms.Cast<OracleParameter>())
            {
                if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
                cmd.Parameters.Add(parameter);
            }
        }

        // 存贮Cache缓存的Hashtable集合
        private readonly Hashtable _parmCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 在缓存中添加参数数组
        /// </summary>
        /// <param name="cacheKey">参数的Key</param>
        /// <param name="cmdParms">参数数组</param>
        public void CacheParameters(string cacheKey, params IDataParameter[] cmdParms)
        {
            _parmCache[cacheKey] = cmdParms;
        }
        /// <summary>
        /// 提取缓存的参数数组
        /// </summary>
        /// <param name="cacheKey">查找缓存的key</param>
        /// <returns>返回被缓存的参数数组</returns>
        public IDataParameter[] GetCachedParameters(string cacheKey)
        {
            var cachedParms = (OracleParameter[])_parmCache[cacheKey];
            if (cachedParms == null)
                return null;
            IDataParameter[] clonedParms = { };
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();
            return clonedParms;
        }

        #endregion
    }
}
