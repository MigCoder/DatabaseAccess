namespace DataSource.Config
{
    /// <summary>
    /// 描述MicrosoftSqlServer数据库配置
    /// </summary>
    public class SqlServerConfig : DataBaseConfig
    {
        #region 链接字符串模板

        /// <summary>
        /// MicrosoftSqlSever连接字符串模板
        /// </summary>
        public const string SqlConnectionStrTemplate =
            "Data Source={0};Initial Catalog={1};User Id={2};Password={3};";

        #endregion

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public override string GetConnectionStr()
        {
            return string.Format(SqlConnectionStrTemplate, ServerHostName, DataBaseName, UserName, PassWord);
        }

        public override string ToString()
        {
            return this.GetConnectionStr();
        }
    }
}
