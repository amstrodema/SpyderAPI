using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class SosClusterBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public SosClusterBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SosCluster>> GetSosClusters() =>
         await _unitOfWork.SosClusters.GetAll();

        public async Task<SosCluster> GetSosClusterByID(Guid id) =>
                  await _unitOfWork.SosClusters.Find(id);
        public async Task<ResponseMessage<SosCluster>> Create(SosCluster SosCluster)
        {
            ResponseMessage<SosCluster> responseMessage = new ResponseMessage<SosCluster>();
            try
            {
                SosCluster.ID = Guid.NewGuid();
                SosCluster.DateCreated = DateTime.Now;
                await _unitOfWork.SosClusters.Create(SosCluster);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = SosCluster;
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
