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
    public class ScheduleBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GroupBusiness _groupBusiness;
        private readonly ScheduleGroupBusiness _scheduleGroupBusiness;
        private readonly ExaminationBusiness _examinationBusiness;

        public ScheduleBusiness(IUnitOfWork unitOfWork, GroupBusiness  groupBusiness, ScheduleGroupBusiness scheduleGroupBusiness, ExaminationBusiness examinationBusiness)
        {
            _unitOfWork = unitOfWork;
            _groupBusiness = groupBusiness;
            _scheduleGroupBusiness = scheduleGroupBusiness;
            _examinationBusiness = examinationBusiness;
        }

        public async Task<List<Schedule>> GetSchedules() =>
          await _unitOfWork.Schedules.GetAll();

        public async Task<Schedule> GetScheduleByID(Guid id) =>
                  await _unitOfWork.Schedules.Find(id);

        public async Task Create(ScheduleVM scheduleVM)
        {
            Schedule schedule = new Schedule()
            {
                CreatedBy = scheduleVM.CreatedBy,
                NodeID = scheduleVM.NodeID,
                IsActive = scheduleVM.IsActive,
                ExamID = scheduleVM.ExamID,
                ID = Guid.NewGuid(),
                ScheduledDate = scheduleVM.ScheduledDate,
                ScheduledStartTime = scheduleVM.ScheduledStartTime,
                DateCreated = DateTime.Now
            };
            await _unitOfWork.Schedules.Create(schedule);
            await _unitOfWork.Commit();
        }
        public async Task<ResponseMessage<string>> CreateMultiple(ScheduleVM[] scheduleVMs)
        {
            List<Schedule> schedules = new List<Schedule>();

            foreach (var scheduleVM in scheduleVMs)
            {
                Schedule schedule = new Schedule()
                {
                    CreatedBy = scheduleVM.CreatedBy,
                    NodeID = scheduleVM.NodeID,
                    IsActive = scheduleVM.IsActive,
                    ExamID = scheduleVM.ExamID,
                    ID = Guid.NewGuid(),
                    ScheduledDate = scheduleVM.ScheduledDate,
                    ScheduledStartTime = scheduleVM.ScheduledStartTime,
                    DateCreated = DateTime.Now,
                    ScheduleName = scheduleVM.ScheduleName
                };

                schedules.Add(schedule);

            }

            await _unitOfWork.Schedules.CreateMultiple(schedules.ToArray());

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

        public async Task<ResponseMessage<string>> Update(ScheduleVM scheduleVM)
        {
            var thisSchedule = await GetScheduleByID(scheduleVM.ID);

            thisSchedule.ModifiedBy = scheduleVM.ModifiedBy;
            thisSchedule.DateModified = DateTime.Now;
            thisSchedule.ExamID = scheduleVM.ExamID;
            thisSchedule.ScheduledDate = scheduleVM.ScheduledDate;
            thisSchedule.ScheduledStartTime = scheduleVM.ScheduledStartTime;
            thisSchedule.IsActive = scheduleVM.IsActive;
            thisSchedule.ScheduleName = scheduleVM.ScheduleName;

          
            _unitOfWork.Schedules.Update(thisSchedule);

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
            var entity = await GetScheduleByID(id);
            _unitOfWork.Schedules.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<Schedule>> GetActiveSchedulesByNodeID(Guid nodeID) =>
                  await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID);

        public async Task<ResponseMessage<IEnumerable<ScheduleVM>>> GetActiveScheduleVMsByNodeID(Guid nodeID)
        {
            ResponseMessage<IEnumerable<ScheduleVM>> res = new ResponseMessage<IEnumerable<ScheduleVM>>();

            try
            {
                var schedules = await GetSchedulesByNodeID(nodeID);
                var grpPivots = await _scheduleGroupBusiness.GetScheduleGroupsByNodeID(nodeID);
                var grps = await _groupBusiness.GetGroupsByNodeID(nodeID);
                var exms = await _examinationBusiness.GetExamsByNodeID(nodeID);

                res.Data = from schedule in schedules where schedule.IsActive == true
                           select new ScheduleVM()
                           {
                            NodeID = schedule.NodeID,
                            CreatedBy = schedule.CreatedBy,
                            DateCreated = schedule.DateCreated,
                            DateModified = schedule.DateModified,
                            ExamID = schedule.ExamID,
                            ID = schedule.ID,
                            IsActive = schedule.IsActive,
                            ModifiedBy = schedule.ModifiedBy,
                            ScheduledDate = schedule.ScheduledDate,
                            ScheduledStartTime = schedule.ScheduledStartTime,
                            ScheduleName = schedule.ScheduleName,
                            ExamName = exms.Data.FirstOrDefault(x=> x.ID == schedule.ExamID).Name,
                            Groups = (from grpPV in grpPivots where schedule.ID == grpPV.ScheduleID
                                      join grp in grps.Data on grpPV.GroupID equals grp.ID
                                     select grp).ToArray(),
                            Status = "xxxxx"
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

        public async Task<ResponseMessage<IEnumerable<ScheduleVM>>> GetAllSchedulesByNodeID(Guid nodeID)
        {
            ResponseMessage<IEnumerable<ScheduleVM>> res = new ResponseMessage<IEnumerable<ScheduleVM>>();

            try
            {
                var schedules = await GetSchedulesByNodeID(nodeID);
                var grpPivots = await _scheduleGroupBusiness.GetScheduleGroupsByNodeID(nodeID);
                var grps = await _groupBusiness.GetGroupsByNodeID(nodeID);
                var exms = await _examinationBusiness.GetExamsByNodeID(nodeID);

                res.Data = from schedule in schedules
                           select new ScheduleVM()
                           {
                               NodeID = schedule.NodeID,
                               CreatedBy = schedule.CreatedBy,
                               DateCreated = schedule.DateCreated,
                               DateModified = schedule.DateModified,
                               ExamID = schedule.ExamID,
                               ID = schedule.ID,
                               IsActive = schedule.IsActive,
                               ModifiedBy = schedule.ModifiedBy,
                               ScheduledDate = schedule.ScheduledDate,
                               ScheduledStartTime = schedule.ScheduledStartTime,
                               ScheduleName = schedule.ScheduleName,
                               ExamName = exms.Data.FirstOrDefault(x => x.ID == schedule.ExamID).Name,
                               Groups = (from grpPV in grpPivots
                                         where schedule.ID == grpPV.ScheduleID
                                         join grp in grps.Data on grpPV.GroupID equals grp.ID
                                         select grp).ToArray(),
                               Status = "xxxxx"
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

        public async Task<ResponseMessage<IEnumerable<ScheduleVM>>> ToggleSchedule(Guid scheduleID)
        {
            ResponseMessage<IEnumerable<ScheduleVM>> res = new ResponseMessage<IEnumerable<ScheduleVM>>();

            try
            {
                var schedule = await GetScheduleByID(scheduleID);
                var exam = await _examinationBusiness.GetExamByID(schedule.ExamID);

                if (!schedule.IsActive && exam.IsDraft)
                {
                    res.StatusCode = 209;
                    res.Message = "Scheduled exam is a draft";
                }
                else if (!schedule.IsActive)
                {
                    var allActiveSchedule = await GetActiveSchedulesByNodeID(exam.NodeID);
                    var allGroupsInThisSchedule = await _unitOfWork.ScheduleGrps.GetSchedulesGroupByScheduleID(schedule.ID);

                    var conflictGroups = from scheduleGroup in await _scheduleGroupBusiness.GetScheduleGroupsByNodeID(exam.NodeID)
                                                     join presentInGroup in allGroupsInThisSchedule on scheduleGroup.GroupID equals presentInGroup.GroupID
                                                     join activeSchedule in allActiveSchedule on scheduleGroup.ScheduleID equals activeSchedule.ID
                                                     select activeSchedule;


                    if (conflictGroups.Count() > 0)
                    {
                        res.StatusCode = 209;
                        res.Message = "Resolve group conflict";
                    }
                    else
                    {
                        var allCandidates = await _unitOfWork.CandidateGroups.GetCandidateGroupsByNodeID(exam.NodeID);

                        var allCandidatesInThisSchedule = from grp in allGroupsInThisSchedule
                                                          join candidate in allCandidates on grp.GroupID equals candidate.GroupID
                                                          select candidate;

                        var conflictCandidates = from scheduleGroup in await _scheduleGroupBusiness.GetScheduleGroupsByNodeID(exam.NodeID)
                                                            join candidate in allCandidates on scheduleGroup.GroupID equals candidate.GroupID
                                                            join activeSchedule in allActiveSchedule on scheduleGroup.ScheduleID equals activeSchedule.ID
                                                            join candidateInCurrent in allCandidatesInThisSchedule on candidate.CandidateID equals candidateInCurrent.CandidateID
                                                            select candidate;
                        if (conflictCandidates.Count() > 0)
                        {
                            res.StatusCode = 209;
                            res.Message = "Resolve candidate conflict";
                        }
                        else
                        {
                            schedule.IsActive = !schedule.IsActive;
                            res.StatusCode = 200;
                            res.Message = "Successful!";
                        }       
                    }

                }
                else
                {
                    schedule.IsActive = !schedule.IsActive;
                    res.StatusCode = 200;
                    res.Message = "Successful!";
                }

                _unitOfWork.Schedules.Update(schedule);

                if (await _unitOfWork.Commit() > 0)
                {
                    var hol= await GetAllSchedulesByNodeID(schedule.NodeID);
                    res.Data = hol.Data;
                }
                else
                {
                    res.StatusCode = 201;
                    res.Message = "Something went wrong!";
                }

                
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }

            return res;
        }
        public async Task<IEnumerable<Schedule>> GetSchedulesByNodeID(Guid nodeID) =>
                await _unitOfWork.Schedules.GetSchedulesByNodeID(nodeID);
    }
}
