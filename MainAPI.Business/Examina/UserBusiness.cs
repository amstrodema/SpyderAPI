using MainAPI.Data.Interface;
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
   public class UserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetUsers() =>
          await _unitOfWork.Users.GetAll();

        public async Task<User> GetUserByID(Guid id) =>
                  await _unitOfWork.Users.Find(id);

        public async Task<User> GetUserByStaffID(string staffID) =>
                  await _unitOfWork.Users.GetUserByStaffID(staffID);

        public async Task<IEnumerable<User>> GetUsersByNodeID(Guid nodeID) =>
                  await _unitOfWork.Users.GetUsersByNodeID(nodeID);

        public async Task<ResponseMessage<IEnumerable<User>>> GetInstructorsByNodeID(Guid nodeID)
        {
            var users = await _unitOfWork.Users.GetUsersByNodeID(nodeID);
             

            ResponseMessage<IEnumerable<User>> res = new ResponseMessage<IEnumerable<User>>();

            try
            {

                res.Data = from user in users
                           where user.Level == 2
                           select (user);

                res.StatusCode = 200;
            }
            catch (Exception)
            {
                res.StatusCode = 201;
                res.Message = "Something went wrong!";
            }
            return res;
        }

        public async Task Create(User user)
        {
            user.Password = EncryptionService.Encrypt(user.Password);

            await _unitOfWork.Users.Create(user);
            await _unitOfWork.Commit();
        }

        public async Task Update(User user)
        {
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetUserByID(id);
            _unitOfWork.Users.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<ResponseMessage<User>> ValidateUser(LoginVM login)
        {
            ResponseMessage<User> responseMessage = new ResponseMessage<User>();
            try
            {
                User user = await GetUserByStaffID(login.Username);
                if (EncryptionService.Validate(login.Password, user.Password))
                {
                    responseMessage.Data = user;
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                    responseMessage.Flag = 1;
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Incorrect login details!";
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 1018;
                responseMessage.Message = "Something went wrong!";
            }

            return responseMessage;
        }
    }
}
