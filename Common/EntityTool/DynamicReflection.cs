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

            //一个实例表达式
            var instanceParam = Expression.Parameter(type, "Instance");

            foreach (var prop in props)
            {
                //不是可写可读的属性跳过
                if (!prop.CanRead || !prop.CanWrite) continue;

                var setMethodInfo = prop.GetSetMethod();
                var getMethodInfo = prop.GetGetMethod();

                //实例是必须的
                var instance = Expression.Convert(instanceParam, setMethodInfo.ReflectedType);

                #region 获取Set委托

                //每个Set方法只有一个参数"Value"
                var setParam = setMethodInfo.GetParameters()[0];
                //一个Object类型的参数表达式
                var objSetParam = Expression.Parameter(typeof(object), setParam.Name);
                //将刚才的Object参数强转成属性的数据类型,报错就报错关我屁事......
                var castSetParam = Expression.Convert(objSetParam, setParam.ParameterType);
                //生成一个Set方法的调用表达式，把刚才强转好的值递给它
                var setInvoke = Expression.Call(instance, setMethodInfo, castSetParam);
                //生成lambda语句
                var lambda = Expression.Lambda<Action<T, object>>(setInvoke, instanceParam, objSetParam);
                //编译得到委托
                var setValueDelgate = lambda.Compile();

                #endregion

                #region 获取Get委托

                //生成一个Get方法的调用表达式
                var getInvoke = Expression.Call(instance, getMethodInfo);
                //将调用表达式的返回值装进一个Object,因为你不知道它返回了什么
                var castInvoke = Expression.Convert(getInvoke, typeof(object));
                //生成lambda语句
                var getLambda = Expression.Lambda<Func<T, object>>(castInvoke, instanceParam);
                //编译得到委托
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
