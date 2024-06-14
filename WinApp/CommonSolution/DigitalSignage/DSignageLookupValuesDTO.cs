/********************************************************************************************
 * Project Name - DSignageLookupValuesDTO
 * Description  - Data object of DSignageLookupValues
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2017   Lakshminarayana          Created
 *2.70.2      30-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 *2.110.0     27-Nov-2020   Prajwal S                Modified : Default Constructor
 *2.110.0     04-Mar-2021   Prajwal S                Modified : Digital Signage changes
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the DSignageLookupValues data object class. This acts as data holder for the DSignageLookupValues business object
    /// </summary>
    public class DSignageLookupValuesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DSLookupValueID field
            /// </summary>
            DSLOOKUP_VALUE_ID,

            /// <summary>
            /// Search by DSLookupID field
            /// </summary>
            DSLOOKUP_ID,

            /// <summary>
            /// Search by ValActiveFlag field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int dSLookupValueID;
        private int dSLookupID;
        private int valDisplayOrder;
        private int? beforeSpacingRows;
        private string value1;
        private string val1TextColor;
        private string val1Font;
        private int val1DataType;
        private int val1Indentation;
        private string val1BackColor;
        private string val1Description;
        private string val1Height;
        private string val1Width;
        private int val1ContentLayout;
        private string value2;
        private string val2TextColor;
        private string val2Font;
        private int val2DataType;
        private int val2Indentation;
        private string val2BackColor;
        private string val2Description;
        private string val2Height;
        private string val2Width;
        private int val2ContentLayout;
        private string value3;
        private string val3TextColor;
        private string val3Font;
        private int val3DataType;
        private int val3Indentation;
        private string val3BackColor;
        private string val3Description;
        private string val3Height;
        private string val3Width;
        private int val3ContentLayout;
        private string value4;
        private string val4TextColor;
        private string val4Font;
        private int val4DataType;
        private int val4Indentation;
        private string val4BackColor;
        private string val4Description;
        private string val4Height;
        private string val4Width;
        private int val4ContentLayout;
        private string value5;
        private string val5TextColor;
        private string val5Font;
        private int val5DataType;
        private int val5Indentation;
        private string val5BackColor;
        private string val5Description;
        private string val5Height;
        private string val5Width;
        private int val5ContentLayout;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DSignageLookupValuesDTO()
        {
            log.LogMethodEntry();
            dSLookupValueID = -1;
            masterEntityId = -1;
            valDisplayOrder = 0;
            isActive = true;
            val1DataType = -1;
            val2DataType = -1;
            val3DataType = -1;
            val4DataType = -1;
            val5DataType = -1;
            val1Indentation = -1;
            val2Indentation = -1;
            val3Indentation = -1;
            val4Indentation = -1;
            val5Indentation = -1;
            val1ContentLayout = -1;
            val2ContentLayout = -1;
            val3ContentLayout = -1;
            val4ContentLayout = -1;
            val5ContentLayout = -1;
            dSLookupID = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public DSignageLookupValuesDTO(int dSLookupValueID, int dSLookupID, int valDisplayOrder, int? beforeSpacingRows,
                                       string value1, string val1TextColor, string val1Font, int val1DataType, int val1Indentation, string val1BackColor, string val1Description, string val1Height, string val1Width, int val1ContentLayout, 
                                       string value2, string val2TextColor, string val2Font, int val2DataType, int val2Indentation, string val2BackColor, string val2Description, string val2Height, string val2Width, int val2ContentLayout, 
                                       string value3, string val3TextColor, string val3Font, int val3DataType, int val3Indentation, string val3BackColor, string val3Description, string val3Height, string val3Width, int val3ContentLayout, 
                                       string value4, string val4TextColor, string val4Font, int val4DataType, int val4Indentation, string val4BackColor, string val4Description, string val4Height, string val4Width, int val4ContentLayout,
                                       string value5, string val5TextColor, string val5Font, int val5DataType, int val5Indentation, string val5BackColor, string val5Description, string val5Height, string val5Width, int val5ContentLayout, 
                                       bool isActive)
            : this()
        {
            log.LogMethodEntry(dSLookupValueID, dSLookupID, valDisplayOrder, beforeSpacingRows, 
                               value1, val1TextColor,val1Font, val1DataType, val1Indentation, val1BackColor, val1Description, val1Height, val1Width, val1ContentLayout,
                               value2, val2TextColor, val2Font, val2DataType, val2Indentation, val2BackColor, val2Description, val2Height, val2Width, val2ContentLayout,
                               value3, val3TextColor, val3Font, val3DataType, val3Indentation, val3BackColor, val3Description, val3Height, val3Width, val3ContentLayout,
                               value4, val4TextColor, val4Font, val4DataType, val4Indentation, val4BackColor, val4Description, val4Height, val4Width, val4ContentLayout,
                               value5, val5TextColor, val5Font, val5DataType, val5Indentation, val5BackColor, val5Description, val5Height, val5Width, val5ContentLayout, isActive);
            this.dSLookupValueID = dSLookupValueID;
            this.dSLookupID = dSLookupID;
            this.valDisplayOrder = valDisplayOrder;
            this.beforeSpacingRows = beforeSpacingRows;
            this.value1 = value1;
            this.val1TextColor = val1TextColor;
            this.val1Font = val1Font;
            this.val1DataType = val1DataType;
            this.val1Indentation = val1Indentation;
            this.val1BackColor = val1BackColor;
            this.val1Description = val1Description;
            this.val1Height = val1Height;
            this.val1Width = val1Width;
            this.val1ContentLayout = val1ContentLayout;
            this.value2 = value2;
            this.val2TextColor = val2TextColor;
            this.val2Font = val2Font;
            this.val2DataType = val2DataType;
            this.val2Indentation = val2Indentation;
            this.val2BackColor = val2BackColor;
            this.val2Description = val2Description;
            this.val2Height = val2Height;
            this.val2Width = val2Width;
            this.val2ContentLayout = val2ContentLayout;
            this.value3 = value3;
            this.val3TextColor = val3TextColor;
            this.val3Font = val3Font;
            this.val3DataType = val3DataType;
            this.val3Indentation = val3Indentation;
            this.val3BackColor = val3BackColor;
            this.val3Description = val3Description;
            this.val3Height = val3Height;
            this.val3Width = val3Width;
            this.val3ContentLayout = val3ContentLayout;
            this.value4 = value4;
            this.val4TextColor = val4TextColor;
            this.val4Font = val4Font;
            this.val4DataType = val4DataType;
            this.val4Indentation = val4Indentation;
            this.val4BackColor = val4BackColor;
            this.val4Description = val4Description;
            this.val4Height = val4Height;
            this.val4Width = val4Width;
            this.val4ContentLayout = val4ContentLayout;
            this.value5 = value5;
            this.val5TextColor = val5TextColor;
            this.val5Font = val5Font;
            this.val5DataType = val5DataType;
            this.val5Indentation = val5Indentation;
            this.val5BackColor = val5BackColor;
            this.val5Description = val5Description;
            this.val5Height = val5Height;
            this.val5Width = val5Width;
            this.val5ContentLayout = val5ContentLayout;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DSignageLookupValuesDTO(int dSLookupValueID, int dSLookupID, int valDisplayOrder, int? beforeSpacingRows,
                                        string value1, string val1TextColor, string val1Font, int val1DataType, int val1Indentation, string val1BackColor, string val1Description, string val1Height, string val1Width, int val1ContentLayout,
                                        string value2, string val2TextColor, string val2Font, int val2DataType, int val2Indentation, string val2BackColor, string val2Description, string val2Height, string val2Width, int val2ContentLayout,
                                        string value3, string val3TextColor, string val3Font, int val3DataType, int val3Indentation, string val3BackColor, string val3Description, string val3Height, string val3Width, int val3ContentLayout,
                                        string value4, string val4TextColor, string val4Font, int val4DataType, int val4Indentation, string val4BackColor, string val4Description, string val4Height, string val4Width, int val4ContentLayout,
                                        string value5, string val5TextColor, string val5Font, int val5DataType, int val5Indentation, string val5BackColor, string val5Description, string val5Height, string val5Width, int val5ContentLayout,
                                        bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId,
                                        bool synchStatus, string guid)
            : this(dSLookupValueID, dSLookupID, valDisplayOrder, beforeSpacingRows,
                                         value1, val1TextColor, val1Font, val1DataType, val1Indentation, val1BackColor, val1Description, val1Height, val1Width, val1ContentLayout,
                                         value2, val2TextColor, val2Font, val2DataType, val2Indentation, val2BackColor, val2Description, val2Height, val2Width, val2ContentLayout,
                                         value3, val3TextColor, val3Font, val3DataType, val3Indentation, val3BackColor, val3Description, val3Height, val3Width, val3ContentLayout,
                                         value4, val4TextColor, val4Font, val4DataType, val4Indentation, val4BackColor, val4Description, val4Height, val4Width, val4ContentLayout,
                                         value5, val5TextColor, val5Font, val5DataType, val5Indentation, val5BackColor, val5Description, val5Height, val5Width, val5ContentLayout, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DSLookupValueID field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int DSLookupValueID
        {
            get
            {
                return dSLookupValueID;
            }

            set
            {
                this.IsChanged = true;
                dSLookupValueID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DSLookupID field
        /// </summary>
        [Browsable(false)]
        public int DSLookupID
        {
            get
            {
                return dSLookupID;
            }

            set
            {
                this.IsChanged = true;
                dSLookupID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active Flag")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValDisplayOrder field
        /// </summary>
        [DisplayName("Display Order")]
        public int ValDisplayOrder
        {
            get
            {
                return valDisplayOrder;
            }

            set
            {
                this.IsChanged = true;
                valDisplayOrder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Value1 field
        /// </summary>
        [DisplayName("Header 1")]
        public string Value1
        {
            get
            {
                return value1;
            }

            set
            {
                this.IsChanged = true;
                value1 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1TextColor field
        /// </summary>
        [DisplayName("Hdr-1 Text Color")]
        public string Val1TextColor
        {
            get
            {
                return val1TextColor;
            }

            set
            {
                this.IsChanged = true;
                val1TextColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1Font field
        /// </summary>
        [DisplayName("Hdr-1 Font")]
        public string Val1Font
        {
            get
            {
                return val1Font;
            }

            set
            {
                this.IsChanged = true;
                val1Font = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1DataType field
        /// </summary>
        [DisplayName("Hdr-1 Data Type")]
        public int Val1DataType
        {
            get
            {
                return val1DataType;
            }

            set
            {
                this.IsChanged = true;
                val1DataType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1Indentation field
        /// </summary>
        [DisplayName("Hdr-1 Alignment")]
        public int Val1Indentation
        {
            get
            {
                return val1Indentation;
            }

            set
            {
                this.IsChanged = true;
                val1Indentation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1BackColor field
        /// </summary>
        [DisplayName("Hdr-1 Back Color")]
        public string Val1BackColor
        {
            get
            {
                return val1BackColor;
            }

            set
            {
                this.IsChanged = true;
                val1BackColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1Description field
        /// </summary>
        [Browsable(false)]
        public string Val1Description
        {
            get
            {
                return val1Description;
            }

            set
            {
                this.IsChanged = true;
                val1Description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1Height field
        /// </summary>
        [DisplayName("Hdr-1 Height")]
        public string Val1Height
        {
            get
            {
                return val1Height;
            }

            set
            {
                this.IsChanged = true;
                val1Height = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val1Width field
        /// </summary>
        [DisplayName("Hdr-1 Width")]
        public string Val1Width
        {
            get
            {
                return val1Width;
            }

            set
            {
                this.IsChanged = true;
                val1Width = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val1Width field
        /// </summary>
        [DisplayName("Hdr-1 Content Layout")]
        public int Val1ContentLayout
        {
            get
            {
                return val1ContentLayout;
            }

            set
            {
                this.IsChanged = true;
                val1ContentLayout = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Value2 field
        /// </summary>
        [DisplayName("Header 2")]
        public string Value2
        {
            get
            {
                return value2;
            }

            set
            {
                this.IsChanged = true;
                value2 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2TextColor field
        /// </summary>
        [DisplayName("Hdr-2 Text Color")]
        public string Val2TextColor
        {
            get
            {
                return val2TextColor;
            }

            set
            {
                this.IsChanged = true;
                val2TextColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2Font field
        /// </summary>
        [DisplayName("Hdr-2 Font")]
        public string Val2Font
        {
            get
            {
                return val2Font;
            }

            set
            {
                this.IsChanged = true;
                val2Font = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2DataType field
        /// </summary>
        [DisplayName("Hdr-2 Data Type")]
        public int Val2DataType
        {
            get
            {
                return val2DataType;
            }

            set
            {
                this.IsChanged = true;
                val2DataType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2Indentation field
        /// </summary>
        [DisplayName("Hdr-2 Alignment")]
        public int Val2Indentation
        {
            get
            {
                return val2Indentation;
            }

            set
            {
                this.IsChanged = true;
                val2Indentation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2BackColor field
        /// </summary>
        [DisplayName("Hdr-2 Back Color")]
        public string Val2BackColor
        {
            get
            {
                return val2BackColor;
            }

            set
            {
                this.IsChanged = true;
                val2BackColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2Description field
        /// </summary>
        [Browsable(false)]
        public string Val2Description
        {
            get
            {
                return val2Description;
            }

            set
            {
                this.IsChanged = true;
                val2Description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2Height field
        /// </summary>
        [DisplayName("Hdr-2 Height")]
        public string Val2Height
        {
            get
            {
                return val2Height;
            }

            set
            {
                this.IsChanged = true;
                val2Height = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val2Width field
        /// </summary>
        [DisplayName("Hdr-2 Width")]
        public string Val2Width
        {
            get
            {
                return val2Width;
            }

            set
            {
                this.IsChanged = true;
                val2Width = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val2Width field
        /// </summary>
        [DisplayName("Hdr-2 Content Layout")]
        public int Val2ContentLayout
        {
            get
            {
                return val2ContentLayout;
            }

            set
            {
                this.IsChanged = true;
                val2ContentLayout = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Value3 field
        /// </summary>
        [DisplayName("Header 3")]
        public string Value3
        {
            get
            {
                return value3;
            }

            set
            {
                this.IsChanged = true;
                value3 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3TextColor field
        /// </summary>
        [DisplayName("Hdr-3 Text Color")]
        public string Val3TextColor
        {
            get
            {
                return val3TextColor;
            }

            set
            {
                this.IsChanged = true;
                val3TextColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3Font field
        /// </summary>
        [DisplayName("Hdr-3 Font")]
        public string Val3Font
        {
            get
            {
                return val3Font;
            }

            set
            {
                this.IsChanged = true;
                val3Font = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3DataType field
        /// </summary>
        [DisplayName("Hdr-3 Data Type")]
        public int Val3DataType
        {
            get
            {
                return val3DataType;
            }

            set
            {
                this.IsChanged = true;
                val3DataType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3Indentation field
        /// </summary>
        [DisplayName("Hdr-3 Alignment")]
        public int Val3Indentation
        {
            get
            {
                return val3Indentation;
            }

            set
            {
                this.IsChanged = true;
                val3Indentation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3BackColor field
        /// </summary>
        [DisplayName("Hdr-3 Back Color")]
        public string Val3BackColor
        {
            get
            {
                return val3BackColor;
            }

            set
            {
                this.IsChanged = true;
                val3BackColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3Description field
        /// </summary>
        [Browsable(false)]
        public string Val3Description
        {
            get
            {
                return val3Description;
            }

            set
            {
                this.IsChanged = true;
                val3Description = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val3Height field
        /// </summary>
        [DisplayName("Hdr-3 Height")]
        public string Val3Height
        {
            get
            {
                return val3Height;
            }

            set
            {
                this.IsChanged = true;
                val3Height = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val3Width field
        /// </summary>
        [DisplayName("Hdr-3 Width")]
        public string Val3Width
        {
            get
            {
                return val3Width;
            }

            set
            {
                this.IsChanged = true;
                val3Width = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val1Width field
        /// </summary>
        [DisplayName("Hdr-3 Content Layout")]
        public int Val3ContentLayout
        {
            get
            {
                return val3ContentLayout;
            }

            set
            {
                this.IsChanged = true;
                val3ContentLayout = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Value4 field
        /// </summary>
        [DisplayName("Header 4")]
        public string Value4
        {
            get
            {
                return value4;
            }

            set
            {
                this.IsChanged = true;
                value4 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4TextColor field
        /// </summary>
        [DisplayName("Hdr-4 Text Color")]
        public string Val4TextColor
        {
            get
            {
                return val4TextColor;
            }

            set
            {
                this.IsChanged = true;
                val4TextColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4Font field
        /// </summary>
        [DisplayName("Hdr-4 Font")]
        public string Val4Font
        {
            get
            {
                return val4Font;
            }

            set
            {
                this.IsChanged = true;
                val4Font = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4DataType field
        /// </summary>
        [DisplayName("Hdr-4 Data Type")]
        public int Val4DataType
        {
            get
            {
                return val4DataType;
            }

            set
            {
                this.IsChanged = true;
                val4DataType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4Indentation field
        /// </summary>
        [DisplayName("Hdr-4 Alignment")]
        public int Val4Indentation
        {
            get
            {
                return val4Indentation;
            }

            set
            {
                this.IsChanged = true;
                val4Indentation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4BackColor field
        /// </summary>
        [DisplayName("Hdr-4 Back Color")]
        public string Val4BackColor
        {
            get
            {
                return val4BackColor;
            }

            set
            {
                this.IsChanged = true;
                val4BackColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4Description field
        /// </summary>
        [Browsable(false)]
        public string Val4Description
        {
            get
            {
                return val4Description;
            }

            set
            {
                this.IsChanged = true;
                val4Description = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val4Height field
        /// </summary>
        [DisplayName("Hdr-4 Height")]
        public string Val4Height
        {
            get
            {
                return val4Height;
            }

            set
            {
                this.IsChanged = true;
                val4Height = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val4Width field
        /// </summary>
        [DisplayName("Hdr-4 Width")]
        public string Val4Width
        {
            get
            {
                return val4Width;
            }

            set
            {
                this.IsChanged = true;
                val4Width = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val4Width field
        /// </summary>
        [DisplayName("Hdr-4 Content Layout")]
        public int Val4ContentLayout
        {
            get
            {
                return val4ContentLayout;
            }

            set
            {
                this.IsChanged = true;
                val4ContentLayout = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Value5 field
        /// </summary>
        [DisplayName("Header 5")]
        public string Value5
        {
            get
            {
                return value5;
            }

            set
            {
                this.IsChanged = true;
                value5 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5TextColor field
        /// </summary>
        [DisplayName("Hdr-5 Text Color")]
        public string Val5TextColor
        {
            get
            {
                return val5TextColor;
            }

            set
            {
                this.IsChanged = true;
                val5TextColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5Font field
        /// </summary>
        [DisplayName("Hdr-5 Font")]
        public string Val5Font
        {
            get
            {
                return val5Font;
            }

            set
            {
                this.IsChanged = true;
                val5Font = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5DataType field
        /// </summary>
        [DisplayName("Hdr-5 Data Type")]
        public int Val5DataType
        {
            get
            {
                return val5DataType;
            }

            set
            {
                this.IsChanged = true;
                val5DataType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5Indentation field
        /// </summary>
        [DisplayName("Hdr-5 Alignment")]
        public int Val5Indentation
        {
            get
            {
                return val5Indentation;
            }

            set
            {
                this.IsChanged = true;
                val5Indentation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5BackColor field
        /// </summary>
        [DisplayName("Hdr-5 Back Color")]
        public string Val5BackColor
        {
            get
            {
                return val5BackColor;
            }

            set
            {
                this.IsChanged = true;
                val5BackColor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5Description field
        /// </summary>
        [Browsable(false)]
        public string Val5Description
        {
            get
            {
                return val5Description;
            }

            set
            {
                this.IsChanged = true;
                val5Description = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val5Height field
        /// </summary>
        [DisplayName("Hdr-5 Height")]
        public string Val5Height
        {
            get
            {
                return val5Height;
            }

            set
            {
                this.IsChanged = true;
                val5Height = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Val5Width field
        /// </summary>
        [DisplayName("Hdr-5 Width")]
        public string Val5Width
        {
            get
            {
                return val5Width;
            }

            set
            {
                this.IsChanged = true;
                val5Width = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Val5Width field
        /// </summary>
        [DisplayName("Hdr-5 Content Layout")]
        public int Val5ContentLayout
        {
            get
            {
                return val5ContentLayout;
            }

            set
            {
                this.IsChanged = true;
                val5ContentLayout = value;
            }
        }
        /// <summary>
        /// Get/Set method of the BeforeSpacingRows field
        /// </summary>
        [DisplayName("Spacing in Pixels")]
        public int? BeforeSpacingRows
        {
            get
            {
                return beforeSpacingRows;
            }

            set
            {
                this.IsChanged = true;
                beforeSpacingRows = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || dSLookupValueID < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current DSignageLookupValuesDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DSignageLookupValuesDTO-----------------------------\n");
            returnValue.Append(" DSLookupValueID : " + DSLookupValueID);
            returnValue.Append(" DSLookupID : " + DSLookupID);
            returnValue.Append(" ValDisplayOrder : " + ValDisplayOrder);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append(" BeforeSpacingRows : " + BeforeSpacingRows);
            returnValue.Append(" Value1 : " + Value1);
            returnValue.Append(" Val1TextColor : " + Val1TextColor);
            returnValue.Append(" Val1Font : " + Val1Font);
            returnValue.Append(" Val1DataType : " + Val1DataType);
            returnValue.Append(" Val1Indentation : " + Val1Indentation);
            returnValue.Append(" Val1BackColor : " + Val1BackColor);
            returnValue.Append(" Val1Description : " + Val1Description);
            returnValue.Append(" Val1Height : " + Val1Height);
            returnValue.Append(" Val1Width : " + Val1Width);
            returnValue.Append(" Val1ContentLayout : " + Val1ContentLayout);
            returnValue.Append(" Value2 : " + Value2);
            returnValue.Append(" Val2TextColor : " + Val2TextColor);
            returnValue.Append(" Val2Font : " + Val2Font);
            returnValue.Append(" Val2DataType : " + Val2DataType);
            returnValue.Append(" Val2Indentation : " + Val2Indentation);
            returnValue.Append(" Val2BackColor : " + Val2BackColor);
            returnValue.Append(" Val2Description : " + Val2Description);
            returnValue.Append(" Val2Height : " + Val2Height);
            returnValue.Append(" Val2Width : " + Val2Width);
            returnValue.Append(" Val2ContentLayout : " + Val2ContentLayout);
            returnValue.Append(" Value3 : " + Value3);
            returnValue.Append(" Val3TextColor : " + Val3TextColor);
            returnValue.Append(" Val3Font : " + Val3Font);
            returnValue.Append(" Val3DataType : " + Val3DataType);
            returnValue.Append(" Val3Indentation : " + Val3Indentation);
            returnValue.Append(" Val3BackColor : " + Val3BackColor);
            returnValue.Append(" Val3Description : " + Val3Description);
            returnValue.Append(" Val3Height : " + Val3Height);
            returnValue.Append(" Val3Width : " + Val3Width);
            returnValue.Append(" Val3ContentLayout : " + Val3ContentLayout);
            returnValue.Append(" Value4 : " + Value4);
            returnValue.Append(" Val4TextColor : " + Val4TextColor);
            returnValue.Append(" Val4Font : " + Val4Font);
            returnValue.Append(" Val4DataType : " + Val4DataType);
            returnValue.Append(" Val4Indentation : " + Val4Indentation);
            returnValue.Append(" Val4BackColor : " + Val4BackColor);
            returnValue.Append(" Val4Description : " + Val4Description);
            returnValue.Append(" Val4Height : " + Val4Height);
            returnValue.Append(" Val4Width : " + Val4Width);
            returnValue.Append(" Val4ContentLayout : " + Val4ContentLayout);
            returnValue.Append(" Value5 : " + Value5);
            returnValue.Append(" Val5TextColor : " + Val5TextColor);
            returnValue.Append(" Val5Font : " + Val5Font);
            returnValue.Append(" Val5DataType : " + Val5DataType);
            returnValue.Append(" Val5Indentation : " + Val5Indentation);
            returnValue.Append(" Val5BackColor : " + Val5BackColor);
            returnValue.Append(" Val5Description : " + Val5Description);
            returnValue.Append(" Val5Height : " + Val1Height);
            returnValue.Append(" Val5Width : " + Val1Width);
            returnValue.Append(" Val5ContentLayout : " + Val5ContentLayout);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();
        }
    }
}
