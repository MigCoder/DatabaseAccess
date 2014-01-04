using System.Collections.Generic;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 描述一个数据库
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 命名空间集合
        /// </summary>
        public IEnumerable<string> NameSpaces { get; set; }

        /// <summary>
        /// 表集合
        /// </summary>
        public IEnumerable<DbTable> Tables { get; set; }

        /// <summary>
        /// 存储过程集合
        /// </summary>
        public IEnumerable<DbProc> Procs { get; set; }
    }
}
