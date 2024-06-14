/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom textbox validation helper file
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy            Created for POS UI Redesign 
 *2.120.0     05-Apr-2021   Amitha Joy            New validation added for decimal
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.CommonUI
{
    public static class CustomTextBoxValidationHelper
    {
        #region Memebers
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        public static bool IsValid(string input, ValidationType validationType, out string validaitionMessage)
        {
            log.LogMethodEntry(input, validationType);
            validaitionMessage = String.Empty;
            switch (validationType)
            {
                case ValidationType.AlphabetsOnly:
                    return IsAlphabetsOnlyValid(input, out validaitionMessage);
                case ValidationType.NumberOnly:
                    return IsNumberOnlyValid(input, out validaitionMessage);
                case ValidationType.DecimalOnly:
                    return IsDecimalOnly(input, out validaitionMessage);
                case ValidationType.Alphanumeric:
                    return IsAlphanumericValid(input, out validaitionMessage);
                case ValidationType.ZipCode:
                    return IsZipValid(input, out validaitionMessage);
                case ValidationType.None:
                    return true;
                case ValidationType.Login:
                    return true;
                default:
                    return true;
            }
        }

        private static bool IsAlphabetsOnlyValid(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = String.Empty;
            if (input == null || input == "")
            {
                return true;
            }

            Regex r = new Regex("^[a-zA-Z ]+$");
            if (r.IsMatch(input))
            {
                return true;
            }
            else
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Enter only alphabets");
                return false;
            }
        }

        private static bool IsNumberOnlyValid(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = String.Empty;
            int result;
            if (input == null || input == "")
            {
                return true;
            }
            if (int.TryParse(input, out result))
            {
                log.LogMethodExit();
                return true;
            }
            else
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Enter only numbers");
                return false;
            }
        }

        private static bool IsDecimalOnly(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = string.Empty;
            decimal result;
            if (input == null || input == "")
            {
                log.LogMethodExit();
                return true;
            }
            if (decimal.TryParse(input, out result))
            {
                log.LogMethodExit();
                return true;
            }
            else
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Enter only decimal");
                log.LogMethodExit();
                return false;
            }
        }

        private static bool IsAlphanumericValid(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = String.Empty;
            if (input == null || input == "")
            {
                return true;
            }
            Regex r = new Regex("^[a-zA-Z0-9 ]+$");
            if (r.IsMatch(input))
            {
                return true;
            }
            else
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Special characters not allowed");
                return false;
            }
        }

        private static bool IsDatewithYearValid(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = String.Empty;
            if (input == null || input == "")
            {
                return true;
            }
            DateTime dt;
            //return DateTime.TryParse(input, out dt);
            try
            {
                dt = DateTime.ParseExact(input, ParafaitDefaultViewContainerList.GetParafaitDefault(TranslateHelper.executioncontext, "DATE_FORMAT"), CultureInfo.InvariantCulture); // should be as per formats from parafait defaults container

                return true;
            }
            catch (Exception ex)
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Enter Valid Date Format") + ex;
                return false;
            }
        }

        private static bool IsDatewithoutyearValid(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = String.Empty;
            if (input == null || input == "")
            {
                return true;
            }
            DateTime dt;
            //return DateTime.TryParse(input, out dt);
            try
            {
                dt = DateTime.ParseExact(input, ParafaitDefaultViewContainerList.GetParafaitDefault(TranslateHelper.executioncontext, "DATE_FORMAT").Replace("-yyyy", "")
                    .Replace("-YY", "").Replace(" yyyy", "").Replace(" yy", "").Replace("/yyyy", "").Replace("/yy", ""), CultureInfo.InvariantCulture); // should be as per formats from parafait defaults container
                return true;
            }
            catch
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Enter Valid Date Format");
                return false;
            }
        }

        private static bool IsZipValid(string input, out string validaitionMessage)
        {
            log.LogMethodEntry(input);
            validaitionMessage = String.Empty;
            if (input == null || input == "")
            {

                return true;
            }
            if (input.Length != 5)
            {
                validaitionMessage = TranslateHelper.TranslateMessage("Enter valid zip code");
                return false;
            }
            int n;
            return int.TryParse(input, out n);
        }
        #endregion

    }
}
