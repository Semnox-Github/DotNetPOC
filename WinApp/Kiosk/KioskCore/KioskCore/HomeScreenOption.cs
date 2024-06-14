/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - HomeScreenOption
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.6.0    26-Oct-2023   Guru S A             Created for Dynamic home screen options
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskCore
{
    /// <summary>
    /// HomeScreenOption
    /// </summary>
    public class HomeScreenOption
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string optionCode;
        //private string optionName;
        protected bool showTheOption;
        protected int sortOrderPosition;
        protected ExecutionContext executionContext;
        private static string LARGEFONT = "LF";
        private static string SMALLFONT = "SF";
        private static string MEDIUMFONT = "MF";
        /// <summary>
        /// LARGE_FONT
        /// </summary>
        public static string LARGE_FONT { get {return LARGEFONT;} }
        /// <summary>
        /// MEDIUM_FONT
        /// </summary>
        public static string MEDIUM_FONT { get { return MEDIUMFONT; } }
        /// <summary>
        /// SMALL_FONT
        /// </summary>
        public static string SMALL_FONT { get { return SMALLFONT; }  }

        /// <summary>
        /// HomeScreenOption
        /// </summary>
        /// <param name="executionContext"></param>
        public HomeScreenOption(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }
        /// <summary>
        /// GetOptionCode
        /// </summary>
        /// <returns></returns>
        public string GetOptionCode()
        {
            log.LogMethodEntry(); 
            log.LogMethodExit(optionCode);
            return optionCode;
        }
        /// <summary>
        /// GetOptionImageName
        /// </summary>
        /// <returns></returns>
        public string GetOptionImageName()
        {
            log.LogMethodEntry();
            string imgName = GetOptionImageName(optionCode);
            log.LogMethodExit(imgName);
            return imgName;
        }
        /// <summary>
        /// GetOptionButtonName
        /// </summary>
        /// <returns></returns>
        public string GetOptionButtonName()
        {
            log.LogMethodEntry();
            string btnName = GetOptionButtonName(optionCode);
            log.LogMethodExit(btnName);
            return btnName;
        }
        /// <summary>
        /// GetSortOrder
        /// </summary>
        /// <returns></returns>
        public int GetSortOrder()
        {
            log.LogMethodEntry(); 
            log.LogMethodExit(sortOrderPosition);
            return sortOrderPosition;
        }
        /// <summary>
        /// CanShowTheOption
        /// </summary>
        /// <returns></returns>
        public bool CanShowTheOption()
        {
            log.LogMethodEntry();
            log.LogMethodExit(showTheOption);
            return showTheOption;
        }
        /// <summary>
        /// GetOptionCode
        /// </summary>
        /// <returns></returns>
        public string GetFontSize()
        {
            log.LogMethodEntry();
            string fontSize = GetOptionFontSize(optionCode);
            log.LogMethodExit(fontSize);
            return fontSize;
        }

        protected virtual bool IsValidOption(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            log.LogMethodExit("Please implement IsValidOption");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement IsValidOption"));
        }

        protected virtual bool SetShowTheOption(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            log.LogMethodExit("Please implement SetShowTheOption");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement SetShowTheOption"));
        }
        protected virtual string GetOptionImageName(string optionCode)
        {

            log.LogMethodEntry(optionCode);
            log.LogMethodExit("Please implement GetOptionImageName");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement GetOptionImageName"));
        }
        protected virtual string GetOptionButtonName(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            log.LogMethodExit("Please implement GetOptionButtonName");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement GetOptionButtonName"));
        }

        protected virtual string GetOptionFontSize(string optionCode)
        {
            log.LogMethodEntry(optionCode);
            log.LogMethodExit("Please implement GetOptionFontSize");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement GetOptionFontSize"));
        }
    }
}
