/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Object of GameServerReportBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        23-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParafaitServer
{
    public class GameServerReportDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private string aPName;
        private string machineName;
        private string id;
        private string send;
        private double rate;
        private List<MachineDTO> machineDTOList;

        public GameServerReportDTO()
        {
            log.LogMethodEntry();
            aPName = string.Empty;
            machineName = string.Empty;
            id = string.Empty;
            machineDTOList = new List<MachineDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the APName field
        /// </summary>
        [DisplayName("AP")]
        public string APName
        {
            get
            {
                return aPName;
            }
            set
            {
                aPName = value;
            }
        }

        ///// <summary>
        ///// Get/Set method of the MachineName field
        ///// </summary>
        //[DisplayName("MachineName")]
        //public string MachineName
        //{
        //    get
        //    {
        //        return machineName;
        //    }
        //    set
        //    {
        //        machineName = value;
        //    }
        //}

        ///// <summary>
        ///// Get/Set method of the Id field
        ///// </summary>
        //[DisplayName("Id")]
        //public string Id
        //{
        //    get
        //    {
        //        return id;
        //    }
        //    set
        //    {
        //        id = value;
        //    }
        //}

        /// <summary>
        /// Get/Set method of the Send field
        /// </summary>
        [DisplayName("Send")]
        public string Send
        {
            get
            {
                return send;
            }
            set
            {
                send = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Rate field
        /// </summary>
        [DisplayName("Rate")]
        public double Rate
        {
            get
            {
                return rate;
            }
            set
            {
                rate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineDTOList field
        /// </summary>
        public List<MachineDTO> MachineDTOList
        {
            get { return machineDTOList; }
            set { machineDTOList = value; }
        }

    }
}
