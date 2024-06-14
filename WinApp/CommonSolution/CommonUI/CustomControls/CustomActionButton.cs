/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom action button
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
    public enum ActionStyle
    {
        Active = 0,
        Passive = 1,
        Light = 2
    }

    public class CustomActionButton : Button
    {
        #region Members
        private static readonly DependencyProperty HideBackgroundDependencyProperty = DependencyProperty.Register("HideBackground", typeof(bool), typeof(CustomActionButton), new PropertyMetadata(false));
        public static readonly DependencyProperty ActionStyleDependencyProperty = DependencyProperty.Register("ActionStyle", typeof(ActionStyle), typeof(CustomActionButton), new PropertyMetadata(ActionStyle.Active));
        public static readonly DependencyProperty TextSizeDependencyProperty = DependencyProperty.Register("TextSize", typeof(TextSize), typeof(CustomActionButton), new PropertyMetadata(TextSize.Small));
        public static readonly DependencyProperty TextTrimmingDependencyProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(CustomActionButton), new PropertyMetadata(TextTrimming.None));
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public TextSize TextSize
        {
            get { return (TextSize)GetValue(TextSizeDependencyProperty); }
            set { SetValue(TextSizeDependencyProperty, value); }
        }
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingDependencyProperty); }
            set { SetValue(TextTrimmingDependencyProperty, value); }
        }
        public bool HideBackground
        {
            get { return (bool)GetValue(HideBackgroundDependencyProperty); }
            set { SetValue(HideBackgroundDependencyProperty, value); }
        }

        public ActionStyle ActionStyle
        {
            get { return (ActionStyle)GetValue(ActionStyleDependencyProperty); }
            set { SetValue(ActionStyleDependencyProperty, value); }
        }
        #endregion

        #region Constuctors
        static CustomActionButton()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomActionButton), new FrameworkPropertyMetadata(typeof(CustomActionButton)));
            log.LogMethodExit();
        }
        #endregion
    }
}
