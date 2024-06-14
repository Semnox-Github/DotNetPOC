/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data object of the WaiverSignatureDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70        01-Jul -2019   Girish Kundar    Modified : Added Parametrized Constructor with required fields.
 *2.70.2        15-Oct-2019    GUru S A         Waiver phase 2 changes
  ********************************************************************************************* */
using System;
using System.ComponentModel;
using System.Drawing;

namespace Semnox.Parafait.Waiver
{
    public class WaiverSignatureDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private Image sigImage;
        private string signedwaiverFileName;
        //private CustomerDTO customerDTO;
        private WaiversDTO waiverSetDetailDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public WaiverSignatureDetailsDTO()
        {
            log.LogMethodEntry();
            id = -1;
            //customerDTO = new CustomerDTO();
            waiverSetDetailDTO = new WaiversDTO();
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public WaiverSignatureDetailsDTO(int id, Image sigImage, string signedwaiverFileName)
            : this()
        {
            log.LogMethodEntry(id, sigImage, signedwaiverFileName);
            this.id = id;
            this.sigImage = sigImage;
            this.signedwaiverFileName = signedwaiverFileName;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [ReadOnly(true)]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>
        public Image ImageFile
        {
            get
            {
                return sigImage;
            }
            set
            {
                sigImage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>
        public string SignedWaiverFileName { get { return signedwaiverFileName; } set { signedwaiverFileName = value; } }

        ///// <summary>
        ///// Get/Set method of the customDataSetDTO field
        ///// </summary>
        //[Browsable(false)]
        //public CustomerDTO CustomerDTO
        //{
        //    get
        //    {
        //        return customerDTO;
        //    }

        //    set
        //    {
        //        customerDTO = value;
        //    }
        //}

        /// <summary>
        /// Get/Set method of the customDataSetDTO field
        /// </summary>
        [Browsable(false)]
        public WaiversDTO WaiverSetDetailDTO
        {
            get
            {
                return waiverSetDetailDTO;
            }

            set
            {
                waiverSetDetailDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
