/********************************************************************************************
 * Project Name - Product
 * Description  - Represents the different types of static menu content button types
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelContentButtonType
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string value;
        private readonly int horizontalCellCount;
        private readonly int verticalCellCount;
        public static readonly ProductMenuPanelContentButtonType NORMAL = new ProductMenuPanelContentButtonType("N", 2, 1);
        public static readonly ProductMenuPanelContentButtonType SMALL = new ProductMenuPanelContentButtonType("S", 1, 1);
        public static readonly ProductMenuPanelContentButtonType LARGE = new ProductMenuPanelContentButtonType("L", 2, 2);

        private ProductMenuPanelContentButtonType(string value, int horizontalCellCount, int verticalCellCount)
        {
            log.LogMethodEntry(value, horizontalCellCount, verticalCellCount);
            this.horizontalCellCount = horizontalCellCount;
            this.verticalCellCount = verticalCellCount;
            this.value = value;
            log.LogMethodExit();
        }

        public int VerticalCellCount
        {
            get
            {
                return verticalCellCount;
            }
        }

        public int HorizontalCellCount
        {
            get
            {
                return horizontalCellCount;
            }
        }

        public string ButtonTypeString
        {
            get
            {
                return value;
            }
        }

        public string ToButtonTypeString()
        {
            return value;
        }

        public static ProductMenuPanelContentButtonType FromString(string buttonType)
        {
            log.LogMethodEntry(buttonType);
            switch (buttonType)
            {
                case "N":
                    {
                        log.LogMethodExit(NORMAL);
                        return NORMAL;
                    }
                case "S":
                    {
                        log.LogMethodExit(SMALL);
                        return SMALL;
                    }
                case "L":
                    {
                        log.LogMethodExit(LARGE);
                        return LARGE;
                    }
                default:
                    {
                        string errorMessage = buttonType + " button type not found";
                        log.LogMethodExit("Throwing Exception - " + errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }
            }
        }

        public ProductMenuPanelContentButtonType GetSmallerButtonType()
        {
            log.LogMethodEntry();
            switch (value)
            {
                case "N":
                    {
                        log.LogMethodExit(SMALL);
                        return SMALL;
                    }
                case "S":
                    {
                        string errorMessage = "Smallest button type";
                        log.LogMethodExit("Throwing Exception - " + errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }
                case "L":
                    {
                        log.LogMethodExit(NORMAL);
                        return NORMAL;
                    }
                default:
                    {
                        string errorMessage = "Button type not found";
                        log.LogMethodExit("Throwing Exception - " + errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }
            }
        }

        public override string ToString()
        {
            return value;
        }
    }
}
