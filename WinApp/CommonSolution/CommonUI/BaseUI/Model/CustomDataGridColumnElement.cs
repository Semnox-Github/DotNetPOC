using System;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public enum DataGridButtonType
    {
        Content = 0,
        More = 1,
        Add = 2,
        Remove = 3,
        Edit = 4,
        Custom = 5
    }
    public enum ArthemeticOperationType
    {
        Add,
        Subtract,
        Multiply
    }

    public enum ListOperationType
    {
        Sum,
        Count
    }
    public class CustomDataGridColumnElement : ViewModelBase, ICloneable
    {
        #region Members        
        private bool isReadOnly;
        private bool isEnable;
        private bool canUserSort;
        private int maxLength;
        private double dataGridColumnFixedSize;
        private string heading;
        private string styleName;
        private string displayMemberPath;
        private string sourcePropertyName;
        private string dataGridColumnStringFormat;
        private string childOrSecondarySourcePropertyName;
        private DataEntryType type;
        private DataGridButtonType buttonType;
        private TextSize contentButtonTextSize;
        private NumberKeyboardType numberKeyboardType;
        private TextTrimming contentButtonTextTrimming;
        private ListOperationType childPropertyListOperationType;
        private DataGridLengthUnitType dataGridColumnLengthUnitType;
        private HorizontalAlignment dataGridColumnhorizontalAlignment;
        private object converter;
        private object converterParameter;
        private FontWeight fontWeight;
        private Thickness contentButtonPadding;
        private ObservableCollection<object> options;
        private ObservableCollection<object> secondarySource;
        private Dictionary<string, ArthemeticOperationType> properties;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public NumberKeyboardType NumberKeyboardType
        {
            get { return numberKeyboardType; }
            set
            {
                SetProperty(ref numberKeyboardType, value);
            }
        }
        public bool CanUserSort
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(canUserSort);
                return canUserSort;
            }
            set
            {
                log.LogMethodEntry(canUserSort, value);
                SetProperty(ref canUserSort, value);
                log.LogMethodExit(canUserSort);
            }
        }
        public Thickness ContentButtonPadding
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(contentButtonPadding);
                return contentButtonPadding;
            }
            set
            {
                log.LogMethodEntry(contentButtonPadding, value);
                SetProperty(ref contentButtonPadding, value);
                log.LogMethodExit(contentButtonPadding);
            }
        }
        public TextTrimming ContentButtonTextTrimming
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(contentButtonTextTrimming);
                return contentButtonTextTrimming;
            }
            set
            {
                log.LogMethodEntry(contentButtonTextTrimming, value);
                SetProperty(ref contentButtonTextTrimming, value);
                log.LogMethodExit(contentButtonTextTrimming);
            }
        }
        public TextSize ContentButtonTextSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(contentButtonTextSize);
                return contentButtonTextSize;
            }
            set
            {
                log.LogMethodEntry(contentButtonTextSize, value);
                SetProperty(ref contentButtonTextSize, value);
                log.LogMethodExit(contentButtonTextSize);
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
                log.LogMethodEntry(fontWeight, value);
                SetProperty(ref fontWeight, value);
                log.LogMethodExit(fontWeight);
            }
        }
        public object Converter
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(converter);
                return converter;
            }
            set
            {
                log.LogMethodEntry(converter, value);
                SetProperty(ref converter, value);
                log.LogMethodExit(converter);
            }
        }
        public object ConverterParameter
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(converterParameter);
                return converterParameter;
            }
            set
            {
                log.LogMethodEntry(converterParameter, value);
                SetProperty(ref converterParameter, value);
                log.LogMethodExit(converterParameter);
            }
        }
        public ObservableCollection<object> SecondarySource
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(secondarySource);
                return secondarySource;
            }
            set
            {
                log.LogMethodEntry(secondarySource, value);
                SetProperty(ref secondarySource, value);
                log.LogMethodExit(secondarySource);
            }
        }
        public string ChildOrSecondarySourcePropertyName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(childOrSecondarySourcePropertyName);
                return childOrSecondarySourcePropertyName;
            }
            set
            {
                log.LogMethodEntry(childOrSecondarySourcePropertyName, value);
                SetProperty(ref childOrSecondarySourcePropertyName, value);
                log.LogMethodExit(childOrSecondarySourcePropertyName);
            }
        }
        public string SourcePropertyName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(sourcePropertyName);
                return sourcePropertyName;
            }
            set
            {
                log.LogMethodEntry(sourcePropertyName, value);
                SetProperty(ref sourcePropertyName, value);
                log.LogMethodExit(sourcePropertyName);
            }
        }
        public ListOperationType ChildPropertyListOperationType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(childPropertyListOperationType);
                return childPropertyListOperationType;
            }
            set
            {
                log.LogMethodEntry(childPropertyListOperationType, value);
                SetProperty(ref childPropertyListOperationType, value);
                log.LogMethodExit(childPropertyListOperationType);
            }
        }
        public Dictionary<string, ArthemeticOperationType> Properties
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(properties);
                return properties;
            }
            set
            {
                log.LogMethodEntry(properties, value);
                SetProperty(ref properties, value);
                log.LogMethodExit(properties);
            }
        }
        public HorizontalAlignment DataGridColumnHorizontalAlignment
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridColumnhorizontalAlignment);
                return dataGridColumnhorizontalAlignment;
            }
            set
            {
                log.LogMethodEntry(dataGridColumnhorizontalAlignment, value);
                SetProperty(ref dataGridColumnhorizontalAlignment, value);
                log.LogMethodEntry(dataGridColumnhorizontalAlignment);
            }
        }
        public string DataGridColumnStringFormat
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridColumnStringFormat);
                return dataGridColumnStringFormat;
            }
            set
            {
                log.LogMethodEntry(dataGridColumnStringFormat, value);
                SetProperty(ref dataGridColumnStringFormat, value);
                log.LogMethodEntry(dataGridColumnStringFormat);
            }
        }
        /// <summary>
        /// MaxLength
        /// </summary>
        public int MaxLength
        {
            get { return maxLength; }
            set { SetProperty(ref maxLength, value); }
        }
        public double DataGridColumnFixedSize
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridColumnFixedSize);
                return dataGridColumnFixedSize;
            }
            set
            {
                log.LogMethodEntry(dataGridColumnFixedSize, value);
                SetProperty(ref dataGridColumnFixedSize, value);
                if (dataGridColumnFixedSize > 1)
                {
                    DataGridColumnLengthUnitType = DataGridLengthUnitType.Pixel;
                }
                log.LogMethodEntry(dataGridColumnFixedSize);
            }
        }
        public DataGridLengthUnitType DataGridColumnLengthUnitType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridColumnLengthUnitType);
                return dataGridColumnLengthUnitType;
            }
            set
            {
                log.LogMethodEntry(dataGridColumnLengthUnitType, value);
                SetProperty(ref dataGridColumnLengthUnitType, value);
                log.LogMethodEntry(dataGridColumnLengthUnitType);
            }
        }
        public ObservableCollection<object> Options
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(options);
                return options;
            }
            set
            {
                log.LogMethodEntry(options, value);
                SetProperty(ref options, value);
                log.LogMethodExit(options);
            }
        }
        public string DisplayMemberPath
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayMemberPath);
                return displayMemberPath;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayMemberPath, value);
                log.LogMethodExit(displayMemberPath);
            }
        }
        public bool IsEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(isEnable);
                return isEnable;
            }
            set
            {
                log.LogMethodEntry(isEnable, value);
                SetProperty(ref isEnable, value);
                log.LogMethodExit(isEnable);
            }
        }
        public bool IsReadOnly
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(isReadOnly);
                return isReadOnly;
            }
            set
            {
                log.LogMethodEntry(isReadOnly, value);
                SetProperty(ref isReadOnly, value);
                log.LogMethodExit(isReadOnly);
            }
        }
        public DataGridButtonType ActionButtonType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(buttonType);
                return buttonType;
            }
            set
            {
                log.LogMethodEntry(buttonType, value);
                SetProperty(ref buttonType, value);
                log.LogMethodExit(buttonType);
            }
        }
        public DataEntryType Type
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(type);
                return type;
            }
            set
            {
                log.LogMethodEntry(type, value);
                SetProperty(ref type, value);
                log.LogMethodExit(type);
            }
        }
        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading);
                return heading;
            }
            set
            {
                log.LogMethodEntry(heading, value);
                SetProperty(ref heading, value);
                log.LogMethodExit();
            }
        }
        public string StyleName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(styleName);
                return styleName;
            }
            set
            {
                log.LogMethodEntry(styleName, value);
                SetProperty(ref styleName, value);
                log.LogMethodExit();
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        public CustomDataGridColumnElement()
        {
            log.LogMethodEntry();

            isEnable = true;
            isReadOnly = false;
            canUserSort = true;
            maxLength = int.MaxValue;
            dataGridColumnFixedSize = 1;
            heading = string.Empty;
            styleName = string.Empty;
            displayMemberPath = string.Empty;
            dataGridColumnStringFormat = string.Empty;

            type = DataEntryType.TextBlock;
            contentButtonTextSize = TextSize.Small;
            buttonType = DataGridButtonType.Content;
            contentButtonTextTrimming = TextTrimming.None;
            numberKeyboardType = NumberKeyboardType.Positive;
            dataGridColumnLengthUnitType = DataGridLengthUnitType.Auto;
            dataGridColumnhorizontalAlignment = HorizontalAlignment.Left;

            fontWeight = FontWeights.Bold;
            contentButtonPadding = new Thickness(32, 0, 32, 0);

            options = new ObservableCollection<object>();

            log.LogMethodExit();
        }

        public CustomDataGridColumnElement(bool isEnable = true, bool isReadOnly = false, bool canUserSort = true, HorizontalAlignment dataGridColumnhorizontalAlignment = HorizontalAlignment.Left,
            DataGridLengthUnitType dataGridColumnLengthUnitType = DataGridLengthUnitType.Auto, string dataGridColumnStringFormat = null, double dataGridColumnFixedSize = 1, DataEntryType type = DataEntryType.TextBlock,
            ObservableCollection<object> options = null, string displayMemberPath = null, string heading = "", DataGridButtonType buttonType = DataGridButtonType.Content,
            TextTrimming contentButtonTextTrimming = TextTrimming.None, TextSize contentButtonTextSize = TextSize.Small, string styleName = ""
            , int maxLength = int.MaxValue)
        {
            log.LogMethodEntry(isEnable, isReadOnly, dataGridColumnhorizontalAlignment, dataGridColumnLengthUnitType, dataGridColumnStringFormat, dataGridColumnFixedSize,
                type, options, displayMemberPath, heading, buttonType, contentButtonTextTrimming, contentButtonTextSize, styleName);
            this.isEnable = isEnable;
            this.isReadOnly = isReadOnly;
            this.canUserSort = canUserSort;
            this.maxLength = maxLength;
            this.contentButtonTextSize = contentButtonTextSize;
            numberKeyboardType = NumberKeyboardType.Positive;
            this.dataGridColumnFixedSize = dataGridColumnFixedSize;
            this.contentButtonTextTrimming = contentButtonTextTrimming;
            this.dataGridColumnStringFormat = dataGridColumnStringFormat;
            this.dataGridColumnLengthUnitType = dataGridColumnLengthUnitType;
            this.dataGridColumnhorizontalAlignment = dataGridColumnhorizontalAlignment;
            this.type = type;
            this.heading = heading;
            this.buttonType = buttonType;
            fontWeight = FontWeights.Bold;
            this.displayMemberPath = displayMemberPath;
            contentButtonPadding = new Thickness(32, 0, 32, 0);
            this.options = options == null ? new ObservableCollection<object>() : options;

            log.LogMethodExit();
        }

        public object Clone()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return this.MemberwiseClone();
        }
        #endregion
    }
}
