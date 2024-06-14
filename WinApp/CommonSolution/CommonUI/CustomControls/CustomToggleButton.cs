/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - Custom discount textblock
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda           Created for POS UI Redesign 
********************************************************************************************/

using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace Semnox.Parafait.CommonUI
{
    public class CustomToggleButton : ToggleButton
    {
        #region Members
        public static readonly DependencyProperty ItemsSourceDependencyProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<DisplayTag>), typeof(CustomToggleButton), new PropertyMetadata(new ObservableCollection<DisplayTag>()));
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public ObservableCollection<DisplayTag> ItemsSource
        {
            get
            {
                return (ObservableCollection<DisplayTag>)GetValue(ItemsSourceDependencyProperty);
            }
            set
            {
                SetValue(ItemsSourceDependencyProperty, value);
            }
        }
        #endregion

        #region Constructors
        static CustomToggleButton()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomToggleButton), new FrameworkPropertyMetadata(typeof(CustomToggleButton)));
            log.LogMethodExit();
        }
        #endregion
    }
}
