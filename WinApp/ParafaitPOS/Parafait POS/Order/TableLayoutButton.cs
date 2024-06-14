using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    public class TableLayoutButton : System.Windows.Controls.Grid
    {
        System.Windows.Shapes.Rectangle rectangle;
        System.Windows.Controls.TextBlock label;
        string tableId;
        List<object> checkInTag;
        public TableLayoutButton(string value, System.Windows.Media.Brush brush)
        {
            rectangle = new System.Windows.Shapes.Rectangle();
            rectangle.Fill = brush;
            rectangle.Height = 80;
            rectangle.Width = 80;
            rectangle.RadiusX = 4;
            rectangle.RadiusY = 4;
            rectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            rectangle.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            rectangle.Margin = new System.Windows.Thickness(5);
            Children.Add(rectangle);
            label = new System.Windows.Controls.TextBlock();
            label.Text = value;
            label.TextAlignment = System.Windows.TextAlignment.Center;
            label.Foreground = System.Windows.Media.Brushes.White;
            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Children.Add(label);
        }

        public System.Windows.Media.Brush BackGround
        {
            set
            {
                rectangle.Fill = value;
            }
        }

        public string TableId
        {
            get
            {
                return tableId;
            }
            set
            {
                tableId = value;
            }
        }

        public string Text
        {
            get
            {
                return label.Text;
            }

            set
            {
                label.Text = value;
            }
        }

        public System.Windows.Media.Brush ForeGround
        {
            set
            {
                label.Foreground = value;
            }
        }

        public List<object> CheckInTag
        {
            get
            {
                return checkInTag;
            }

            set
            {
                checkInTag = value;
            }
        }
    }
}
