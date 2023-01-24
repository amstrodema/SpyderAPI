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
    public class PaymentBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Payment>> GetPayments() =>
         await _unitOfWork.Payments.GetAll();

        public async Task<Payment> GetPaymentByID(Guid id) =>
                  await _unitOfWork.Payments.Find(id);
        public async Task<ResponseMessage<Payment>> Create(Payment Payment)
        {
            ResponseMessage<Payment> responseMessage = new ResponseMessage<Payment>();
            try
            {
                Payment.ID = Guid.NewGuid();
                Payment.DateCreated = DateTime.Now;
                await _unitOfWork.Payments.Create(Payment);
                if (await _unitOfWork.Commit() >= 1)
                {
                    responseMessage.Data = Payment;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Operation successful!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Operation not successful!";
                }
            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong. Try Again!";
            }

            return responseMessage;
        }
    }
}
