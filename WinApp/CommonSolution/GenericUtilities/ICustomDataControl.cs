//using Semnox.Core.GenericUtilities.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// interface for custom data control
    /// </summary>
    public interface ICustomDataControl
    {
        /// <summary>
        /// assigns custom data dto to the control
        /// </summary>
        /// <param name="customDataDTO"></param>
        void SetCustomDataDTO(CustomDataDTO customDataDTO);

        /// <summary>
        /// returns the custom attribute id of the controls
        /// </summary>
        int CustomAttributeId { get; }

        /// <summary>
        /// updates the current values to the custom data dto
        /// </summary>
        void Save();

        /// <summary>
        /// Display error state
        /// </summary>
        void ShowErrorState();

        /// <summary>
        /// Clear error state
        /// </summary>
        void ClearErrorState();

        /// <summary>
        /// show and hides the control
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Changes whether the ui is enabled
        /// </summary>
        /// <param name="value"></param>
        void SetControlsEnabled(bool value);
        /// <summary>
        /// Sets the font size
        /// </summary>
        /// <param name="font"></param>
        void SetFont(Font font);
    }
}
