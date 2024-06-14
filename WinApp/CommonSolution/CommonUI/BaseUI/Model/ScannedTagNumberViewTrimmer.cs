using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.CommonUI
{
    class ScannedTagNumberViewTrimmer
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int leftTrimValue;
        private readonly int rightTrimValue;
        private readonly TagNumberViewLengthList tagNumberViewLengthList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ScannedTagNumberViewTrimmer(ExecutionContext executionContext)
        {
            leftTrimValue = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "LEFT_TRIM_CARD_NUMBER", 0);
            rightTrimValue = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "RIGHT_TRIM_CARD_NUMBER", 0);
            tagNumberViewLengthList = new TagNumberViewLengthList(executionContext);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="leftTrimValue"></param>
        /// <param name="rightTrimValue"></param>
        /// <param name="tagNumberLengthList"></param>
        public ScannedTagNumberViewTrimmer(int leftTrimValue,
                                       int rightTrimValue,
                                       TagNumberViewLengthList tagNumberViewLengthList)
        {
            this.leftTrimValue = leftTrimValue;
            this.rightTrimValue = rightTrimValue;
            this.tagNumberViewLengthList = tagNumberViewLengthList;
        }

        /// <summary>
        /// Given tag number string can be trimmed to a valid tag number
        /// </summary>
        /// <param name="scannedTagNumberValue"></param>
        /// <returns></returns>
        public bool CanBeTrimmedToValidTagNumberString(string scannedTagNumberValue)
        {
            log.LogMethodEntry(scannedTagNumberValue, tagNumberViewLengthList);
            if (scannedTagNumberValue.Length < tagNumberViewLengthList.MinimumValidTagNumberLength)
            {
                log.LogMethodExit(false, "Scanned tag number length is less than the minimum valid tag number length");
                return false;
            }

            int nearestValidTagNumberLength = tagNumberViewLengthList.GetNearestTagNumberLength(scannedTagNumberValue.Length);
            if ((scannedTagNumberValue.Length - leftTrimValue - rightTrimValue) > nearestValidTagNumberLength)
            {
                log.LogMethodExit(false, "Scanned tag number can't be trimmed to a valid tag number");
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Trims the tag number based on the left trim and right trim configuration
        /// </summary>
        /// <param name="scannedTagNumber"></param>
        /// <returns></returns>
        public string Trim(string scannedTagNumber)
        {
            log.LogMethodEntry(scannedTagNumber);

            if (tagNumberViewLengthList.Contains(scannedTagNumber.Length))
            {
                log.LogMethodExit(scannedTagNumber);
                return scannedTagNumber;
            }

            int nearestValidTagNumberLength = tagNumberViewLengthList.GetNearestTagNumberLength(scannedTagNumber.Length);

            int intermediateLeftTrimValue = leftTrimValue;
            if (scannedTagNumber.Length - intermediateLeftTrimValue < nearestValidTagNumberLength)
            {
                intermediateLeftTrimValue = scannedTagNumber.Length - nearestValidTagNumberLength;
            }

            int intermediateRightTrimValue = rightTrimValue;
            if (scannedTagNumber.Length - intermediateLeftTrimValue == nearestValidTagNumberLength)
            {
                intermediateRightTrimValue = 0;
            }
            else if (scannedTagNumber.Length - intermediateLeftTrimValue - intermediateRightTrimValue <
                     nearestValidTagNumberLength)
            {
                intermediateRightTrimValue =
                    scannedTagNumber.Length - intermediateLeftTrimValue - nearestValidTagNumberLength;
            }

            string result = scannedTagNumber.Substring(intermediateLeftTrimValue,
                scannedTagNumber.Length - intermediateLeftTrimValue - intermediateRightTrimValue);
            log.LogMethodExit(result);
            return result;
        }
    }
}
