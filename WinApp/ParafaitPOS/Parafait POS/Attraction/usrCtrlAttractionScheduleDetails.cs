/********************************************************************************************
 * Project Name - Attraction
 * Description  - AttractionSchedule form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.2        11-Nov-2019      Nitin Pai      Club speed enhancement
 *2.110.0       05-Feb-2021      Nitin Pai      Converted to WPF Component
 *2.130         07-Jun-2021      Nitin Pai      Funstasia Fix: Do not show over capacity or -ive capacity in overlapping schedule scenario
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Parafait_POS.Attraction
{
    public partial class usrCtrlAttractionScheduleDetails : System.Windows.Controls.ContentControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScheduleDetailsDTO scheduleDetails;
        private ExecutionContext executionContext;
        private Utilities utilities;
        private System.Windows.Media.Color backColor;
        public System.Windows.Media.Color BackColor { get { return backColor; } }
        public System.Windows.Media.Color PrevColor { get; set; }
        private DateTime selectedDate;
        private int desiredUnits;
        private int screenWidth;
        private System.Windows.Input.MouseButtonEventHandler clickAction;
        private bool pastSchedule;
        internal delegate void SchduleLineClickedDelegate();
        internal SchduleLineClickedDelegate scheduleLine;
        double cumilativeWidth = 0;

        public usrCtrlAttractionScheduleDetails(ExecutionContext executionContext, Utilities utilities, ScheduleDetailsDTO scheduleDetails, int desiredUnits,
            System.Windows.Media.Color backColor, DateTime selectedDate, int width, bool pastSchedule, System.Windows.Input.MouseButtonEventHandler clickAction)
        {
            InitializeComponent();
            this.scheduleDetails = scheduleDetails;
            this.PrevColor = this.backColor = backColor;
            this.executionContext = executionContext;
            this.utilities = utilities;
            this.selectedDate = selectedDate;
            this.clickAction = clickAction;
            this.Width = this.screenWidth = width;
            this.desiredUnits = desiredUnits;
            this.pastSchedule = pastSchedule;
            LoadComponents();
        }

        private void LoadComponents()
        {
            lblFacilityName = new System.Windows.Controls.Label();
            lblScheduleName = new System.Windows.Controls.Label();
            lblScheduleTime = new System.Windows.Controls.Label();
            lblPrice = new System.Windows.Controls.Label();
            lblTotalUnits = new System.Windows.Controls.Label();
            lblBookedUnits = new System.Windows.Controls.Label();
            lblAvailableUnits = new System.Windows.Controls.Label();
            lblDesiredUnits = new System.Windows.Controls.Label();

            //// 
            //// lblFacilityName
            //// 
            this.lblFacilityName.Background = System.Windows.Media.Brushes.White;
            //this.lblFacilityName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblFacilityName.Location = new System.Drawing.Point(-3, 0);
            this.lblFacilityName.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblFacilityName.Name = "lblFacilityName";
            //this.lblFacilityName.Size = new System.Drawing.Size(125, 32);
            this.lblFacilityName.Height = 32;
            this.lblFacilityName.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblFacilityName.FontSize = 12;
            this.lblFacilityName.FontWeight = System.Windows.FontWeights.Bold;
            this.lblFacilityName.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblFacilityName.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblScheduleName
            //// 
            this.lblScheduleName.Background = System.Windows.Media.Brushes.White;
            //this.lblScheduleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblScheduleName.Location = new System.Drawing.Point(123, 0);
            this.lblScheduleName.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblScheduleName.Name = "lblScheduleName";
            //this.lblScheduleName.Size = new System.Drawing.Size(125, 32);
            this.lblScheduleName.Height = 32;
            this.lblScheduleName.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblScheduleName.FontSize = 12;
            this.lblScheduleName.FontWeight = System.Windows.FontWeights.Bold;
            this.lblScheduleName.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblScheduleName.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblScheduleTime
            //// 
            this.lblScheduleTime.Background = System.Windows.Media.Brushes.White;
            //this.lblScheduleTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblScheduleTime.Location = new System.Drawing.Point(249, 0);
            this.lblScheduleTime.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblScheduleTime.Name = "lblScheduleTime";
            this.lblScheduleTime.Height = 32;
            this.lblScheduleTime.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblScheduleTime.FontSize = 12;
            this.lblScheduleTime.FontWeight = System.Windows.FontWeights.Bold;
            this.lblScheduleTime.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblScheduleTime.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblPrice
            //// 
            this.lblPrice.Background = System.Windows.Media.Brushes.White;
            //this.lblPrice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblPrice.Location = new System.Drawing.Point(350, 0);
            this.lblPrice.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Height =32;
            this.lblPrice.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblPrice.FontSize = 12;
            this.lblPrice.FontWeight = System.Windows.FontWeights.Bold;
            this.lblPrice.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblPrice.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblTotalUnits
            //// 
            this.lblTotalUnits.Background = System.Windows.Media.Brushes.White;
            //this.lblTotalUnits.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblTotalUnits.Location = new System.Drawing.Point(451, 0);
            this.lblTotalUnits.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblTotalUnits.Name = "lblTotalUnits";
            this.lblTotalUnits.Height = 32;
            this.lblTotalUnits.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblTotalUnits.FontSize = 12;
            this.lblTotalUnits.FontWeight = System.Windows.FontWeights.Bold;
            this.lblTotalUnits.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblTotalUnits.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblDesiredUnits
            //// 
            this.lblDesiredUnits.Background = System.Windows.Media.Brushes.White;
            //this.lblDesiredUnits.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblDesiredUnits.Location = new System.Drawing.Point(552, 0);
            this.lblDesiredUnits.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblDesiredUnits.Name = "lblDesiredUnits";
            //this.lblDesiredUnits.Size = new System.Drawing.Size(100, 32);
            this.lblDesiredUnits.Height = 32;
            this.lblDesiredUnits.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblDesiredUnits.FontSize = 12;
            this.lblDesiredUnits.FontWeight = System.Windows.FontWeights.Bold;
            this.lblDesiredUnits.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblDesiredUnits.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblBookedUnits
            //// 
            this.lblBookedUnits.Background = System.Windows.Media.Brushes.White;
            //this.lblBookedUnits.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblBookedUnits.Location = new System.Drawing.Point(653, 0);
            this.lblBookedUnits.Margin = new System.Windows.Thickness(1, 1, 0, 0);
            this.lblBookedUnits.Name = "lblBookedUnits";
            this.lblBookedUnits.Height = 32;
            this.lblBookedUnits.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblBookedUnits.FontSize = 12;
            this.lblBookedUnits.FontWeight = System.Windows.FontWeights.Bold;
            this.lblBookedUnits.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblBookedUnits.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            //// 
            //// lblAvailableUnits
            //// 
            this.lblAvailableUnits.Background = System.Windows.Media.Brushes.White;
            //this.lblAvailableUnits.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.lblAvailableUnits.Location = new System.Drawing.Point(754, 1);
            this.lblAvailableUnits.Margin = new System.Windows.Thickness(1, 1, 1, 0);
            this.lblAvailableUnits.Name = "lblAvailableUnits";
            this.lblAvailableUnits.Height = 32;
            this.lblAvailableUnits.FontFamily = new System.Windows.Media.FontFamily("Arial");
            this.lblAvailableUnits.FontSize = 12;
            this.lblAvailableUnits.FontWeight = System.Windows.FontWeights.Bold;
            this.lblAvailableUnits.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.lblAvailableUnits.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            

        }
        private void usrCtrlAttractionScheduleDetails_Load(object sender, EventArgs e)
        {
            LoadLineDetails();
        }

        private void LoadLineDetails()
        {
            log.LogMethodEntry();

            if (scheduleDetails != null)
            {
                if (!pastSchedule)
                {
                    lblFacilityName.MouseDown += clickAction;
                    lblScheduleName.MouseDown += clickAction;
                    lblScheduleTime.MouseDown += clickAction;
                    lblPrice.MouseDown += clickAction;
                    lblTotalUnits.MouseDown += clickAction;
                    lblDesiredUnits.MouseDown += clickAction;
                    lblBookedUnits.MouseDown += clickAction;
                    lblAvailableUnits.MouseDown += clickAction;
                }

                lblFacilityName.Tag = lblScheduleName.Tag = lblScheduleTime.Tag = lblPrice.Tag = lblTotalUnits.Tag
                = lblBookedUnits.Tag = lblAvailableUnits.Tag = scheduleDetails;

                lblFacilityName.Content = scheduleDetails.FacilityMapName;
                lblFacilityName.Name = "FacilityNameDTOID" + scheduleDetails.ScheduleId.ToString();

                lblScheduleName.Content = scheduleDetails.ScheduleName;
                lblScheduleName.Name = "ScheduleNameDTOID" + scheduleDetails.ScheduleId.ToString();

                int hours = Decimal.ToInt32(scheduleDetails.ScheduleFromTime);
                int minutes = (int)((scheduleDetails.ScheduleFromTime - hours) * 100);
                lblScheduleTime.Content = selectedDate.Date.AddMinutes(hours * 60 + minutes).ToString("hh:mm tt");
                lblScheduleTime.Name = "ScheduleTimeDTOID" + scheduleDetails.ScheduleId.ToString();

                lblPrice.Content = string.Format("{0:" + utilities.getAmountFormat() + "}", scheduleDetails.Price);
                lblPrice.Name = "PriceDTOID" + scheduleDetails.ScheduleId.ToString();

                lblTotalUnits.Content = scheduleDetails.TotalUnits.ToString();
                lblTotalUnits.Name = "TotalUnitsDTOID" + scheduleDetails.ScheduleId.ToString();

                lblDesiredUnits.Content = string.Format("{0:" + utilities.getNumberFormat() + "}", desiredUnits);
                lblDesiredUnits.Name = "DesiredUnitsDTOID" + scheduleDetails.ScheduleId.ToString();

                int bookedUnits = Math.Min(scheduleDetails.BookedUnits == null ? 0 : Convert.ToInt32(scheduleDetails.BookedUnits), scheduleDetails.TotalUnits == null ? 0 : Convert.ToInt32(scheduleDetails.TotalUnits));
                lblBookedUnits.Content = string.Format("{0:" + utilities.getNumberFormat() + "}", Math.Max(0, bookedUnits));
                lblBookedUnits.Name = "BookedUnitsDTOID" + scheduleDetails.ScheduleId.ToString();

                lblAvailableUnits.Content = Math.Max(0, scheduleDetails.AvailableUnits == null ? 0 : Convert.ToInt32(scheduleDetails.AvailableUnits)).ToString();
                lblAvailableUnits.Name = "AvailableUnitsDTOID" + scheduleDetails.ScheduleId.ToString();
            }

            SetControlSize();

            System.Windows.Controls.WrapPanel uIElementCollection = new System.Windows.Controls.WrapPanel();
            uIElementCollection.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            uIElementCollection.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            uIElementCollection.Width = cumilativeWidth;
            uIElementCollection.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            uIElementCollection.Children.Add(lblFacilityName);
            uIElementCollection.Children.Add(lblScheduleName);
            uIElementCollection.Children.Add(lblScheduleTime);
            uIElementCollection.Children.Add(lblPrice);
            uIElementCollection.Children.Add(lblTotalUnits);
            uIElementCollection.Children.Add(lblDesiredUnits);
            uIElementCollection.Children.Add(lblBookedUnits);
            uIElementCollection.Children.Add(lblAvailableUnits);
            
            
            
            this.Content = uIElementCollection;

            

            log.LogMethodExit();
        }

        private void SetControlSize()
        {
            //lblFacilityName.Top = this.Top - 1;
            //lblScheduleName.Top = this.Top - 1;
            //lblScheduleTime.Top = this.Top - 1;
            //lblPrice.Top = this.Top - 1;
            //lblTotalUnits.Top = this.Top - 1;
            //lblDesiredUnits.Top = this.Top - 1;
            //lblBookedUnits.Top = this.Top - 1;
            //lblAvailableUnits.Top = this.Top - 1;

            cumilativeWidth = 0;
            //lblFacilityName.Left = 0;
            cumilativeWidth += lblFacilityName.Width = (int)(this.Width * 0.18);

            //lblScheduleName.Left = lblFacilityName.Right + 1;
            cumilativeWidth += lblScheduleName.Width = (int)(this.Width * 0.18);

            //lblScheduleTime.Left = lblScheduleName.Right + 1;
            cumilativeWidth += lblScheduleTime.Width = (int)(this.Width * 0.10);
            //lblPrice.Left = lblScheduleTime.Right + 1;
            cumilativeWidth += lblPrice.Width = (int)(this.Width * 0.1);
            //lblTotalUnits.Left = lblPrice.Right + 1;
            cumilativeWidth += lblTotalUnits.Width = (int)(this.Width * 0.1);
            //lblDesiredUnits.Left = lblTotalUnits.Right + 1;
            cumilativeWidth += lblDesiredUnits.Width = (int)(this.Width * 0.1);
            //lblBookedUnits.Left = lblDesiredUnits.Right + 1;
            cumilativeWidth += lblBookedUnits.Width = (int)(this.Width * 0.1);
            //lblAvailableUnits.Left = lblBookedUnits.Right + 1;
            cumilativeWidth += lblAvailableUnits.Width = (int)(this.Width - cumilativeWidth - 9);
            cumilativeWidth += 10;
            SetControlColor(backColor);
        }

        public void SetControlColor(System.Windows.Media.Color backColor)
        {
            //if (scheduleDetails.AvailableUnits == 0)
            //{
            //    backColor = Color.LightGray;
            //}

            System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(backColor);
            lblFacilityName.Background = lblScheduleName.Background = lblScheduleTime.Background = lblPrice.Background = lblTotalUnits.Background = lblDesiredUnits.Background
                = lblBookedUnits.Background = lblAvailableUnits.Background = solidColorBrush;

            if (scheduleDetails.AvailableUnits > 0 && (scheduleDetails.AvailableUnits < (int)(0.10 * scheduleDetails.TotalUnits)))
            {
                lblAvailableUnits.Background = System.Windows.Media.Brushes.IndianRed;
            }
        }

        private void usrCtrlAttractionScheduleDetails_AutoSizeChanged(object sender, EventArgs e)
        {
            this.Width = ((usrCtrlAttractionScheduleDetails)sender).DesiredSize.Width;
            SetControlSize();
        }


    }
}
