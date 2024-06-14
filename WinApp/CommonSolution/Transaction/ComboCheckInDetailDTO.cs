using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CustomCheckInDetailDTO
    /// </summary>
    public class ComboCheckInDetailDTO
    {
        /// <summary>
        /// CheckInDetailDTOList
        /// </summary>
        public List<CheckInDetailDTO> CheckInDetailDTOList { get; set; }

        /// <summary>
        /// CheckInProductId
        /// </summary>
        public int CheckInProductId { get; set; }
        /// <summary>
        /// CreateLinesWithCheckINDetails
        /// </summary>
        public bool CreateLinesWithCheckInDetails { get; set; }
        /// <summary>
        /// CustomCheckInDetailDTO
        /// </summary>
        public ComboCheckInDetailDTO()
        {
            CheckInDetailDTOList = new List<CheckInDetailDTO>();
            CheckInProductId = -1;
            CreateLinesWithCheckInDetails = true;
        }
    }
}

