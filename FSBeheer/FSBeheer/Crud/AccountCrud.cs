using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class AccountCrud : AbstractCrud
    {

        public AccountCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public ObservableCollection<AccountVM> GetAccounts()
        {
            var Account = CustomFSContext.Accounts
                .ToList()
                .Select(i => new AccountVM(i));
            var _Accounts = new ObservableCollection<AccountVM>(Account);
            return _Accounts;
        }
    }
}
