using MainAPI.Data.Interface;
using MainAPI.Models;
using MainAPI.Models.Examina;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
    public class GroupBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private ResponseMessage<Group> response = new ResponseMessage<Group>();
        ResponseMessage<IEnumerable<Group>> res = new ResponseMessage<IEnumerable<Group>>();
        public GroupBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Group>> GetGroups() =>
          await _unitOfWork.Groups.GetAll();

        public async Task<Group> GetGroupByID(Guid id) =>
                  await _unitOfWork.Groups.Find(id);

        public async Task Create(Group Group)
        {
            await _unitOfWork.Groups.Create(Group);
            await _unitOfWork.Commit();
        }
        public async Task<ResponseMessage<Group>> CreateMultiple(Group[] groups)
        {
            response = new ResponseMessage<Group>();
            var grps = await GetGroups();

                string code = "";

            for (int i = 0; i < groups.Length; i++)
            {
                do
                {
                    code = GenService.Gen10DigitCode();
                } while (grps.Find(e=> e.Code == code) != null);

                groups[i].Code = code;
                groups[i].DateCreated = DateTime.Now;
            }

            await _unitOfWork.Groups.CreateMultiple(groups);
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

        public async Task<ResponseMessage<Group>> Update(Group group)
        {
            group.DateModified = DateTime.Now;
            _unitOfWork.Groups.Update(group);
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
            var entity = await GetGroupByID(id);
            _unitOfWork.Groups.Delete(entity);
            await _unitOfWork.Commit();
        }
        public async Task<ResponseMessage<IEnumerable<Group>>> GetGroupsByNodeID(Guid nodeID)
        {
            res = new ResponseMessage<IEnumerable<Group>>();

            var active = from schdlGrp in await _unitOfWork.ScheduleGrps.GetScheduleGroupsByNodeID(nodeID)
                         join activ in await _unitOfWork.Schedules.GetActiveSchedulesByNodeID(nodeID) on schdlGrp.ScheduleID equals activ.ID
                        select schdlGrp;

            try
            {
                res.Data = from grp in await _unitOfWork.Groups.GetGroupsByNodeID(nodeID)
                           select new Group()
                           {
                               NodeID = grp.NodeID,
                               Name = grp.Name,
                               ModifiedBy = grp.ModifiedBy,
                               IsActive = grp.IsActive,
                               Code = grp.Code,
                               CreatedBy = grp.CreatedBy,
                               DateCreated = grp.DateCreated,
                               DateModified = grp.DateModified,
                               Description = grp.Description,
                               ID = grp.ID,
                               Flag = active.FirstOrDefault(c => c.GroupID == grp.ID) == default ? 0 : 1
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
    }
}
