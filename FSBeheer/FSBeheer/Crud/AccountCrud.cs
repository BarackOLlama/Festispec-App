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

        public ObservableCollection<AccountVM> GetAllAccounts()
        {
            var accounts = CustomFSContext.Accounts
                .ToList()
                .Select(i => new AccountVM(i));
            return new ObservableCollection<AccountVM>(accounts);
        }

        public ObservableCollection<AccountVM> GetAllAccountsFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var accounts = CustomFSContext.Accounts
                .ToList()
                .Where(a =>
                a.Id.ToString().ToLower().Contains(must_contain) ||
                a.Username.ToLower().Contains(must_contain) ||
                a.Role.Content.ToLower().Contains(must_contain)
                ).Distinct()
                .Select(a => new AccountVM(a));
            return new ObservableCollection<AccountVM>(accounts);
        }
    }
}
