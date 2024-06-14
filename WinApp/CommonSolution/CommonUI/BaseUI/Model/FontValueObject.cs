/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents a font and it properties 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.130.0     19-Jul-2021      Lakshminarayana     Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Semnox.Parafait.CommonUI
{
    public class FontValueObject : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Windows.Media.FontFamily fontFamily;
        private decimal fontSize;
        private System.Windows.FontStyle fontStyle = System.Windows.FontStyles.Normal;
        private System.Windows.FontWeight fontWeight = System.Windows.FontWeights.Normal;
        public FontValueObject(string stringRepresntation)
        {
            log.LogMethodEntry(stringRepresntation);
            string[] splits = stringRepresntation.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ValidateFont(stringRepresntation))
            {
                throw new ArgumentException("Invalid argument " + "stringRepresntation");
            }
            Font font = ConvertStringToFont(stringRepresntation);
            fontFamily = new System.Windows.Media.FontFamily(font.FontFamily.Name);
            fontSize = (decimal) font.Size;
            if ((font.Style & System.Drawing.FontStyle.Italic) != 0)
            {
                fontStyle = System.Windows.FontStyles.Italic;
            }
            if((font.Style & System.Drawing.FontStyle.Bold) != 0)
            {
                fontWeight = System.Windows.FontWeights.Bold;
            }
            log.LogMethodExit();
        }

        public FontValueObject(System.Windows.Media.FontFamily fontFamily, decimal fontSize, System.Windows.FontStyle fontStyle, System.Windows.FontWeight fontWeight)
            : this(GetStringRepresentation(fontFamily, fontSize, fontStyle, fontWeight))
        {
            log.LogMethodEntry(fontFamily, fontSize, fontStyle, fontWeight);
            log.LogMethodExit();
        }

        internal bool ValidateFont(string stringFont)
        {
            log.LogMethodEntry(stringFont);
            bool valid = false;
            try
            {
                TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
                Font font = ConvertStringToFont(stringFont);
                string fontText = fontConverter.ConvertToString(font);
                valid = string.Equals(fontText, stringFont);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private Font ConvertStringToFont(string fontString)
        {
            log.LogMethodEntry(fontString);
            Font fontObject = null;
            TypeConverter fconverter = TypeDescriptor.GetConverter(typeof(Font));
            try
            {
                fontObject = fconverter.ConvertFromInvariantString(fontString) as Font;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                try
                {
                    fontObject = fconverter.ConvertFromString(fontString) as Font;
                }
                catch (Exception exp)
                {
                    log.Error(exp);
                }

            }
            if (fontObject == null)
            {
                throw new Exception("Please check the font string value. It should be compatible with regional culture settings.");
            }
            log.LogMethodExit(fontObject);
            return fontObject;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return fontFamily;
            yield return fontSize;
            yield return fontStyle;
            yield return fontWeight;
        }

        private static string GetStringRepresentation(System.Windows.Media.FontFamily fontFamily, decimal fontSize, System.Windows.FontStyle fontStyle, System.Windows.FontWeight fontWeight)
        {
            log.LogMethodEntry(fontFamily, fontSize, fontStyle, fontWeight);
            string styleString = string.Empty;
            string fontStyleString = GetFontStyleString(fontStyle);
            string fontWeightString = GetFontWeightString(fontWeight);
            if(string.IsNullOrWhiteSpace(fontStyleString) == false ||
                string.IsNullOrWhiteSpace(fontWeightString) == false)
            {
                styleString = "style=";
                string appender = string.Empty;
                if(string.IsNullOrWhiteSpace(fontStyleString) == false)
                {
                    appender = ", ";
                    styleString = styleString + fontStyleString;
                }
                if (string.IsNullOrWhiteSpace(fontWeightString) == false)
                {
                    styleString = styleString + appender;
                    styleString = styleString + fontWeightString;
                }
            }
            string result = fontFamily.ToString() + ", " + fontSize.ToString() + ", " + styleString;
            log.LogMethodExit(result);
            return result;
        }

        private static string GetFontStyleString(System.Windows.FontStyle fontStyle)
        {
            log.LogMethodEntry(fontStyle);
            string result = string.Empty;
            if(fontStyle == System.Windows.FontStyles.Italic)
            {
                result =  "Italic";
            }
            log.LogMethodExit(result);
            return result;
        }

        public System.Windows.Media.FontFamily FontFamily
        {
            get
            {
                return fontFamily;
            }
        }

        public decimal FontSize
        {
            get
            {
                return fontSize;
            }
        }

        public System.Windows.FontStyle FontStyle
        {
            get
            {
                return fontStyle;
            }
        }

        public System.Windows.FontWeight FontWeight
        {
            get
            {
                return fontWeight;
            }
        }

        private static string GetFontWeightString(System.Windows.FontWeight fontWeight)
        {
            log.LogMethodEntry(fontWeight);
            string result = string.Empty;
            if (fontWeight == System.Windows.FontWeights.Bold)
            {
                result = "Bold";
            }
            log.LogMethodExit(result);
            return result;
        }

        public override string ToString()
        {
            return GetStringRepresentation(fontFamily, fontSize, fontStyle, fontWeight);
        }
    }
}
