using FSBeheer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _message;
        public AccountVM Account { get; set; }
        private HomeViewModel _homeViewModel;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                base.RaisePropertyChanged();
            }
        }

        public RelayCommand VerifyLoginCommand { get; set; }

        public LoginViewModel(HomeViewModel homeViewModel)
        {
            _homeViewModel = homeViewModel;
            //this.CreateNewAccount();
            VerifyLoginCommand = new RelayCommand(VerifyLogin);
            Message = "";
            Account = new AccountVM();
        }

        public void VerifyLogin()
        {
            string username;
            string password;
            string salt;

            using (var context = new CustomFSContext())
            {
                username = Account.Username;
                password = Account.Password;
                //check whether we can make a connection to the db
                try
                {
                    context.Accounts.First();
                }
                catch (Exception e)
                {
                    Message = "Could not connect to database.";
                    return;
                }

                //check whether there's there's any such username in the database.
                var findSalt = context.Accounts.Where(e => e.Username == username).FirstOrDefault();
                var all = context.Accounts.ToList().Select(e => new AccountVM(e));
                salt = findSalt?.Salt;
                //short circuti
                if (salt == null)
                {
                    Message = "No such username/password combination found.";
                    return;
                }
                //get the salted version of the user input with the salt from the account in the database
                string saltedPW = BCrypt.Net.BCrypt.HashPassword(password, salt);

                //check whether there's any account in the database with that username/password combination
                var findUser = context.Accounts.ToList().Where(e => e.Username == username && e.Password == saltedPW).FirstOrDefault();

                if (findUser == null)
                {
                    Message = "No such username/password combination found.";
                }
                else
                {
                    Message = "Login succesful!";
                    //save the fact that the user has logged in in a variable.
                    //Loginbox should automatically close.
                    _homeViewModel.Account = new AccountVM() { Username = username };
                }

            }
        }

        public void CreateNewAccount()
        {
            //create any username/password account combination you want

            string username = "username";
            string password = "password";
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string saltedPW = BCrypt.Net.BCrypt.HashPassword(password, salt);

            using (var context = new CustomFSContext())
            {
                //You do not ever save the plaintext (var password) in the database, only the salted
                context.Accounts.Add(new Account() { Id = 0, Username = username, RoleId = 1, Password = saltedPW, Salt = salt, IsAdmin = true });
                context.SaveChanges();
            }

        }
    }
}
