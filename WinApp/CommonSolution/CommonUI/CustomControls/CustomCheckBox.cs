/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom checkbox
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            Adjust spaces for multi screen
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public class CustomCheckBox : CheckBox
    {
        #region Members        
        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomCheckBox), new PropertyMetadata(true));

        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomCheckBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomCheckBox), new PropertyMetadata(Size.Small));

        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomCheckBox), new PropertyMetadata(false));

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties
        public bool ErrorTextVisible
        {
            get
            {
                return (bool)GetValue(ErrorTextVisibleDependencyProperty);
            }
            set
            {
                SetValue(ErrorTextVisibleDependencyProperty, value);
            }
        }

        public string Heading
        {
            get { return (string)GetValue(HeadingDependencyProperty); }
            set { SetValue(HeadingDependencyProperty, value); }
        }

        public Size Size
        {
            get { return (Size)GetValue(SizeDependencyProperty); }
            set { SetValue(SizeDependencyProperty, value); }
        }

        public bool IsMandatory
        {
            get { return (bool)GetValue(IsMandatoryDependencyProperty); }
            set { SetValue(IsMandatoryDependencyProperty, value); }
        }
        #endregion

        #region Constuctor
        static CustomCheckBox()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomCheckBox), new FrameworkPropertyMetadata(typeof(CustomCheckBox)));
            log.LogMethodExit();
        }
        #endregion
    }
}
