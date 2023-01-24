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
   public class ScheduleGroupBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleGroupBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ScheduleGroup>> GetScheduleGrps() =>
          await _unitOfWork.ScheduleGrps.GetAll();

        public async Task<ScheduleGroup> GetScheduleGroupByID(Guid id) =>
                  await _unitOfWork.ScheduleGrps.Find(id);

        public async Task Create(ScheduleGroup ScheduleGroup)
        {
            await _unitOfWork.ScheduleGrps.Create(ScheduleGroup);
            await _unitOfWork.Commit();
        }

        public async Task Update(ScheduleGroup ScheduleGroup)
        {
            _unitOfWork.ScheduleGrps.Update(ScheduleGroup);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetScheduleGroupByID(id);
            _unitOfWork.ScheduleGrps.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ScheduleGroup> GetScheduleGroupByScheduleID(Guid scheduleID) =>
                  await _unitOfWork.ScheduleGrps.GetScheduleGroupByScheduleID(scheduleID);

        public async Task<IEnumerable<ScheduleGroup>> GetSchedulesGroupByScheduleID(Guid scheduleID) =>
                  await _unitOfWork.ScheduleGrps.GetSchedulesGroupByScheduleID(scheduleID);
        public async Task<IEnumerable<ScheduleGroup>> GetScheduleGroupsByNodeID(Guid nodeID) =>
                  await _unitOfWork.ScheduleGrps.GetScheduleGroupsByNodeID(nodeID);

        public async Task<ResponseMessage<IEnumerable<ScheduleGroup>>> _GetSchedulesGroupByScheduleID(Guid scheduleID)
        {
            ResponseMessage<IEnumerable<ScheduleGroup>> res = new ResponseMessage<IEnumerable<ScheduleGroup>>();

            try
            {
                res.Data = await GetSchedulesGroupByScheduleID(scheduleID);
                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

        public async Task<ResponseMessage<string>> AttachGroupsToSchedule(ScheduleGroupsVM scheduleGroupsVM)
        {
            var oldScheduleGroups = await GetSchedulesGroupByScheduleID(scheduleGroupsVM.scheduleID);

            var scheduleGroupHolder = new List<ScheduleGroup>();

            foreach (var groupID in scheduleGroupsVM.GroupIDs)
            {
                var holder = oldScheduleGroups.FirstOrDefault(d => d.GroupID == groupID);

                if (holder == default)
                {
                    ScheduleGroup scheduleGroup = new ScheduleGroup()
                    {
                        ScheduleID = scheduleGroupsVM.scheduleID,
                        GroupID = groupID,
                        CreatedBy = scheduleGroupsVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        ID = Guid.NewGuid(),
                        NodeID = scheduleGroupsVM.NodeID
                    };

                    scheduleGroupHolder.Add(scheduleGroup);
                }
            }

            foreach (var item in oldScheduleGroups)
            {
                if (scheduleGroupsVM.GroupIDs.FirstOrDefault(x => x == item.GroupID) == default)
                {
                    _unitOfWork.ScheduleGrps.Delete(item);
                }
            }

            await _unitOfWork.ScheduleGrps.CreateMultiple(scheduleGroupHolder.ToArray());

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
    }
}
