﻿using System.Web;
using System.Web.Mvc;

namespace asp.net_mvc_webapi_实用的接口加密方法示例
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
