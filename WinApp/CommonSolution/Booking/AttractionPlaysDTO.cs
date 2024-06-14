/********************************************************************************************
 * Project Name - AttractionPlays DTO
 * Description  - Data object of ReservationCore
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-August-2017    Rakshith            Created 
 * *******************************************************************************************
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// AttractionPlaysDTO   Class
    /// </summary>
    [Table("CheckInFacility")]
    public class AttractionPlaysDTO
    {


        /// <summary>
        /// Default constructor
        /// </summary>
        public AttractionPlaysDTO()
        {
            this.AttractionPlayId = -1;
            this.PlayName = "";
            this.ExpiryDate = DateTime.MinValue;
            this.SiteId = -1;
            this.Price = 0;
            this.SynchStatus = false;
            this.MasterEntityId = -1;
                

        }

        /// <summary>
        /// Constructor with some  data fields
        /// </summary>
        public AttractionPlaysDTO(  int AttractionPlayId, string PlayName)
        {
            this.AttractionPlayId = AttractionPlayId;
            this.PlayName = PlayName;
        }

        /// <summary>
        /// Get/Set method of the AttractionPlayId field
        /// </summary>
        [Key]
        public int AttractionPlayId { get; set; }

        /// <summary>
        /// Get/Set method of the PlayName field
        /// </summary>
        public string PlayName { get; set; }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        public float Price { get; set; }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public Guid? Guid { get; set; }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Column("site_id")]
        public int? SiteId { get; set; }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool? SynchStatus { get; set; }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int? MasterEntityId { get; set; }


    }
}
