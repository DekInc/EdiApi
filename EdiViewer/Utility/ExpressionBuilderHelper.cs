using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
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
            public string TypeO { get; set; }
            public object Value { get; set; }
            public Comparison Comparison { get; set; }
        }
        public static List<T> W2uiSearch<T>(List<T> Records, IFormCollection GridForm)
        {
            int GridLimit = Convert.ToInt32(GridForm["limit"].Fod());
            int GridOffset = Convert.ToInt32(GridForm["offset"].Fod());
            List<ExpressionFilter> ListGridSearch = new List<ExpressionFilter>();
            List<ExpressionFilter> ListGridSort = new List<ExpressionFilter>();
            ConstructList(ref ListGridSearch, ref ListGridSort, GridForm);
            var ExpressionTree = ConstructAndExpressionTree<T>(ListGridSearch);
            if (ListGridSearch.Count > 0)
            {
                var AnonFunc = ExpressionTree.Compile();
                Records = Records.Where(AnonFunc).Skip(GridOffset).Take(GridLimit).ToList();                
            } else
                Records = Records.Skip(GridOffset).Take(GridLimit).ToList();
            if (ListGridSort.Count > 0)
                Records = Records.AsQueryable().OrderBy(ListGridSort.Fod().PropertyName + " " + ListGridSort.Fod().Value.ToString()).ToList();
            return Records;
        }
        public static List<T> W2uiSearchNoSkip<T>(List<T> Records, IFormCollection GridForm) {
            int GridLimit = Convert.ToInt32(GridForm["limit"].Fod());
            int GridOffset = Convert.ToInt32(GridForm["offset"].Fod());
            List<ExpressionFilter> ListGridSearch = new List<ExpressionFilter>();
            List<ExpressionFilter> ListGridSort = new List<ExpressionFilter>();
            ConstructList(ref ListGridSearch, ref ListGridSort, GridForm);
            var ExpressionTree = ConstructAndExpressionTree<T>(ListGridSearch);
            if (ListGridSearch.Count > 0) {
                var AnonFunc = ExpressionTree.Compile();
                Records = Records.Where(AnonFunc).ToList();
            }
            if (ListGridSort.Count > 0)
                Records = Records.AsQueryable().OrderBy(ListGridSort.Fod().PropertyName + " " + ListGridSort.Fod().Value.ToString()).ToList();
            return Records;
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
                    exp = Expression.Or(exp, ExpressionRetriever.GetExpression<T>(param, filters[i]));
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
                if (filter.TypeO == "text")
                    constant = Expression.Constant(Convert.ToString(filter.Value), typeof(string));
                if (filter.TypeO == "int")
                    constant = Expression.Constant(Convert.ToInt32(filter.Value), typeof(int));
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
        public static void ConstructList(ref List<ExpressionFilter> ListGridSearch, ref List<ExpressionFilter> ListGridSort, IFormCollection GridForm)
        {
            for (UInt16 i = 0; i < 100; i++)
            {
                if (GridForm[$"sort[0][field]"].Fod() != null) {
                    ListGridSort.Add(new ExpressionFilter() {
                        PropertyName = GridForm[$"sort[0][field]"].Fod(),
                        Value = GridForm[$"sort[0][direction]"].Fod()
                    });
                }
                if (GridForm[$"search[{i}][field]"].Fod() == null) break;                
                ListGridSearch.Add(new Utility.ExpressionBuilderHelper.ExpressionFilter()
                {
                    PropertyName = GridForm[$"search[{i}][field]"].Fod(),
                    Value = GridForm[$"search[{i}][value]"].Fod(),
                    TypeO = GridForm[$"search[{i}][type]"].Fod()
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
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.Contains;
                        break;
                    case "contains":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.Contains;
                        break;
                    case "ends":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.EndsWith;
                        break;
                    case "more":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.GreaterThan;
                        break;
                    case "less":
                        ListGridSearch.Last().Comparison = Utility.ExpressionBuilderHelper.Comparison.LessThan;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
