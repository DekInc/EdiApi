using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EdiViewer.Utility
{
    public class ExpressionBuilderHelper
    {
        public enum Comparison
        {
            Equal,
            LessThan,
            LessThanOrEqual,
            GreaterThan,
            GreaterThanOrEqual,
            NotEqual,
            Contains, //for strings  
            StartsWith, //for strings  
            EndsWith //for strings  
        }
        public class ExpressionFilter
        {
            public string PropertyName { get; set; }
            public object Value { get; set; }
            public Comparison Comparison { get; set; }
        }
        public static List<T> W2uiSearch<T>(List<T> Records, IFormCollection GridForm)
        {
            int GridLimit = Convert.ToInt32(GridForm["limit"].Fod());
            int GridOffset = Convert.ToInt32(GridForm["offset"].Fod());
            List<ExpressionFilter> ListGridSearch = new List<ExpressionFilter>();
            Utility.ExpressionBuilderHelper.ConstructList(ref ListGridSearch, GridForm);
            var ExpressionTree = Utility.ExpressionBuilderHelper.ConstructAndExpressionTree<T>(ListGridSearch);
            if (ListGridSearch.Count > 0)
            {
                var AnonFunc = ExpressionTree.Compile();
                return Records.Where(AnonFunc).Skip(GridOffset).Take(GridLimit).ToList();
            } else
            {
                return Records.Skip(GridOffset).Take(GridLimit).ToList();
            }
        }
        public static Expression<Func<T, bool>> ConstructAndExpressionTree<T>(List<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
                return null;
            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;
            if (filters.Count == 1)
                exp = ExpressionRetriever.GetExpression<T>(param, filters[0]);
            else
            {
                exp = ExpressionRetriever.GetExpression<T>(param, filters[0]);
                for (int i = 1; i < filters.Count; i++)
                {
                    exp = Expression.And(exp, ExpressionRetriever.GetExpression<T>(param, filters[i]));
                }
            }
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
        public static class ExpressionRetriever
        {
            private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            public static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
            {
                MemberExpression member = Expression.Property(param, filter.PropertyName);
                ConstantExpression constant = Expression.Constant(filter.Value);
                switch (filter.Comparison)
                {
                    case Comparison.Equal:
                        return Expression.Equal(member, constant);
                    case Comparison.GreaterThan:
                        return Expression.GreaterThan(member, constant);
                    case Comparison.GreaterThanOrEqual:
                        return Expression.GreaterThanOrEqual(member, constant);
                    case Comparison.LessThan:
                        return Expression.LessThan(member, constant);
                    case Comparison.LessThanOrEqual:
                        return Expression.LessThanOrEqual(member, constant);
                    case Comparison.NotEqual:
                        return Expression.NotEqual(member, constant);
                    case Comparison.Contains:
                        return Expression.Call(member, containsMethod, constant);
                    case Comparison.StartsWith:
                        return Expression.Call(member, startsWithMethod, constant);
                    case Comparison.EndsWith:
                        return Expression.Call(member, endsWithMethod, constant);
                    default:
                        return null;
                }
            }
        }
        public static void ConstructList(ref List<ExpressionFilter> ListGridSearch, IFormCollection GridForm)
        {
            for (UInt16 i = 0; i < 100; i++)
            {
                if (GridForm[$"search[{i}][field]"].Fod() == null) break;
                ListGridSearch.Add(new Utility.ExpressionBuilderHelper.ExpressionFilter()
                {
                    PropertyName = GridForm[$"search[{i}][field]"].Fod(),
                    Value = GridForm[$"search[{i}][value]"].Fod()
                    //Field = GridForm[$"search[{i}][field]"].Fod(),
                    //Type = GridForm[$"search[{i}][type]"].Fod(),
                    //Operator = GridForm[$"search[{i}][operator]"].Fod(),
                    //Value = GridForm[$"search[{i}][value]"].Fod()
                });
                string SearchType = GridForm[$"search[{i}][operator]"].Fod();
                switch (SearchType)
                {
                    case "is":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.Equal;
                        break;
                    case "begins":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.StartsWith;
                        break;
                    case "contains":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.Contains;
                        break;
                    case "ends":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.EndsWith;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
