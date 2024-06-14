/* Project Name - ReservationCoreBL Programs 
* Description  - Data object of the ReservationCore
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith                Created 
********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
//using Semnox.Core.HR.Users;

namespace Semnox.Parafait.Booking
{ 
    public class FacilitySeats
    {

        public List<FacilitySeatLayoutDTO> GetSeatLayout(int attractionScheduleId)
        {
            FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler();
            return faciltySeatDatahandler.GetSeatLayout(attractionScheduleId);
        }
        public List<CoreKeyValueStruct> GetSeatLayoutDetails(int attractionScheduleId)
        {
            List<CoreKeyValueStruct>   CoreKeyValueStructList=new List<CoreKeyValueStruct>();
            int row=0;
            int col=0;
            List<FacilitySeatLayoutDTO> facilitySeatLayoutList = GetSeatLayout(attractionScheduleId);

            foreach(FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilitySeatLayoutList)
            {
                if(facilitySeatLayoutDTO.Type=='R' && facilitySeatLayoutDTO.HasSeats=='Y')
                {
                    row++;
                }
                if (facilitySeatLayoutDTO.Type == 'C' && facilitySeatLayoutDTO.HasSeats == 'Y')
                {
                    col++;
                }
            }

            CoreKeyValueStructList.Add(new CoreKeyValueStruct("row", row.ToString()));
            CoreKeyValueStructList.Add(new CoreKeyValueStruct("column", col.ToString()));
            
            return CoreKeyValueStructList;
        }

        public List<FacilitySeatsDTO> GetSeats(int attractionScheduleId,DateTime scheduleDate)
        {
            FaciltySeatDatahandler faciltySeatDatahandler = new FaciltySeatDatahandler();
            return faciltySeatDatahandler.GetSeats(attractionScheduleId, scheduleDate);
        }
    }
}
