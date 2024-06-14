/********************************************************************************************
 * Project Name - Transaction Services - ScheduleDetailsViewDTO
 * Description  - View DTO of Attraction Schedules. 
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
  *2.110      20-Jan-2021   Nitin Pai                Created
 ***************************************************************************************************/
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction.TransactionFunctions
{
    public class ScheduleDetailsViewDTO
    {
        private ScheduleDetailsDTO scheduleDetailsDTO;
        private DayAttractionScheduleDTO dayAttractionScheduleDTO;
        private bool visible;
        private bool pastSchedule;
        private String backColor;

        public ScheduleDetailsDTO ScheduleDetailsDTO
        {
            get { return scheduleDetailsDTO; } set { scheduleDetailsDTO = value; }
        }

        public DayAttractionScheduleDTO DayAttractionScheduleDTO
        {
            get { return dayAttractionScheduleDTO; }
            set { dayAttractionScheduleDTO = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool PastSchedule
        {
            get { return pastSchedule; }
            set { pastSchedule = value; }
        }

        public String BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

    }
}
