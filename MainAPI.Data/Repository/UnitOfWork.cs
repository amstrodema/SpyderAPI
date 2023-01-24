using MainAPI.Data.Interface;
using MainAPI.Data.Interface.Spyder;
using MainAPI.Data.Interface.Spyder.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainAPIContext _db;


        public UnitOfWork(MainAPIContext db, IUser users,IStatusEnum statusEnums, IWallet wallets, ICountry countries, IPetition petitions, INotification notifications
            ,ITransaction transactions, IHall halls, IComment comments, IVote votes, ILike likes, IFlagReport flagReports, IDeath deaths, IFeature features, IMarriage marriages, IFeatureGroup featureGroups,
            IFeatureType featureTypes, IImage images, ILink links, IItemType itemTypes, IMissing missings, IConfession confessions, IMajorFunctionLog majorFunctionLogs, IInbox inboxes,
            ILogInMonitor logInMonitors, ISystemLog systemLogs, IPayment payments, ISos sos, ISosCluster sosClusters, ILocationCenter locationCenters, IParams param,
            IWithdrawal withdrawals, IArea areas, ISettings settings)
        {
            _db = db;
            Users = users;
            StatusEnums = statusEnums;
            Wallets = wallets;
            Countries = countries;
            Petitions = petitions;
            Notifications = notifications;
            Transactions = transactions;
            Halls = halls;
            Comments = comments;
            Votes = votes;
            Likes = likes;
            FlagReports = flagReports;
            Deaths = deaths;
            Features = features;
            Marriages = marriages;
            FeatureGroups = featureGroups;
            FeatureTypes = featureTypes;
            Images = images;
            Links = links;
            ItemTypes = itemTypes;
            Missings = missings;
            Confessions = confessions;
            MajorFunctionLogs = majorFunctionLogs;
            Inboxes = inboxes;
            LogInMonitors = logInMonitors;
            SystemLogs = systemLogs;
            Payments = payments;
            Sos = sos;
            SosClusters = sosClusters;
            LocationCenters = locationCenters;
            Params = param;
            Withdrawals = withdrawals;
            Areas = areas;
            Settings = settings;
        }

        public IUser Users { get; }
        public IStatusEnum StatusEnums { get; }
        public IWallet Wallets { get; }
        public ICountry Countries { get; }
        public IPetition Petitions { get; }
        public INotification Notifications { get; }
        public ITransaction Transactions { get; }
        public IHall Halls { get; }
        public IComment Comments { get; }
        public IVote Votes { get; }
        public ILike Likes { get; }
        public IFlagReport FlagReports { get; }
        public IDeath Deaths { get; }
        public IFeature Features { get; }
        public IMarriage Marriages { get; }
        public IFeatureGroup FeatureGroups { get; }
        public IFeatureType FeatureTypes { get; }
        public IImage Images { get; }
        public ILink Links { get; }
        public IItemType ItemTypes { get; }
        public IMissing Missings { get; }
        public IConfession Confessions { get; }
        public IMajorFunctionLog MajorFunctionLogs { get; }
        public IInbox Inboxes { get; }
        public ILogInMonitor LogInMonitors { get; }
        public ISystemLog SystemLogs { get; }
        public IPayment Payments { get; }
        public ISos Sos { get; }
        public ISosCluster SosClusters { get; }
        public ILocationCenter LocationCenters { get; }
        public IParams Params { get; }
        public IWithdrawal Withdrawals { get; }
        public IArea Areas { get; }
        public ISettings Settings { get; }

        public async Task<int> Commit() =>
            await _db.SaveChangesAsync();

        public void Rollback() => Dispose();

        public void Dispose() =>
            _db.DisposeAsync();
    }
}
