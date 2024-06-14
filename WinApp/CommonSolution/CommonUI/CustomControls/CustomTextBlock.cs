/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom textblock
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public enum TextSize
    {
        Large = 0,
        Medium = 1,
        Small = 2,
        XSmall = 3
    }

    public class CustomTextBlock : TextBlock
    {
        #region Members
        public static readonly DependencyProperty TextSizeDependencyProperty = DependencyProperty.Register("TextSize", typeof(TextSize), typeof(CustomTextBlock), new PropertyMetadata(TextSize.Small));
        #endregion

        #region Properties
        public TextSize TextSize
        {
            get
            {
                return (TextSize)GetValue(TextSizeDependencyProperty);
            }
            set
            {
                SetValue(TextSizeDependencyProperty, value);
            }
        }
        #endregion

        #region Constructors
        static CustomTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTextBlock), new FrameworkPropertyMetadata(typeof(CustomTextBlock)));
        }
        #endregion
    }
}
