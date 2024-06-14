/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - Represents a Tag number
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By          Remarks          
 ********************************************************************************************* 
 *2.50.0      14-June-2019   Lakshminarayana      Created
 *2.70.2        12-Aug-2019    Deeksha              Added logger methods.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a tag number
    /// </summary>
    public class TagNumber : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string value;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        public TagNumber(ExecutionContext executionContext, string tagNumberString)
            : this(executionContext, tagNumberString, new TagNumberLengthList(executionContext))
        {
            log.LogMethodEntry(executionContext, tagNumberString);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <param name="tagNumberLengthList"></param>
        public TagNumber(ExecutionContext executionContext, string tagNumberString,
                         TagNumberLengthList tagNumberLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberString, tagNumberLengthList);

            string validationMessage = Validate(executionContext, tagNumberString, tagNumberLengthList);
            if (string.IsNullOrWhiteSpace(validationMessage) == false)
            {
                log.LogMethodExit(null, "Throwing Exception -" + validationMessage);
                throw new ArgumentException(validationMessage);
            }

            value = tagNumberString;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the tag number string representation
        /// </summary>
        public string Value
        {
            get { return value; }
        }

        /// <summary>
        /// Returns the string representation of the tag number
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            log.LogMethodExit(value);
            return value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            log.LogMethodEntry();
            log.LogMethodExit(value);
            yield return value;
        }

        /// <summary>
        /// Returns whether the given tagNumber string is valid
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <returns></returns>
        public static bool IsValid(ExecutionContext executionContext, string tagNumberString)
        {
            log.LogMethodEntry(executionContext, tagNumberString);
            bool result = IsValid(executionContext, tagNumberString,
                new TagNumberLengthList(executionContext));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns whether the given tagNumber string is valid
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <param name="tagNumberLengthList"></param>
        /// <returns></returns>
        public static bool IsValid(ExecutionContext executionContext, string tagNumberString,
            TagNumberLengthList tagNumberLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberString, tagNumberLengthList);
            string validationMessage = Validate(executionContext, tagNumberString, tagNumberLengthList);
            bool result = string.IsNullOrWhiteSpace(validationMessage);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates the tag number string and returns the validation error 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <returns></returns>
        public static string Validate(ExecutionContext executionContext, string tagNumberString)
        {
            log.LogMethodEntry(executionContext, tagNumberString);
            string result = Validate(executionContext, tagNumberString,
                new TagNumberLengthList(executionContext));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates the tag number string and returns the validation error 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <param name="tagNumberLengthList"></param>
        /// <returns></returns>
        public static string Validate(ExecutionContext executionContext, string tagNumberString,
            TagNumberLengthList tagNumberLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberString, tagNumberLengthList);
            string message;
            if (string.IsNullOrWhiteSpace(tagNumberString))
            {
                message = MessageContainerList.GetMessage(executionContext, 2204);
                log.LogMethodExit(message, "Tag number string is empty.");
                log.LogMethodExit(message);
                return message;
            }

            if (tagNumberLengthList.Contains(tagNumberString.Length) == false)
            {
                message = MessageContainerList.GetMessage(executionContext, 195, tagNumberString.Length,
                    "(" + tagNumberLengthList + ")");
                log.LogMethodExit(message, "Tag number string length doesn't match any valid tag number length.");
                return message;
            }
            log.LogMethodExit();
            return string.Empty;
        }
    }
}