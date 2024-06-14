/* Project Name - AutoCompleteComboBox
* Description  - Custom checkbox with auto complete option
* 
**************
**Version Log
**************
*Version     Date           Modified By         Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Semnox.Core.GenericUtilities
{
    public class AutoCompleteComboBox: ComboBox
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AutoCompleteComboBox()
        {
            log.LogMethodEntry();
            this.Validating += Combobox_Validating;
            AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AutoCompleteSource = AutoCompleteSource.ListItems;
            log.LogMethodExit();
        }

        private void Combobox_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ComboBox currentObject = (ComboBox)sender;
            if (currentObject != null && (currentObject.SelectedValue == null && currentObject.SelectedItem == null))
            {
                MessageBox.Show(MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 1837));
                //currentObject.Focus();
                e.Cancel = true;
            }
            log.LogMethodExit();
        }


    }
}
