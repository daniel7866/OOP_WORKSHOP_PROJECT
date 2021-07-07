using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_WORKSHOP_PROJECT
{
    public abstract class Services
    {
        public static string SaveImage(IFormFile file, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string newFilePath = path + DateTime.Now.GetHashCode() + file.FileName;
            using (FileStream fileStream = System.IO.File.Create(newFilePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            return newFilePath;
        }
    }
}
