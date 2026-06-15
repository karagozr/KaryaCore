using System.Collections;
using System.Linq.Expressions;
using DevExtreme.AspNet.Data;
using Karya.Core.Common.Data;

namespace Karya.Test.Web.Api.Helpers;




public static class DevExtremeExpressionMapper
{
    public static SelectFilterOptions<TEntity> ToCleanOptions<TEntity>(this DataSourceLoadOptionsBase source) where TEntity : class
    {
        var options = new SelectFilterOptions<TEntity>
        {
            Skip = source.Skip,
            Take = source.Take,
            RequireTotalCount = source.RequireTotalCount
        };

        // 1. FILTRE DÖNÜŞÜMÜ (Derleme Hatasını Kökten Çözen Yapı)
        if (source.Filter != null && source.Filter.Count > 0)
        {
            var spyQuery = new ExpressionSpyQueryable<TEntity>();

            var filterOnlyOptions = new DataSourceLoadOptionsBase { Filter = source.Filter };

            DataSourceLoader.Load(spyQuery, filterOnlyOptions);

            if (spyQuery.CapturedExpression != null)
            {
                options.FilterExpression = ExtractWhereExpression<TEntity>(spyQuery.CapturedExpression);
            }
        }

        if (source.Sort != null && source.Sort.Length > 0)
        {
            options.OrderByExpression = query =>
            {
                IOrderedQueryable<TEntity>? orderedQuery = null;

                for (int i = 0; i < source.Sort.Length; i++)
                {
                    var sortItem = source.Sort[i];
                    var propertyName = sortItem.Selector;

                    var param = Expression.Parameter(typeof(TEntity), "x");
                    var prop = Expression.Property(param, propertyName);
                    var exp = Expression.Lambda(prop, param);

                    string methodName = i == 0
                        ? (sortItem.Desc ? "OrderByDescending" : "OrderBy")
                        : (sortItem.Desc ? "ThenByDescending" : "ThenBy");

                    var methodCall = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new Type[] { typeof(TEntity), prop.Type },
                        i == 0 ? query.Expression : orderedQuery!.Expression,
                        Expression.Quote(exp)
                    );

                    orderedQuery = (IOrderedQueryable<TEntity>)query.Provider.CreateQuery<TEntity>(methodCall);
                }

                return orderedQuery ?? (IOrderedQueryable<TEntity>)query;
            };
        }

        return options;
    }

    private static Expression<Func<TEntity, bool>>? ExtractWhereExpression<TEntity>(Expression expression)
    {
        if (expression is MethodCallExpression methodCall && methodCall.Method.Name == "Where")
        {
            if (methodCall.Arguments.Count >= 2 && methodCall.Arguments[1] is UnaryExpression unary && unary.Operand is Expression<Func<TEntity, bool>> lambda)
            {
                return lambda;
            }
        }
        return null;
    }
}

#region DevExtreme Expression Yakalayıcı (Casus Sınıflar)

// DevExtreme'in oluşturduğu LINQ sorgu ağacını hafızada yakalayan hafif (mock) IQueryable yapısı
internal class ExpressionSpyQueryable<T> : IQueryable<T>, IQueryProvider
{
    public Expression? CapturedExpression { get; private set; }

    public Expression Expression => Expression.Constant(this);
    public Type ElementType => typeof(T);
    public IQueryProvider Provider => this;

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Array.Empty<T>()).GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    public IQueryable CreateQuery(Expression expression)
    {
        CapturedExpression = expression; // DevExtreme'in ürettiği Where ağacını burada yakalıyoruz!
        return this;
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        if (this is ExpressionSpyQueryable<TElement> spy)
        {
            spy.CapturedExpression = expression;
            return spy;
        }
        var newSpy = new ExpressionSpyQueryable<TElement> { CapturedExpression = expression };
        return newSpy;
    }

    public object? Execute(Expression expression) => null;
    public TResult Execute<TResult>(Expression expression) => default!;
}
#endregion