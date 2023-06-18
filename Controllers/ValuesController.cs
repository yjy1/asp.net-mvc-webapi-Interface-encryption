using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace asp.net_mvc_webapi_实用的接口加密方法示例.Controllers
{
    public class ValuesController : ApiController
    {
        //参考资料：
        //作者：谷歌''''s    
        //博客地址：https://www.jb51.net/article/126376.htm
        //来源：脚本之家
        //asp.net mvc webapi 实用的 接口加密 方法示例

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "哈基米", "厉不厉害你坤哥" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
