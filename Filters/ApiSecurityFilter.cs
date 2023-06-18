using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace asp.net_mvc_webapi_实用的接口加密方法示例.Filters
{
    public class ApiSecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            var method = request.Method.Method;
            //注意：这个key要在 app 里和我们的 webapi 里 各保存相同的一份！
            var staffId = "^***********************************$";  

            string timestamp = string.Empty, nonce = string.Empty, signature = string.Empty;

            if (request.Headers.Contains("timestamp"))
                timestamp = request.Headers.GetValues("timestamp").FirstOrDefault();

            if (request.Headers.Contains("nonce"))  //随机数
                nonce = request.Headers.GetValues("nonce").FirstOrDefault();

            if (request.Headers.Contains("signature"))  //签名 95c5bd73cbf28680c0d5a5b91369c8d8   请求参数的 aa8cdaf911305aaf9fcf92c69c69131e
                signature = request.Headers.GetValues("signature").FirstOrDefault();

            //判断请求头必要参数
            if (string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(signature))
                throw new SecurityException();

            double ts = 0;
            bool timespanvalidate = double.TryParse(timestamp, out ts);

            //判断是否超时30分钟 1687017653000 是以6月18号00：00点生成的  ，当前时间是6月18号00：53 已过期
            bool falg = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds - ts > 30 * 60 * 1000  ;

            //判断时间戳是否有效或过期
            if (falg || (!timespanvalidate))
                throw new SecurityException("timeSpanValidate");

            var data = string.Empty;
            IDictionary<string, string> sortedParams = null;

            switch (method.ToUpper())
            {
                case "POST":
                case "PUT":
                case "DELETE":

                    Stream stream = HttpContext.Current.Request.InputStream;
                    StreamReader streamReader = new StreamReader(stream);
                    sortedParams = new SortedDictionary<string, string>(new JsonSerializer().Deserialize<Dictionary<string, string>>(new JsonTextReader(streamReader)));

                    break;

                case "GET":

                    IDictionary<string, string> parameters = new Dictionary<string, string>();

                    foreach (string key in HttpContext.Current.Request.QueryString)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            parameters.Add(key, HttpContext.Current.Request.QueryString[key]);
                        }
                    }

                    sortedParams = new SortedDictionary<string, string>(parameters);

                    break;

                default:
                    throw new SecurityException("defaultOptions");
            }

            StringBuilder query = new StringBuilder();

            if (sortedParams != null)
            {
                foreach (var sort in sortedParams.OrderBy(k => k.Key))
                {
                    if (!string.IsNullOrEmpty(sort.Key))
                    {
                        query.Append(sort.Key).Append(sort.Value);
                    }
                }

                data = query.ToString().Replace(" ", "");
            }

            //程序加密字符
            var md5Staff = GetMD5(string.Concat(timestamp + nonce + staffId + data)/*, 32*/); //程序生成的 aa8cdaf911305aaf9fcf92c69c69131e

            //程序加密的和请求头的进行比较
            if (!md5Staff.Equals(signature))
                throw new SecurityException("md5Staff");

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        public static string GetMD5(string str)
        {
            //创建MD5对象
            MD5 md5 = MD5.Create();
            //开始加密： 通过传入的明文密码，转换为字节数组，还可以通过其他的方式来获取密文
            //1. 需要将字符串转换成字节数组，
            byte[] buffer = Encoding.Default.GetBytes(str);
            // 2.把明文字符串转换得到的字节数组，传给md5，获取密文（哈希值）得到一个字节数组
            byte[] MD5buffer = md5.ComputeHash(buffer);
            //如果直接输出会出现乱码，原因：
            // 如果是把 125 128 212 这个十六进制的数解析出来可能是 假定 我爱你
            //我们要得到这段编码，那么给字节数组中每个元素都转换为字符串然后拼接起来就可以了

            //将字节数组转换成字符串
            //将字节数组转换为字符串三种方法：
            // 1.将字节数组中每个元素按照指定的编码格式解析成字符串
            //2.直接将数组Tostring（）；
            //3.字节数组中每个元素ToString（）

            //这里提供 string 和 stringbuider两种方法拼接
            //string s = null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < MD5buffer.Length; i++)
            {
                sb.Append(MD5buffer[i].ToString("x2"));  //ToString（）；传入参数目的是限定转换为字符串的格式
                                                         //s += MD5buffer[i].ToString();
            }
            //return s;
            return sb.ToString();
        }
    }

}