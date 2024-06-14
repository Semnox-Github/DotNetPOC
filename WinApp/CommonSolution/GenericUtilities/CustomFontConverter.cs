/********************************************************************************************
 * Project Name - Custom Font Converter
 * Description  - Custom Font Converter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      11-Feb-2020   Deeksha                 Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Semnox.Core.GenericUtilities
{
    public static class CustomFontConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Font ConvertStringToFont(ExecutionContext executionContext, string fontString)
        {
            log.LogMethodEntry(executionContext, fontString);
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
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2490));
            }
            log.LogMethodExit(fontObject);
            return fontObject;
        }
    }
}
