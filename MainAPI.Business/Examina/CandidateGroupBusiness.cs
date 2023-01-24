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
   public class CandidateGroupBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public CandidateGroupBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CandidateGroup>> GetCandidateGroups() =>
          await _unitOfWork.CandidateGroups.GetAll();

        public async Task<CandidateGroup> GetCandidateGroupByID(Guid id) =>
                  await _unitOfWork.CandidateGroups.Find(id);
        public async Task<IEnumerable<CandidateGroup>> GetCandidateGroupsByNodeID(Guid nodeID) =>
                  await _unitOfWork.CandidateGroups.GetCandidateGroupsByNodeID(nodeID);

        public async Task Create(CandidateGroup CandidateGroup)
        {
            await _unitOfWork.CandidateGroups.Create(CandidateGroup);
            await _unitOfWork.Commit();
        }
        public async Task<ResponseMessage<string>> AttachCandidatesToGroup(CandidateGroupVM candidateGroupVM)
        {
            ResponseMessage<string> response = new ResponseMessage<string>();

            if (await GroupIsEngaged(candidateGroupVM.GroupID, candidateGroupVM.NodeID))
            {
                response.StatusCode = 201;
                response.Message = "Exclude engaged group(s)";

                return response;
            }

            var oldGroupCandidates = await GetCandidateGroupsByGroupID(candidateGroupVM.GroupID);

            var candidateGroupHolder = new List<CandidateGroup>();

            foreach (var candidateID in candidateGroupVM.CandidateIDs)
            {
                var holder = oldGroupCandidates.Data.FirstOrDefault(d => d.CandidateID == candidateID);

                if (holder == default)
                {
                    CandidateGroup candidateGroup = new CandidateGroup()
                    {
                        GroupID = candidateGroupVM.GroupID,
                        CandidateID = candidateID,
                        CreatedBy = candidateGroupVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        ID = Guid.NewGuid(),
                        NodeID = candidateGroupVM.NodeID
                    };

                    candidateGroupHolder.Add(candidateGroup);
                }
            }

            foreach (var item in oldGroupCandidates.Data)
            {
                if (candidateGroupVM.CandidateIDs.FirstOrDefault(x => x == item.CandidateID) == default)
                {
                     _unitOfWork.CandidateGroups.Delete(item);
                }
            }

            await _unitOfWork.CandidateGroups.CreateMultiple(candidateGroupHolder.ToArray());

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
        public async Task<ResponseMessage<string>> AttachGroupsToCandidate(CandidateGroupVM candidateGroupVM)
        {
            ResponseMessage<string> response = new ResponseMessage<string>();
            if (await GroupsAreEngaged(candidateGroupVM.GroupIDs, candidateGroupVM.NodeID))
            {
                response.StatusCode = 201;
                response.Message = "Exclude engaged group(s)";

                return response;
            }
            var oldCandidateGroups = await GetCandidateGroupsByCandidateID(candidateGroupVM.CandidateID);

            var candidateGroupHolder = new List<CandidateGroup>();

            foreach (var grpID in candidateGroupVM.GroupIDs)
            {
                var holder = oldCandidateGroups.Data.FirstOrDefault(d => d.GroupID == grpID);

                if (holder == default)
                {
                    CandidateGroup candidateGroup = new CandidateGroup()
                    {
                        GroupID = grpID,
                        CandidateID = candidateGroupVM.CandidateID,
                        CreatedBy = candidateGroupVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        ID = Guid.NewGuid(),
                        NodeID = candidateGroupVM.NodeID
                    };

                    candidateGroupHolder.Add(candidateGroup);
                }
            }

            foreach (var item in oldCandidateGroups.Data)
            {
                if (candidateGroupVM.GroupIDs.FirstOrDefault(x => x == item.GroupID) == default)
                {
                    _unitOfWork.CandidateGroups.Delete(item);
                }
            }

            await _unitOfWork.CandidateGroups.CreateMultiple(candidateGroupHolder.ToArray());

           
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

        public async Task<ResponseMessage<string>> Update(CandidateGroupVM candidateGroupVM)
        {
            foreach (var grpID in candidateGroupVM.GroupIDs)
            {
                CandidateGroup candidateGroup;

                foreach (var candidateID in candidateGroupVM.CandidateIDs)
                {
                    candidateGroup = new CandidateGroup()
                    {
                        GroupID = grpID,
                        CandidateID = candidateID,
                        CreatedBy = candidateGroupVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        ID = Guid.NewGuid(),
                        NodeID = candidateGroupVM.NodeID
                    };

                    await _unitOfWork.CandidateGroups.Create(candidateGroup);
                }
            }

            // _unitOfWork.CandidateGroups.UpdateMultiple(candidateGroup);

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
            var entity = await GetCandidateGroupByID(id);
            _unitOfWork.CandidateGroups.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<IEnumerable<CandidateGroup>>> GetCandidateGroupsByCandidateID(Guid candidateID)
        {

            ResponseMessage<IEnumerable<CandidateGroup>> res = new ResponseMessage<IEnumerable<CandidateGroup>>();

            try
            {

                res.Data = await _unitOfWork.CandidateGroups.GetCandidateGroupsByCandidateID(candidateID);

                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

        public async Task<ResponseMessage<IEnumerable<CandidateGroup>>> GetCandidateGroupsByGroupID(Guid groupID)
        {

            ResponseMessage<IEnumerable<CandidateGroup>> res = new ResponseMessage<IEnumerable<CandidateGroup>>();

            try
            {

                res.Data = await _unitOfWork.CandidateGroups.GetCandidateGroupsByGroupID(groupID);

                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

   
        public async Task<bool> GroupIsEngaged(Guid groupID, Guid nodeID)
        {
            var x = from scheduleGrp in await _unitOfWork.ScheduleGrps.GetScheduleGroupsByNodeID(nodeID) where scheduleGrp.GroupID == groupID
                    join activeSchedule in await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID) on scheduleGrp.ScheduleID equals activeSchedule.ID
                    select activeSchedule;

            return x.Count() > 0 ? true : false;
        }


        public async Task<bool> GroupsAreEngaged(Guid[] groupIDs, Guid nodeID)
        {
            var x = from scheduleGrp in await _unitOfWork.ScheduleGrps.GetScheduleGroupsByNodeID(nodeID)
                    join grpID in groupIDs on scheduleGrp.GroupID equals grpID
                    join activeSchedule in await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID) on scheduleGrp.ScheduleID equals activeSchedule.ID
                    select activeSchedule;

            return x.Count() > 0 ? true : false;
        }
    }
}
