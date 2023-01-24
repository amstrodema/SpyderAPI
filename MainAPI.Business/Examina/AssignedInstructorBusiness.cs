using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class AssignedInstructorBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignedInstructorBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AssignedInstructor>> GetAssignedInstructors() =>
          await _unitOfWork.AssignedInstructors.GetAll();

        public async Task<AssignedInstructor> GetAssignedInstructorByID(Guid id) =>
                  await _unitOfWork.AssignedInstructors.Find(id);

        public async Task Create(AssignedInstructor assignedInstructor)
        {
            await _unitOfWork.AssignedInstructors.Create(assignedInstructor);
            await _unitOfWork.Commit();
        }

        public async Task Update(AssignedInstructor assignedInstructor)
        {
            _unitOfWork.AssignedInstructors.Update(assignedInstructor);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetAssignedInstructorByID(id);
            _unitOfWork.AssignedInstructors.Delete(entity);
            await _unitOfWork.Commit();
        }
        public async Task<IEnumerable<AssignedInstructor>> GetAssignedInstructorsByNodeID(Guid nodeID) =>
                await _unitOfWork.AssignedInstructors.GetAssignedInstructorsByNodeID(nodeID);

    }
}
