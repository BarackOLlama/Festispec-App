using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FSBeheer.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        //private string _message;
        public AccountVM Account { get; set; }

        public RelayCommand<Window> VerifyLoginCommand { get; set; }

        public LoginViewModel()
        {
            //this.CreateNewAccount();
            VerifyLoginCommand = new RelayCommand<Window>(VerifyLogin);
            Account = new AccountVM();
            Account.Username = "sjakie@festispec.com";
            Account.Password = "password";
        }

        public void VerifyLogin(Window window)
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
                catch (Exception)
                {
                    MessageBox.Show("Kan momenteel niet verbinden met de inlogservice.");
                    return;
                }

                //check whether there's there's any such username in the database.
                var findSalt = context.Accounts.Where(e => e.Username == username).FirstOrDefault();
                salt = findSalt?.Salt;
                //short circuti
                if (salt == null)
                {
                    MessageBox.Show("Gebruikersnaam/wachtwoord combinatie onbekend.");
                    return;
                }
                //get the salted version of the user input with the salt from the account in the database
                string saltedPW = BCrypt.Net.BCrypt.HashPassword(password, salt);

                //check whether there's any account in the database with that username/password combination
                var findUser = context.Accounts.ToList().Where(e => e.Username == username && e.Password == saltedPW).FirstOrDefault();

                if (findUser == null)
                {
                    MessageBox.Show("Gebruikersnaam/wachtwoord combinatie onbekend.");
                }
                else
                {
                    if (findUser.IsDeleted)
                    {
                        MessageBox.Show("De geselecteerde account is niet toegankelijk.\nAls dit een fout is, neem contact op met de beheerder.");
                        return;
                    }

                    if (!findUser.IsAdmin)
                    {
                        MessageBox.Show("Alleen administrateur-accounts kunnen inloggen op deze applicatie.");
                        return;
                    }

                    //if ()
                    //{
                    //    MessageBox.Show("De account waar u op probeert in te loggen is verlopen.");
                    //    return;
                    //}

                    //save the fact that the user has logged in in a variable.
                    //Loginbox should automatically close.
                    new HomeView().Show();
                    window.Close();
                }

            }
        }

        public void CreateNewAccount(string username, string password)
        {
            //move to an AccountCreationViewModel
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