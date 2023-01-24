using MainAPI.Business;
using MainAPI.Business.Spyder;
using MainAPI.Business.Spyder.Feature;
using MainAPI.Data.Interface;
using MainAPI.Data.Interface.Spyder;
using MainAPI.Data.Interface.Spyder.Feature;
using MainAPI.Data.Repository;
using MainAPI.Data.Repository.Spyder;
using MainAPI.Data.Repository.Spyder.Feature;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public static class DependencyInjections
    {
        public static void Register (IServiceCollection services)
        {
            services

                .AddTransient<EncryptionService>()
                .AddTransient<JWTService>()

                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient(typeof(IGeneric<>), typeof(GenericRepository<>))

                .AddTransient<IUser, UserRepository>().AddTransient<UserBusiness>()
                .AddTransient<IWallet, WalletRepository>().AddTransient<WalletBusiness>()
                .AddTransient<ICountry, CountryRepository>().AddTransient<CountryBusiness>()
                .AddTransient<IPetition, PetitionRepository>().AddTransient<PetitionBusiness>()
                .AddTransient<INotification, NotificationRepository>().AddTransient<NotificationBusiness>()
                .AddTransient<ITransaction, TransactionRepository>().AddTransient<TransactionBusiness>()
                .AddTransient<IHall, HallRepository>().AddTransient<HallBusiness>()
                .AddTransient<IComment, CommentRepository>().AddTransient<CommentBusiness>()
                .AddTransient<IVote, VoteRepository>().AddTransient<VoteBusiness>()
                .AddTransient<ILike, LikeRepository>().AddTransient<LikeBusiness>()
                .AddTransient<IFlagReport, FlagReportRepository>().AddTransient<FlagReportBusiness>()

                .AddTransient<IDeath, DeathRepository>().AddTransient<DeathBusiness>()
                .AddTransient<IFeature, FeatureRepository>().AddTransient<FeatureBusiness>()
                .AddTransient<IMarriage, MarriageRepository>().AddTransient<MarriageBusiness>()
                .AddTransient<IFeatureGroup, FeatureGroupRepository>().AddTransient<FeatureGroupBusiness>()
                .AddTransient<IFeatureType, FeatureTypeRepository>().AddTransient<FeatureTypeBusiness>()
                .AddTransient<IImage, ImageRepository>().AddTransient<ImageBusiness>()
                .AddTransient<ILink, LinkRepository>().AddTransient<LinkBusiness>()
                .AddTransient<IItemType, ItemTypeRepository>().AddTransient<ItemTypeBusiness>()
                .AddTransient<IMissing, MissingRepository>().AddTransient<MissingBusiness>()
                .AddTransient<IConfession, ConfessionRepository>().AddTransient<ConfessionBusiness>()

                .AddTransient<IInbox, InboxRepository>().AddTransient<InboxBusiness>()
                .AddTransient<ISystemLog, SystemLogsRepository>().AddTransient<SystemLogsBusiness>()
                .AddTransient<IMajorFunctionLog, MajorFunctionLogRepository>().AddTransient<MajorFunctionLogBusiness>()
                .AddTransient<ILogInMonitor, LogInMonitorRepository>().AddTransient<LogInMonitorBusiness>()

                .AddTransient<IPayment, PaymentRepository>().AddTransient<PaymentBusiness>()
                .AddTransient<ISos, SosRepository>().AddTransient<SosBusiness>()
                .AddTransient<ISosCluster, SosClusterRepository>().AddTransient<SosClusterBusiness>()
                .AddTransient<ILocationCenter, LocationCenterRepository>().AddTransient<LocationCenterBusiness>()
                .AddTransient<IWithdrawal, WithdrawalRepository>().AddTransient<WithdrawalBusiness>()
                .AddTransient<IArea, AreaRepository>().AddTransient<AreaBusiness>()

                .AddTransient<IStatusEnum, StatusEnumRepository>().AddTransient<StatusEnumBusiness>()
                .AddTransient<IParams, ParamsRepository>().AddTransient<ParamsBusiness>()
                .AddTransient<ISettings, SettingsRepository>().AddTransient<SettingsBusiness>()
                //.AddTransient<IWebHostEnvironment>()
                .AddTransient<GenericBusiness>();
        }
    }
}
