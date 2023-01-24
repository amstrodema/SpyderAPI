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
    public class ExaminationBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private QuestionBusiness _questionBusiness;
        private readonly UserBusiness _userBusiness;
        private readonly InstructorExamBusiness _instructorExamBusiness;

        public ExaminationBusiness(IUnitOfWork unitOfWork, QuestionBusiness questionBusiness, UserBusiness userBusiness, InstructorExamBusiness instructorExamBusiness)
        {
            _questionBusiness = questionBusiness;
            _userBusiness = userBusiness;
            _instructorExamBusiness = instructorExamBusiness;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Exam>> GetExams() =>
          await _unitOfWork.Exams.GetAll();

        public async Task<Exam> GetExamByID(Guid id) =>
                  await _unitOfWork.Exams.Find(id);

        public async Task Create(Exam exam)
        {
            await _unitOfWork.Exams.Create(exam);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<Exam>> CreateMultiple(Exam[] exams)
        {
            ResponseMessage<Exam> response = new ResponseMessage<Exam>();

            for (int i = 0; i < exams.Length; i++)
            {
                exams[i].DateCreated = DateTime.Now;
            }

            await _unitOfWork.Exams.CreateMultiple(exams);
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

        public async Task<ResponseMessage<Exam>> Update(Exam exam)
        {
            ResponseMessage<Exam> response = new ResponseMessage<Exam>();

            exam.DateModified = DateTime.Now;
            _unitOfWork.Exams.Update(exam);

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
            var entity = await GetExamByID(id);
            _unitOfWork.Exams.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<IEnumerable<ExamVM>>> GetExamsByNodeID(Guid nodeID, Guid userID)
        {

            ResponseMessage<IEnumerable<ExamVM>> res = new ResponseMessage<IEnumerable<ExamVM>>();

            try
            {
                var user = await _userBusiness.GetUserByID(userID);

                var exams = await _unitOfWork.Exams.GetExamsByNodeID(nodeID);
                var qstn = await _questionBusiness.GetQuestionsByNodeID(nodeID);

                var scdles = await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID);

                if (user.Level == 2)
                {
                    var exms = await _instructorExamBusiness.GetExamInstructorsByInstructorID(user.ID);

                    res.Data = from exam in exams where exms.FirstOrDefault(m=> m.ExamID == exam.ID) != default
                               select new ExamVM()
                               {
                                   Duration = exam.Duration,
                                   DurationTitle = exam.DurationTitle,
                                   ID = exam.ID,
                                   Instructions = exam.Instructions,
                                   IsDraft = exam.IsDraft,
                                   Name = exam.Name,
                                   Questions = null,
                                   Progress = (qstn.Where(c => c.ExamID == exam.ID).Count() / exam.TotalExaminationQuestionNo) * 100,
                                   Status = ((qstn.Where(c => c.ExamID == exam.ID).Count() - exam.TotalExaminationQuestionNo)) < 0 ? "Incomplete" : "Complete",
                                   totalExaminationQuestionNo = exam.TotalExaminationQuestionNo,
                                   NodeID = exam.NodeID,
                                   CreatedBy = exam.CreatedBy,
                                   DateCreated = exam.DateCreated,
                                   DateModified = exam.DateModified,
                                   IsActive = exam.IsActive,
                                   ModifiedBy = exam.ModifiedBy,
                                   postedByUserID = exam.PostedByUserID,
                                   Flag = scdles.FirstOrDefault(c => c.ExamID == exam.ID) == default ? 0 : 1
                               };

                }
                else
                {
                    res.Data = from exam in exams
                               select new ExamVM()
                               {
                                   Duration = exam.Duration,
                                   DurationTitle = exam.DurationTitle,
                                   ID = exam.ID,
                                   Instructions = exam.Instructions,
                                   IsDraft = exam.IsDraft,
                                   Name = exam.Name,
                                   Questions = null,
                                   Progress = (qstn.Where(c => c.ExamID == exam.ID).Count() / exam.TotalExaminationQuestionNo) * 100,
                                   Status = ((qstn.Where(c => c.ExamID == exam.ID).Count() - exam.TotalExaminationQuestionNo)) < 0 ? "Incomplete" : "Complete",
                                   totalExaminationQuestionNo = exam.TotalExaminationQuestionNo,
                                   NodeID = exam.NodeID,
                                   CreatedBy = exam.CreatedBy,
                                   DateCreated = exam.DateCreated,
                                   DateModified = exam.DateModified,
                                   IsActive = exam.IsActive,
                                   ModifiedBy = exam.ModifiedBy,
                                   postedByUserID = exam.PostedByUserID,
                                   Flag = scdles.FirstOrDefault(c => c.ExamID == exam.ID) == default ? 0 : 1
                               };

                }



                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

        public async Task<ResponseMessage<IEnumerable<ExamVM>>> GetExamsByNodeID(Guid nodeID)
        {
            ResponseMessage<IEnumerable<ExamVM>> res = new ResponseMessage<IEnumerable<ExamVM>>();

            try
            {
                var exams = await _unitOfWork.Exams.GetExamsByNodeID(nodeID);
                var qstn = await _questionBusiness.GetQuestionsByNodeID(nodeID);

                res.Data = from exam in exams
                           select new ExamVM()
                           {
                               Duration = exam.Duration,
                               DurationTitle = exam.DurationTitle,
                               ID = exam.ID,
                               Instructions = exam.Instructions,
                               IsDraft = exam.IsDraft,
                               Name = exam.Name,
                               Questions = null,
                               Progress = (qstn.Where(c => c.ExamID == exam.ID).Count() / exam.TotalExaminationQuestionNo) * 100,
                               Status = ((qstn.Where(c => c.ExamID == exam.ID).Count() - exam.TotalExaminationQuestionNo)) < 0 ? "Incomplete" : "Complete",
                               totalExaminationQuestionNo = exam.TotalExaminationQuestionNo,
                               NodeID = exam.NodeID,
                               CreatedBy = exam.CreatedBy,
                               DateCreated = exam.DateCreated,
                               DateModified = exam.DateModified,
                               IsActive = exam.IsActive,
                               ModifiedBy = exam.ModifiedBy,
                               postedByUserID = exam.PostedByUserID
                           };

                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

        public async Task<ResponseMessage<IEnumerable<ExamVM>>> GetExamsDetailsByNodeID(Guid nodeID)
        {
            ResponseMessage<IEnumerable<ExamVM>> res = new ResponseMessage<IEnumerable<ExamVM>>();

            try
            {
                var exams = await _unitOfWork.Exams.GetExamsByNodeID(nodeID);
                var qstn = await _questionBusiness.GetQuestionVMs(nodeID);
                var scdles = await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID);

                res.Data = from exam in exams
                           select new ExamVM()
                           {
                               Duration = exam.Duration,
                               DurationTitle = exam.DurationTitle,
                               ID = exam.ID,
                               Instructions = exam.Instructions,
                               IsDraft = exam.IsDraft,
                               Name = exam.Name,
                               Questions = (from questionVM in qstn where questionVM.ExamID == exam.ID
                                            select questionVM).ToArray(),
                               Progress = 0,
                               Status = "",
                               totalExaminationQuestionNo = exam.TotalExaminationQuestionNo,
                               NodeID = exam.NodeID,
                               CreatedBy = exam.CreatedBy,
                               DateCreated = exam.DateCreated,
                               DateModified = exam.DateModified,
                               IsActive = exam.IsActive,
                               ModifiedBy = exam.ModifiedBy,
                               postedByUserID =exam.PostedByUserID,
                               Flag = scdles.FirstOrDefault(c => c.ExamID == exam.ID) == default ? 0 : 1
                           };

                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }


        public async Task<ResponseMessage<ExamVM>> GetNodeExamDetailsByExamID(Guid nodeID, Guid examID)
        {
            ResponseMessage<ExamVM> res = new ResponseMessage<ExamVM>();

            try
            {
                var exams = await GetExamsDetailsByNodeID(nodeID);

                res.Data = exams.Data.FirstOrDefault(x => x.ID == examID);

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
