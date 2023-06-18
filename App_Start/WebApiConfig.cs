using asp.net_mvc_webapi_实用的接口加密方法示例.Filters;
using asp.net_mvc_webapi_实用的接口加密方法示例.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace asp.net_mvc_webapi_实用的接口加密方法示例
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new ApiSecurityFilter());

            config.Filters.Add(new ApiHandleErrorAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
