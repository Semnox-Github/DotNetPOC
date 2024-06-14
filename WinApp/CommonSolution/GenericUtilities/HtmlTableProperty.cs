/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - HtmlTableProperty 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
#region Using directives

using System;

#endregion

namespace Semnox.Core.GenericUtilities
{

    /// <summary>
    /// Struct used to define a Html Table
    /// Html Defaults are based on FrontPage default table
    /// </summary>
    [Serializable]
    public struct HtmlTableProperty
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // properties defined for the table
        public string					CaptionText;
        public HorizontalAlignOption	CaptionAlignment;
        public VerticalAlignOption		CaptionLocation;
        public byte						BorderSize;
        public HorizontalAlignOption	TableAlignment;
        public byte						TableRows;
        public byte						TableColumns;
        public ushort					TableWidth;
        public MeasurementOption		TableWidthMeasurement;
        public byte						CellPadding;
        public byte						CellSpacing;


        /// <summary>
        /// Constructor defining a base table with default attributes
        /// </summary>
        public HtmlTableProperty(bool htmlDefaults)
        {
            log.LogMethodEntry(htmlDefaults);
            //define base values
            CaptionText = string.Empty;
            CaptionAlignment = HorizontalAlignOption.Default;
            CaptionLocation = VerticalAlignOption.Default;
            TableAlignment = HorizontalAlignOption.Default;

            // define values based on whether HTML defaults are required
            if (htmlDefaults)
            {
                BorderSize = 2;
                TableRows = 3;
                TableColumns = 3;
                TableWidth = 50;
                TableWidthMeasurement = MeasurementOption.Percent;
                CellPadding = 1;
                CellSpacing = 2;
            }
            else
            {
                BorderSize = 0;
                TableRows = 1;
                TableColumns = 1;
                TableWidth = 0;
                TableWidthMeasurement = MeasurementOption.Pixel;
                CellPadding = 0;
                CellSpacing = 0;
            }
            log.LogMethodExit();
        }

    } //HtmlTableProperty
    
}