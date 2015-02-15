using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LabTechnicalTest.API.Logic
{
    class PostFile
    {
        public static void Run(HttpContext context)
        {
            try
            {
                var file = GetFileData("file", context);

                var s = file.InputStream;

                StreamReader sr = new StreamReader(s);
                string inputData = sr.ReadToEnd();

                var output = ProcessInput(inputData);

                APIHelpers.SerializeAndWriteJSONOutput(context, output);
            }
            catch (Exception ex)
            {
                //write output as exception message, and use default output of -1
                var output = new OutputFormat()
                {
                    Result = -1,
                    Message = ex.Message
                };

                APIHelpers.SerializeAndWriteJSONOutput(context, output);
            }
        }

        private static OutputFormat ProcessInput(string inputData)
        {
            string message = "";

            var output = new OutputFormat();

            //validate that input is valid...

            //check that input contains list of comma-separated ints between 0 and 2,147,483,647
            var parsedData = ParseData(inputData, out message);
            if (parsedData == null || !string.IsNullOrEmpty(message))
            {
                output.Message = message;
                output.Result = -1;
                return output;
            }

            //number of elements is from 1 to 100,000
            if (parsedData.Count < 1 || parsedData.Count > 100000)
            {
                output.Message = string.Format("Length of input was '{0}', length should be between 1 and 100,000", parsedData.Count);
                output.Result = -1;
                return output;
            }

            //all is well with the input data, now we can actually process it



            return output;
        }

        private static List<int> ParseData(string input, out string message)
        {
            var values = input.Split(',');
            List<int> output = new List<int>();
            foreach (var value in values)
            {
                int parsedInt = -1;
                if (Int32.TryParse(value, out parsedInt))
                {
                    //the parse should fail for a value above 2147483647, as that's the max value for an int32, but I'm checking just to be safe...
                    if (parsedInt < 0 || parsedInt > 2147483647)
                    {
                        message = string.Format("Value '{0}' is not in the range 0-2147483647", value);
                        return null;
                    }
                    else
                    {
                        output.Add(parsedInt);
                    }
                }
                else
                {
                    message = string.Format("Value '{0}' is not a valid int.", value);
                    return null;
                }
            }

            message = "";
            return output;
        }

        private static HttpPostedFile GetFileData(string fieldName, HttpContext context)
        {
            if (context.Request.Files[fieldName] != null)
            {
                return context.Request.Files[fieldName];
            }
            else
            {
                return null;
            }
        }

        public class OutputFormat
        {
            public int Result { get; set; }
            public string Message { get; set; }
        }
    }
}
