using MainAPI.Data.Interface;
using MainAPI.Data.Interface.Examina;
using MainAPI.Models;
using MainAPI.Models.Examina;
using MainAPI.Models.ViewModel.Examina;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
    public class CandidateBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SettingBusiness _settingBusiness;
        private readonly ScheduleBusiness _scheduleBusiness;
        private readonly ScheduleGroupBusiness _scheduleGroupBusiness;
        private readonly CandidateGroupBusiness _candidateGroupBusiness;
        private readonly GroupBusiness _groupBusiness;
        private readonly ExaminationBusiness _examBusiness;
        private readonly QuestionBusiness _questionBusiness;
        private readonly QuestionImageBusiness _questionImageBusiness;
        private readonly OptionBusiness _optionBusiness;
        private readonly OptionImageBusiness _optionImageBusiness;

        public CandidateBusiness() { }
        public CandidateBusiness(IUnitOfWork unitOfWork, SettingBusiness settingBusiness, ScheduleBusiness scheduleBusiness,
            ScheduleGroupBusiness scheduleGroupBusiness, CandidateGroupBusiness candidateGroupBusiness, GroupBusiness groupBusiness,
            ExaminationBusiness examinationBusiness, QuestionBusiness questionBusiness, QuestionImageBusiness questionImageBusiness, OptionBusiness optionBusiness, OptionImageBusiness optionImageBusiness)
        {
            _unitOfWork = unitOfWork;
            _settingBusiness = settingBusiness;
            _scheduleBusiness = scheduleBusiness;
            _scheduleGroupBusiness = scheduleGroupBusiness;
            _candidateGroupBusiness = candidateGroupBusiness;
            _groupBusiness = groupBusiness;
            _questionBusiness = questionBusiness;
            _questionImageBusiness = questionImageBusiness;
            _optionBusiness = optionBusiness;
            _optionImageBusiness = optionImageBusiness;
            _examBusiness = examinationBusiness;
        }

        public async Task<List<Candidate>> GetCandidates() =>
          await _unitOfWork.Candidates.GetAll();

        public async Task<Candidate> GetCandidateByID(Guid id) =>
                  await _unitOfWork.Candidates.Find(id);

        public async Task Create(Candidate Candidate)
        {
            await _unitOfWork.Candidates.Create(Candidate);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<Candidate>> CreateMultiple(Candidate[] candidates)
        {
            ResponseMessage<Candidate>  response = new ResponseMessage<Candidate>();
            var cndts = await GetCandidates();

            string code = "";

            for (int i = 0; i < candidates.Length; i++)
            {
                do
                {
                    code = GenService.Gen10CapsDigitCode();
                } while (cndts.Find(e => e.CandidateNo == code) != null);

                candidates[i].CandidateNo = code;
                candidates[i].DateCreated = DateTime.Now;
            }

            await _unitOfWork.Candidates.CreateMultiple(candidates);
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

        public async Task<ResponseMessage<Candidate>> Update(Candidate Candidate)
        {
            ResponseMessage<Candidate> response = new ResponseMessage<Candidate>();

            //check if candidate is engaged
            if (await CandidateIsEngaged(Candidate.ID, Candidate.NodeID))
            {
                response.StatusCode = 201;
                response.Message = "Candidate is engaged";

                return response;
            }

            Candidate.DateModified = DateTime.Now;
            _unitOfWork.Candidates.Update(Candidate);

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

        public async Task<bool> CandidateIsEngaged(Guid candidateID, Guid nodeID)
        {
            var x = from scheduleGrp in await _unitOfWork.ScheduleGrps.GetScheduleGroupsByNodeID(nodeID)
                    join activeSchedule in await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID) on scheduleGrp.ScheduleID equals activeSchedule.ID
                    join candidateGrp in await _unitOfWork.CandidateGroups.GetCandidateGroupsByCandidateID(candidateID) on scheduleGrp.GroupID equals candidateGrp.GroupID
                    select candidateGrp;

            return x.Count() > 0 ? true : false;
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetCandidateByID(id);
            _unitOfWork.Candidates.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<IEnumerable<Candidate>>> GetCandidatesByNodeID(Guid nodeID)
        {
            ResponseMessage<IEnumerable<Candidate>> res = new ResponseMessage<IEnumerable<Candidate>>();

            var active = from schdlGrp in await _unitOfWork.ScheduleGrps.GetScheduleGroupsByNodeID(nodeID)
                         join activ in await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID) on schdlGrp.ScheduleID equals activ.ID
                         join candidateGrp in await _unitOfWork.CandidateGroups.GetCandidateGroupsByNodeID(nodeID) on schdlGrp.GroupID equals candidateGrp.GroupID
                         select candidateGrp;
                         

            try
            {
                res.Data = from candidate in await _unitOfWork.Candidates.GetCandidatesByNodeID(nodeID)
                           select new Candidate()
                           {
                               CandidateNo = candidate.CandidateNo,
                               ID = candidate.ID,
                               CreatedBy = candidate.CreatedBy,
                               Flag = active.FirstOrDefault(c => c.CandidateID == candidate.ID) == default ? 0 : 1,
                               DateCreated = candidate.DateCreated,
                               DateModified = candidate.DateModified,
                               Gender = candidate.Gender,
                               Image = candidate.Image,
                               IsActive = candidate.IsActive,
                               ModifiedBy = candidate.ModifiedBy,
                               Name = candidate.Name,
                               NodeID = candidate.NodeID
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

        public async Task<Candidate> GetCandidateByCandidateNo(string candidateNo) => await _unitOfWork.Candidates.GetCandidateByCandidateNo(candidateNo);
        
        public async Task<ResponseMessage<ExamPackage>> ValidateCandidate(LoginVM loginVM)
        {

            ResponseMessage<ExamPackage> responseMessage = new ResponseMessage<ExamPackage>();
            try
            {
                Candidate entity = await GetCandidateByCandidateNo(loginVM.Username);

                if (entity != null)
                {
                    var settings = await _settingBusiness.GetSettingsByNodeID(entity.NodeID);

                    Setting passwordSetting = settings.FirstOrDefault(x => x.Key == "GeneralPassword");

                    if (passwordSetting != null)
                    {
                        if (EncryptionService.Validate(loginVM.Password, passwordSetting.ValueString))
                        {
                            var schedules = await _scheduleBusiness.GetActiveScheduleVMsByNodeID(entity.NodeID);
                            var holder = await _candidateGroupBusiness.GetCandidateGroupsByCandidateID(entity.ID);
                            IEnumerable<CandidateGroup> candidateGroups = holder.Data;

                            var grps = await _groupBusiness.GetGroups(); //for retriving grp name

                            foreach (var schedule in schedules.Data)
                            {
                                var scheduleGrps = await _scheduleGroupBusiness.GetSchedulesGroupByScheduleID(schedule.ID);

                                foreach (var scheduled in scheduleGrps)
                                {
                                    var grp = candidateGroups.FirstOrDefault(x => x.GroupID == scheduled.GroupID);

                                    var exam = await _examBusiness.GetExamByID(schedule.ExamID);
                                    var questions = await _questionBusiness.GetQuestionsByExamID(exam.ID);

                                    if (grp != null)
                                    {
                                        ExamPackage examPackage = new ExamPackage();

                                        examPackage.Candidate = new CandidateVM()
                                        {
                                            CandidateNo = entity.CandidateNo,
                                            Gender = entity.Gender,
                                            GroupID = scheduled.GroupID,
                                            GroupName = grps.FirstOrDefault(x => x.ID == scheduled.GroupID).Name,
                                            ID = entity.ID,
                                            Image = entity.Image,
                                            Name = entity.Name
                                        };

                                        var optionImg = await _optionImageBusiness.GetOptionImageObjectsByNodeID(entity.NodeID);

                                        examPackage.Exam = new ExamVM()
                                        {
                                            Duration = exam.Duration,
                                            DurationTitle = exam.DurationTitle,
                                            ID = exam.ID,
                                            Instructions = exam.Instructions,
                                            IsDraft = exam.IsDraft,
                                            Name = exam.Name,
                                            Progress = 0,
                                            Questions = (from quest in questions
                                                         join questImg in await _questionImageBusiness.GetQuestionImages() on quest.ID equals questImg.QuestionID into questImgMem
                                                         join optn in await _optionBusiness.GetOptions() on quest.ID equals optn.QuestionID  into mem
                                                        // from optnX in mem join optImg in await _optionImageBusiness.GetOptionImages() on optnX.ID equals optImg.OptionID into optImgMem
                                                         select new QuestionVM()
                                                        {
                                                            CreatedBy = quest.CreatedBy,
                                                            DateCreated = quest.DateCreated.ToString(),
                                                            DateModified = quest.DateModified.ToString(),
                                                            ExamID = quest.ExamID,
                                                            ID = quest.ID,
                                                            IsActive = quest.IsActive,
                                                            IsTheory = quest.IsTheory,
                                                            ModifiedBy = quest.ModifiedBy,
                                                            NodeID = quest.NodeID,
                                                            QuestionText= quest.QuestionText,
                                                            Options = (from opt in mem
                                                                       join flar in optionImg on opt.ID equals flar.OptionID into dex
                                                                      select new OptionVM()
                                                                      {
                                                                           ID = opt.ID,
                                                                           IsActive = opt.IsActive,
                                                                           NodeID = opt.NodeID,
                                                                           OptionText = opt.OptionText,
                                                                           QuestionID = opt.QuestionID,
                                                                           OptionImages = dex.ToArray()
                                                                      }).ToArray(),
                                                            QuestionImages = questImgMem.ToArray()
                                                        }).ToArray(),
                                            Status = exam.Status,
                                            totalExaminationQuestionNo = exam.TotalExaminationQuestionNo
                                        };

                                        examPackage.Settings = new SettingVM()
                                        {
                                            AllowTheory = settings.FirstOrDefault(x => x.Key == "AllowTheory").ValueBoolean,
                                            AutoStartExams = settings.FirstOrDefault(x => x.Key == "AutoStartExams").ValueBoolean,
                                            ConfirmationAlert = settings.FirstOrDefault(x => x.Key == "ConfirmationAlert").ValueString,
                                            ExamHostName = settings.FirstOrDefault(x => x.Key == "ExamHostName").ValueString,
                                            ExaminationInstructionBtnText = settings.FirstOrDefault(x => x.Key == "ExaminationInstructionBtnText").ValueString,
                                            ExaminationRule = settings.FirstOrDefault(x => x.Key == "ExaminationRule").ValueString,
                                            ExaminationRuleTitle = settings.FirstOrDefault(x => x.Key == "ExaminationRuleTitle").ValueString,
                                            HostLogoImage = settings.FirstOrDefault(x => x.Key == "HostLogoImage").ValueString,
                                            LandingPageBtnText = settings.FirstOrDefault(x => x.Key == "LandingPageBtnText").ValueString,
                                            MainDeptName = settings.FirstOrDefault(x => x.Key == "MainDeptName").ValueString,
                                            MainQuickNote = settings.FirstOrDefault(x => x.Key == "MainQuickNote").ValueString,
                                            GeneralPassword = settings.FirstOrDefault(x => x.Key == "GeneralPassword").ValueString,
                                        };

                                        responseMessage.Data = examPackage;

                                        responseMessage.Message = "Successfully Completed";
                                        responseMessage.StatusCode = 200;
                                        return responseMessage;
                                    }
                                }
                            }

                            responseMessage.Message = "No Exam Scheduled";
                            responseMessage.StatusCode = 201;
                            return responseMessage;
                        }
                        else
                        {
                            responseMessage.Message = "Incorrect login parameters!";
                            responseMessage.StatusCode = 201;
                            return responseMessage;
                        }
                    }
                    else
                    {
                        responseMessage.Message = "Settings Not Found";
                        responseMessage.StatusCode = 201;
                        return responseMessage;
                    }
                }
                else
                {
                    responseMessage.Message = "Invalid Details!";
                    responseMessage.StatusCode = 1018;
                    return responseMessage;
                }
            }
            catch (Exception)
            {
                responseMessage.Message = "Something went wrong";
                responseMessage.StatusCode = 206;
                return responseMessage;
            }            
        }
    }
}
