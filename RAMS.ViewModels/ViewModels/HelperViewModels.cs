using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    /// <summary>
    /// ConfirmationViewModel view model declares properties for _Confirmation, _Success, and _Error partial views
    /// </summary>
    public class ConfirmationViewModel
    {
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Refresh Edit Form?")]
        public bool RefreshEditForm { get; set; }

        [Display(Name = "Refresh List?")]
        public bool RefreshList { get; set; }

        [Display(Name = "Clear Messages?")]
        public bool ClearMessages { get; set; }

        /// <summary>
        /// Default ConfirmationViewModel constructor sets all the properties default values
        /// </summary>
        public ConfirmationViewModel()
        {
            this.Message = "";

            this.RefreshEditForm = false;

            this.RefreshList = false;

            this.ClearMessages = false;
        }

        /// <summary>
        /// UserConfirmationViewModel constructor which sets the Message property
        /// </summary>
        /// <param name="message">Setter for message property</param>
        public ConfirmationViewModel(string message)
        {
            this.Message = message;

            this.RefreshEditForm = false;

            this.RefreshList = false;

            this.ClearMessages = false;
        }

        /// <summary>
        /// ConfirmationViewModel constructor which sets Message, RefreshEditForm, and RefreshList properties
        /// </summary>
        /// <param name="message">Setter for Message property</param>
        /// <param name="refreshEditForm">Setter for RefreshEditForm property</param>
        /// <param name="refreshList">Setter for RefreshList property</param>
        public ConfirmationViewModel(string message, bool refreshEditForm, bool refreshList)
        {
            this.Message = message;

            this.RefreshEditForm = refreshEditForm;

            this.RefreshList = refreshList;
        }
    }


}
