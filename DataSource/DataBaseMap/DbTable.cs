using System.Collections.Generic;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 描述一张数据库表
    /// </summary>
    public class DbTable
    {
        #region 构造

        /// <summary>
        /// 创建一个数据库表描述实体
        /// </summary>
        public DbTable()
        {

        }

        /// <summary>
        /// 创建一个数据库表描述实体
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="nameSpace">命名空间（默认为dbo）</param>
        public DbTable(string name, string dbName, string nameSpace = "dbo")
        {
            this.Name = name;
            this.DbName = dbName;
            this.NameSpace = nameSpace;
        }

        #endregion

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表注释
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 所属数据库名
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// 所属命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 列集合
        /// </summary>
        public IEnumerable<DbField> Columns { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Name, this.Description ?? string.Empty);
        }
    }
}
