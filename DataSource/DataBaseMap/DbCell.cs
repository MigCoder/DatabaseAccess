using System.Data;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 描述数据库表内的一个数据单元格，即数据库的最小单位
    /// </summary>
    public class DbCell
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 所属的数据库列
        /// </summary>
        public DbField OwnColumn { get; set; }
    }
}
