using System.Data;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 描述一个数据库字段(列)
    /// </summary>
    public class DbField
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段注释
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 字段的数据类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 单元格集合
        /// </summary>
        public DbCell Cells { get; set; }
    }
}
