/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - custom radio button
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     07-July-2021  Raja                   Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public class CustomRadioButton : RadioButton
    {
        #region Members
        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomRadioButton), new PropertyMetadata(Size.Small));

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public Size Size
        {
            get
            {
                return (Size)GetValue(SizeDependencyProperty);
            }
            set
            {
                SetValue(SizeDependencyProperty, value);
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        public CustomRadioButton()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
            #endregion
        }
}
