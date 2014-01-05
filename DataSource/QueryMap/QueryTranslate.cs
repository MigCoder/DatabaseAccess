using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataSource.QueryMap
{
    /// <summary>
    /// 查询式翻译
    /// </summary>
    public class QueryTranslate<T> : IQueryable<T>
    {
        public QueryTranslate(Expression exp, IQueryProvider provider)
        {
            this.Expression = exp;
            this.Provider = provider;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Provider.Execute(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }
        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }

    }

    public class HkQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new QueryTranslate<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
