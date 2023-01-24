using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace MainAPI.Services
{
    public class ImageService
    {
        public static string SaveImageInFolder(string image, string imageID, string folderName)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, folderName);
            string uniqueFileName = imageID +".png";
            string filePath = Path.Combine(folder, uniqueFileName);

            if (!(Directory.Exists(folder)))
            {
                Directory.CreateDirectory(folder);
            }

            int index = image.IndexOf(",");
            image = image.Substring(index + 1);
          //  string cleandata = image.Replace("data:image/png;base64,", "");
            byte[] data = Convert.FromBase64String(image);
            MemoryStream ms = new MemoryStream(data);

            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    ms.CopyTo(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return uniqueFileName;
        }
        public static string GetImageFromFolder(string uniqueFileName, string folderName)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, folderName);

            string filePath = Path.Combine(folder, uniqueFileName);

            try
            {
                byte[] imageArray = File.ReadAllBytes(filePath);
                string val = "data:image/png;base64,"+Convert.ToBase64String(imageArray);

                return val;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
