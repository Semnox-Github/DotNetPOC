using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.ViewContainer
{
    public class TagNumberViewLengthList : List<int>
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static HashSet<int> GetValidTagNumberLengthListFromString(string pipeSeparatedTagNumberLength)
        {
            log.LogMethodEntry(pipeSeparatedTagNumberLength);
            HashSet<int> result = new HashSet<int>();
            string[] tagNumberLengthStringList =
                pipeSeparatedTagNumberLength.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tagNumberLengthString in tagNumberLengthStringList)
            {
                int tagNumberLength;
                if (int.TryParse(tagNumberLengthString, out tagNumberLength) && tagNumberLength > 0)
                {
                    result.Add(tagNumberLength);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public TagNumberViewLengthList(ExecutionContext executionContext)
            : this(GetValidTagNumberLengthListFromString(ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "CARD_NUMBER_LENGTH")))
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="pipeSeparatedTagNumberLength"></param>
        public TagNumberViewLengthList(string pipeSeparatedTagNumberLength)
        : this(GetValidTagNumberLengthListFromString(pipeSeparatedTagNumberLength))
        {
            log.LogMethodEntry(pipeSeparatedTagNumberLength);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="validTagNumberLengthList"></param>
        public TagNumberViewLengthList(HashSet<int> validTagNumberLengthList)
        {
            log.LogMethodEntry(validTagNumberLengthList);
            if (validTagNumberLengthList == null ||
                validTagNumberLengthList.Count == 0 ||
                validTagNumberLengthList.Any(x => x > 0) == false)
            {
                Add(10);
                log.LogMethodExit(null, "No valid tag number length is defined. defaulting to 10.");
                return;
            }
            AddRange(validTagNumberLengthList.Where(x => x > 0).OrderByDescending(x => x));
            log.LogMethodExit();
        }

        public override string ToString()
        {
            return string.Join(", ", this.OrderBy(x => x));
        }

        public int GetNearestTagNumberLength(int scannedTagNumberLength)
        {
            log.LogMethodEntry(scannedTagNumberLength);
            if (scannedTagNumberLength < MinimumValidTagNumberLength)
            {
                string message =
                    "Invalid scannedTagNumberLength. scannedTagNumberLength should be greater than MinimumValidTagNumberLength(" +
                    MinimumValidTagNumberLength + ")";
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ArgumentException(message);
            }
            int result = this[0];
            foreach (int tagNumberLength in this)
            {
                if (tagNumberLength > scannedTagNumberLength) continue;
                result = tagNumberLength;
                break;
            }
            log.LogMethodExit(result);
            return result;
        }

        public int MinimumValidTagNumberLength
        {
            get { return this[Count - 1]; }
        }

        public int MaximumValidTagNumberLength
        {
            get { return this[0]; }
        }
    }
}
