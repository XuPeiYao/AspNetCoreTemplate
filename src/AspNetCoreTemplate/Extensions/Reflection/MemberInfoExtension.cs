using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Extensions.Reflection {
    /// <summary>
    /// 針對類別成員取得方式的擴充方法
    /// </summary>
    public static class MemberInfoExtension {
        /// <summary>
        /// 取得屬性或欄位的MemberInfo物件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj">擴充對象</param>
        /// <param name="path">路徑</param>
        /// <returns>MemberInfo</returns>
        public static MemberInfo GetMemberInfo<TEntity>(this TEntity obj, Expression<Func<TEntity, object>> path) {
            MemberExpression body = (MemberExpression)path.Body;
            return body.Member;
        }
    }
}
