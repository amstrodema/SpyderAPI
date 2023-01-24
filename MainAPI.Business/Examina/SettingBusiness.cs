using MainAPI.Data.Interface;
using MainAPI.Models.Examina;
using MainAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Business.Examina
{
   public class SettingBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Setting>> GetSettings() =>
          await _unitOfWork.Settings.GetAll();

        public async Task<Setting> GetSettingByID(Guid id) =>
                  await _unitOfWork.Settings.Find(id);

        public async Task Create(Setting setting)
        {
            if (setting.Key == "GeneralPassword")
            {
               setting.ValueString = EncryptionService.Encrypt(setting.ValueString);
            }

            setting.IsActive = true;
            setting.DateCreated = DateTime.Now;

            await _unitOfWork.Settings.Create(setting);
            await _unitOfWork.Commit();
        }

        public async Task Update(Setting Setting)
        {
            _unitOfWork.Settings.Update(Setting);
            await _unitOfWork.Commit();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetSettingByID(id);
            _unitOfWork.Settings.Delete(entity);
            await _unitOfWork.Commit();
        }

        public async Task<Setting> GetSettingByKey(string key) =>
                  await _unitOfWork.Settings.GetSettingByKey(key);

        public async Task<IEnumerable<Setting>> GetSettingsByNodeID(Guid nodeID) =>
                  await _unitOfWork.Settings.GetSettingsByNodeID(nodeID);
    }
}
