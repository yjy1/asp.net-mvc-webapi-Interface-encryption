using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace asp.net_mvc_webapi_实用的接口加密方法示例.Handler
{
    public class ApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// add by laiyunba 
        /// </summary>
        /// <param name="filterContext">context oop</param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            //LoggerFactory.CreateLog().LogError(Messages.error_unmanagederror, filterContext.Exception);

        }
    }
}