using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LabTechnicalTest.API
{
    public static class APIHelpers
    {
        public static void SerializeAndWriteJSONOutput(HttpContext context, object content)
        {
            WriteJSONOutput(context, SerializeContent(content));
        }

        public static void WriteJSONOutput(HttpContext context, string output)
        {
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Write(output);
        }

        public static string SerializeContent(object content)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            //this allows for null fields to simply not exist in serialised output, lowering the weight of the data being transmitted
            jsonSerializer.NullValueHandling = NullValueHandling.Ignore;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            jsonSerializer.Serialize(sw, content);
            return sb.ToString();
        }
    }
}
