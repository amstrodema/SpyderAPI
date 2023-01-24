using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using MainAPI.Models.ViewModel.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class OptionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OptionImageBusiness _optionImageBusiness;

        public OptionBusiness(IUnitOfWork unitOfWork, OptionImageBusiness optionImageBusiness)
        {
            _unitOfWork = unitOfWork;
            _optionImageBusiness = optionImageBusiness;
        }

        public async Task<List<Option>> GetOptions() =>
          await _unitOfWork.Options.GetAll();

        public async Task<Option> GetOptionByID(Guid id) =>
                  await _unitOfWork.Options.Find(id);

        public async Task<IEnumerable<Option>> GetOptionsByNodeID(Guid nodeID) =>
                  await _unitOfWork.Options.GetOptionsByNodeID(nodeID);

        public async Task<IEnumerable<Option>> GetOptionsByQuestionID(Guid questionID) =>
                  await _unitOfWork.Options.GetOptionsByQuestionID(questionID);

        public async Task<IEnumerable<OptionVM>> GetOptionVMs(Guid nodeID)
        {
            var nodeExamOptionsImages = await _optionImageBusiness.GetOptionImageObjectsByNodeID(nodeID);

            return from option in await GetOptionsByNodeID(nodeID)
                   select new OptionVM()
                   {
                       ID = option.ID,
                      OptionText = option.OptionText,
                      OptionImages = (from optn in nodeExamOptionsImages where optn.OptionID == option.ID
                                      select optn).ToArray(),
                      QuestionID = option.QuestionID,
                      IsAnswer = option.IsAnswer,
                      CreatedBy = option.CreatedBy,
                      DateCreated = option.DateCreated,
                      DateModified = option.DateModified,
                      IsActive = option.IsActive,
                      ModifiedBy = option.ModifiedBy,
                      NodeID = option.NodeID
                   };
        }

        public async Task Create(Option Option)
        {
            await _unitOfWork.Options.Create(Option);
            await _unitOfWork.Commit();
        }

        public async Task Update(Option Option)
        {
            _unitOfWork.Options.Update(Option);
            await _unitOfWork.Commit();
        }
        public async Task Update(Option[] Option)
        {
            _unitOfWork.Options.UpdateMultiple(Option);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetOptionByID(id);
            _unitOfWork.Options.Delete(entity);
            await _unitOfWork.Commit();
        }
    }
}
