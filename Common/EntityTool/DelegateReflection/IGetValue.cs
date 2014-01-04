namespace Common.EntityTool.DelegateReflection
{
    /// <summary>
    /// 通用的实体值获取接口
    /// </summary>
    public interface IGetValue
    {
        /// <summary>
        /// 获取目标对象指定属性的值
        /// </summary>
        /// <param name="target">实体对象</param>
        object GetValue(object target);
    }
}
