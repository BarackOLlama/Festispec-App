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

        public InspectorVM SelectedInspector { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

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
            SaveChangesCommand = new RelayCommand(SaveChanges);
            Inspector = SelectedInspector;
        }

        private void AddInspector()
        {
            _context.InspectorCrud.GetAllInspectors().Add(Inspector);
        }


        private void SaveChanges()
        {
            if (InspectorIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.InspectorCrud.GetAllInspectors().Add(Inspector);
                    _context.SaveChanges();

                    Messenger.Default.Send(true, "UpdateInspectorList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
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
            if (result == MessageBoxResult.Cancel)
            {
                _context.Dispose();
                Inspector = null;
                window?.Close();
            }
        }


        public bool InspectorIsValid()
        {

            if (Inspector.Address.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een adres hebben.");
                return false;
            }

            if (Inspector.BankNumber.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een banknummer hebben.");
                return false;
            }

            if (Inspector.CertificationDate <= new DateTime(1990, 1, 1))
            {
                MessageBox.Show("Een inspecteur moet een certificatiedatum hebben.");
                return false;
            }

            if (Inspector.InvalidDate<= Inspector.CertificationDate)
            {
                MessageBox.Show("De certificering van een inspecteur kan alleen invalide worden na de originele certificeringsdatum.");
                return false;
            }

            if (Inspector.City.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet in een stad wonen.");
                return false;
            }

            if (Inspector.Email.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een e-mail adres hebben.");
                return false;
            }

            if (new EmailAddressAttribute().IsValid(Inspector.Email))
            {
                MessageBox.Show("De ingevoerde e-mail adres is incorrect.");
                return false;
            }

            if (Inspector.PhoneNumber == null)
            {
                MessageBox.Show("Een inspecteur moet een telefoonnummer hebben.");
                return false;
            }

            if (!Regex.Match(Inspector.PhoneNumber, @"^(\+[0-9]{9})$").Success)
            {
                MessageBox.Show("De ingevoerde telefoonnummer is incorrect.");
                return false;
            }

            if (Inspector.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een naam hebben.");
                return false;
            }

            if (Inspector.Zipcode.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspecteur moet een postcode hebben.");
                return false;
            }

            if (!Regex.Match(Inspector.Zipcode, "^[0-9]{5}(?:-[0-9]{4})?$").Success)
            {
                MessageBox.Show("De ingevoerde zipcode is incorrect.");
                return false;
            }

            return true;
        }


    }
}
