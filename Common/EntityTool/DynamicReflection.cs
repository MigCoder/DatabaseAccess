using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.EntityTool
{
    /// <summary>
    /// 实体类属性反射表达式
    /// </summary>
    public class DynamicReflection<T>
    {

        #region 缓存

        /// <summary>
        /// 实体类型公共属性缓存
        /// </summary>
        public Dictionary<string, PropertyInfo> Props = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// 获取值委托
        /// </summary>
        private readonly Dictionary<string, Func<T, object>> _getValuesDelegate;

        /// <summary>
        /// 设置值委托
        /// </summary>
        private readonly Dictionary<string, Action<T, object>> _setValuesDelegate;

        #endregion

        /// <summary>
        /// 构建一个指定实体类型的动态反射类
        /// </summary>
        public DynamicReflection()
        {
            //初始化好委托容器
            _setValuesDelegate = new Dictionary<string, Action<T, object>>();
            _getValuesDelegate = new Dictionary<string, Func<T, object>>();

            this.Init();
        }

        /// <summary>
        /// 初始化实体类型的属性操作委托集合
        /// </summary>
        private void Init()
        {
            //获取实体类型的所有公有属性(不要静态属性)
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var info in props) Props[info.Name] = info;

            //一个实例参数表达式（Action<T,object>中的T）
            var instanceParam = Expression.Parameter(type, "Instance");

            foreach (var prop in props)
            {
                //不是可写可读的属性跳过
                if (!prop.CanRead || !prop.CanWrite) continue;

                var setMethodInfo = prop.GetSetMethod();
                var getMethodInfo = prop.GetGetMethod();

                #region 获取Set委托

                var setParam = setMethodInfo.GetParameters()[0];
                var objSetParam = Expression.Parameter(typeof(object), setParam.Name);
                var castSetParam = Expression.Convert(objSetParam, prop.PropertyType);

                var setInvoke = Expression.Call(instanceParam, setMethodInfo, castSetParam);

                var lambda = Expression.Lambda<Action<T, object>>(setInvoke, instanceParam, objSetParam);
                var setValueDelgate = lambda.Compile();

                #endregion

                #region 获取Get委托

                var getInvoke = Expression.Call(instanceParam, getMethodInfo);
                var castReturnValue = Expression.Convert(getInvoke, typeof(object));

                var getLambda = Expression.Lambda<Func<T, object>>(castReturnValue, instanceParam);
                var getValueDelegate = getLambda.Compile();

                #endregion

                _setValuesDelegate[prop.Name] = setValueDelgate;
                _getValuesDelegate[prop.Name] = getValueDelegate;
            }
        }

        /// <summary>
        /// 设置指定实例的指定名称属性为指定值
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void SetValue(T instance, string name, object value)
        {
            Action<T, object> setFunc;
            if (_setValuesDelegate.TryGetValue(name, out setFunc))
            {
                setFunc.Invoke(instance, value);
            }
        }

        /// <summary>
        /// 获取指定实例的指定名称属性值
        /// </summary>
        /// <param name="instance">实例对象</param>
        /// <param name="name">属性名</param>
        public object GetValue(T instance, string name)
        {
            Func<T, object> getFunc;
            if (_getValuesDelegate.TryGetValue(name, out getFunc))
            {
                return getFunc.Invoke(instance);
            }

            return null;
        }
    }
}
