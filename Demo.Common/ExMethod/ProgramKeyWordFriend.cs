using System.Collections.Generic;

namespace Demo.Common.ExMethod
{
    /// <summary>
    /// 对if/else/switch/while 等基本关键词的替代扩展(程序员福音啊，再贵也要用一个✪ ω ✪)
    /// </summary>
    public static class ProgramKeyWordFriend
    {
        /// <summary>
        /// 使用一个比较集合和一个结果集合自动执行Switch语句，两个集合数量应一致
        /// </summary>
        /// <typeparam name="TIn">比较集合内元素的类型</typeparam>
        /// <typeparam name="TResult">结果集合内元素的类型</typeparam>
        /// <param name="tin">要Switch的对象</param>
        /// <param name="compares">比较集合</param>
        /// <param name="results">结果集合</param>
        /// <param name="defValue">比较失败时的默认值</param>
        public static TResult Switch<TIn, TResult>(this TIn tin, IEnumerable<TIn> compares, IEnumerable<TResult> results, TResult defValue = default(TResult))
        {
            if (tin == null || compares == null || results == null) return defValue;

            var comparesEnumer = compares.GetEnumerator();
            var resultsEnumer = results.GetEnumerator();

            while (comparesEnumer.MoveNext())
            {
                if (resultsEnumer.MoveNext())
                {
                    if (tin.Equals(comparesEnumer.Current))
                    {
                        return resultsEnumer.Current;
                    }
                }
                else
                {
                    break;
                }
            }

            return defValue;
        }

        //        /// <summary>
        //        /// 使用一个比较集合和一个结果集合自动执行Switch语句，两个集合数量应一致,比较成功后可执行一系列以Tin为参数的Action委托
        //        /// </summary>
        //        /// <typeparam name="TIn">比较集合内元素的类型</typeparam>
        //        /// <typeparam name="TResult">结果集合内元素的类型</typeparam>
        //        /// <param name="tin">要Switch的对象</param>
        //        /// <param name="compares">比较集合</param>
        //        /// <param name="results">结果集合</param>
        //        /// <param name="defValue">比较失败时的默认值</param>
        //        /// <param name="actions">委托集合</param>
        //        private static TResult Switch<TIn, TResult>(this TIn tin, IEnumerable<TIn> compares, IEnumerable<TResult> results, TResult defValue = default(TResult), params Action<TIn>[] actions = null)
        //        {
        //            if (tin == null || compares == null || results == null) return defValue;
        //
        //            var comparesEnumer = compares.GetEnumerator();
        //            var resultsEnumer = results.GetEnumerator();
        //
        //            while (comparesEnumer.MoveNext())
        //            {
        //                if (resultsEnumer.MoveNext())
        //                {
        //                    if (tin.Equals(comparesEnumer.Current))
        //                    {
        //                        foreach (var action in actions)
        //                        {
        //                            action(comparesEnumer.Current);
        //                        }
        //                        return resultsEnumer.Current;
        //                    }
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //
        //            return defValue;
        //        }
        //    
    }
}
