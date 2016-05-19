using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        Access2003, Access2007, Access2013, Mssql, Mysql, Oracle, Sqlite
    }
    #endregion
    public class Db
    {

        #region 数据库配置
        //调用代码配置在web.config里，数据库路径为：Access，Sqlite为相对路径，其他为全路径
        //<appSettings>
        //    <add key="access" value="数据库"/>
        //    <add key="sqlite" value="数据库"/>
        //    <add key="sqlserver" value="server=(local);uid=用户名;pwd=密码;database=数据库"/>
        //    <add key="oracle" value="Provider=MSDAORA.1;Password=密码;User ID=用户名;Data Source=数据库"/>
        //    <add key="mysql" value="server=localhost;user id=用户名;password=密码;database=数据库"/>
        //</appSettings>
        //<connectionStrings>
        //    <add name="sqlserver" connectionString="data source=.;initial catalog=数据库;user id=用户名;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
        //</connectionStrings>
        #endregion
        
        /// <summary>
        /// 处理连接多数据库采用IOC注入
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="connstr"></param>
        /// <returns></returns>
        public static DataBaseManager GetConn(MyType mt = MyType.Access2003, string connstr = "access")
        {
            //var connStr = ConfigurationManager.AppSettings[connstr].ToString(CultureInfo.InvariantCulture);
            var connStr = ConfigurationManager.ConnectionStrings[connstr].ConnectionString;
            var filepath = string.Empty;
            var conn = string.Empty;
            var builder = new ContainerBuilder();
            builder.RegisterType<DataBaseManager>();
            switch (mt)
            {
                case MyType.Access2003:
                    filepath = Application.StartupPath == Environment.CurrentDirectory? Application.StartupPath + "\\" + connStr: HttpContext.Current.Server.MapPath(connStr);
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
                //case MyType.Oracle:
                //    conn = connStr;
                //    builder.Register(c => new Oracle(conn)).As<IDataBase>();
                //    break;
            }

            using (var container = builder.Build())
            {
                return container.Resolve<DataBaseManager>();
            }
        }
    }
}
