using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
//using Six
namespace MainAPI.Services
{
    public class ImageService
    {
        private static void ResizeImage(string base64Image, int width, int height, string imageID, string folderName)
        {
            // Decode the base64 string to a byte array
            byte[] bytes = Convert.FromBase64String(base64Image);

            // Load the image from the byte array using ImageSharp
            using (Image image = Image.Load(bytes))
            {
                // Resize the image while retaining its aspect ratio and quality
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max,
                    Compand = false,
                    Sampler = KnownResamplers.Lanczos3,
                }));


                    var roota = AppDomain.CurrentDomain;
                    var root = roota.BaseDirectory;
                    string folder = Path.Combine(root, "Images");
                    folder = Path.Combine(folder, "Small");
                    folder = Path.Combine(folder, folderName);
                    string uniqueFileName = imageID + ".png";
                    string filePath = Path.Combine(folder, uniqueFileName);

                    if (!(Directory.Exists(folder)))
                    {
                        Directory.CreateDirectory(folder);
                    }

                // Convert the image back to a base64 string
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    byte[] imageBytes = ms.ToArray();

                    try
                    {

                        MemoryStream ms2 = new MemoryStream(imageBytes);

                        try
                        {
                            using (Stream stream = new FileStream(filePath, FileMode.Create))
                            {
                                ms2.CopyTo(stream);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }


        public static string SaveImageInFolder(string image, string imageID, string folderName)
        {

            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, "Large");
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

            try
            {
                ResizeImage(image, 100, 100, imageID, folderName);
            }
            catch (Exception)
            {

                throw;
            }

            //ResizeImage(data, imageID, folderName);

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
            folder = Path.Combine(folder, "Large");
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
        public static string GetSmallImageFromFolder(string uniqueFileName, string folderName)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, "Small");
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
