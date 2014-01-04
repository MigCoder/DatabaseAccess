namespace DataSource.QueryMap
{
    /// <summary>
    /// 表示一个查询表达式
    /// </summary>
    public class QueryExpression
    {
        /// <summary>
        /// 获取表达式与上一个表达式节点间的逻辑
        /// </summary>
        public LogicType PreviousLogic { get; set; }

        /// <summary>
        /// 获取表达式与下一个表达式节点间的逻辑
        /// </summary>
        public LogicType NextLogic { get; set; }


    }
}
