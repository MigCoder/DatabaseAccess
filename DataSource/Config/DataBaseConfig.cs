using System;

namespace DataSource.Config
{
    /// <summary>
    /// 用于描述数据库配置
    /// </summary>
    public abstract class DataBaseConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DataBaseType { get; set; }

        /// <summary>
        /// 数据库服务器名称
        /// </summary>
        public string ServerHostName { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// 数据文件名（通常用于Access数据库）
        /// </summary>
        public string DataBaseFileName { get; set; }

        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public virtual string GetConnectionStr()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MySql类型数据库
        /// </summary>
        MySql,
        /// <summary>
        /// Access类型数据库
        /// </summary>
        MicrosoftAccess,
        /// <summary>
        /// MicrosoftSqlServer类型数据库
        /// </summary>
        MicrosoftSql,
        /// <summary>
        /// Oracle类型数据库
        /// </summary>
        Oracle
    }
}
