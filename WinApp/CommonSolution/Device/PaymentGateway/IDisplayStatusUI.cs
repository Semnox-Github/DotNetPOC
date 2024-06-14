using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Interface to handle the ui
    /// </summary>
    public interface IDisplayStatusUI: IDisposable
    {
        /// <summary>
        /// Cancel clicked
        /// </summary>
         event EventHandler CancelClicked;
         event EventHandler CheckNowClicked;
        /// <summary>
        /// To Show the window
        /// </summary>
        void ShowStatusWindow();
        /// <summary>
        /// Close the status window
        /// </summary>
        void CloseStatusWindow();
        /// <summary>
        /// Enable and disable the cancel button based on the parameter
        /// </summary>
        /// <param name="isEnable">Takes true or false value </param>
        void EnableCancelButton(bool isEnable);
        /// <summary>
        /// EnableCheckNowButton button based on the parameter
        /// </summary>
        /// <param name="isEnable">Takes true or false value </param>
        void EnableCheckNowButton(bool isEnable);
        /// <summary>
        /// Displaying the text
        /// </summary>
        /// <param name="text">String value  </param>
        void DisplayText(string text);
    }
}
