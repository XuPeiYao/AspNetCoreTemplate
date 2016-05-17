using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Extensions.EntityFramework {
    /// <summary>
    /// 針對EntityFramework的查詢結果的擴充方法
    /// </summary>
    public static class IQueryableExtension {
        /// <summary>
        /// 完整載入屬性
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source">擴充對象</param>
        /// <param name="Level">目標階層數量(預設僅目前物件屬性)</param>
        /// <returns></returns>
        public static IQueryable<TEntity> FullInclude<TEntity>(this IQueryable<TEntity> source, int Level = 0) where TEntity : class {
            Type GenericType = source.GetType().GetGenericArguments()?[0];

            PropertyInfo[] Properties = GenericType.GetProperties();

            IQueryable<TEntity> result = source;

            foreach (var Property in Properties) {
                Attribute EFAttribute = (Attribute)Property.GetCustomAttribute<ForeignKeyAttribute>(true) ??
                                        (Attribute)Property.GetCustomAttribute<InversePropertyAttribute>(true);
                if (EFAttribute == null) {
                    var temp = GenericType.GetInterfaces()
                                .Select(x => x.GetProperty(Property.Name))
                                .Where(x => x != null).FirstOrDefault();
                    if (temp != null) {
                        EFAttribute = (Attribute)temp.GetCustomAttribute<ForeignKeyAttribute>(true) ??
                                      (Attribute)temp.GetCustomAttribute<InversePropertyAttribute>(true);
                    }
                }

                if (EFAttribute == null) continue;

                #region 建構Lambda
                var header = Expression.Parameter(GenericType, "item");
                Expression<Func<TEntity, object>> func = Expression.Lambda<Func<TEntity, object>>(
                    Expression.MakeMemberAccess(header, Property),
                    header
                );
                var header2 = Expression.Parameter(typeof(object), "item");
                Expression<Func<dynamic, object>> func2 = Expression.Lambda<Func<dynamic, object>>(
                    Expression.MakeMemberAccess(header, Property),
                    header2
                );
                #endregion

                List<Type> typeList = new List<Type>();
                if (Level > 0) {
                    result = result.Include(func).ThenFullInclude<TEntity>(func2, typeList, Level - 1);//解決單層讀取問題
                } else {
                    result = result.Include(func);
                }
            }
            return result;
        }

        private static IIncludableQueryable<TEntity, dynamic> ThenFullInclude<TEntity>(this IIncludableQueryable<TEntity, dynamic> source, Expression<Func<dynamic, object>> Func, List<Type> typeList, int Level) where TEntity : class {
            //var temp = Func.Compile().Invoke(source.First());

            MemberExpression e = (MemberExpression)Func.Body;
            Type type = ((PropertyInfo)e.Member).PropertyType;
            if (type.Namespace == "System.Collections.Generic") {
                type = type.GenericTypeArguments.First();
            }
            if (typeList.Contains(type)) return source;

            IIncludableQueryable<TEntity, dynamic> result = source;
            PropertyInfo[] Properties = type.GetProperties();

            foreach (var Property in Properties) {
                Attribute EFAttribute = (Attribute)Property.GetCustomAttribute<ForeignKeyAttribute>(true) ??
                                        (Attribute)Property.GetCustomAttribute<InversePropertyAttribute>(true);
                if (EFAttribute == null) {
                    var temp = type.GetInterfaces()
                                .Select(x => x.GetProperty(Property.Name))
                                .Where(x => x != null).FirstOrDefault();
                    if (temp != null) {
                        EFAttribute = (Attribute)temp.GetCustomAttribute<ForeignKeyAttribute>(true) ??
                                      (Attribute)temp.GetCustomAttribute<InversePropertyAttribute>(true);
                    }
                }

                if (EFAttribute == null) continue;

                if (Property.PropertyType == source.ElementType) continue;

                #region 建構Lambda
                var header = Expression.Parameter(typeof(object), "item");
                Expression<Func<dynamic, dynamic>> func = Expression.Lambda<Func<dynamic, dynamic>>(
                    Expression.MakeMemberAccess(Expression.Convert(header, type), Property),
                    header
                );
                #endregion

                List<Type> typeList2 = new List<Type>(typeList.ToArray());
                typeList2.Add(type);//Function Return Type

                if (Level > 0) {
                    result = result.ThenInclude(func).ThenFullInclude(func, typeList2, Level - 1);
                } else {
                    result = result.ThenInclude(func);//.ThenLazyLoad<TEntity>(func);//解決單層讀取問題
                }
            }
            return result;
        }
    }
}
