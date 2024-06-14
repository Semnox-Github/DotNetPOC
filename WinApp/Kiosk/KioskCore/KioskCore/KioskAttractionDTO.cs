/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - Custom DTO for Attractions
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.4.0    16-Jun-2023   Sathyavathi             Created for Attraction Sale in Kiosk
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Linq;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.KioskCore
{
    public partial class KioskAttractionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private int quantity;
        private AttractionBookingDTO attractionBookingDTO; //Used in Single attrcation.It is null for Combo attraction.
        private List<KioskAttractionChildDTO> childAttractionBookingDTOList; //Used in Combo. It is null for single attraction.
        private List<Card> cardList; 

        public int ProductId { get { return productId; } }
        public int Quantity { get { return quantity; } }
        public AttractionBookingDTO AttractionBookingDTO { set { attractionBookingDTO = value; } get { return attractionBookingDTO; } }
        public List<KioskAttractionChildDTO> ChildAttractionBookingDTOList { get { return childAttractionBookingDTOList; } }
        public List<Card> CardList { get { return cardList; } set { cardList = value; } }
        public int GetAttractionChildIndex(int comboProductId) { return GetATRChildIndex(comboProductId); }
        public bool SecondOrOtherATRChildHasSelectedSlot() { return CheckForSelectedSlotOnSecondAndOtherATRChildren(); } 
        public List<AttractionBooking> GetExistingAttractionBookings { get { return GetAttractionBookings(); } }
        /// <summary>
        /// KioskAttractionDTO
        /// </summary> 
        public KioskAttractionDTO(int productId, int quantity)
        {
            log.LogMethodEntry(productId, quantity);
            LoadData(productId, quantity);
            log.LogMethodExit();
        }

        /// <summary>
        /// Clone Construtor
        /// </summary>
        /// <param name="kioskAttractionDTO"></param>
        public KioskAttractionDTO(KioskAttractionDTO kioskAttractionDTO)
        {
            log.LogMethodEntry(kioskAttractionDTO);
            if (kioskAttractionDTO != null)
            {
                this.productId = kioskAttractionDTO.ProductId;
                this.quantity = kioskAttractionDTO.Quantity;
                if (kioskAttractionDTO.AttractionBookingDTO != null)
                {
                    this.attractionBookingDTO = new AttractionBookingDTO(kioskAttractionDTO.AttractionBookingDTO);
                }
                if (kioskAttractionDTO.cardList != null && kioskAttractionDTO.cardList.Any())
                {
                    this.cardList = new List<Card>(kioskAttractionDTO.cardList);
                }
                if (kioskAttractionDTO.childAttractionBookingDTOList != null && kioskAttractionDTO.childAttractionBookingDTOList.Any())
                {
                    this.childAttractionBookingDTOList = new List<KioskAttractionChildDTO>();
                    foreach (KioskAttractionChildDTO item in kioskAttractionDTO.childAttractionBookingDTOList)
                    {
                        KioskAttractionChildDTO cloneItem = new KioskAttractionChildDTO(item);
                        this.childAttractionBookingDTOList.Add(cloneItem);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void LoadData(int productId, int quantity)
        {
            try
            {
                this.productId = productId;
                this.quantity = quantity;
                BuildKioskAttractionChildDTOs();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in KioskAttrcationDTO(): " + ex);
                throw;
            }
        }

        private void BuildKioskAttractionChildDTOs()
        {
            log.LogMethodEntry();
            try
            {
                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);
                if (productsContainerDTO.ComboProductContainerDTOList != null && productsContainerDTO.ComboProductContainerDTOList.Any())
                {
                    if (childAttractionBookingDTOList == null)
                    {
                        childAttractionBookingDTOList = new List<KioskAttractionChildDTO>();
                    }
                    int i = 1;
                    foreach (ComboProductContainerDTO comboChildProduct in productsContainerDTO.ComboProductContainerDTOList)
                    {
                        KioskAttractionChildDTO kioskAttractionChildDTO = new KioskAttractionChildDTO(comboChildProduct);
                        if (kioskAttractionChildDTO.ChildProductType == ProductTypeValues.ATTRACTION)
                        {
                            kioskAttractionChildDTO.AttractionChildIndex = i;
                            i++;
                        }
                        childAttractionBookingDTOList.Add(kioskAttractionChildDTO);
                    }
                }
                else
                {
                    if (productsContainerDTO.ProductType == ProductTypeValues.ATTRACTION)
                    {
                        this.attractionBookingDTO = new AttractionBookingDTO();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in BuildKioskAttractionChildDTOs of PurchaseProductDTO(): " + ex);
                throw;
            }
            log.LogMethodExit();
        }

        private int GetATRChildIndex(int comboProductId)
        {
            log.LogMethodEntry(comboProductId);
            int atrIndex = -1;
            if (this.childAttractionBookingDTOList != null && this.childAttractionBookingDTOList.Any())
            {
                KioskAttractionChildDTO kioskAttractionChildDTO = this.childAttractionBookingDTOList.Find(cp => cp.ComboProductId == comboProductId);
                if (kioskAttractionChildDTO != null)
                {
                    atrIndex = kioskAttractionChildDTO.AttractionChildIndex;
                }
            }
            else if (this.AttractionBookingDTO != null)
            {
                atrIndex = 1;
            }
            log.LogMethodExit(atrIndex);
            return atrIndex;
        }

        private bool CheckForSelectedSlotOnSecondAndOtherATRChildren()
        {
            log.LogMethodEntry();
            bool retValue = false;
            if (this.childAttractionBookingDTOList != null && this.childAttractionBookingDTOList.Any())
            {
                for (int i = 0; i < this.childAttractionBookingDTOList.Count; i++)
                {
                    if (i == 0) { continue; }
                    if (this.childAttractionBookingDTOList[i].ChildAttractionBookingDTO != null 
                        && this.childAttractionBookingDTOList[i].ChildAttractionBookingDTO.BookingId > -1)
                    {
                        retValue = true;
                        break;
                    }
                }
            } 
            log.LogMethodExit(retValue);
            return retValue;
        }         

        private List<AttractionBooking> GetAttractionBookings()
        {
            log.LogMethodEntry();
            List<AttractionBooking> attractionBookings = new List<AttractionBooking>();
            if (this.attractionBookingDTO != null)
            {
                AttractionBooking attractionBooking = new AttractionBooking(KioskStatic.Utilities.ExecutionContext, attractionBookingDTO);
                attractionBookings.Add(attractionBooking);
            }
            else if (this.childAttractionBookingDTOList != null && this.childAttractionBookingDTOList.Any())
            {
                foreach (KioskAttractionChildDTO childDTO in childAttractionBookingDTOList)
                {
                    AttractionBooking attractionBooking = new AttractionBooking(KioskStatic.Utilities.ExecutionContext, childDTO.ChildAttractionBookingDTO);
                    attractionBookings.Add(attractionBooking);
                }
            }
            log.LogMethodExit(attractionBookings);
            return attractionBookings;
        }
    }

    public partial class KioskAttractionChildDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ComboProductContainerDTO comboProductContainer;
        private AttractionBookingDTO childAttractionBookingDTO;
        private List<Card> cardList;
        private int attractionChildIndex;

        public int ComboProductId { get { return comboProductContainer.ComboProductId; } }
        public int ChildProductQuantity { get { return Convert.ToInt32(comboProductContainer.Quantity); } }
        public int ChildProductId { get { return comboProductContainer.ChildProductId; } }
        public string ChildProductName { get { return comboProductContainer.ChildProductName; } }
        public string ChildProductType { get { return comboProductContainer.ChildProductType; } }
        public AttractionBookingDTO ChildAttractionBookingDTO { set { childAttractionBookingDTO = value; } get { return childAttractionBookingDTO; } }
        public List<Card> CardList { get { return cardList; } set { cardList = value; } }
        public int AttractionChildIndex { get { return attractionChildIndex; } set { attractionChildIndex = value; } }
        public ComboProductContainerDTO ComboProductContainer { get { return comboProductContainer; } }

        public KioskAttractionChildDTO(ComboProductContainerDTO comboProductContainer)
        {
            log.LogMethodEntry(comboProductContainer);
            LoadData(comboProductContainer);
            log.LogMethodExit();
        }

        private void LoadData(ComboProductContainerDTO comboProductContainer)
        {
            log.LogMethodEntry(comboProductContainer);
            try
            {
                this.comboProductContainer = comboProductContainer;
                this.attractionChildIndex = -1;
                if (this.comboProductContainer.ChildProductType == ProductTypeValues.ATTRACTION)
                {
                    this.childAttractionBookingDTO = new AttractionBookingDTO();
                }
                this.cardList = new List<Card>();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in KioskAttractionChildDTO(): " + ex);
                throw;
            }
            log.LogMethodExit();
        }

        public KioskAttractionChildDTO(KioskAttractionChildDTO kioskAttractionChildDTO)
        {
            log.LogMethodEntry(kioskAttractionChildDTO);
            if (kioskAttractionChildDTO != null)
            {
                LoadData(kioskAttractionChildDTO.ComboProductContainer);
                if (kioskAttractionChildDTO.childAttractionBookingDTO != null)
                {
                    this.childAttractionBookingDTO = new AttractionBookingDTO(kioskAttractionChildDTO.childAttractionBookingDTO);
                }
                if (kioskAttractionChildDTO.cardList != null)
                {
                    this.cardList = new List<Card>(kioskAttractionChildDTO.cardList);
                }
                this.attractionChildIndex = kioskAttractionChildDTO.attractionChildIndex;
            }
            log.LogMethodExit();
        }
    }
}
