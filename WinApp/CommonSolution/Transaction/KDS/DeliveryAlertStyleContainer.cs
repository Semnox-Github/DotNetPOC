/********************************************************************************************
 * Project Name - Transaction
 * Description  - Represents a combined KDS Terminal
 *
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks
 *********************************************************************************************
 *1.00        10-09-2019      lakshminarayana rao     Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Contains the collection of delivery alert styles
    /// </summary>
    public class DeliveryAlertStyleContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<DeliveryAlertStyle> deliveryAlertStyles;
        private readonly int minimumInterval;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="pipeSeparatedDeliveryAlertIntervals">pipe Separated Delivery Alert Intervals</param>
        /// <param name="pipeSeparatedDeliveryAlertColors">pipe Separated Delivery Alert Colors</param>
        public DeliveryAlertStyleContainer(string pipeSeparatedDeliveryAlertIntervals,
                                           string pipeSeparatedDeliveryAlertColors)
        {
            log.LogMethodEntry(pipeSeparatedDeliveryAlertIntervals, pipeSeparatedDeliveryAlertColors);
            List<int> intervals = GetIntervals(pipeSeparatedDeliveryAlertIntervals);
            minimumInterval = intervals.Min();
            List<Color> colors = GetColors(intervals, pipeSeparatedDeliveryAlertColors);
            deliveryAlertStyles = new List<DeliveryAlertStyle>();
            for (int i = 0; i < intervals.Count; i++)
            {
                Color color = colors.Count <= i ? colors[colors.Count - 1] : colors[i];
                DeliveryAlertStyle deliveryAlertStyle = new DeliveryAlertStyle(color, intervals[i]);
                deliveryAlertStyles.Add(deliveryAlertStyle);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public DeliveryAlertStyleContainer(ExecutionContext executionContext) :
            this(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "KDS_DELIVERY_ALERT_INTERVALS", "5|10"),
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "KDS_DELIVERY_ALERT_COLORS"))
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Delivery alert color based on the time elapsed. If the time elapsed is less than the minimum
        /// interval returns the default color. 
        /// </summary>
        /// <param name="timeElapsed">time Elapsed</param>
        /// <param name="defaultColor">default Color</param>
        /// <returns></returns>
        public Color GetDeliveryAlertColor(int timeElapsed, Color defaultColor)
        {
            log.LogMethodEntry(timeElapsed, defaultColor);
            Color result = deliveryAlertStyles.First().Color;
            if (timeElapsed < minimumInterval)
            {
                log.LogMethodExit(defaultColor, "Returning the defaultColor as the timeElapsed is less than minimumInterval");
                return defaultColor;
            }
            for (int i = deliveryAlertStyles.Count - 1; i >= 0; i--)
            {
                if (timeElapsed >= deliveryAlertStyles[i].Interval)
                {
                    result = deliveryAlertStyles[i].Color;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<int> GetIntervals(string pipeSeparatedDeliveryAlertIntervals)
        {
            log.LogMethodEntry(pipeSeparatedDeliveryAlertIntervals);
            string[] intervalStrings = string.IsNullOrWhiteSpace(pipeSeparatedDeliveryAlertIntervals) ? new[] { "5", "10" } :
                pipeSeparatedDeliveryAlertIntervals.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> intervals = new List<int>();
            foreach (string intervalString in intervalStrings)
            {
                int i;
                if (int.TryParse(intervalString.Trim(), out i))
                {
                    intervals.Add(i);
                }
            }

            if (intervals.Any() == false)
            {
                intervals.Add(5);
                intervals.Add(10);
            }

            List<int> distinctOrderedIntervals = intervals.Distinct().OrderBy(x => x).ToList();
            log.LogMethodExit(distinctOrderedIntervals);
            return distinctOrderedIntervals;
        }

        private List<Color> GetColors(List<int> intervals, string pipeSeparatedDeliveryAlertColors)
        {
            log.LogMethodEntry(intervals, pipeSeparatedDeliveryAlertColors);
            List<Color> result = GetColorsFromString(pipeSeparatedDeliveryAlertColors);
            if (result.Any() == false)
            {
                result = GetDefaultColors(intervals.Count);
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<Color> GetColorsFromString(string pipeSeparatedDeliveryAlertColors)
        {
            log.LogMethodEntry(pipeSeparatedDeliveryAlertColors);
            List<Color> result = new List<Color>();
            if (string.IsNullOrWhiteSpace(pipeSeparatedDeliveryAlertColors))
            {
                log.LogMethodExit(result);
                return result;
            }

            string[] colorStrings =
                pipeSeparatedDeliveryAlertColors.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string colorString in colorStrings)
            {
                if (IsValidColorString(colorString))
                {
                    result.Add(GetColorFrom(colorString));
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsValidColorString(string colorString)
        {
            log.LogMethodEntry(colorString);
            bool valid;
            string trimmedColorString = colorString.Replace("#", string.Empty).Trim();
            try
            {
                GetColorFrom(trimmedColorString);
                valid = true;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while validating color string", ex);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private static Color GetColorFrom(string colorString)
        {
            return ColorTranslator.FromHtml(colorString);
        }

        private List<Color> GetDefaultColors(int intervalsCount)
        {
            log.LogMethodEntry(intervalsCount);
            List<Color> result;
            if (intervalsCount == 1)
            {
                result = new List<Color>() { Color.Red };
            }
            else if (intervalsCount == 2)
            {
                result = new List<Color>() { Color.Yellow, Color.Red };
            }
            else if (intervalsCount == 3)
            {
                result = new List<Color>() { Color.Yellow, Color.Orange, Color.Red };
            }
            else
            {
                result = new List<Color>() { Color.Yellow, Color.Orange, Color.OrangeRed, Color.Red };
            }
            return result;
        }
    }
}
