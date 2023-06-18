using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace asp.net_mvc_webapi_实用的接口加密方法示例.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}