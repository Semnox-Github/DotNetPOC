/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common -  model for generic display item
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/

using System.Windows;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public enum ItemType
    {
        ImageWithText = 0,
        Text = 1
    }

    public class DisplayItemDetails
    {
        #region Members
        private int stock;
        private ItemType objectType;
        private int itemid;
        private int inventoryid;
        private ObservableCollection<string> displayItemDetail;
        private ObservableCollection<string> displayInventoryDetails;
        private FontWeight fontWeight;
        private double fontSize;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Members

        #region Properties

        public ItemType ObjectType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(objectType);
                return objectType;
            }
            set
            {
                log.LogMethodEntry(value);
                objectType = value;
            }
        }

        public int ItemId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemid);
                return itemid;
            }
            set
            {
                log.LogMethodEntry(value);
                itemid = value;
            }
        }

        public int InventoryId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(inventoryid);
                return inventoryid;
            }
            set
            {
                log.LogMethodEntry(value);
                inventoryid = value;
            }
        }

        public int Stock
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(stock);
                return stock;
            }
            set
            {
                log.LogMethodEntry(value);
                stock = value;
            }
        }

        public ObservableCollection<string> DisplayItemDetail
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayItemDetail);
                return displayItemDetail;
            }
            set
            {
                log.LogMethodEntry(value);
                displayItemDetail = value;
            }
        }

        public ObservableCollection<string> DisplayInventoryDetails
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayInventoryDetails);
                return displayInventoryDetails;
            }
            set
            {
                log.LogMethodEntry(value);
                displayInventoryDetails = value;
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontWeight);
                return fontWeight;
            }
            set
            {
                log.LogMethodEntry(value);
                fontWeight = value;
            }
        }

        public double FontSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontSize);
                return fontSize;
            }
            set
            {
                log.LogMethodEntry(value);
                fontSize = value;
            }
        }


        #endregion Properties

    }
}
