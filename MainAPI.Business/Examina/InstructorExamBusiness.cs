using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Examina;
using MainAPI.Models.ViewModel.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
    public class InstructorExamBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorExamBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<InstructorExam>> GetInstructorExams() =>
          await _unitOfWork.InstructorExams.GetAll();

        public async Task<InstructorExam> GetInstructorExamByID(Guid id) =>
                  await _unitOfWork.InstructorExams.Find(id);
        public async Task<IEnumerable<InstructorExam>> _GetExamInstructorsByNodeID(Guid nodeID) =>
                  await _unitOfWork.InstructorExams.GetInstructorExamsByNodeID(nodeID);
        public async Task<IEnumerable<InstructorExam>> GetExamInstructorsByExamID(Guid examID) =>
                  await _unitOfWork.InstructorExams.GetExamInstructorsByExamID(examID);
        public async Task<IEnumerable<InstructorExam>> GetExamInstructorsByInstructorID(Guid instructorID) =>
                  await _unitOfWork.InstructorExams.GetExamInstructorsByInstructorID(instructorID);
        public async Task Create(InstructorExam instructorExam)
        {
            await _unitOfWork.InstructorExams.Create(instructorExam);
            await _unitOfWork.Commit();
        }


        public async Task<ResponseMessage<IEnumerable<InstructorExam>>> GetExamInstructorsByNodeID(Guid nodeID)
        {
            ResponseMessage<IEnumerable<InstructorExam>> res = new ResponseMessage<IEnumerable<InstructorExam>>();

            try
            {
                res.Data = await _GetExamInstructorsByNodeID(nodeID);
                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

        public async Task<ResponseMessage<string>> AttachInstructorsToExam(InstructorExamVM instructorExamVM)
        {
            var oldExamInstructors = await GetExamInstructorsByExamID(instructorExamVM.ExamID);

            var examInstructorHolder = new List<InstructorExam>();

            foreach (var instructorID in instructorExamVM.InstructorIDs)
            {
                var holder = oldExamInstructors.FirstOrDefault(d => d.InstructorID == instructorID);

                if (holder == default)
                {
                    InstructorExam instructorExam = new InstructorExam()
                    {
                        ExamID = instructorExamVM.ExamID,
                        InstructorID = instructorID,
                        CreatedBy = instructorExamVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        ID = Guid.NewGuid(),
                        NodeID = instructorExamVM.NodeID
                    };

                    examInstructorHolder.Add(instructorExam);
                }
            }

            foreach (var item in oldExamInstructors)
            {
                if (instructorExamVM.InstructorIDs.FirstOrDefault(x => x == item.InstructorID) == default)
                {
                    _unitOfWork.InstructorExams.Delete(item);
                }
            }

            await _unitOfWork.InstructorExams.CreateMultiple(examInstructorHolder.ToArray());

            ResponseMessage<string> response = new ResponseMessage<string>();
            if (await _unitOfWork.Commit() > 0)
            {
                response.StatusCode = 200;
                response.Message = "Successful!";
            }
            else
            {
                response.StatusCode = 201;
                response.Message = "Error! Something went wrong";
            }

            return response;
        }
        
        public async Task Delete(Guid id)
        {
            var entity = await GetInstructorExamByID(id);
            _unitOfWork.InstructorExams.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<IEnumerable<InstructorExam>>> GetAllInstructorsAssignedToExam(Guid examID)
        {

            ResponseMessage<IEnumerable<InstructorExam>> res = new ResponseMessage<IEnumerable<InstructorExam>>();

            try
            {

                res.Data = await GetExamInstructorsByExamID(examID);

                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

      
}
}
