using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class ClientBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Client>> GetClients() =>
          await _unitOfWork.Clients.GetAll();

        public async Task<Client> GetClientByID(Guid id) =>
                  await _unitOfWork.Clients.Find(id);

        public async Task Create(Client Client)
        {
            await _unitOfWork.Clients.Create(Client);
            await _unitOfWork.Commit();
        }

        public async Task Update(Client Client)
        {
            _unitOfWork.Clients.Update(Client);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetClientByID(id);
            _unitOfWork.Clients.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<Client> GetClientByCode(string code) =>
                  await _unitOfWork.Clients.GetClientByCode(code);

        public async Task<Client> GetClientByLicense(string license) =>
                  await _unitOfWork.Clients.GetClientByLicense(license);
    }
}
