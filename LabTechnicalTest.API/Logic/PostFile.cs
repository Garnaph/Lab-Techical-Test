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
            var file = GetFileData("file", context);

            var s = file.InputStream;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageUploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string strFileName = file.FileName;

            string filePath = path + "\\" + strFileName;
            FileStream fs = new FileStream(filePath, FileMode.Create);
            byte[] read = new byte[256];
            int count = s.Read(read, 0, read.Length);
            while (count > 0)
            {
                fs.Write(read, 0, count);
                count = s.Read(read, 0, read.Length);
            }
            fs.Close();

            //var wrapper = GetData();

            //APIHelpers.SerializeAndWriteJSONOutput(context, wrapper);
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
    }
}
