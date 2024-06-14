/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - EnterImageForm 
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
    /// Form used to enter an Html Image attribute
    /// Consists of Href, Text and Image Alignment
    /// </summary>
    internal partial class EnterImageForm : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Public form constructor
        /// </summary>
        public EnterImageForm()
        {
            //
            // Required for Windows Form Designer support
            //
            log.LogMethodEntry();
            InitializeComponent();

            // define the text for the alignment
            this.listAlign.Items.AddRange(Enum.GetNames(typeof(ImageAlignOption)));

            // ensure default value set for target
            this.listAlign.SelectedIndex = 4;
            log.LogMethodExit();
        } //EnterHrefForm


        /// <summary>
        /// Property for the text to display
        /// </summary>
        public string ImageText
        {
            get
            {
                return this.hrefText.Text;
            }
            set
            {
                this.hrefText.Text = value;
            }

        } //ImageText

        /// <summary>
        /// Property for the href for the image
        /// </summary>
        public string ImageLink
        {
            get
            {
                return this.hrefLink.Text.Trim();
            }
            set
            {
                this.hrefLink.Text = value.Trim();
            }

        } //ImageLink

        /// <summary>
        /// Property for the image align
        /// </summary>
        public ImageAlignOption ImageAlign
        {
            get
            {
                return (ImageAlignOption)this.listAlign.SelectedIndex;
            }
            set
            {
                this.listAlign.SelectedIndex = (int)value;
            }
        }

    }
}
