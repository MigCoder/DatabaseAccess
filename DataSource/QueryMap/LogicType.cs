namespace DataSource.QueryMap
{
    /// <summary>
    /// 表示SQL语句间的逻辑（And、Or）
    /// </summary>
    public enum LogicType
    {
        /// <summary>
        /// 逻辑或
        /// </summary>
        Or,
        /// <summary>
        /// 逻辑与
        /// </summary>
        And,
        /// <summary>
        /// 无逻辑（代表一个语句的起始或者终结）
        /// </summary>
        None
    }
}
