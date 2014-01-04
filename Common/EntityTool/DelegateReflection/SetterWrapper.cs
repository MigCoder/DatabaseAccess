using System;
using System.Reflection;

namespace Common.EntityTool.DelegateReflection
{
    /// <summary>
    /// 实体类型值设置包装器
    /// </summary>
    /// <typeparam name="TTarget">实体类型</typeparam>
    /// <typeparam name="TValue">值的数据类型</typeparam>
    public class SetterWrapper<TTarget, TValue> : ISetValue
    {
        private readonly Action<TTarget, TValue> _setter;

        /// <summary>
        /// 构造一个实体类型值设置包装器
        /// </summary>
        /// <param name="propertyInfo">属性元数据</param>
        public SetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<TTarget, TValue>)Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), null, m);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="target">实体对象</param>
        /// <param name="val">要设置的值</param>
        public void SetValue(TTarget target, TValue val)
        {
            _setter(target, val);
        }

        /// <summary>
        /// 尝试设置值
        /// </summary>
        /// <param name="target">实体对象</param>
        /// <param name="val">要设置的值></param>
        void ISetValue.SetValue(object target, object val)
        {
            _setter((TTarget)target, (TValue)val);
        }
    }
}
