/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - EnterHrefForm UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace Semnox.Core.GenericUtilities
{

    /// <summary>
    /// Form used to enter an Html Anchor attribute
    /// Consists of Href, Text and Target Frame
    /// </summary>
    public partial class EnterHrefForm : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Public form constructor
        /// </summary>
        public EnterHrefForm()
        {
            //
            // Required for Windows Form Designer support
            //
            log.LogMethodEntry();
            InitializeComponent();

            // define the text for the targets
            this.listTargets.Items.AddRange(Enum.GetNames(typeof(NavigateActionOption)));

            // ensure default value set for target
            this.listTargets.SelectedIndex = 0;
            log.LogMethodExit();

        } //EnterHrefForm


        /// <summary>
        /// Property for the text to display
        /// </summary>
        public string HrefText
        {
            get
            {
                return this.hrefText.Text;
            }
            set
            {
                this.hrefText.Text = value;
            }

        } //HrefText

        /// <summary>
        /// Property for the href target
        /// </summary>
        public NavigateActionOption HrefTarget
        {
            get
            {
                return (NavigateActionOption)this.listTargets.SelectedIndex;
            }
        }

        /// <summary>
        /// Property for the href for the text
        /// </summary>
        public string HrefLink
        {
            get
            {
                return this.hrefLink.Text.Trim();
            }
            set
            {
                this.hrefLink.Text = value.Trim();
            }

        } //HrefLink

    }
}
