using MainAPI.Business.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Models.ViewModel.Spyder;
using MainAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Controllers.Spyder
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageBusiness imageBusiness;
        private readonly JWTService _jwtService;
        private readonly LinkBusiness linkBusiness;

        public ImageController(ImageBusiness imageBusiness, JWTService jWTService, LinkBusiness linkBusiness)
        {
            this.imageBusiness = imageBusiness;
            _jwtService = jWTService;
            this.linkBusiness = linkBusiness;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var images = await imageBusiness.GetImages();
            return Ok(images);
        }

        [HttpGet("GetGallery")]
        public async Task<ActionResult> GetGallery(Guid itemID)
        {
            galleryVM galleryVM = new galleryVM();
            galleryVM.Images = await imageBusiness.GetItemImages(itemID);
            galleryVM.Links = await linkBusiness.GetLinksByItemID(itemID);
            return Ok(galleryVM);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var image = await imageBusiness.GetImageByID(id);
            return Ok(image);
        }
        [HttpPost]
        public async Task<ActionResult> Post(Image image)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid entries!");

            var res = await imageBusiness.Create(image);
            return Ok(res);
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put([FromBody] Feature feature, Guid id)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest("Invalid entries!");

        //    if (image.ID != id)
        //        return BadRequest("Invalid record!");

        //    int res = await imageBusiness.Update(feature);
        //    ResponseMessage<Hall> responseMessage = new ResponseMessage<Hall>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record updated!";
        //        responseMessage.StatusCode = 200;
        //        responseMessage.Data = image;
        //    }
        //    else
        //    {
        //        responseMessage.Message = "No record updated!";
        //        responseMessage.StatusCode = 201;
        //    }
        //    return Ok(responseMessage);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(Guid id)
        //{
        //    int res = await imageBusiness.Delete(id);
        //    ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        //    if (res >= 1)
        //    {
        //        responseMessage.Message = "Record deleted!";
        //        responseMessage.StatusCode = 200;
        //    }
        //    else
        //    {
        //        responseMessage.Message = "No record deleted!";
        //        responseMessage.StatusCode = 201;
        //    }
        //    return Ok(responseMessage);
        //}
    }
}
