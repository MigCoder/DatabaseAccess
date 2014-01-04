using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 描述一个数据库存储过程
    /// </summary>
    public class DbProc
    {
        /// <summary>
        /// 创建一个数据库存储过程描述实体
        /// </summary>
        /// <param name="name">存储过程名</param>
        /// <param name="nameSpace">命名空间（默认为dbo）</param>
        /// <param name="hasReturnValue">是否拥有返回值</param>
        public DbProc(string name, bool hasReturnValue = false, string nameSpace = "dbo")
        {
            this.Name = name;
            this.NameSpace = nameSpace;
        }

        /// <summary>
        /// 存储过程名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 存储过程注释
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
        /// 受影响的行数
        /// </summary>
        public int AffectedRow { get; set; }

        #region 返回值与输出参数

        /// <summary>
        /// 存储过程是否拥有返回值
        /// </summary>
        public bool HasReturnValue { get; set; }

        /// <summary>
        /// 存储过程返回值
        /// </summary>
        public int ReturnValue
        {
            get { return (int)this.Params.First(x => x.Name == "@Return").Value; }
        }

        /// <summary>
        /// 存储过程输出值
        /// </summary>
        public List<DbParam> OutDatas
        {
            get { return this.Params.Where(x => x.IsOutParam == 1).ToList(); }
        }

        #endregion


        #region 存储过程参数

        /// <summary>
        /// 参数集合
        /// </summary>
        public List<DbParam> Params { get; set; }

        /// <summary>
        /// 将存储过程参数转化为Sql参数
        /// </summary>
        public SqlParameter[] ParamsToSql()
        {
            if (this.HasReturnValue)
            {
                this.Params.Add(new DbParam()
                    {
                        Name = "@Return",
                        DbType = DbType.Int32,
                        IsOutParam = (int)ParameterDirection.ReturnValue
                    });
            }

            return this.Params.Select(x => x.ToSqlParam()).ToArray();
        }

        /// <summary>
        /// 索引存储过程的参数
        /// </summary>
        public DbParam this[int index]
        {
            get { return this.Params[index]; }
            set { this.Params[index] = value; }
        }

        /// <summary>
        /// 索引存储过程的参数
        /// </summary>
        public DbParam this[string name]
        {
            get { return this.Params.First(x => x.Name == name); }
            set
            {
                var index = this.Params.FindIndex(x => x.Name == name);
                this.Params[index] = value;
            }
        }

        #endregion

    }
}
