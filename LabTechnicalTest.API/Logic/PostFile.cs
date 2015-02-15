using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LabTechnicalTest.API.Logic
{
    public class PostFile
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

        public static OutputFormat ProcessInput(string inputData)
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

            int inputLength = parsedData.Count;

            //number of elements is from 1 to 100,000
            if (inputLength < 1 || inputLength > 100000)
            {
                output.Message = string.Format("Length of input was '{0}', length should be between 1 and 100,000", inputLength);
                output.Result = -1;
                return output;
            }

            //all is well with the input data, now we can actually process it

            //use float, as inputLength might be odd, and we want the value for half of this to reflect accurately
            float threshold = ((float)inputLength) / 2;

            Dictionary<int, int> ongoingData = new Dictionary<int, int>();
            for (int i = 0; i < inputLength;i++)
            {
                int currentValue = parsedData[i];

                //If over half the list has been processed, and we don't yet have a winner, we're not going to get one.
                //Very important to only do this check as we move onto a number we have not seen before, however, as the winner may span over the halfway mark
                if ((!ongoingData.ContainsKey(currentValue))
                    &&
                    (i > threshold))
                {
                    output.Result = -1;
                    output.Message = string.Format("No clear winner after passing over half the items in the list. Data length '{0}', Threshold '{1}'", inputLength, threshold);
                    return output;
                }

                int currentCount = 0;

                if (ongoingData.ContainsKey(currentValue))
                {
                    currentCount = ongoingData[currentValue] + 1;
                    ongoingData[currentValue] = currentCount;
                }
                else
                {
                    currentCount = 1;
                    ongoingData.Add(currentValue, currentCount);
                }

                if (currentCount > threshold)
                {
                    output.Result = currentValue;
                    output.Message = string.Format("Value '{0}' is the winner, with a count of at least '{1}'. Data length was '{2}', giving a threshold of '{3}'.",
                        currentValue, currentCount, inputLength, threshold
                        );
                    return output;
                }
            }

            output.Message = "No clear winner";
            return output;
        }

        private static List<int> ParseData(string input, out string message)
        {
            if (string.IsNullOrEmpty(input))
            {
                message = "Input was null or empty";
                return null;
            }

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
                    message = string.Format("Value '{0}' is not a valid Int32 value (between -2,147,483,648 and 2,147,483,647).", value);
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
