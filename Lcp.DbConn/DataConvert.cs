using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lcp.DbConn
{
    public class DataConvert
    {
        #region 转化实体类对象
        /// <summary>
        /// 记录集转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ReaderToModel<T>(IDataReader dr)
        {
            try
            {
                using (dr)
                {
                    if (dr.Read())
                    {
                        List<string> list = new List<string>(dr.FieldCount);
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            list.Add(dr.GetName(i).ToLower());
                        }
                        T model = Activator.CreateInstance<T>();
                        foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (list.Contains(pi.Name.ToLower()))
                            {
                                if (!IsNullOrDBNull(dr[pi.Name]))
                                {
                                    pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                                }
                            }
                        }
                        return model;
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reader转Object，从数据库字段转化为实体类对象
        /// </summary>
        /// <param name="reader">数据库记录集</param>
        /// <param name="targetObj">实体类对象</param>
        public static void ReaderToObject(IDataReader reader, object targetObj)
        {
            using (reader)
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var propertyInfo = targetObj.GetType().GetProperty(reader.GetName(i));
                    if (propertyInfo == null) continue;
                    if (reader.GetValue(i) == DBNull.Value) continue;
                    propertyInfo.SetValue(targetObj,
                        propertyInfo.PropertyType.IsEnum
                            ? Enum.ToObject(propertyInfo.PropertyType, reader.GetValue(i))
                            : reader.GetValue(i), null);
                }
            }
        }
        /// <summary>
        /// DataSet转Object
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="targetObj">实体类对象</param>
        public static void DataSetToObject(DataSet ds, object targetObj)
        {
            DataTableToObject(ds.Tables[0], targetObj);
        }
        /// <summary>
        /// DataTable转Object
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <param name="targetObj">实体类对象</param>
        public static void DataTableToObject(DataTable dt, object targetObj)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var propertys = targetObj.GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    var tempName = pi.Name;
                    // 检查DataTable是否包含此列
                    if (!dt.Columns.Contains(tempName)) continue;
                    // 判断此属性是否有Setter
                    if (!pi.CanWrite) continue;
                    var value = dr[tempName];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(targetObj, value, null);
                    }
                }
            }
        }
        #endregion

        #region 泛型集合与DataSet互相转换
        /// <summary>    
        /// 集合装换DataSet    
        /// </summary>    
        /// <param name="pList">集合</param>    
        /// <returns></returns>      
        public static DataSet ToDataSet(IList pList)
        {
            var result = new DataSet();
            var dataTable = new DataTable();
            if (pList.Count > 0)
            {
                var propertys = pList[0].GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    dataTable.Columns.Add(pi.Name, pi.PropertyType);
                }

                foreach (var array in from object t in pList select propertys.Select(pi => pi.GetValue(t, null)).ToArray())
                {
                    dataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(dataTable);
            return result;
        }

        /// <summary>    
        /// 泛型集合转换DataSet    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="list">泛型集合</param>    
        /// <returns></returns>      
        public static DataSet ToDataSet<T>(IList<T> list)
        {
            return ToDataSet(list, null);
        }

        /// <summary>    
        /// 泛型集合转换DataSet    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="pList">泛型集合</param>    
        /// <param name="pPropertyName">待转换属性名数组</param>    
        /// <returns></returns>    
        public static DataSet ToDataSet<T>(IList<T> pList, params string[] pPropertyName)
        {
            var propertyNameList = new List<string>();
            if (pPropertyName != null)
                propertyNameList.AddRange(pPropertyName);

            var result = new DataSet();
            var dataTable = new DataTable();
            if (pList.Count > 0)
            {
                var propertys = pList[0].GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        // 没有指定属性的情况下全部属性都要转换
                        dataTable.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            dataTable.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                foreach (var t in pList)
                {
                    var tempList = new List<object>();
                    foreach (var pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            var obj = pi.GetValue(t, null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (!propertyNameList.Contains(pi.Name)) continue;
                            var obj = pi.GetValue(t, null);
                            tempList.Add(obj);
                        }
                    }
                    var array = tempList.ToArray();
                    dataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(dataTable);
            return result;
        }

        /// <summary>
        /// 重载DataSet转换为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pDataSet"></param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet pDataSet)
        {
            return DataSetToIList<T>(pDataSet, 0);
        }

        /// <summary>    
        /// DataSet转换为泛型集合    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="pDataSet">DataSet</param>    
        /// <param name="pTableIndex">待转换数据表索引</param>    
        /// <returns></returns>     
        public static IList<T> DataSetToIList<T>(DataSet pDataSet, int pTableIndex)
        {
            if (pDataSet == null || pDataSet.Tables.Count < 0)
                return null;
            if (pTableIndex > pDataSet.Tables.Count - 1)
                return null;
            if (pTableIndex < 0)
                pTableIndex = 0;

            var pData = pDataSet.Tables[pTableIndex];
            // 返回值初始化    
            IList<T> result = new List<T>();
            for (var j = 0; j < pData.Rows.Count; j++)
            {
                var t = (T)Activator.CreateInstance(typeof(T));
                var propertys = t.GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    for (var i = 0; i < pData.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值
                        if (!pi.Name.Equals(pData.Columns[i].ColumnName)) continue;
                        // 数据库NULL值单独处理    
                        pi.SetValue(t, pData.Rows[j][i] != DBNull.Value ? pData.Rows[j][i] : null, null);
                        break;
                    }
                }
                result.Add(t);
            }
            return result;
        }

        /// <summary>    
        /// DataSet转换为泛型集合    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="pDataSet">DataSet</param>    
        /// <param name="pTableName">待转换数据表名称</param>    
        /// <returns></returns>    
        public static IList<T> DataSetToIList<T>(DataSet pDataSet, string pTableName)
        {
            var tableIndex = 0;
            if (pDataSet == null || pDataSet.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(pTableName))
                return null;
            for (var i = 0; i < pDataSet.Tables.Count; i++)
            {
                // 获取Table名称在Tables集合中的索引值    
                if (!pDataSet.Tables[i].TableName.Equals(pTableName)) continue;
                tableIndex = i;
                break;
            }
            return DataSetToIList<T>(pDataSet, tableIndex);
        }
        #endregion

        #region DataReader转换成实体（或List）
        /// <summary>   
        /// DataReader转换为obj list   
        /// </summary>   
        /// <typeparam name="T">泛型</typeparam>   
        /// <param name="reader">数据记录集</param>   
        /// <returns>返回泛型类型</returns>   
        public static IList<T> DataReaderToList<T>(IDataReader dr)
        {
            using (dr)
            {
                List<string> field = new List<string>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    field.Add(dr.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (dr.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.Contains(property.Name.ToLower()))
                        {
                            if (!IsNullOrDBNull(dr[property.Name]))
                            {
                                property.SetValue(model, HackType(dr[property.Name], property.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }

        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        private static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }

        #endregion

        
    }
}
