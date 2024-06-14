/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom scroll viewer
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
    public enum Position
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3,
        Middle = 4
    }

    public class CustomScrollViewer : ScrollViewer
    {
        #region Members
        public static readonly DependencyProperty IsMaxVeriticalWidthReachedDependencyProperty = DependencyProperty.Register("IsMaxVeriticalWidthReached", typeof(bool), typeof(CustomScrollViewer), new PropertyMetadata(false));

        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomScrollViewer), new PropertyMetadata(Size.Medium));

        public static readonly DependencyProperty StyleDependencyProperty = DependencyProperty.Register("ScrollBarStyle", typeof(ScrollBarStyle), typeof(CustomScrollViewer), new PropertyMetadata(ScrollBarStyle.Light));

        public static readonly DependencyProperty PositionDependencyProperty = DependencyProperty.Register("Position", typeof(Position), typeof(CustomScrollViewer));
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties   
        public bool IsMaxVeriticalWidthReached
        {
            get { return (bool)GetValue(IsMaxVeriticalWidthReachedDependencyProperty); }
            private set { SetValue(IsMaxVeriticalWidthReachedDependencyProperty, value); }
        }

        public Size Size
        {
            get { return (Size)GetValue(SizeDependencyProperty); }
            set { SetValue(SizeDependencyProperty, value); }
        }

        public ScrollBarStyle ScrollBarStyle
        {
            get { return (ScrollBarStyle)GetValue(StyleDependencyProperty); }
            set { SetValue(StyleDependencyProperty, value); }
        }

        public Position Position
        {
            get { return (Position)GetValue(PositionDependencyProperty); }
            set { SetValue(PositionDependencyProperty, value); }
        }

        #endregion

        #region Methods
        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            log.LogMethodEntry();
            if (this.ScrollableWidth == 0)
            {
                if (this.VerticalOffset == this.ScrollableHeight)
                    Position = Position.Bottom;
                else
                {
                    if (this.VerticalOffset == 0)
                        Position = Position.Top;
                    else
                        Position = Position.Middle;
                }
            }
            else
            {
                if (this.HorizontalOffset == 0)
                    Position = Position.Left;
                else
                {
                    if (this.HorizontalOffset == this.ScrollableWidth)
                        Position = Position.Right;
                    else
                        Position = Position.Middle;
                }
                if (this.VerticalOffset == this.ScrollableHeight)
                    IsMaxVeriticalWidthReached = true;
                else
                    IsMaxVeriticalWidthReached = false;
            }

            base.OnScrollChanged(e);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        static CustomScrollViewer()
        {
            log.LogMethodEntry();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomScrollViewer), new
           FrameworkPropertyMetadata(typeof(CustomScrollViewer)));
            log.LogMethodExit();
        }
        #endregion
    }
}
