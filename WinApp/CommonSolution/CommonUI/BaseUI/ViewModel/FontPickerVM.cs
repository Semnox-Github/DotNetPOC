/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Change password view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     20-Jul-2021   Raja Uthanda          Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Semnox.Core.Utilities;

namespace Semnox.Parafait.CommonUI
{
    public class FontPickerVM : ViewModelBase
    {
        #region Members
        private int fontSize;
        private int defaultFontSize;
        private CustomFamilyTypeFace selectedStyle;
        private string selectedSize;

        private ObservableCollection<FontFamily> fontCollection;
        private ObservableCollection<CustomFamilyTypeFace> styleCollection;
        private ObservableCollection<string> sizeCollection;

        private FontFamily fontFamily;
        private FamilyTypeface fontStyle;
        private FontPickerView fontPickerView;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ButtonClickType clickType;
        private ICommand actionsCommand;
        private ICommand loadedCommand;
        #endregion

        #region Properties        
        public List<string> StyleDisplayProperties
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return new List<string>() { "Style", "Stretch", "Weight" };
            }
        }
        public ButtonClickType ButtonClickType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clickType);
                return clickType;
            }
        }
        public FontFamily FontFamily
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontFamily);
                return fontFamily;
            }
            private set
            {
                log.LogMethodEntry(fontFamily, value);
                SetProperty(ref fontFamily, value);
                log.LogMethodExit(fontFamily);
            }
        }
        public FamilyTypeface FontStyle
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontStyle);
                return fontStyle;
            }
            private set
            {
                log.LogMethodEntry(fontStyle, value);
                SetProperty(ref fontStyle, value);
                log.LogMethodExit(fontStyle);
            }
        }
        public int FontSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontSize);
                return fontSize;
            }
            set
            {
                log.LogMethodEntry(fontSize, value);
                SetProperty(ref fontSize, value);
                log.LogMethodExit(fontSize);
            }
        }
        public CustomFamilyTypeFace SelectedStyle
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedStyle);
                return selectedStyle;
            }
            set
            {
                log.LogMethodEntry(selectedStyle, value);
                SetProperty(ref selectedStyle, value);
                log.LogMethodExit(selectedStyle);
            }
        }
        public string SelectedSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedSize);
                return selectedSize;
            }
            set
            {
                log.LogMethodEntry(selectedSize, value);
                SetProperty(ref selectedSize, value);
                log.LogMethodExit(selectedSize);
            }
        }
        public ObservableCollection<FontFamily> FontCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fontCollection);
                return fontCollection;
            }
        }
        public ObservableCollection<CustomFamilyTypeFace> FontStyleCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(styleCollection);
                return styleCollection;
            }
            private set
            {
                log.LogMethodEntry(styleCollection, value);
                SetProperty(ref styleCollection, value);
                log.LogMethodExit(styleCollection);
            }
        }
        public ObservableCollection<string> SizeCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(sizeCollection);
                return sizeCollection;
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

        #region Constructor
        public FontPickerVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            SetValues(executionContext, null, null, defaultFontSize, null, null);
            log.LogMethodExit();
        }

        public FontPickerVM(ExecutionContext executionContext, ObservableCollection<FontFamily> fontCollection, ObservableCollection<int> sizeCollection,
            FontFamily fontFamily, FamilyTypeface fontStyle, int fontSize)
        {
            log.LogMethodEntry(executionContext, fontCollection, sizeCollection, fontFamily, fontStyle, fontSize);
            SetValues(executionContext, fontFamily, fontStyle, fontSize, fontCollection, sizeCollection);
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void InitalizeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            actionsCommand = new DelegateCommand(OnActionsClicked);
            PropertyChanged += OnPropertyChanged;
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                fontPickerView = parameter as FontPickerView;
            }
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
            if (fontPickerView != null)
            {
                fontPickerView.Close();
            }
            log.LogMethodExit();
        }
        private void SetValues(ExecutionContext executionContext, FontFamily selectedFont, FamilyTypeface selectedStyle, int selectedSize,
            ObservableCollection<FontFamily> fontCollection, ObservableCollection<int> sizeCollection)
        {
            log.LogMethodEntry(executionContext, selectedFont, selectedStyle, selectedSize, fontCollection, sizeCollection);

            InitalizeCommands();

            ExecutionContext = executionContext;
            defaultFontSize = 12;

            SetFontFamilyValues(fontCollection, selectedFont, selectedStyle);
            SetFontSizeValues(sizeCollection, selectedSize);
            log.LogMethodExit();
        }
        private void SetFontFamilyValues(ObservableCollection<FontFamily> fontCollection, FontFamily selectedFont, FamilyTypeface selectedStyle)
        {
            log.LogMethodEntry(fontCollection);
            if (fontCollection == null)
            {
                fontCollection = new ObservableCollection<FontFamily>(Fonts.SystemFontFamilies.OrderBy(f => f.Source));
            }
            this.fontCollection = fontCollection;
            if (fontCollection != null)
            {
                UpdateFontFamily(selectedFont);
                UpdateFontStyle(selectedStyle);
            }
            log.LogMethodExit();
        }
        private void SetFontSizeValues(ObservableCollection<int> sizeCollection, int selectedSize)
        {
            log.LogMethodEntry(sizeCollection);
            ObservableCollection<string> fontSizeCollection = null;
            if (sizeCollection == null || !sizeCollection.Any())
            {
                fontSizeCollection = new ObservableCollection<string>() { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "32", "48", "72" };
            }
            else
            {
                fontSizeCollection = new ObservableCollection<string>(sizeCollection.Select(s => s.ToString()));
            }
            this.sizeCollection = fontSizeCollection;
            UpdateFontSize(selectedSize);
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(!string.IsNullOrEmpty(e.PropertyName))
            {
                switch(e.PropertyName)
                {
                    case "FontFamily":
                        {
                            UpdateStyleCollection();
                        }
                        break;
                    case "SelectedStyle":
                        {
                            UpdateSampleFontStyle();
                        }
                        break;
                    case "SelectedSize":
                        {
                            UpdateSampleFontSize();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void UpdateStyleCollection()
        {
            log.LogMethodEntry();
            if (fontFamily != null && fontFamily.FamilyTypefaces != null)
            {
                FontStyleCollection = new ObservableCollection<CustomFamilyTypeFace>(fontFamily.FamilyTypefaces.Select(s => new CustomFamilyTypeFace(s)));                
                if (styleCollection != null && styleCollection.Count > 0)
                {
                    UpdateFontStyle(fontFamily.FamilyTypefaces.FirstOrDefault());
                }
            }
            log.LogMethodExit();
        }
        private void UpdateSampleFontSize()
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(selectedSize) && selectedSize.All(c => char.IsDigit(c)))
            {
                FontSize = Convert.ToInt32(selectedSize);
            }
            log.LogMethodExit();
        }
        private void UpdateSampleFontStyle()
        {
            log.LogMethodEntry();
            if (FontStyleCollection != null && FontStyleCollection.Contains(selectedStyle))
            {
                int index = FontStyleCollection.IndexOf(selectedStyle);
                if (index > -1)
                {
                    FontStyle = fontFamily.FamilyTypefaces[index];
                }
            }
            log.LogMethodExit();
        }
        private void UpdateFontFamily(FontFamily selectedFont)
        {
            log.LogMethodEntry(selectedFont);
            if (selectedFont != null && fontCollection.Contains(selectedFont))
            {
                FontFamily = selectedFont;
            }
            else if (fontCollection.Count > 0)
            {
                FontFamily = fontCollection.FirstOrDefault();
            }
            log.LogMethodExit();
        }
        private void UpdateFontSize(int selectedSize)
        {
            log.LogMethodEntry(selectedSize);
            if(sizeCollection != null)
            { 
                if (sizeCollection.Contains(selectedSize.ToString()))
                {
                    SelectedSize = selectedSize.ToString();
                }
                else if (sizeCollection.Contains(defaultFontSize.ToString()))
                {
                    SelectedSize = defaultFontSize.ToString();
                }
                else
                {
                    FontSize = defaultFontSize;
                }
            }
            log.LogMethodExit();
        }
        private void UpdateFontStyle(FamilyTypeface selectedStyle)
        {
            log.LogMethodEntry(selectedStyle);
            if (fontFamily != null && styleCollection != null)
            {
                if (selectedStyle != null && styleCollection.Any( s => s.FamilyTypeface.Style.ToString() == selectedStyle.Style.ToString()
                && s.FamilyTypeface.Weight.ToString() == selectedStyle.Weight.ToString()))
                {
                    SelectedStyle = styleCollection.FirstOrDefault(s => s.FamilyTypeface.Style.ToString() == selectedStyle.Style.ToString()
                && s.FamilyTypeface.Weight.ToString() == selectedStyle.Weight.ToString());
                }
                else if (styleCollection.Count > 0)
                {
                    SelectedStyle = styleCollection.FirstOrDefault();
                }
            }
            log.LogMethodExit();
        }                
        private string GetStyle(FamilyTypeface familyTypeface)
        {
            log.LogMethodEntry(familyTypeface);
            string convertedStyle = string.Empty;
            if (familyTypeface != null)
            {
                if (familyTypeface.Style != null)
                {
                    convertedStyle = familyTypeface.Style.ToString();
                }
                if (familyTypeface.Weight != null)
                {
                    convertedStyle += " - " + familyTypeface.Weight.ToString();
                }
                if (familyTypeface.Stretch != null)
                {
                    convertedStyle += " - " + familyTypeface.Stretch.ToString();
                }
            }
            log.LogMethodExit();
            return convertedStyle;
        }
        #endregion
    }
}
