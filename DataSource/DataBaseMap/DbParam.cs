using System;
using System.Data;
using System.Data.SqlClient;
using Common.ExMethod;

namespace DataSource.DataBaseMap
{
    /// <summary>
    /// 描述一个数据库 存储过程/函数 的参数
    /// </summary>
    public class DbParam
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 参数长度
        /// </summary>
        public Int16 Length { get; set; }

        /// <summary>
        /// 参数方向(0输入1输出6存储过程返回值)
        /// </summary>
        public int IsOutParam { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 转换为SQL参数
        /// </summary>
        /// <returns></returns>
        public SqlParameter ToSqlParam()
        {
            var sqlParam = new SqlParameter
                {
                    ParameterName = this.Name,
                    Value = this.Value ?? DBNull.Value,
                    DbType = this.DbType,
                    Size = this.Length,
                    Direction = this.IsOutParam
                                    .Switch(new[] { 0, 1, 6 },
                                    new[] { ParameterDirection.Input,
                                            ParameterDirection.Output,
                                            ParameterDirection.ReturnValue})
                };

            return sqlParam;
        }
    }
}
