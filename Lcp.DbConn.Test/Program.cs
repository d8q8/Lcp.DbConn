using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lcp.DbConn.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Application.StartupPath);
            //Console.WriteLine(Environment.CurrentDirectory);

            #region 连接Access2003数据库
            //using (var _db = Db.GetConn(MyType.Access2003, "access"))
            //{
            //    const string sql = @"select * from admin";
            //    var dt = _db.MyDt(sql);
            //    Console.WriteLine("用户名:{0},密码:{1}", dt.Rows[0]["username"], dt.Rows[0]["password"]);
            //}
            #endregion

            #region 连接Access2007数据库 
            //using (var _db = Db.GetConn(MyType.Access2007, "access2007"))
            //{
            //    const string sql = @"select * from admin";
            //    var dt = _db.MyDt(sql);
            //    Console.WriteLine("用户名:{0},密码:{1}", dt.Rows[0]["username"], dt.Rows[0]["password"]);
            //}
            #endregion

            #region 连接Sqlite数据库
            //using (var _db = Db.GetConn(MyType.Sqlite, "sqlite"))
            //{
            //    const string sql = @"select * from admin";
            //    var dt = _db.MyDt(sql);
            //    Console.WriteLine("用户名:{0},密码:{1}", dt.Rows[0]["username"], dt.Rows[0]["password"]);
            //}
            #endregion

            #region 连接Mysql数据库
            //using (var _db = Db.GetConn(MyType.Mysql, "mysql"))
            //{
            //    const string sql = @"select * from admin";
            //    var dt = _db.MyDt(sql);
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        Console.WriteLine("用户名:{0},密码:{1}", dt.Rows[0]["username"], dt.Rows[0]["password"]);
            //    }
            //}
            #endregion

            #region 连接Mssql数据库
            //using (var _db = Db.GetConn(MyType.Mssql, "mssql"))
            //{
            //    const string sql = @"select top 5 * from AreaFull";
            //    var dt = _db.MyDt(sql);
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        Console.WriteLine("ID:{0},名称:{1}", dr["AreaId"], dr["AreaName"]);
            //    }
            //}
            #endregion

            #region 连接Oracle数据库
            using (var _db = Db.GetConn(MyType.Oracle, "oracle"))
            {
                const string sql = @"select * from ADMIN";
                var dt = _db.MyDt(sql);
                Console.WriteLine("用户名:{0},密码:{1}", dt.Rows[0]["username"], dt.Rows[0]["password"]);
            }
            #endregion
            //暂停
            //Console.ReadLine();

            Console.Write("按任意键退出...");
            Console.ReadKey(true);
        }
    }
}
