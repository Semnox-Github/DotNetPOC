/********************************************************************************************
* Project Name - POS Redesign
* Description  - Common - Change password view
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     15-Jul-2021   Raja Uthanda          Created for POS UI Redesign 
********************************************************************************************/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Semnox.Core.Utilities;

namespace Semnox.Parafait.CommonUI
{
    public class ColorPickerVM : ViewModelBase
    {
        #region Members
        private bool showOpacity;
        private bool isMouseDown;
        
        private byte red;
        private byte green;
        private byte blue;
        private byte opacity;

        private string hexAlpha;
        private string hexRed;
        private string hexGreen;
        private string hexBlue;

        private SolidColorBrush selectedBrush;
        private ColorPickerView colorPickerView;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ButtonClickType clickType;

        private ICommand actionsCommand;
        private ICommand loadedCommand;
        #endregion

        #region Properties        
        public ButtonClickType ButtonClickType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clickType);
                return clickType;
            }
        }
        public bool ShowOpacity
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showOpacity);
                return showOpacity;
            }
            set
            {
                log.LogMethodEntry(showOpacity, value);
                SetProperty(ref showOpacity, value);
                log.LogMethodExit(showOpacity);
            }
        }
        public byte Opacity
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(opacity);
                return opacity;
            }
            set
            {
                log.LogMethodEntry(opacity, value);
                SetProperty(ref opacity, value);
                log.LogMethodExit(opacity);
            }
        }
        public byte Red
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(red);
                return red;
            }
            set
            {
                log.LogMethodEntry(red, value);
                SetProperty(ref red, value);
                log.LogMethodExit(red);
            }
        }
        public byte Green
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(green);
                return green;
            }
            set
            {
                log.LogMethodEntry(green, value);
                SetProperty(ref green, value);
                log.LogMethodExit(green);
            }
        }
        public byte Blue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(blue);
                return blue;
            }
            set
            {
                log.LogMethodEntry(blue, value);
                SetProperty(ref blue, value);
                log.LogMethodExit(blue);
            }
        }
        public string HexaAlpha
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hexAlpha);
                return hexAlpha;
            }
            private set
            {
                log.LogMethodEntry(hexAlpha, value);
                SetProperty(ref hexAlpha, value);
                log.LogMethodExit(hexAlpha);
            }
        }
        public string HexaRed
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hexRed);
                return hexRed;
            }
            private set
            {
                log.LogMethodEntry(hexRed, value);
                SetProperty(ref hexRed, value);
                log.LogMethodExit(hexRed);
            }
        }
        public string HexaGreen
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hexGreen);
                return hexGreen;
            }
            private set
            {
                log.LogMethodEntry(hexGreen, value);
                SetProperty(ref hexGreen, value);
                log.LogMethodExit(hexGreen);
            }
        }
        public string HexaBlue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hexBlue);
                return hexBlue;
            }
            private set
            {
                log.LogMethodEntry(hexBlue, value);
                SetProperty(ref hexBlue, value);
                log.LogMethodExit(hexBlue);
            }
        }
        public SolidColorBrush SelectedBrush
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedBrush);
                return selectedBrush;
            }
            private set
            {
                log.LogMethodEntry(selectedBrush, value);
                SetProperty(ref selectedBrush, value);
                log.LogMethodExit(selectedBrush);
            }
        }
        public ICommand ActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionsCommand);
                return actionsCommand;
            }
        }
        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
        }
        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                colorPickerView = parameter as ColorPickerView;
                if(colorPickerView != null)
                {
                    colorPickerView.canvasImage.MouseDown += OnCanvasMouseDown;
                    colorPickerView.canvasImage.MouseMove += OnCanvasMouseMove;
                    colorPickerView.canvasImage.MouseLeave += OnCanvasMousLeave;
                    colorPickerView.canvasImage.MouseUp += OnCanvasMouseUp;
                }
            }
            log.LogMethodExit();
        }
        private void OnCanvasMousLeave(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            isMouseDown = false;
            log.LogMethodExit();
        }
        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(isMouseDown)
            {
                try
                {
                    BitmapSource bitmapSource = colorPickerView.ColorImage.Source as BitmapSource;
                    if(bitmapSource != null)
                    { 
                        Point mousePosition = Mouse.GetPosition(colorPickerView.canvasImage);
                        int imageX = (int)(mousePosition.X / (colorPickerView.ColorImage.ActualWidth / bitmapSource.PixelWidth));
                        int imageY = (int)(mousePosition.Y / (colorPickerView.ColorImage.ActualHeight / bitmapSource.PixelHeight));
                        if (imageX < 0 || imageY < 0 || imageX > colorPickerView.ColorImage.ActualWidth - 1 || imageY > colorPickerView.ColorImage.ActualHeight - 1)
                        {
                            return;
                        }
                        CroppedBitmap croppedBitmap = new CroppedBitmap(bitmapSource, new Int32Rect(imageX, imageY, 1, 1));
                        if(croppedBitmap != null)
                        { 
                            byte[] pixels = new byte[4];
                            croppedBitmap.CopyPixels(pixels, 4, 0);
                            colorPickerView.ellipsePixel.SetValue(Canvas.LeftProperty, mousePosition.X - (colorPickerView.ellipsePixel.Width / 2.0));
                            colorPickerView.ellipsePixel.SetValue(Canvas.TopProperty, mousePosition.Y - (colorPickerView.ellipsePixel.Height / 2.0));
                            colorPickerView.canvasImage.InvalidateVisual();
                            Red = pixels[2];
                            Green = pixels[1];
                            Blue = pixels[0];
                        }
                    }
                }
                catch(Exception ex)
                {
                    log.LogMethodExit(ex.Message);
                }
            }
            log.LogMethodExit();
        }        
        private void UpdateHexaValues()
        {
            log.LogMethodEntry();
            HexaRed = red.ToString("X2");
            HexaBlue = blue.ToString("X2");
            HexaGreen = green.ToString("X2");
            HexaAlpha = opacity.ToString("X2");
            log.LogMethodExit();
        }
        private void UpdateColor()
        {
            log.LogMethodEntry();
            if (SelectedBrush == null || SelectedBrush.Color.A != opacity || SelectedBrush.Color.R != red || SelectedBrush.Color.G != green || SelectedBrush.Color.B != blue)
            {
                SelectedBrush = new SolidColorBrush(new Color() { A = opacity, R = red, G = green, B = blue });
                UpdateHexaValues();
            }
            log.LogMethodExit();
        }
        private void OnCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            isMouseDown = false;
            log.LogMethodExit();
        }
        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            isMouseDown = true;
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Button button = parameter as Button;
                if (button != null && !string.IsNullOrEmpty(button.Name))
                {
                    switch (button.Name)
                    {                       
                        case "btnCancel":
                            {
                                clickType = ButtonClickType.Cancel;
                            }
                            break;
                        case "btnConfirm":
                            {
                                clickType = ButtonClickType.Ok;
                            }
                            break;
                    }
                    PerformClose();
                }
            }
            log.LogMethodExit();
        }        
        private void PerformClose()
        {
            log.LogMethodEntry();
            if (colorPickerView != null)
            {
                colorPickerView.Close();
            }
            log.LogMethodExit();
        }
        private void InitalizeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            actionsCommand = new DelegateCommand(OnActionsClicked);
            PropertyChanged += OnPropertyChanged;
            log.LogMethodExit();
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(!string.IsNullOrEmpty(e.PropertyName))
            {
                switch(e.PropertyName.ToLower())
                {
                    case "opacity":
                    case "red":
                    case "green":
                    case "blue":
                        {
                            UpdateColor();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }

        private void SetValues(ExecutionContext executionContext, byte alpha, byte red, byte green, byte blue, bool showOpacity)
        {
            log.LogMethodEntry();

            InitalizeCommands();

            this.ExecutionContext = executionContext;

            this.showOpacity = showOpacity;
            this.opacity = alpha;
            this.red = red;
            this.green = green;
            this.blue = blue;
            
            UpdateColor();

            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public ColorPickerVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            SetValues(executionContext, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true);
            log.LogMethodExit();
        }
        
        public ColorPickerVM(ExecutionContext executionContext, byte alpha, byte red, byte green, byte blue, bool showOpacity)
        {
            log.LogMethodEntry(executionContext);
            SetValues(executionContext, alpha, red, green, blue, showOpacity);
            log.LogMethodExit();
        }

        #endregion
    }
}
