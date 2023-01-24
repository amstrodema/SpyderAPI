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
   public class QuestionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OptionBusiness _optionBusiness;
        private readonly OptionImageBusiness _optionImageBusiness;
        private readonly QuestionImageBusiness _questionImageBusiness;

        public QuestionBusiness(IUnitOfWork unitOfWork, OptionBusiness optionBusiness, OptionImageBusiness optionImageBusiness, QuestionImageBusiness questionImageBusiness)
        {
            _unitOfWork = unitOfWork;
            _optionBusiness = optionBusiness;
            _optionImageBusiness = optionImageBusiness;
            _questionImageBusiness = questionImageBusiness;
        }

        public async Task<List<Question>> GetQuestions() =>
          await _unitOfWork.Questions.GetAll();

        public async Task<Question> GetQuestionByID(Guid id) =>
                  await _unitOfWork.Questions.Find(id);

        public async Task Create(Question Question)
        {
            await _unitOfWork.Questions.Create(Question);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<Question>> Update(QuestionVM questionVM)
        {


            IEnumerable<Option> optionList = await _optionBusiness.GetOptionsByQuestionID(questionVM.ID);
            IEnumerable<QuestionImage> questionImageList = await _questionImageBusiness.GetQuestionImagesByQuestionID(questionVM.ID);

            //remove items not in POST

            int count = 0;
            foreach (var optn in optionList)
            {
                if (questionVM.Options.FirstOrDefault(e=> e.ID == optn.ID) == default)
                {
                    await _optionBusiness.Delete(optn.ID);
                }
                count++;
            }

            count = 0;
            foreach (var img in questionImageList)
            {
                if (questionVM.QuestionImages.FirstOrDefault(e => e.ID == img.ID) == default)
                {
                    await _questionImageBusiness.Delete(img.ID);
                }
                count++;
            }


           // IEnumerable<OptionImage> optionImageList = new List<OptionImage>();

            List<Option> options = new List<Option>();
            List<QuestionImage> questionImages = new List<QuestionImage>();
            List<OptionImage> optionImages = new List<OptionImage>();

            List<Option> optionsEdit = new List<Option>();
            List<QuestionImage> questionImagesEdit = new List<QuestionImage>();
            List<OptionImage> optionImagesEdit = new List<OptionImage>();

            ResponseMessage<Question> response = new ResponseMessage<Question>();

            Question question;
            QuestionImage questionImage;
            Option option = new Option();
            OptionImage optionImage;


            question = await GetQuestionByID(questionVM.ID);
            question.IsActive = questionVM.IsActive;
            question.IsTheory = questionVM.IsTheory;
            question.QuestionText = questionVM.QuestionText;
            question.DateModified = DateTime.Now;
            question.ModifiedBy = questionVM.ModifiedBy;

            foreach (var questionImageVM in questionVM.QuestionImages)
            {
                if (questionImageList.FirstOrDefault(e => e.ID == questionImageVM.ID) == default)
                {
                    questionImage = new QuestionImage()
                    {
                        Image = questionImageVM.Image,
                        ID = Guid.NewGuid(),
                        IsActive = questionVM.IsActive,
                        NodeID = questionVM.NodeID,
                        CreatedBy = questionVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        QuestionID = questionVM.ID
                    };
                    questionImages.Add(questionImage);
                }
                else
                {
                    QuestionImage questionImageOld = await _questionImageBusiness.GetQuestionImageByID(questionImageVM.ID);
                    questionImageOld.Image = questionImageVM.Image;
                    questionImageOld.IsActive = questionImageVM.IsActive;
                    questionImageOld.DateModified = question.DateModified;
                    questionImageOld.ModifiedBy = question.ModifiedBy;

                    questionImagesEdit.Add(questionImageOld);
                }
               
            }

            foreach (var optionVM in questionVM.Options)
                {
                if (optionList.FirstOrDefault(e => e.ID == optionVM.ID) == default)
                {
                    option = new Option()
                    {
                        ID = Guid.NewGuid(),
                        IsActive = questionVM.IsActive,
                        NodeID = question.NodeID,
                        CreatedBy = questionVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        QuestionID = question.ID,
                        OptionText = optionVM.OptionText,
                        IsAnswer = optionVM.IsAnswer
                    };
                    options.Add(option);
                }
                else
                {
                    option = await _optionBusiness.GetOptionByID(optionVM.ID);
                    option.OptionText = optionVM.OptionText;
                    option.ModifiedBy = question.ModifiedBy;
                    option.DateModified = DateTime.Now;
                    option.IsAnswer = optionVM.IsAnswer;
                    option.IsActive = optionVM.IsActive;

                    optionsEdit.Add(option);
                }

                var optionImageList = await _optionImageBusiness.GetOptionImageByOptionID(optionVM.ID);

                count = 0;
                foreach (var img in optionImageList)
                {
                    if (optionVM.OptionImages.FirstOrDefault(e => e.ID == img.ID) == default)
                    {
                        await _optionImageBusiness.Delete(img.ID);
                    }
                    count++;
                }

                foreach (var optionImageVM in optionVM.OptionImages)
                    {

                    if (optionImageList.FirstOrDefault(e => e.ID == optionImageVM.ID) == default)
                    {
                        optionImage = new OptionImage()
                        {
                            ID = Guid.NewGuid(),
                            IsActive = question.IsActive,
                            NodeID = question.NodeID,
                            CreatedBy = questionVM.CreatedBy,
                            DateCreated = DateTime.Now,
                            OptionID = option.ID,
                            Image = optionImageVM.Image
                        };
                        optionImages.Add(optionImage);
                    }
                    else
                    {
                        OptionImage optionImageOld = await _optionImageBusiness.GetOptionImageByID(optionImageVM.ID);
                        optionImageOld.Image = optionImageVM.Image;
                        optionImageOld.DateModified = DateTime.Now;
                        optionImageOld.ModifiedBy = question.ModifiedBy;

                        optionImagesEdit.Add(optionImageOld);
                    }
                     
                    }
                }
            

             _unitOfWork.Questions.Update(question);

            await _unitOfWork.QuestionImages.CreateMultiple(questionImages.ToArray());
            _unitOfWork.QuestionImages.UpdateMultiple(questionImagesEdit.ToArray());

            await _unitOfWork.Options.CreateMultiple(options.ToArray());
            _unitOfWork.Options.UpdateMultiple(optionsEdit.ToArray());

            await _unitOfWork.OptionImages.CreateMultiple(optionImages.ToArray());
            _unitOfWork.OptionImages.UpdateMultiple(optionImagesEdit.ToArray());

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
            var entity = await GetQuestionByID(id);
            _unitOfWork.Questions.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamID(Guid examID) =>
                  await _unitOfWork.Questions.GetQuestionsByExamID(examID);

        public async Task<IEnumerable<Question>> GetQuestionsByNodeID(Guid nodeID) =>
                  await _unitOfWork.Questions.GetQuestionsByNodeID(nodeID);

        public async Task<IEnumerable<QuestionVM>> GetQuestionVMs(Guid nodeID)
        {
            var examQuestionsImages = await _questionImageBusiness.GetQuestionImageObjectsByNodeID(nodeID);
            var examOptions = await _optionBusiness.GetOptionVMs(nodeID);

            return from question in await GetQuestionsByNodeID(nodeID)
                      select new QuestionVM()
                      {
                          ID = question.ID,
                          IsTheory = question.IsTheory,
                          QuestionText = question.QuestionText,
                          QuestionImages = (from image in examQuestionsImages where image.QuestionID == question.ID
                                           select image).ToArray(),
                          Options = (from option in examOptions where option.QuestionID == question.ID
                                     select option).ToArray(),
                          ExamID = question.ExamID,
                          CreatedBy = question.CreatedBy,
                          DateCreated = question.DateCreated.ToString(),
                          DateModified = question.DateModified.ToString(),
                          IsActive = question.IsActive,
                          ModifiedBy = question.ModifiedBy,
                          NodeID = question.NodeID
                      };
        }


        public async Task<ResponseMessage<Question>> CreateMultiple(QuestionVM[] questionVMs)
        {
            List<Question> questions = new List<Question>();
            List<Option> options = new List<Option>();
            List<QuestionImage> questionImages = new List<QuestionImage>();
            List<OptionImage> optionImages = new List<OptionImage>();

            ResponseMessage<Question> response = new ResponseMessage<Question>();
            Question question;
            QuestionImage questionImage;
            Option option;
            OptionImage optionImage;

            foreach (var questionVM in questionVMs)
            {
                question = new Question()
                {
                    CreatedBy = questionVM.CreatedBy,
                    DateCreated = DateTime.Now,
                    ExamID = questionVM.ExamID,
                    ID = Guid.NewGuid(),
                    IsActive = questionVM.IsActive,
                    IsTheory = questionVM.IsTheory,
                    NodeID = questionVM.NodeID,
                    QuestionText = questionVM.QuestionText
                };
                questions.Add(question);

                foreach (var questionImageVM in questionVM.QuestionImages)
                {
                    questionImage = new QuestionImage()
                    {
                        Image = questionImageVM.Image,
                        ID = Guid.NewGuid(),
                        IsActive = questionVM.IsActive,
                        NodeID = questionVM.NodeID,
                        CreatedBy = questionVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        QuestionID = question.ID
                    };
                    questionImages.Add(questionImage);
                }

                foreach (var optionVM in questionVM.Options)
                {
                    option = new Option()
                    {
                        ID = Guid.NewGuid(),
                        IsActive = questionVM.IsActive,
                        NodeID = questionVM.NodeID,
                        CreatedBy = questionVM.CreatedBy,
                        DateCreated = DateTime.Now,
                        QuestionID = question.ID,
                        OptionText = optionVM.OptionText,
                        IsAnswer = optionVM.IsAnswer
                    };
                    options.Add(option);

                    foreach (var optionImageVM in optionVM.OptionImages)
                    {
                        optionImage = new OptionImage()
                        {
                            ID = Guid.NewGuid(),
                            IsActive = questionVM.IsActive,
                            NodeID = questionVM.NodeID,
                            CreatedBy = questionVM.CreatedBy,
                            DateCreated = DateTime.Now,
                            OptionID = option.ID,
                            Image = optionImageVM.Image
                        };
                        optionImages.Add(optionImage);
                    }
                }
            }

            await _unitOfWork.Questions.CreateMultiple(questions.ToArray());
            await _unitOfWork.QuestionImages.CreateMultiple(questionImages.ToArray());
            await _unitOfWork.Options.CreateMultiple(options.ToArray());
            await _unitOfWork.OptionImages.CreateMultiple(optionImages.ToArray());

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
