using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class NodeBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public NodeBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Node>> GetNodes() =>
          await _unitOfWork.Nodes.GetAll();

        public async Task<Node> GetNodeByID(Guid id) =>
                  await _unitOfWork.Nodes.Find(id);

        public async Task Create(Node Node)
        {
            await _unitOfWork.Nodes.Create(Node);
            await _unitOfWork.Commit();
        }

        public async Task Update(Node Node)
        {
            _unitOfWork.Nodes.Update(Node);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetNodeByID(id);
            _unitOfWork.Nodes.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<Node> GetNodeByAccessCode(string code) =>
                  await _unitOfWork.Nodes.GetNodeByAccessCode(code);

        public async Task<Node> GetNodeByClientID(Guid clientID) =>
                  await _unitOfWork.Nodes.GetNodeByClientID(clientID);

        public async Task<IEnumerable<Node>> GetNodesByClientID(Guid clientID) =>
                  await _unitOfWork.Nodes.GetNodesByClientID(clientID);
    }
}
