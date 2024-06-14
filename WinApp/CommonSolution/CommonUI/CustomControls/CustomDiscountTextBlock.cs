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
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public class CustomDiscountTextBlock : Control
    {
        #region Members
        public static readonly DependencyProperty ContentDependencyProperty = DependencyProperty.Register("Content", typeof(string), typeof(CustomDiscountTextBlock), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DiscountContentDependencyProperty = DependencyProperty.Register("DiscountContent", typeof(string), typeof(CustomDiscountTextBlock), new PropertyMetadata(string.Empty));
        #endregion

        #region Properties
        public string DiscountContent
        {
            get
            {
                return (string)GetValue(DiscountContentDependencyProperty);
            }
            set
            {
                SetValue(DiscountContentDependencyProperty, value);
            }
        }

        public string Content
        {
            get
            {
                return (string)GetValue(ContentDependencyProperty);
            }
            set
            {
                SetValue(ContentDependencyProperty, value);
            }
        }
        #endregion
    }
}
