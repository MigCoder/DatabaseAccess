namespace Common.EntityTool.DelegateReflection
{
    /// <summary>
    /// 通用的实体值设置接口
    /// </summary>
    public interface ISetValue
    {
        /// <summary>
        /// 设置目标对象指定属性的值
        /// </summary>
        /// <param name="target">实体对象</param>
        /// <param name="val">要设置的值</param>
        void SetValue(object target, object val);
    }
}
