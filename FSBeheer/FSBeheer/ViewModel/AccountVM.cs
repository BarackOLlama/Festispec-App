using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.Model;

namespace FSBeheer.ViewModel
{
    public class AccountVM
    {
        private Account e;

        public AccountVM(Account e)
        {
            this.e = e;
        }

        public AccountVM()
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
