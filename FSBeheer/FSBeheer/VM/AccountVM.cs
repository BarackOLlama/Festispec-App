using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class AccountVM
    {
        private Account _account;

        public AccountVM(Account account)
        {
            _account = account;
        }

        public AccountVM()
        {
            _account = new Account();
        }

        public int Id
        {
            get { return _account.Id; }
        }

        public string Username
        {
            get { return _account.Username; }
            set { _account.Username = value; }
        }

        public string Password
        {
            get { return _account.Password; }
            set { _account.Password = value; }
        }

        public string Salt
        {
            get { return _account.Salt; }
        }

        public bool IsAdmin
        {
            get { return _account.IsAdmin; }
        }

        public int? RoleId
        {
            get { return _account.RoleId; }
        }

        public Role Role
        {
            get { return _account.Role; }
        }
    }
}
