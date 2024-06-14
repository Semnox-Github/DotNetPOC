﻿/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the add product details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalRemoveCreditsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for Point
        /// </summary>
        public List<Points> Point { get; set; }
        /// <summary>
        /// Get/Set for remarks
        /// </summary>
        public string Remarks { get; set; }
     

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExternalRemoveCreditsDTO()
        {
            log.LogMethodEntry();
            Type = String.Empty; ;
            Remarks = String.Empty;
            Point = new List<Points>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public ExternalRemoveCreditsDTO(string type, List<Points> points, string remarks)
        {
            log.LogMethodEntry(type, remarks);
            this.Type = type;
            this.Point = points;
            this.Remarks = remarks;
            log.LogMethodExit();
        }
    }
}