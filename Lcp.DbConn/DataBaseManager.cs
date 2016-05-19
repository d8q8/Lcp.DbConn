using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Lcp.DbConn
{
    public class DataBaseManager:IDisposable
    {
        private IDataBase _db;
        public DataBaseManager(IDataBase db)
        {
            _db = db;
        }

        #region 处理数据函数(不带参数)

        /// <summary>
        /// 获取DataSet数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回带分页自定义内存表</returns>
        private DataSet GetDataSet(string sql, CommandType ctype, int startindex, int pagesize, string dataname)
        {
            return _db.GetDataSet(sql, ctype, startindex, pagesize, dataname);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回自定义内存表</returns>
        private DataSet GetDataSet(string sql, CommandType ctype, string dataname)
        {
            return _db.GetDataSet(sql, ctype, dataname);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回执行的行数</returns>
        private int GetExecuteNonQuery(string sql, CommandType ctype)
        {
            return _db.GetExecuteNonQuery(sql, ctype);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回object对象</returns>
        private object GetExecuteScalar(string sql, CommandType ctype)
        {
            return _db.GetExecuteScalar(sql, ctype);
        }

        /// <summary>
        /// 判断是否存在值
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <param name="ctype">请求类型</param>
        /// <returns>查询结果（true/false）</returns>
        public bool GetExists(string sql, CommandType ctype)
        {
            return GetExists(sql, ctype, null);
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回记录集列表</returns>
        private IDataReader GetDataReader(string sql, CommandType ctype)
        {
            return _db.GetDataReader(sql, ctype);
        }

        #endregion

        #region 处理数据函数(带参数)

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
        private DataSet GetDataSet(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return _db.GetDataSet(sql, ctype, startindex, pagesize, dataname, param);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        private DataSet GetDataSet(string sql, CommandType ctype, string dataname, params IDataParameter[] param)
        {
            return _db.GetDataSet(sql, ctype, dataname, param);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回执行的行数</returns>
        private int GetExecuteNonQuery(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return _db.GetExecuteNonQuery(sql, ctype, param);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回object对象</returns>
        private object GetExecuteScalar(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return _db.GetExecuteScalar(sql, ctype, param);
        }

        /// <summary>
        /// 判断是否存在值
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <param name="ctype">请求类型</param>
        /// <param name="param">参数</param>
        /// <returns>查询结果（true/false）</returns>
        public bool GetExists(string sql, CommandType ctype, params IDataParameter[] param)
        {
            var obj = GetExecuteScalar(sql, ctype, param);
            var i = (Equals(obj, null) || Equals(obj, DBNull.Value)) ? 0 : int.Parse(obj.ToString());
            return (i != 0);
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回记录集列表</returns>
        private IDataReader GetDataReader(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return _db.GetDataReader(sql, ctype, param);
        }

        #endregion

        #region 引用方法(不带参数+可调枚举类型：SQL语句文本或存储过程)

        /// <summary>
        /// 获取List数据列表（带分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <param name="startindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="dataname"></param>
        /// <returns>返回带分页自定义List表</returns>
        public IList<T> MyDsList<T>(string sql, CommandType ctype, int startindex, int pagesize, string dataname)
        {
            var ds = GetDataSet(sql, ctype, startindex, pagesize, dataname);
            return DataConvert.DataSetToIList<T>(ds, dataname);
        }

        /// <summary>
        /// 获取List数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <param name="dataname"></param>
        /// <returns>返回自定义List表</returns>
        public IList<T> MyDsList<T>(string sql, CommandType ctype, string dataname)
        {
            var ds = GetDataSet(sql, ctype, dataname);
            return DataConvert.DataSetToIList<T>(ds, dataname);
        }

        /// <summary>
        /// 获取DataSet数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataSet MyDs(string sql, CommandType ctype, int startindex, int pagesize, string dataname)
        {
            return GetDataSet(sql, ctype, startindex, pagesize, dataname);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet MyDs(string sql, CommandType ctype, string dataname)
        {
            return GetDataSet(sql, ctype, dataname);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回执行的行数</returns>
        public int MyExec(string sql, CommandType ctype = CommandType.Text)
        {
            return GetExecuteNonQuery(sql, ctype);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回object对象</returns>
        public object MyTotal(string sql, CommandType ctype = CommandType.Text)
        {
            return GetExecuteScalar(sql, ctype);
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回bool表达式</returns>
        public bool MyExist(string sql, CommandType ctype = CommandType.Text)
        {
            return GetExists(sql, ctype);
        }

        /// <summary>
        /// 获取数据记录集List列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public IList<T> MyReadList<T>(string sql, CommandType ctype = CommandType.Text)
        {
            var dr = GetDataReader(sql, ctype);
            return DataConvert.DataReaderToList<T>(dr);
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <returns>返回记录集列表</returns>
        public IDataReader MyRead(string sql, CommandType ctype = CommandType.Text)
        {
            return GetDataReader(sql, ctype);
        }

        /// <summary>
        /// 获取数据模型记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public T MyModel<T>(string sql, CommandType ctype = CommandType.Text)
        {
            var dr = GetDataReader(sql, ctype);
            return DataConvert.ReaderToModel<T>(dr);
        }

        #endregion

        #region 引用方法(带参数+可调枚举类型：SQL语句文本或存储过程)

        /// <summary>
        /// 获取List数据列表（带分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <param name="startindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="dataname"></param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义List表</returns>
        public IList<T> MyDsList<T>(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            var ds = GetDataSet(sql, ctype, startindex, pagesize, dataname, param);
            return DataConvert.DataSetToIList<T>(ds, dataname);
        }

        /// <summary>
        /// 获取List数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <param name="dataname"></param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义List表</returns>
        public IList<T> MyDsList<T>(string sql, CommandType ctype, string dataname, params IDataParameter[] param)
        {
            var ds = GetDataSet(sql, ctype, dataname, param);
            return DataConvert.DataSetToIList<T>(ds, dataname);
        }

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
        public DataSet MyDs(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return GetDataSet(sql, ctype, startindex, pagesize, dataname, param);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet MyDs(string sql, CommandType ctype, string dataname, params IDataParameter[] param)
        {
            return GetDataSet(sql, ctype, dataname, param);
        }

        /// <summary>
        /// 获取DataTable数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataTable MyDt(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return MyDs(sql, ctype, startindex, pagesize, dataname, param).Tables[0];
        }

        /// <summary>
        /// 获取DataTable数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataTable MyDt(string sql, CommandType ctype, string dataname, params IDataParameter[] param)
        {
            return MyDs(sql, ctype, dataname, param).Tables[0];
        }

        /// <summary>
        /// 获取DataView数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataView MyDv(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return MyDt(sql, ctype, startindex, pagesize, dataname, param).DefaultView;
        }

        /// <summary>
        /// 获取DataView数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataView MyDv(string sql, CommandType ctype, string dataname, params IDataParameter[] param)
        {
            return MyDt(sql, ctype, dataname, param).DefaultView;
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回执行的行数</returns>
        public int MyExec(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return GetExecuteNonQuery(sql, ctype, param);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回object对象</returns>
        public object MyTotal(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return GetExecuteScalar(sql, ctype, param);
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回bool表达式</returns>
        public bool MyExist(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return GetExists(sql, ctype, param);
        }

        /// <summary>
        /// 获取数据记录集List列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public IList<T> MyReadList<T>(string sql, CommandType ctype, params IDataParameter[] param)
        {
            var dr = GetDataReader(sql, ctype, param);
            return DataConvert.DataReaderToList<T>(dr);
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="ctype">类型</param>
        /// <param name="param">参数</param>
        /// <returns>返回记录集列表</returns>
        public IDataReader MyRead(string sql, CommandType ctype, params IDataParameter[] param)
        {
            return GetDataReader(sql, ctype, param);
        }

        /// <summary>
        /// 获取数据模型记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ctype"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T MyModel<T>(string sql, CommandType ctype, params IDataParameter[] param)
        {
            var dr = GetDataReader(sql, ctype, param);
            return DataConvert.ReaderToModel<T>(dr);
        }

        #endregion

        #region 引用方法(不带参数+不可调枚举类型：SQL文本命令)

        /// <summary>
        /// 获取List数据列表（带分页），带数据名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="startindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public IList<T> MyDsList<T>(string sql, int startindex, int pagesize, string dataname = "ds")
        {
            return MyDsList<T>(sql, CommandType.Text, startindex, pagesize, dataname);
        }

        /// <summary>
        /// 获取List数据列表，带数据名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public IList<T> MyDsList<T>(string sql, string dataname = "ds")
        {
            return MyDsList<T>(sql, CommandType.Text, dataname);
        }

        /// <summary>
        /// 获取DataSet数据列表（带分页），带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataSet MyDs(string sql, int startindex, int pagesize, string dataname = "ds")
        {
            return MyDs(sql, CommandType.Text, startindex, pagesize, dataname);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet MyDs(string sql, string dataname = "ds")
        {
            return MyDs(sql, CommandType.Text, dataname);
        }

        #endregion

        #region 引用方法(带参数+不可调枚举类型：SQL文本命令)

        /// <summary>
        /// 获取List数据列表（带分页），带数据名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="startindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="dataname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> MyDsList<T>(string sql, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return MyDsList<T>(sql, CommandType.Text, startindex, pagesize, dataname, param);
        }

        /// <summary>
        /// 获取List数据列表（带分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="startindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> MyDsList<T>(string sql, int startindex, int pagesize, params IDataParameter[] param)
        {
            return MyDsList<T>(sql, CommandType.Text, startindex, pagesize, "ds", param);
        }

        /// <summary>
        /// 获取List数据列表，带数据名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dataname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> MyDsList<T>(string sql, string dataname, params IDataParameter[] param)
        {
            return MyDsList<T>(sql, CommandType.Text, dataname, param);
        }

        /// <summary>
        /// 获取List数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> MyDsList<T>(string sql, params IDataParameter[] param)
        {
            return MyDsList<T>(sql, CommandType.Text, "ds", param);
        }

        /// <summary>
        /// 获取DataSet数据列表（带分页），带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataSet MyDs(string sql, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return MyDs(sql, CommandType.Text, startindex, pagesize, dataname, param);
        }

        /// <summary>
        /// 获取DataSet数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataSet MyDs(string sql, int startindex, int pagesize, params IDataParameter[] param)
        {
            return MyDs(sql, CommandType.Text, startindex, pagesize, "ds", param);
        }

        /// <summary>
        /// 获取DataSet数据列表，带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet MyDs(string sql, string dataname, params IDataParameter[] param)
        {
            return MyDs(sql, CommandType.Text, dataname, param);
        }

        /// <summary>
        /// 获取DataSet数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataSet MyDs(string sql, params IDataParameter[] param)
        {
            return MyDs(sql, CommandType.Text, "ds", param);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回执行的行数</returns>
        public int MyExec(string sql, params IDataParameter[] param)
        {
            return MyExec(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回object对象</returns>
        public object MyTotal(string sql, params IDataParameter[] param)
        {
            return MyTotal(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param"></param>
        /// <returns>返回bool表达式</returns>
        public bool MyExist(string sql, params IDataParameter[] param)
        {
            return MyExist(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 获取数据记录集List列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> MyReadList<T>(string sql, params IDataParameter[] param)
        {
            return MyReadList<T>(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 获取数据记录集列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回记录集列表</returns>
        public IDataReader MyRead(string sql, params IDataParameter[] param)
        {
            return MyRead(sql, CommandType.Text, param);
        }

        /// <summary>
        /// 获取数据模型记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T MyModel<T>(string sql, params IDataParameter[] param)
        {
            return MyModel<T>(sql, CommandType.Text, param);
        }

        #endregion

        #region 引用缓存(带参数+形参或存储过程)
        /// <summary>
        /// 获取缓存参数
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public IDataParameter[] GetCache(string cacheKey)
        {
            return _db.GetCachedParameters(cacheKey);
        }

        /// <summary>
        /// 设置缓存参数
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="commandParameters"></param>
        public void SetCache(string cacheKey, params IDataParameter[] commandParameters)
        {
            _db.CacheParameters(cacheKey, commandParameters);
        }
        #endregion

        #region 引用参数(键值关系)
        /// <summary>
        /// 创建传参转化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDataParameter MyParam(string name, string value)
        {
            return _db.MyParams(name, value);
        }
        /// <summary>
        /// 设置参数,对象值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDataParameter AddParam(string name, object value)
        {
            return _db.MyParams(name, value);
        }

        #endregion

        #region 引用方法（DataTable与DataView 带参数）
        /// <summary>
        /// 获取DataTable数据列表（带分页），带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataTable MyDt(string sql, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return MyDs(sql, CommandType.Text, startindex, pagesize, dataname, param).Tables[0];
        }

        /// <summary>
        /// 获取DataTable数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataTable MyDt(string sql, int startindex, int pagesize, params IDataParameter[] param)
        {
            return MyDt(sql, startindex, pagesize, "ds", param);
        }

        /// <summary>
        /// 获取DataTable数据列表，带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataTable MyDt(string sql, string dataname, params IDataParameter[] param)
        {
            return MyDs(sql, CommandType.Text, dataname, param).Tables[0];
        }

        /// <summary>
        /// 获取DataTable数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataTable MyDt(string sql, params IDataParameter[] param)
        {
            return MyDt(sql, "ds", param);
        }

        /// <summary>
        /// 获取DataView数据列表（带分页），带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataView MyDv(string sql, int startindex, int pagesize, string dataname, params IDataParameter[] param)
        {
            return MyDt(sql, startindex, pagesize, dataname, param).DefaultView;
        }

        /// <summary>
        /// 获取DataView数据列表（带分页）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="param">参数</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataView MyDv(string sql, int startindex, int pagesize, params IDataParameter[] param)
        {
            return MyDv(sql, startindex, pagesize, "ds", param);
        }

        /// <summary>
        /// 获取DataView数据列表，带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">内存表</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataView MyDv(string sql, string dataname, params IDataParameter[] param)
        {
            return MyDt(sql, dataname, param).DefaultView;
        }

        /// <summary>
        /// 获取DataView数据列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>返回自定义内存表</returns>
        public DataView MyDv(string sql, params IDataParameter[] param)
        {
            return MyDv(sql, "ds", param);
        }

        #endregion

        #region 引用方法（DataTable与DataView 不带参数）
        /// <summary>
        /// 获取DataTable数据列表（带分页），带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataTable MyDt(string sql, int startindex, int pagesize, string dataname = "ds")
        {
            return MyDt(sql, startindex, pagesize, dataname, null);
        }

        /// <summary>
        /// 获取DataTable数据列表，带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回自定义内存表</returns>
        public DataTable MyDt(string sql, string dataname = "ds")
        {
            return MyDt(sql, dataname, null);
        }

        /// <summary>
        /// 获取DataView数据列表（带分页），带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="startindex">页面传递参数</param>
        /// <param name="pagesize">每页分配记录数</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回带分页自定义内存表</returns>
        public DataView MyDv(string sql, int startindex, int pagesize, string dataname = "ds")
        {
            return MyDv(sql, startindex, pagesize, dataname, null);
        }

        /// <summary>
        /// 获取DataView数据列表，带数据名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">内存表</param>
        /// <returns>返回自定义内存表</returns>
        public DataView MyDv(string sql, string dataname = "ds")
        {
            return MyDv(sql, dataname, null);
        }

        #endregion

        #region 释放资源
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DataBaseManager() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
