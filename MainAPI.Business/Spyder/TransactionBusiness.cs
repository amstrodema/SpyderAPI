using MainAPI.Data.Interface;
using MainAPI.Models.Spyder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Spyder
{
    public class TransactionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Transaction>> GetTransactions() =>
          await _unitOfWork.Transactions.GetAll();

        public async Task<Transaction> GetTransactionByID(Guid id) =>
                  await _unitOfWork.Transactions.Find(id);
    }
}
