using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ClarityMapper.Common
{
    public static class ExpressionHelper
    {
        public static void SetPropertyValue<T, TValue>(T target, Expression<Func<T, TValue>> memberLambda,
            TValue value)
        {
            var member = memberLambda.Body as MemberExpression;
            var unary = memberLambda.Body as UnaryExpression;

            var memberExpression = member ?? unary?.Operand as MemberExpression;


            if (memberExpression == null) return;
            var property = memberExpression.Member as PropertyInfo;
            if (property != null)
            {
                property.SetValue(target, value, null);
            }
        }
    }
}