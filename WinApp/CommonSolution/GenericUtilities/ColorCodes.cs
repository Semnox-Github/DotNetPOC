/********************************************************************************************
 * Project Name - ColorCodes
 * Description  - ColorCodes utility
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       22-Apr-2019   Guru S A         Moved from Product datahanlder to new class  
 ********************************************************************************************/
using System;
using System.Drawing;

namespace Semnox.Core.GenericUtilities
{
    public static class ColorCodes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        /// <summary>
        /// method which takes color string as parameter and converts the same to RGB
        /// </summary>
        /// <param name="colorString"> ColorString</param>
        public static string GetColorCode(string colorString)
        {
            log.LogMethodEntry(colorString);
            try
            {
                if (String.IsNullOrEmpty(colorString))
                {
                    log.LogMethodExit("");
                    return "";
                }
                System.Drawing.Color convertedColor;
                if (colorString.Contains(","))
                {
                    log.LogMethodExit(colorString);
                    return colorString;
                }
                else
                {
                    convertedColor = Color.FromName(colorString);
                    if (convertedColor.ToArgb() == 0)
                    {
                        convertedColor = Color.FromArgb(Int32.Parse(colorString, System.Globalization.NumberStyles.HexNumber));
                    }
                    //convertedColor = System.Drawing.ColorTranslator.FromHtml(colorString);
                    log.LogMethodExit(convertedColor.R + "," + convertedColor.G + "," + convertedColor.B);
                    return convertedColor.R + "," + convertedColor.G + "," + convertedColor.B;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing GetColorCode(). " + ex.Message);
                log.LogMethodExit(null,"Throwing Exception" + ex.Message);
                throw;
            }
        }

    }
}
