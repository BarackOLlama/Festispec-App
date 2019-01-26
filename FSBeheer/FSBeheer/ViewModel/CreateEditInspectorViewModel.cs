using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectorViewModel : ViewModelBase
    {

        public InspectorVM Inspector { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        private CustomFSContext _context;

        public CreateEditInspectorViewModel()
        {
            _context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
        }

        private void AddInspector()
        {
            _context.InspectorCrud.GetAllInspectors().Add(Inspector);
        }


        private void SaveChanges(Window window)
        {
            if (!InspectorIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.InspectorCrud.GetAllInspectors().Add(Inspector);
                    _context.SaveChanges();
                    window.Close();
                    Messenger.Default.Send(true, "UpdateInspectorList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void SetInspector(InspectorVM inspector)
        {
            if (inspector == null)
            {
                Inspector = new InspectorVM();
                _context.Inspectors.Add(Inspector.ToModel());
            }
            else
            {
                Inspector = new InspectorVM(_context.Inspectors.FirstOrDefault(c => c.Id == inspector.Id));
            }
            RaisePropertyChanged(nameof(Inspector)); // a sign that a property has changed for viewing
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Sluiten zonder opslaan?", "Bevestiging annulering", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Inspector = null;
                window.Close();
            }
        }


        public bool InspectorIsValid()
        {

            if (Inspector.Address == null)
            {
                MessageBox.Show("Een inspecteur moet een adres hebben.");
                return false;
            }

            if (Inspector.Address.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een adres hebben.");
                return false;
            }

            if (Inspector.BankNumber == null)
            {
                MessageBox.Show("Een inspecteur moet een banknummer hebben.");
                return false;
            }

            if (Inspector.BankNumber.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een banknummer hebben.");
                return false;
            }

            if (!Regex.IsMatch(Inspector.BankNumber, @"^([A-Z]{2}[ \-]?[0-9]{2})(?=(?:[ \-]?[A-Z0-9]){9,30}$)((?:[ \-]?[A-Z0-9]{3,5}){2,7})([ \-]?[A-Z0-9]{1,3})?$"))
            {
                MessageBox.Show("De ingevoerde IBAN is onjuist.");
                return false;
            }

            if (Inspector.CertificationDate == null)
            {
                MessageBox.Show("Een inspecteur moet een certificatiedatum hebben.");
                return false;
            }

            if (Inspector.CertificationDate <= new DateTime(1990, 1, 1))
            {
                MessageBox.Show("Een inspecteur moet een certificatiedatum hebben.");
                return false;
            }

            if (Inspector.City == null)
            {
                MessageBox.Show("Een inspecteur moet in een stad wonen.");
                return false;
            }

            if (Inspector.City.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet in een stad wonen.");
                return false;
            }

            if (Inspector.Email == null)
            {
                MessageBox.Show("Een inspecteur moet een e-mail adres hebben.");
                return false;
            }

            if (Inspector.Email.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een e-mail adres hebben.");
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(Inspector.Email))
            {
                MessageBox.Show("Het ingevoerde e-mail adres is incorrect.");
                return false;
            }

            if (Inspector.InvalidDate == null)
            {
                MessageBox.Show("De certificering van de inspecteur moet eventueel verlopen.");
                return false;
            }

            if (Inspector.InvalidDate <= Inspector.CertificationDate)
            {
                MessageBox.Show("De certificering van een inspecteur kan alleen invalide worden na de originele certificeringsdatum.");
                return false;
            }

            if (Inspector.PhoneNumber == null)
            {
                MessageBox.Show("Een inspecteur moet een telefoonnummer hebben.");
                return false;
            }

            if (Regex.IsMatch(Inspector.PhoneNumber, @"^\+361"))//mobile number
            {
                if (!Regex.IsMatch(Inspector.PhoneNumber, @"^\+361 ?[0-9]{4} ?[0-9]{4}$"))
                {
                    MessageBox.Show("Het ingevoerde nummer voldoet niet aan de juiste opmaak.\n" +
                        "+361 1234 5678 heeft een correcte opbouw. Spaties zijn optioneel.");
                    return false;
                }
            }
            else if (!Regex.IsMatch(Inspector.PhoneNumber, @"^\d{3} ?\d{4} ?\d{3}$"))//not a mobile number
            {
                MessageBox.Show("Het ingevoerde telefoonnummer voldoet niet aan de juiste opbouw.\n" +
                    "Een correct telefoonnummer is bijvoorbeeld 072 5505 232. Spaties zijn optioneel.\n" +
                    "Als het telefoonnummer mobiel is begin dan met +316.");
                return false;
            }

            if (Inspector.Name == null)
            {
                MessageBox.Show("Een inspecteur moet een naam hebben.");
                return false;
            }

            if (Inspector.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een naam hebben.");
                return false;
            }

            if (Inspector.Zipcode == null)
            {
                MessageBox.Show("Een inspecteur moet een postcode hebben.");
                return false;
            }

            if (Inspector.Zipcode.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een postcode hebben.");
                return false;
            }

            if (!Regex.Match(Inspector.Zipcode, "^[0-9][0-9][0-9][0-9][A-Z][A-Z]").Success)
            {
                MessageBox.Show("De ingevoerde zipcode is incorrect." + Inspector.Zipcode);
                return false;
            }

            return true;
        }


    }
}
