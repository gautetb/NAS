using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS.Model
{
    /// <summary>
    /// This class is the view model for the admins to be displayed on the admin site as well as retreive data from forms.
    /// </summary>
    public class AdminViewModel
    {
        /// <summary>
        /// Store for the Name property, which contains the full name of the admin for the view
        /// </summary>
        [Required(ErrorMessage = "Fornavn må oppgis")]
        [RegularExpression(@"[a-zA-ZøæåØÆÅ .\- ]{2,30}", ErrorMessage = "Vennligst skriv inn fullt navn kun med tegn fra alfabetet (inkludert æøå).")]
        public string Name { get; set; }

        /// <summary>
        /// Store for the Username property
        /// </summary>
        [Required(ErrorMessage = "Epost må oppgis")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Ugyldig epost")]
        public string Username { get; set; }

        /// <summary>
        /// Store for the ConfirmUsername property, used for form validation when logging inn and registering admins
        /// </summary>
        [Compare("Username", ErrorMessage = "Epost-bekreftelse matcher ikke")]
        public string ConfirmUsername { get; set; }

        /// <summary>
        /// Store for the Password property
        /// </summary>
        [Required(ErrorMessage = "Passord må oppgis")]
        public string Password { get; set; }

        /// <summary>
        /// Store for the ConfirmPassword property, used for form validation when logging inn and registering admins
        /// </summary>
        [Compare("Password", ErrorMessage = "Passordbekreftelse matcher ikke")]
        public string ConfirmPassword { get; set; }
    }
}
