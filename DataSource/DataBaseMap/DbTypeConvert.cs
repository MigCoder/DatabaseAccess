using System;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 提供数据库类型与.Net类型的转换
    /// </summary>
    public class DbTypeConvert
    {
        /// <summary>
        /// 将SQL数据库类型名称转换为.Net类型名称
        /// </summary>
        /// <param name="typename">类型名称</param>
        public static string ToCsharpName(string typename)
        {
            switch (typename)
            {
                case "bit": return "Boolean";
                case "bigint": return "Int64";
                case "int": return "Int32";
                case "smallint": return "Int16";
                case "tinyint": return "Byte";
                case "numeric":
                case "money":
                case "smallmoney":
                case "decimal": return "Decimal";
                case "float": return "Double";
                case "real": return "Single";
                case "smalldatetime":
                case "timestamp":
                case "datetime": return "DateTime";
                case "char":
                case "varchar":
                case "text":
                case "Unicode":
                case "nvarchar":
                case "ntext": return "string";
                case "binary":
                case "varbinary":
                case "image": return "Byte[]";
                case "uniqueidentifier": return "Guid";
                default: return "Object";
            }

        }

        /// <summary>
        /// 将一个Object转换为指定的类型(用于从数据库读取出的数据兼容)
        /// </summary>
        /// <param name="netType">要转换的Net类型</param>
        /// <param name="sqlValue">从数据库读取出来的值</param>
        public static object ToCsharpValue(Type netType, object sqlValue)
        {
            string typeName = netType.Name;
            if (netType.BaseType != null && netType.BaseType.Name == "Enum")
            {
                typeName = "Enum";
            }
            switch (typeName)
            {
                case "Boolean": return Convert.ToBoolean(sqlValue);
                case "Int64": return Convert.ToInt64(sqlValue);
                case "Int32": return Convert.ToInt32(sqlValue);
                case "Int16": return Convert.ToInt16(sqlValue);
                case "Byte": return Convert.ToByte(sqlValue);
                case "Decimal": return Convert.ToDecimal(sqlValue);
                case "Double": return Convert.ToDouble(sqlValue);
                case "Single": return Convert.ToSingle(sqlValue);
                case "DateTime": return Convert.ToDateTime(sqlValue);
                case "string": return Convert.ToString(sqlValue);
                case "Guid": return Guid.Parse(sqlValue.ToString());
                case "Enum":
                    {
                        var type = Enum.GetUnderlyingType(netType);
                        return ToCsharpValue(type, sqlValue);
                    }
                default:
                    return sqlValue;
            }
        }
    }
}
