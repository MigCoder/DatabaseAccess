using System;
using System.Reflection;

namespace Demo.Data.Core.EntityTool.DelegateReflection
{
    public class GetterWrapper<TTarget, TValue> : IGetValue
    {
        private readonly Func<TTarget, TValue> _getter;

        /// <summary>
        /// 构建一个属性值获取器
        /// </summary>
        /// <param name="propertyInfo">属性元信息</param>
        public GetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanRead == false)
                throw new InvalidOperationException("属性不支持读操作。");

            MethodInfo m = propertyInfo.GetGetMethod(true);
            _getter = (Func<TTarget, TValue>)Delegate.CreateDelegate(typeof(Func<TTarget, TValue>), null, m);
        }

        /// <summary>
        /// 获取对象的值
        /// </summary>
        public TValue GetValue(TTarget target)
        {
            return _getter(target);
        }

        /// <summary>
        /// 尝试获取实体对象指定属性的值
        /// </summary>
        /// <param name="target">实体对象</param>
        public object GetValue(object target)
        {
            return _getter((TTarget)target);
        }
    }
}
