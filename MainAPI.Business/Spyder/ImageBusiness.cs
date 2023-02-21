using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class ImageBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly WalletBusiness walletBusiness;

        public ImageBusiness(IUnitOfWork unitOfWork, WalletBusiness walletBusiness)
        {
            _unitOfWork = unitOfWork;
            this.walletBusiness = walletBusiness;
        }

        public async Task<List<Image>> GetImages() =>
         await _unitOfWork.Images.GetAll();

        public async Task<Image> GetImageByID(Guid id)
        {
            var img = await _unitOfWork.Images.Find(id);
            img.Original = ImageService.GetImageFromFolder(img.Original, "Gallery");
            return img;
        }

        public async Task<IEnumerable<Image>> GetItemImages(Guid itemID)
        {
            return from img in await _unitOfWork.Images.GetItemImages(itemID)
                   select new Image()
                   {
                       Original = ImageService.GetImageFromFolder(img.Original, "Gallery"),
                       CreatedBy = img.CreatedBy,
                       DateCreated = img.DateCreated,
                       ID = img.ID,
                       IsActive = img.IsActive,
                       ItemID = img.ItemID,
                       Medium = img.Medium,
                       Small = img.Small
                   };
        }

        public async Task<ResponseMessage<Image>> Create(Image Image)
        {
            ResponseMessage<Image> responseMessage = new ResponseMessage<Image>();
            try
            {
                Image.ID = Guid.NewGuid();
                Image.DateCreated = DateTime.Now;
                Image.IsActive = true;

                Params param = await _unitOfWork.Params.GetParamByCode("gallery_image_cost");
                decimal gallery_image_cost = 0;

                try
                {
                    gallery_image_cost = decimal.Parse(param.Value);
                }
                catch (Exception)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Try again...";
                    return responseMessage;
                }

                var res = await walletBusiness.Payment(Image.CreatedBy, gallery_image_cost, await _unitOfWork.Users.GetUserCountryByUserID(Image.CreatedBy), "Gallery Image", Image.ID.ToString());

                if (res.StatusCode != 200)
                {
                    responseMessage.StatusCode = res.StatusCode;
                    responseMessage.Message = res.Message;
                    return responseMessage;
                }

                Image.Original = ImageService.SaveImageInFolder(Image.Original, Guid.NewGuid().ToString(), "Gallery");
                await _unitOfWork.Images.Create(Image);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Image;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Failed. Try Again!";
            }

            return responseMessage;
        }

    }
}
