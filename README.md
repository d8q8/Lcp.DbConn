# Lcp.DbConn
针对以前的ADO.NET采用AutoFac注入封装，更新加简化代码量，扩展方便

**类库简称:白菜NET"内裤".**
<pre>
针对ADO.NET提供一个通用的多库适配内裤,简化开发过程,使用简单小巧,灵活自活,
你也可以针对一种数据库只拷贝单个类来使用,来减小代码量,如增加数据库再扩展.
欢迎提问题或留QQ:17624522/Email:d8q8#163.com.
</pre>

1>使用方法

```c#
//枚举数据库类型
public enum MyType
{
    Access2003, Access2007, Access2013, Access2016, Mssql, Mysql, Oracle, Sqlite
}
//使用很简单,如MyType.Access2003
```

2>数据库配置
```c#
//调用代码配置在web.config里，数据库路径为：Access，Sqlite为相对路径，其他为全路径
//==============================================================================
//以下两种节点配置选一种即可
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
```
3>简单调用方式
```c#
using (var _db = Db.GetConn(MyType.Sqlite, "sqlite"))//Sqlite数据库
//using (var _db = Db.GetConn(MyType.Access2013, "access2013"))//Access数据库
{
    const string sql = "select * from admin";
    var dt = _db.MyDt(sql);
    Console.Write("用户名:{0},密码:{1}", dt.Rows[0]["username"], dt.Rows[0]["password"]);
}
```
4>其他调用方式
```c#
//其他调用方式也比较简单,如下
_db.MyDsList<T>(sql);//返回记录集List列表
_db.MyDs(sql);//返回DataSet数据集
_db.MyDt(sql);//返回DataTable数据表
_db.MyDv(sql);//返回DataView数据视图
_db.MyReadList<T>(sql);//返回DataReader数据列表
_db.MyRead(sql);//返回DataReader数据视图
_db.MyModel(sql);//返回数据模型
_db.MyExec(sql);//处理增删改操作,返回int类型
_db.MyTotal(sql);//处理计算查询结果操作,返回object类型,可自行转化成总数
_db.MyExist(sql);//判断数据是否存在,返回bool类型
_db.GetCache(键);//获取缓存
_db.SetCache(键,值);//设置缓存
_db.MyParam(参数名称,参数赋值);//设置参数,赋值为字符串
_db.AddParam(参数名称,参数赋值);//同上,只是赋值为对象
```
5>重载调用方式
```c#
//跟上面类似调用方式,只是参数多些,如下
//MyDt(string sql, CommandType ctype, int startindex, int pagesize, string dataname, params IDataParameter[] param)
_db.MyDt(sql, 命令类型, 开始页数索引, 每页多少个, 数据集别名, 变参);
//其他的方法基本都有重载,请自己查看源码吧.
```

祝各位好运,使用简单方便,学习NET立刻简单了,不是一点点,加油,童鞋们,都能成为NET开发者.