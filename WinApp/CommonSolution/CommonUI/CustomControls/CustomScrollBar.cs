/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom scroll bar
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Semnox.Parafait.CommonUI
{
    public enum ScrollBarStyle
    {
        Light = 0,
        Dark = 1
    }
    public class CustomScrollBar : ScrollBar
    {

        #region Members
        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomScrollBar), new PropertyMetadata(Size.Medium));

        public static readonly DependencyProperty StyleDependencyProperty = DependencyProperty.Register("ScrollBarStyle", typeof(ScrollBarStyle), typeof(CustomScrollBar), new PropertyMetadata(ScrollBarStyle.Light));

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties   

        public Size Size
        {
            get { return (Size)GetValue(SizeDependencyProperty); }
            set { SetValue(SizeDependencyProperty, value); }
        }

        public ScrollBarStyle ScrollbarStyle
        {
            get { return (ScrollBarStyle)GetValue(StyleDependencyProperty); }
            set { SetValue(StyleDependencyProperty, value); }
        }

        #endregion

        #region Constructor
        static CustomScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomScrollBar), new
           FrameworkPropertyMetadata(typeof(CustomScrollBar)));
        }
        #endregion

    }
}
