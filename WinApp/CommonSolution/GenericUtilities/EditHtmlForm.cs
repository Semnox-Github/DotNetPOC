/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - EditHtmlForm UI 
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

using System.Windows.Forms;

#endregion

namespace Semnox.Core.GenericUtilities
{

    /// <summary>
    /// Form used to Edit or View Html contents
    /// If a property RedOnly is true contents are considered viewable
    /// No Html parsing is performed on the resultant data
    /// </summary>
    internal partial class EditHtmlForm : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // read only property for the form
        private bool _readOnly;

        // string values for the form title
        private const string editCommand = "Cancel";
        private const string viewCommand = "Close";

        /// <summary>
        /// Public Form constructor
        /// </summary>
        public EditHtmlForm()
        {
            //
            // Required for Windows Form Designer support
            //
            log.LogMethodEntry();
            InitializeComponent();

            // ensure content is empty
            this.htmlText.Text = string.Empty;
            this.ReadOnly = true;
            log.LogMethodExit();

        } //EditHtmlForm

        /// <summary>
        /// Property to modify the caption of the display
        /// </summary>
        public void SetCaption(string caption)
        {
            log.LogMethodEntry(caption);
            this.Text = caption;
            log.LogMethodExit();
        }

        /// <summary>
        /// Property to set and get the HTML contents
        /// </summary>
        public string HTML
        {
            get
            {
                return this.htmlText.Text.Trim();
            }
            set
            {
                this.htmlText.Text = (value != null)?value.Trim():string.Empty;
                this.htmlText.SelectionStart = 0;
                this.htmlText.SelectionLength = 0;
            }

        } //HTML

        /// <summary>
        /// Property that determines if the html is editable
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                this.bOK.Visible = !_readOnly;
                this.htmlText.ReadOnly = _readOnly;
                this.bCancel.Text = _readOnly?viewCommand:editCommand;
            }

        } //ReadOnly

    }
}
