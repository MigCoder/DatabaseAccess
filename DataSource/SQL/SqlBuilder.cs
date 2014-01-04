using System.Text;
using Common.EntityTool.DelegateReflection;

namespace DataSource.SQL
{
    /// <summary>
    /// SQL操作语句生成器(Fuck还没想好怎么实现比较高效)
    /// </summary>
    public static class SqlBuilder
    {
        #region 静态字符串

        /// <summary>
        /// 数据库新增语句模板
        /// </summary>
        private const string InsertTemplate = "Insert Into '{0}'('{1}') Values('{2}'";

        #endregion

        /// <summary>
        /// 生成指定类型实体的数据库新增语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="data">实体数据</param>
        public static string BuildCreateSql<T>(T data)
        {
            var builder = new StringBuilder();
            var warper = new DelegateReflection<T>();

            return string.Empty;
        }
    }
}
