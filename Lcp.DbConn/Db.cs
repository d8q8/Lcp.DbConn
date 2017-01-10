using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Lcp.DbConn
{

    #region 枚举数据库类型
    /// <summary>
    /// 获取当前数据库
    /// </summary>
    public enum MyType
    {
        Access2003, Access2007, Access2013, Access2016, Mssql, Mysql, Oracle, Sqlite
    }
    #endregion
    public class Db
    {

        #region 数据库配置
        //调用代码配置在web.config里，数据库路径为：Access，Sqlite为相对路径，其他为全路径
        /*==============================================================================
         * 以下两种节点配置选一种即可
            <appSettings>
                <add key="access" value="数据库"/>
                <add key="access2007" value="数据库"/>
                <add key="sqlite" value="数据库"/>
                <add key="mssql" value="server=(local);uid=用户名;pwd=密码;database=数据库"/>
                <add key="oracle" value="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=服务名)));User Id=用户名;Password=密码;"/>
                <add key="mysql" value="server=localhost;user id=用户名;password=密码;database=数据库"/>
            </appSettings>

            <connectionStrings>
                <add name="access" connectionString="数据库" providerName="System.Data.OleDb" />
                <add name="access2007" connectionString="数据库" providerName="System.Data.OleDb" />
                <add name="sqlite" connectionString="数据库" providerName="System.Data.SQLite" />
                <add name="mysql" connectionString="server=localhost;user id=用户名;password=密码;database=数据库" providerName="System.Data.MySqlClient" />
                <add name="mssql" connectionString="server=(local);uid=用户名;pwd=密码;database=数据库" providerName="System.Data.SqlClient" />
                <add name="oracle" connectionString="Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=服务名)));User Id=用户名;Password=密码;" providerName="Oracle.ManagedDataAccess.Client"/>
            </connectionStrings>
        *================================================================================*/
        #endregion

        /// <summary>
        /// 处理连接多数据库采用IOC注入
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="connstr"></param>
        /// <returns></returns>
        public static DataBaseManager GetConn(MyType mt = MyType.Access2003, string connstr = "access")
        {
            //配置节点
            var connStr = ConfigurationManager.ConnectionStrings[connstr] != null ? ConfigurationManager.ConnectionStrings[connstr].ConnectionString : ConfigurationManager.AppSettings[connstr].ToString(CultureInfo.InvariantCulture);
            var filepath = string.Empty;
            var conn = string.Empty;
            var builder = new ContainerBuilder();
            builder.RegisterType<DataBaseManager>();
            switch (mt)
            {
                case MyType.Access2003:
                    filepath = Application.StartupPath == Environment.CurrentDirectory ? Application.StartupPath + "\\" + connStr : HttpContext.Current.Server.MapPath(connStr);
                    conn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filepath);
                    builder.Register(c => new Access(conn)).As<IDataBase>();
                    break;
                case MyType.Access2007:
                    filepath = Application.StartupPath == Environment.CurrentDirectory ? Application.StartupPath + "\\" + connStr : HttpContext.Current.Server.MapPath(connStr);
                    conn = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False", filepath);
                    builder.Register(c => new Access(conn)).As<IDataBase>();
                    break;
                case MyType.Access2013:
                    filepath = Application.StartupPath == Environment.CurrentDirectory ? Application.StartupPath + "\\" + connStr : HttpContext.Current.Server.MapPath(connStr);
                    conn = string.Format(@"Provider=Microsoft.ACE.OLEDB.15.0;Data Source={0};Persist Security Info=False", filepath);
                    builder.Register(c => new Access(conn)).As<IDataBase>();
                    break;
                case MyType.Access2016:
                    filepath = Application.StartupPath == Environment.CurrentDirectory ? Application.StartupPath + "\\" + connStr : HttpContext.Current.Server.MapPath(connStr);
                    conn = string.Format(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={0};Persist Security Info=False", filepath);
                    builder.Register(c => new Access(conn)).As<IDataBase>();
                    break;
                case MyType.Sqlite:
                    filepath = Application.StartupPath == Environment.CurrentDirectory ? Application.StartupPath + "\\" + connStr : HttpContext.Current.Server.MapPath(connStr);
                    conn = string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false;", filepath);
                    builder.Register(c => new Sqlite(conn)).As<IDataBase>();
                    break;
                case MyType.Mysql:
                    conn = connStr;
                    builder.Register(c => new Mysql(conn)).As<IDataBase>();
                    break;
                case MyType.Mssql:
                    conn = connStr;
                    builder.Register(c => new Mssql(conn)).As<IDataBase>();
                    break;
                case MyType.Oracle:
                    conn = connStr;
                    builder.Register(c => new Oracle(conn)).As<IDataBase>();
                    break;
            }

            using (var container = builder.Build())
            {
                return container.Resolve<DataBaseManager>();
            }
        }
    }
}
