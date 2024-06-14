/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu panel content
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
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuPanelContentVM : ViewModelBase
    {
        #region Members 
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO;
        private readonly Brush defaultBackgroundColor;
        private readonly FontFamily defaultFont;
        private readonly Brush defaultTextColor;
        private readonly double defaultFontSize;
        private readonly PanelMargin panelMargin;
        private ProductMenuPanelContentButtonType buttonType;
        private BitmapImage contentImage;
        #endregion
        public ProductMenuPanelContentVM(ExecutionContext executionContext, 
                                         ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO,
                                         PanelMargin panelMargin,
                                         Brush defaultBackgroundColor,
                                         FontFamily defaultFont,
                                         Brush defaultTextColor,
                                         double defaultFontSize)
        {
            log.LogMethodEntry(executionContext, productMenuPanelContentContainerDTO);
            ExecutionContext = executionContext;
            this.productMenuPanelContentContainerDTO = productMenuPanelContentContainerDTO;
            buttonType = ProductMenuPanelContentButtonType.FromString(productMenuPanelContentContainerDTO.ButtonType);
            this.panelMargin = panelMargin;
            this.defaultBackgroundColor = defaultBackgroundColor;
            this.defaultFont = defaultFont;
            this.defaultTextColor = defaultTextColor;
            this.defaultFontSize = defaultFontSize;
            log.LogMethodExit();
        }

        public string Name
        {
            get
            {
                return productMenuPanelContentContainerDTO.Name;
            }
        }

        public ProductMenuPanelContentButtonType ButtonType
        {
            get
            {
                return buttonType;
            }
        }

        public BitmapImage ContentImage
        {
            get
            {
                if (contentImage == null && string.IsNullOrWhiteSpace(productMenuPanelContentContainerDTO.ImageURL) == false)
                {
                    LoadContentImage();
                }
                return contentImage;
            }
        }

        private async void LoadContentImage()
        {
            try
            {
                FileResource fileResource = FileResourceFactory.GetFileResource(ExecutionContext, "IMAGE_DIRECTORY", productMenuPanelContentContainerDTO.ImageURL, false);
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

        public double Top
        {
            get
            {
                return productMenuPanelContentContainerDTO.RowIndex * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_HEIGHT", 96) + (productMenuPanelContentContainerDTO.RowIndex + 1) * (panelMargin.Top + panelMargin.Bottom);
            }
        }

        public ProductMenuPanelContentContainerDTO ProductMenuPanelContentContainerDTO
        {
            get
            {
                return productMenuPanelContentContainerDTO;
            }
        }

        public double Left
        {
            get
            {
                return productMenuPanelContentContainerDTO.ColumnIndex * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_WIDTH", 144) + (productMenuPanelContentContainerDTO.ColumnIndex + 1) * (panelMargin.Left + panelMargin.Right);
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

        public Brush ButtonColor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(productMenuPanelContentContainerDTO.BackColor))
                {
                    return defaultBackgroundColor;
                }
                else
                {
                    Brush brush = defaultBackgroundColor;
                    try
                    {
                        brush = new BrushConverter().ConvertFromString(productMenuPanelContentContainerDTO.BackColor) as SolidColorBrush;
                    }
                    catch (Exception)
                    {
                        string[] stringList = productMenuPanelContentContainerDTO.BackColor.Replace("argb(", string.Empty).Replace("rgb(", string.Empty).Replace(")", string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
                if (brush is SolidColorBrush)
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
                if (string.IsNullOrWhiteSpace(productMenuPanelContentContainerDTO.TextColor))
                {
                    return defaultTextColor;
                }
                else
                {
                    Brush brush = defaultTextColor;
                    try
                    {
                        brush = new BrushConverter().ConvertFromString(productMenuPanelContentContainerDTO.TextColor) as SolidColorBrush;
                    }
                    catch (Exception)
                    {
                        string[] stringList = productMenuPanelContentContainerDTO.TextColor.Replace("argb(", string.Empty).Replace("rgb(", string.Empty).Replace(")", string.Empty).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
                if (string.IsNullOrWhiteSpace(productMenuPanelContentContainerDTO.Font))
                {
                    return defaultFont;
                }
                else
                {
                    FontFamily fontFamily = defaultFont;
                    try
                    {
                        fontFamily = new FontFamily(productMenuPanelContentContainerDTO.Font);
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
                return defaultFontSize;
            }
        }

        public FontWeight ButtonTextFontWeight
        {
            get
            {
                return FontWeights.Bold;
            }
        }

        public FontStyle ButtonTextFontStyle
        {
            get
            {
                return FontStyles.Normal;
            }
        }

        public override string ToString()
        {
            return "R:" + productMenuPanelContentContainerDTO.RowIndex + "C:" + productMenuPanelContentContainerDTO.ColumnIndex + "(" + Name + ")";
        }

    }
}
