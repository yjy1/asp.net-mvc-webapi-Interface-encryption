using System;
using System.Reflection;

namespace asp.net_mvc_webapi_实用的接口加密方法示例.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}