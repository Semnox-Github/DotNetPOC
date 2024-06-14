/********************************************************************************************
 * Project Name - POSUI
 * Description  - value object class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class PanelMargin
    {
        private double left;
        private double right;
        private double top;
        private double bottom;

        public PanelMargin(double leftMargin, double rightMargin, double topMargin, double bottomMargin)
        {
            this.left = leftMargin;
            this.right = rightMargin;
            this.top = topMargin;
            this.bottom = bottomMargin;
        }

        public PanelMargin(double value)
        {
            this.left = value;
            this.right = value;
            this.top = value;
            this.bottom = value;
        }

        public PanelMargin(double horizontal, double vertical)
        {
            this.left = horizontal;
            this.right = horizontal;
            this.top = vertical;
            this.bottom = vertical;
        }

        public double Left
        {
            get
            {
                return left;
            }
        }

        public double Right
        {
            get
            {
                return right;
            }
        }

        public double Top
        {
            get
            {
                return top;
            }
        }
        public double Bottom
        {
            get
            {
                return bottom;
            }
        }
    }
}
