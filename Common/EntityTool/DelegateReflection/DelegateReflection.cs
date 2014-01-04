using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.EntityTool.DelegateReflection
{
    /// <summary>
    /// 使用委托替代反射（注：该类并不保证线程安全）(2注：已经被效率更高的DynamicReflection取代它可以滚粗了)
    /// </summary>
    /// <typeparam name="T">要反射的类型</typeparam>
    public class DelegateReflection<T>
    {
        public Dictionary<string, IGetValue> GetFuncs;
        public Dictionary<string, ISetValue> SetFuncs;
        public Dictionary<string, PropertyInfo> Props = new Dictionary<string, PropertyInfo>();

        public DelegateReflection()
        {
            var type = typeof(T);
            var props = type.GetProperties();
            foreach (var info in props) Props[info.Name] = info;

            this.GetFuncs = new Dictionary<string, IGetValue>();
            this.SetFuncs = new Dictionary<string, ISetValue>();

            foreach (PropertyInfo prop in Props.Values)
            {
                var getFunc = CreatePropertyGetterWrapper(prop);
                var setFunc = CreatePropertySetterWrapper(prop);

                this.GetFuncs.Add(prop.Name, getFunc);
                this.SetFuncs.Add(prop.Name, setFunc);
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propName">属性名</param>
        /// <param name="value">值</param>
        public void SetValue(T obj, string propName, object value)
        {
            ISetValue setFunc;
            if (this.SetFuncs.TryGetValue(propName, out setFunc))
            {
                setFunc.SetValue(obj, value);
            }
        }

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propName">属性名</param>
        public object GetValue(T obj, string propName)
        {
            IGetValue getFunc;
            if (this.GetFuncs.TryGetValue(propName, out getFunc))
            {
                return getFunc.GetValue(obj);
            }

            return null;
        }

        /// <summary>
        /// 创建一个属性值设置器
        /// </summary>
        /// <param name="propertyInfo">属性元信息</param>
        public static ISetValue CreatePropertySetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo mi = propertyInfo.GetSetMethod(true);

            if (mi.GetParameters().Length > 1)
                throw new NotSupportedException("不支持构造索引器属性的委托。");

            Type instanceType = typeof(SetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
        }

        /// <summary>
        /// 创建一个属性值获取器
        /// </summary>
        /// <param name="propertyInfo">属性元信息</param>
        public static IGetValue CreatePropertyGetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanRead == false)
                throw new InvalidOperationException("属性不支持读操作。");

            MethodInfo mi = propertyInfo.GetGetMethod(true);

            if (mi.GetParameters().Length > 0)
                throw new NotSupportedException("不支持构造索引器属性的委托。");

            Type instanceType = typeof(GetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (IGetValue)Activator.CreateInstance(instanceType, propertyInfo);
        }
    }

}
