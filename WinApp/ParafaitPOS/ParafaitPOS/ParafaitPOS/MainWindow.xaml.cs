using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.GamesUI;



namespace ParafaitPOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM mainWindowVM;

        public Grid ContentGrid
        {
            get
            {
                return this.Template.FindName("ContentGrid", this) as Grid;
            }
        }

        public Grid ActionGrid
        {
            get
            {
                return this.Template.FindName("ActionGrid", this) as Grid;
            }
        }

        public TextBlock ZeroSelectedTextBlock
        {
            get
            {
                return this.Template.FindName("ZeroSelectedTextBlock", this) as TextBlock;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = mainWindowVM = new MainWindowVM(App.machineUserContext, this);

            //ButtonGroupControl.DataContext = new ButtonGroupVM()
            //{
            //    IsFirstButtonEnabled = true,
            //    IsSecondButtonEnabled = false,
            //    FirstButtonContent = "IN SERVICE",
            //    SecondButtonContent = "OUT OF SERVICE"
            //};


            //ComboBoxGroupControl.DataContext = new ComboGroupVM()
            //{
            //    ComboList = new ObservableCollection<Common.UserControls.Model.ComboBoxField>()
            //    {
            //        new ComboBoxField() { Header = "Status", Items = new ObservableCollection<string>() { "Active", "NonActive" }, SelectedItem = "Active" },
            //        new ComboBoxField() { Header = "Connection", Items = new ObservableCollection<string>() { "In Office", "Out of Office" }, SelectedItem = "In Office" },
            //        new ComboBoxField() { Header = "Connection", Items = new ObservableCollection<string>() { "In Office", "Out of Office" }, SelectedItem = "Out of Office" }
            //    }
            //};


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MainWindowVM mainWindowVM = this.Main_Grid.DataContext as MainWindowVM;

            //mainWindowVM.FooterVM.MessageType = MessageType.Error;

            //mainWindowVM.FooterVM.Message = "Hi!";

            DatePickerView popupnWindowBase = new DatePickerView();

            popupnWindowBase.Owner = this;

            DatePickerVM datePickerViewModel = new DatePickerVM();

            popupnWindowBase.DataContext = datePickerViewModel;

            popupnWindowBase.Closing += PopupnWindowBase_Closing;

            popupnWindowBase.Show();
        }

        private void PopupnWindowBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // OutputTextBox.Text = (sender as DatePickerView).SelectedDate;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ContextSearchView contextSearchView = new ContextSearchView();

            contextSearchView.Owner = this;

            List<DisplayParameters> searchList = new List<DisplayParameters>()
            {
                new DisplayParameters() { Id = 1, ParameterNames = new ObservableCollection<string> () { "Raja PC", "Master 1", "Car Game" } },

                new DisplayParameters() { Id = 2, ParameterNames = new ObservableCollection<string> () { "Sem PC_1", "Master 1", "Water Game" } },

                new DisplayParameters() { Id = 3, ParameterNames = new ObservableCollection<string> () { "Par PC", "Master 2", "Car Game" } },

                new DisplayParameters() { Id = 4, ParameterNames = new ObservableCollection<string> () { "Sem PC_2", "Master 2", "Fight Game" } },
            };

            ObservableCollection<DisplayParameters> paramlist = new ObservableCollection<DisplayParameters>(searchList);

            contextSearchView.DataContext = new ContextSearchVM(mainWindowVM.ExecutionContext)
            {
                // parameter list.
                SearchParams = paramlist,

                // Search index.
                SearchIndexes = new ObservableCollection<int>() { 0 },
            };

            contextSearchView.Closing += ContextSearchView_Closing;

            contextSearchView.Show();
        }

        private void ContextSearchView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //OutputTextBox.Text = (sender as ContextSearchView).SelectedId.ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GenericDataEntryView dataEntryView = new GenericDataEntryView();

            dataEntryView.Owner = this;

            GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(mainWindowVM.ExecutionContext)
            {
                Heading = "Generic Data Entry Window",

                ErrorMessage = "The required field must not be empty",

                DataEntryCollections = new ObservableCollection<DataEntryElement>()
                  {
                      new DataEntryElement()
                      {
                           Type = DataEntryType.TextBox,
                           DefaultValue = "Enter UserName",
                           IsMandatory = true,
                           Text = "Raja",
                           ErrorMessage = "Please enter albhapet only",
                           Heading = "User Name",
                           ValidationType = Semnox.Parafait.CommonUI.ValidationType.AlphabetsOnly
                      },
                      new DataEntryElement()
                      {
                           Type = DataEntryType.TextBox,
                           DefaultValue = "Enter Password",
                           IsMandatory = true,
                           ErrorMessage = "Please enter correct password",
                           Heading = "Password",
                           IsMasked = true
                      },
                      new DataEntryElement()
                      {
                           Type = DataEntryType.ComboBox,
                           DefaultValue = "Select Age",
                           IsMandatory = true,
                           //Options = new ObservableCollection<string>() { "0 - 18", "18-35", "35 - 75" },
                           Heading = "Age",
                      }
                  }
            };

            dataEntryView.DataContext = dataEntryVM;

            dataEntryView.Show();
        }

    }
}
