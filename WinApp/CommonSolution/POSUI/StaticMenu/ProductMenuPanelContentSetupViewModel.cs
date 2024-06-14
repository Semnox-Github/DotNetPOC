/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu panel content setup
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.FileResources;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuPanelContentSetupViewModel : ViewModelBase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PanelMargin panelMargin;
        private string name = "Hellow";
        private ProductMenuPanelContentDTO productMenuPanelContentDTO;
        private ProductMenuPanelContentButtonType buttonType;
        private bool selected;
        private Brush defaultBackgroundColor;
        private FontFamily defaultFont;
        private Brush defaultTextColor;
        private double defaultFontSize;
        private BitmapImage contentImage;
        public ProductMenuPanelContentSetupViewModel(ExecutionContext executionContext, 
                                                     ProductMenuPanelContentDTO productMenuPanelContentDTO, 
                                                     PanelMargin panelMargin,
                                                     Brush defaultBackgroundColor,
                                                     FontFamily defaultFont,
                                                     Brush defaultTextColor,
                                                     double defaultFontSize)
        {
            log.LogMethodEntry(executionContext, productMenuPanelContentDTO, panelMargin, defaultBackgroundColor, defaultFont, defaultTextColor, defaultFontSize);
            this.ExecutionContext = executionContext;
            this.productMenuPanelContentDTO = productMenuPanelContentDTO;
            buttonType = ProductMenuPanelContentButtonType.FromString(productMenuPanelContentDTO.ButtonType);
            selected = false;
            ExecuteAction(SetObjectNameAndImage);
            this.panelMargin = panelMargin;
            this.defaultBackgroundColor = defaultBackgroundColor;
            this.defaultFont = defaultFont;
            this.defaultTextColor = defaultTextColor;
            this.defaultFontSize = defaultFontSize;
            log.LogMethodExit();
        }

        public ProductMenuPanelContentSetupViewModel(ExecutionContext executionContext, 
                                                     ProductMenuPanelContentButtonType buttonType, 
                                                     PanelMargin panelMargin,
                                                     Brush defaultBackgroundColor,
                                                     FontFamily defaultFont,
                                                     Brush defaultTextColor,
                                                     double defaultFontSize)
        {
            log.LogMethodEntry(executionContext, productMenuPanelContentDTO, panelMargin);
            this.ExecutionContext = executionContext;
            productMenuPanelContentDTO = new ProductMenuPanelContentDTO(-1, -1, string.Empty, string.Empty, string.Empty , string.Empty , string.Empty , string.Empty , - 1, -1, buttonType.ToButtonTypeString(), true);
            this.buttonType = buttonType;
            selected = false;
            this.panelMargin = panelMargin;
            this.defaultBackgroundColor = defaultBackgroundColor;
            this.defaultFont = defaultFont;
            this.defaultTextColor = defaultTextColor;
            this.defaultFontSize = defaultFontSize;
            ExecuteAction(SetObjectNameAndImage);
            log.LogMethodExit();
        }

        private async void SetObjectNameAndImage()
        {
            try
            {
                log.LogMethodEntry();
                Name = string.Empty;
                if (string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ObjectType) ||
                   string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ObjectGuid))
                {
                    log.LogMethodExit(null, "either ObjectType or ObjectGuid is empty");
                    return;
                }
                if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT)
                {
                    ProductsContainerDTO productsContainerDTO = ProductViewContainerList.GetProductsContainerDTOOrDefault(ExecutionContext, ManualProductType.SELLABLE.ToString(), productMenuPanelContentDTO.ObjectGuid);
                    if (productsContainerDTO == null)
                    {
                        productsContainerDTO = ProductViewContainerList.GetProductsContainerDTOOrDefault(ExecutionContext, ManualProductType.REDEEMABLE.ToString(), productMenuPanelContentDTO.ObjectGuid);
                    }
                    if (productsContainerDTO != null)
                    {
                        Name = productsContainerDTO.ProductName;
                        if(string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ImageURL) &&
                            string.IsNullOrWhiteSpace(productsContainerDTO.ImageFileName) == false) 
                        {
                            LoadContentImage(productsContainerDTO.ImageFileName);
                        }
                    }
                }
                if (productMenuPanelContentDTO.ObjectType == ProductMenuObjectTypes.PRODUCT_MENU_PANEL)
                {
                    IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                    List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.GetProductMenuPanelDTOList(isActive: "1", guid: productMenuPanelContentDTO.ObjectGuid);
                    if (productMenuPanelDTOList != null && productMenuPanelDTOList.Any())
                    {
                        Name = productMenuPanelDTOList[0].Name;
                        if (string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ImageURL) &&
                            string.IsNullOrWhiteSpace(productMenuPanelDTOList[0].ImageURL) == false)
                        {
                            LoadContentImage(productMenuPanelDTOList[0].ImageURL);
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while fetching the name", ex);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public BitmapImage ContentImage
        {
            get
            {
                if(contentImage == null && string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ImageURL) == false)
                {
                    LoadContentImage(productMenuPanelContentDTO.ImageURL);
                }
                return contentImage;
            }
            set
            {

            }
        }

        private async void LoadContentImage(string fileName)
        {
            try
            {
                FileResource fileResource = FileResourceFactory.GetFileResource(ExecutionContext, "IMAGE_DIRECTORY", fileName, false);
                using (Stream stream = await fileResource.Get())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                        contentImage = bitmapImage;
                    }
                    OnPropertyChanged("ContentImage");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading content image", ex);
            }
        }

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
                OnPropertyChanged("ButtonColor");
                OnPropertyChanged("ButtonBorderColor");
            }
        }

        public double Top
        {
            get
            {
                return productMenuPanelContentDTO.RowIndex * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_HEIGHT", 96) + (productMenuPanelContentDTO.RowIndex + 1) * (panelMargin.Top + panelMargin.Bottom);
            }
        }

        public ProductMenuPanelContentDTO ProductMenuPanelContentDTO
        {
            get
            {
                return productMenuPanelContentDTO;
            }
        }

        public double Left
        {
            get
            {
                return productMenuPanelContentDTO.ColumnIndex * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_WIDTH", 144) + (productMenuPanelContentDTO.ColumnIndex + 1) * (panelMargin.Left + panelMargin.Right);
            }
        }

        public double Width
        {
            get
            {
                return buttonType.HorizontalCellCount * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_WIDTH", 144) + (buttonType.HorizontalCellCount - 1) * (panelMargin.Left + panelMargin.Right);
            }
        }

        public double Height
        {
            get
            {
                return buttonType.VerticalCellCount * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_HEIGHT", 96) + (buttonType.VerticalCellCount - 1) * (panelMargin.Top + panelMargin.Bottom);
            }
        }

        public int RowIndex
        {
            get
            {
                return productMenuPanelContentDTO.RowIndex;
            }
            set
            {
                productMenuPanelContentDTO.RowIndex = value;
                OnPropertyChanged("RowIndex");
                OnPropertyChanged("Top");
            }

        }

        public string ImageURL
        {
            get
            {
                return productMenuPanelContentDTO.ImageURL;
            }
            set
            {
                productMenuPanelContentDTO.ImageURL = value;
                contentImage = null;
                OnPropertyChanged("ImageURL");
                OnPropertyChanged("ContentImage");
            }
        }

        public string TextColor
        {
            get
            {
                return productMenuPanelContentDTO.TextColor;
            }
            set
            {
                productMenuPanelContentDTO.TextColor = value;
                OnPropertyChanged("TextColor");
                OnPropertyChanged("ButtonTextColor");
            }
        }

        public string Font
        {
            get
            {
                return productMenuPanelContentDTO.Font;
            }
            set
            {
                productMenuPanelContentDTO.Font = value;
                OnPropertyChanged("Font");
                OnPropertyChanged("ButtonTextFont");
                OnPropertyChanged("ButtonTextFontSize");
                OnPropertyChanged("ButtonTextFontWeight");
                OnPropertyChanged("ButtonTextFontStyle");
            }
        }

        public string BackColor
        {
            get
            {
                return productMenuPanelContentDTO.BackColor;
            }
            set
            {
                productMenuPanelContentDTO.BackColor = value;
                OnPropertyChanged("BackColor");
                OnPropertyChanged("ButtonColor");
                OnPropertyChanged("ButtonBorderColor");
            }
        }

        public int ColumnIndex
        {
            get
            {
                return productMenuPanelContentDTO.ColumnIndex;
            }
            set
            {
                productMenuPanelContentDTO.ColumnIndex = value;
                OnPropertyChanged("ColumnIndex");
                OnPropertyChanged("Left");
            }
        }

        public PanelMargin PanelMargin
        {
            get
            {
                return panelMargin;
            }
            set
            {
                panelMargin = value;
                OnPropertyChanged("Height");
                OnPropertyChanged("Width");
                OnPropertyChanged("Top");
                OnPropertyChanged("Left");
            }
        }

        public bool IsContentAssigned
        {
            get
            {
                return string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ObjectGuid) == false &&
                    string.IsNullOrWhiteSpace(productMenuPanelContentDTO.ObjectType) == false;
            }
            
        }

        public ProductMenuPanelContentButtonType ButtonType
        {
            get
            {
                return buttonType;
            }
            set
            {
                buttonType = value;
                productMenuPanelContentDTO.ButtonType = buttonType.ButtonTypeString;
                OnPropertyChanged("ButtonType");
                OnPropertyChanged("Width");
                OnPropertyChanged("Height");
                OnPropertyChanged("ButtonColor");
            }
        }

        public Brush ButtonColor
        {
            get
            {
                if(string.IsNullOrWhiteSpace(BackColor))
                {
                    return defaultBackgroundColor;
                }
                else
                {
                    Brush brush = defaultBackgroundColor;
                    try
                    {
                        brush = new BrushConverter().ConvertFromString(BackColor) as SolidColorBrush;
                    }
                    catch (Exception)
                    {
                        string[] stringList = BackColor.Replace("argb(", string.Empty).Replace("rgb(", string.Empty).Replace(")", string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (stringList.Length != 3)
                        {
                            return defaultTextColor;
                        }
                        brush = new SolidColorBrush(new Color() { A = 255, R = Convert.ToByte(stringList[0]), G = Convert.ToByte(stringList[1]), B = Convert.ToByte(stringList[2]) });
                    }
                    return brush;
                }
            }
        }

        public Brush ButtonBorderColor
        {
            get
            {
                Brush result = (Brush)Application.Current.Resources["Control.BorderBrush"]; 
                Brush brush = ButtonColor;
                if(brush is SolidColorBrush)
                {
                    SolidColorBrush solidColorBrush = brush as SolidColorBrush;
                    Color color = solidColorBrush.Color;
                    float brightness = 0.7f;
                    Color darkColor = Color.FromArgb(color.A,
                                                    (byte)(color.R * brightness),
                                                    (byte)(color.G * brightness),
                                                    (byte)(color.B * brightness));
                    result = new SolidColorBrush(darkColor);
                }
                return result;
            }
        }

        public Brush ButtonTextColor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TextColor))
                {
                    return defaultTextColor;
                }
                else
                {
                    Brush brush = defaultTextColor;
                    try
                    {
                        brush = new BrushConverter().ConvertFromString(TextColor) as SolidColorBrush;
                    }
                    catch (Exception)
                    {
                        string[] stringList = TextColor.Replace("argb(", string.Empty).Replace("rgb(", string.Empty).Replace(")", string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (stringList.Length != 3)
                        {
                            return defaultTextColor;
                        }
                        brush = new SolidColorBrush(new Color() { A = 255, R = Convert.ToByte(stringList[0]), G = Convert.ToByte(stringList[1]), B = Convert.ToByte(stringList[2]) });
                    }
                    return brush;
                }
            }
        }

        public FontFamily ButtonTextFont
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Font))
                {
                    return defaultFont;
                }
                else
                {
                    FontFamily fontFamily = defaultFont;
                    try
                    {
                        FontValueObject fontValueObject = new FontValueObject(Font);
                        fontFamily = fontValueObject.FontFamily;
                    }
                    catch (Exception)
                    {
                        fontFamily = defaultFont;
                    }
                    return fontFamily;
                }
            }
        }

        public double ButtonTextFontSize
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Font))
                {
                    return defaultFontSize;
                }
                else
                {
                    double fontSize = defaultFontSize;
                    try
                    {
                        FontValueObject fontValueObject = new FontValueObject(Font);
                        fontSize = (double)fontValueObject.FontSize;
                    }
                    catch (Exception)
                    {
                        fontSize = defaultFontSize;
                    }
                    return fontSize;
                }
            }
        }

        public FontWeight ButtonTextFontWeight
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Font))
                {
                    return FontWeights.Bold;
                }
                else
                {
                    FontWeight fontWeight = FontWeights.Bold;
                    try
                    {
                        FontValueObject fontValueObject = new FontValueObject(Font);
                        fontWeight = fontValueObject.FontWeight;
                    }
                    catch (Exception)
                    {
                        fontWeight = FontWeights.Bold;
                    }
                    return fontWeight;
                }
            }
        }

        public FontStyle ButtonTextFontStyle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Font))
                {
                    return FontStyles.Normal;
                }
                else
                {
                    FontStyle fontStyle = FontStyles.Normal;
                    try
                    {
                        FontValueObject fontValueObject = new FontValueObject(Font);
                        fontStyle = fontValueObject.FontStyle;
                    }
                    catch (Exception)
                    {
                        fontStyle = FontStyles.Normal;
                    }
                    return fontStyle;
                }
            }
        }

        public string ObjectType
        {
            get
            {
                return productMenuPanelContentDTO.ObjectType;
            }
            set
            {
                productMenuPanelContentDTO.ObjectType = value;
                ExecuteAction(SetObjectNameAndImage);
                OnPropertyChanged("Name");
                OnPropertyChanged("ImageURL");
            }
        }

        public string ObjectGuid
        {
            get
            {
                return productMenuPanelContentDTO.ObjectGuid;
            }
            set
            {
                productMenuPanelContentDTO.ObjectGuid = value;
                ExecuteAction(SetObjectNameAndImage);
                OnPropertyChanged("Name");
                OnPropertyChanged("ImageURL");
            }
        }

        public override string ToString()
        {
            return "R:" + productMenuPanelContentDTO.RowIndex + "C:" + productMenuPanelContentDTO.ColumnIndex + "("+ name+")";
        }
    }

    
}
