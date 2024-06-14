using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.ViewContainer
{
    public class TagNumberView : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log =
        new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string value;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        public TagNumberView(ExecutionContext executionContext, string tagNumberString)
            : this(executionContext, tagNumberString, new TagNumberViewLengthList(executionContext))
        {
            log.LogMethodEntry(executionContext, tagNumberString);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <param name="tagNumberViewLengthList"></param>
        public TagNumberView(ExecutionContext executionContext, string tagNumberString,
                         TagNumberViewLengthList tagNumberViewLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberString, tagNumberViewLengthList);

            string validationMessage = Validate(executionContext, tagNumberString, tagNumberViewLengthList);
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
                new TagNumberViewLengthList(executionContext));
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
            TagNumberViewLengthList tagNumberLengthList)
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
                new TagNumberViewLengthList(executionContext));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates the tag number string and returns the validation error 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberString"></param>
        /// <param name="tagNumberViewLengthList"></param>
        /// <returns></returns>
        public static string Validate(ExecutionContext executionContext, string tagNumberString,
            TagNumberViewLengthList tagNumberViewLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberString, tagNumberViewLengthList);
            string message;
            if (string.IsNullOrWhiteSpace(tagNumberString))
            {
                message = MessageViewContainerList.GetMessage(executionContext, 2204);
                log.LogMethodExit(message, "Tag number string is empty.");
                log.LogMethodExit(message);
                return message;
            }

            if (tagNumberViewLengthList.Contains(tagNumberString.Length) == false)
            {
                message = MessageViewContainerList.GetMessage(executionContext, 195, tagNumberString.Length,
                    "(" + tagNumberViewLengthList + ")");
                log.LogMethodExit(message, "Tag number string length doesn't match any valid tag number length.");
                return message;
            }
            log.LogMethodExit();
            return string.Empty;
        }
    }
}
