using MainAPI.Data.Interface.Spyder;
using MainAPI.Data.Interface.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Interface
{
    public interface IUnitOfWork
    {
        Task<int> Commit();
        void Rollback();

        IUser Users { get; }
        IStatusEnum StatusEnums { get; }
        IWallet Wallets { get; }
        ICountry Countries { get; }
        IPetition Petitions { get; }
        INotification Notifications { get; }
        ITransaction Transactions { get; }
        IHall Halls { get; }
        IComment Comments { get; }
        IVote Votes { get; }

        ILike Likes { get; }
        IFlagReport FlagReports { get; }

         IDeath Deaths { get; }
         IFeature Features { get; }
         IMarriage Marriages { get; }
         IFeatureGroup FeatureGroups { get; }
         IFeatureType FeatureTypes { get; }
         IImage Images { get; }
         ILink Links { get; }
         IItemType ItemTypes { get; }
         IMissing Missings { get; }
         IConfession Confessions { get; }

         IMajorFunctionLog MajorFunctionLogs { get; }
         IInbox Inboxes { get; }
         ILogInMonitor LogInMonitors { get; }
         ISystemLog SystemLogs { get; }

        public IPayment Payments { get; }
        public ISos Sos { get; }
        public ISosCluster SosClusters { get; }
        public ILocationCenter LocationCenters { get; }
        public IWithdrawal Withdrawals { get; }
        public IArea Areas { get; }
        public IParams Params { get; }

        public ISettings Settings { get; }
    }
}
