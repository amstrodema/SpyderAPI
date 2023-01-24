using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class QuestionImageBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionImageBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<QuestionImage>> GetQuestionImages() =>
          await _unitOfWork.QuestionImages.GetAll();

        public async Task<QuestionImage> GetQuestionImageByID(Guid id) =>
                  await _unitOfWork.QuestionImages.Find(id);
        public async Task<IEnumerable<QuestionImage>> GetQuestionImagesByQuestionID(Guid questionID) =>
                  await _unitOfWork.QuestionImages.GetQuestionImagesByQuestionID(questionID);
        public async Task<IEnumerable<QuestionImage>> GetQuestionImageObjectsByNodeID(Guid nodeID) =>
                  await _unitOfWork.QuestionImages.GetQuestionImageObjectsByNodeID(nodeID);

        public async Task Create(QuestionImage QuestionImage)
        {
            await _unitOfWork.QuestionImages.Create(QuestionImage);
            await _unitOfWork.Commit();
        }

        public async Task Update(QuestionImage QuestionImage)
        {
            _unitOfWork.QuestionImages.Update(QuestionImage);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetQuestionImageByID(id);
            _unitOfWork.QuestionImages.Delete(entity);
            await _unitOfWork.Commit();
        }
    }
}
