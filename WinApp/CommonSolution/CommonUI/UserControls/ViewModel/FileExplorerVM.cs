/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common FileExplorerVM to save/get the files.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140    25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;

using iTextSharp.text.pdf;

using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.GenericUtilities.FileResources;

namespace Semnox.Parafait.CommonUI
{
    public class FileExplorerVM : ViewModelBase
    {
        #region Members
        public enum Orientation
        {
            Right = 0,
            Bottom = 1
        }

        private bool secure;
        private bool isChanged;
        private bool htmlExtenstion;
        private bool showClearButton;
        private bool showBrowseButton;
        private bool useRandomFileName;
        private bool isEncryptedPDFFile;

        private string fileName;
        private string filterText;
        private string localFilePath;
        private string defaultValueName;
        private string encryptionKeyDefaultValue;

        private Orientation buttonsOrientation;

        private TemporaryFile temporaryFile;
        private FileExplorerUserControl fileExplorerUserControl;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);

        private ICommand actionsCommand;
        #endregion

        #region Properties
        public bool IsHTMLExtenstion
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(htmlExtenstion);
                return htmlExtenstion;
            }
            private set
            {
                log.LogMethodEntry(htmlExtenstion, value);
                SetProperty(ref htmlExtenstion, value);
                log.LogMethodExit(htmlExtenstion);
            }
        }
        public BitmapImage BitmapImage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return temporaryFile == null ? null : temporaryFile.BitmapImage;
            }
        }
        public bool ShowBrowseButton
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showBrowseButton);
                return showBrowseButton;
            }
            set
            {
                log.LogMethodEntry(showBrowseButton, value);
                SetProperty(ref showBrowseButton, value);
                log.LogMethodExit(showBrowseButton);
            }
        }
        public bool ShowClearButton
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showClearButton);
                return showClearButton;
            }
            set
            {
                log.LogMethodEntry(showClearButton, value);
                SetProperty(ref showClearButton, value);
                log.LogMethodExit(showClearButton);
            }
        }
        public TemporaryFile TemporaryFile
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(temporaryFile);
                return temporaryFile;
            }
            private set
            {
                log.LogMethodEntry(temporaryFile, value);
                if (temporaryFile != null)
                {
                    temporaryFile.Dispose();
                    temporaryFile = null;
                }
                SetProperty(ref temporaryFile, value);
                log.LogMethodExit(temporaryFile);
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if(value == fileName)
                {
                    return;
                }
                localFilePath = string.Empty;
                isChanged = false;
                SetProperty(ref fileName, value);
                LoadFileResource();
            }
        }
        public Orientation ButtonsOrientation
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonsOrientation);
                return buttonsOrientation;
            }
            set
            {
                log.LogMethodEntry(buttonsOrientation, value);
                SetProperty(ref buttonsOrientation, value);
                log.LogMethodExit(buttonsOrientation);
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
        #endregion

        #region Constructor
        public FileExplorerVM(ExecutionContext executionContext, string defaultValueName, string fileName, bool secure = false, 
            bool useRandomFileName = false, Orientation buttonsOrientation = Orientation.Right, bool showClearButton = true, bool showBrowseButton = true,
            bool isEncryptedPDFFile = false, string encryptionKeyDefaultValue = "")
        {
            log.LogMethodEntry(executionContext, defaultValueName, fileName, secure, useRandomFileName, buttonsOrientation, showClearButton, showBrowseButton,
                isEncryptedPDFFile, encryptionKeyDefaultValue);
            ExecutionContext = executionContext;
            InitializeCommands();
            this.isEncryptedPDFFile = isEncryptedPDFFile;
            this.encryptionKeyDefaultValue = encryptionKeyDefaultValue;
            string imageTypes = "*.BMP;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.PNG";
            filterText = "All Files|*.PDF;" + imageTypes + "|Image Files|" + imageTypes + "|PDF Files|*.PDF;";            
            this.defaultValueName = defaultValueName;
            this.fileName = fileName;
            this.secure = secure;
            this.useRandomFileName = useRandomFileName;
            this.buttonsOrientation = buttonsOrientation;
            this.showClearButton = showClearButton;
            this.showBrowseButton = showBrowseButton;
            LoadFileResource();
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            PropertyChanged += OnPropertyChanged;
            actionsCommand = new DelegateCommand(OnActionsClicked);
            log.LogMethodExit();
        }
        private async void LoadFileResource()
        {
            log.LogMethodEntry();
            if(string.IsNullOrWhiteSpace(fileName))
            {
                PerformClear();
                return;
            }
            FileResource fileResource = FileResourceFactory.GetFileResource(ExecutionContext, defaultValueName, fileName, secure);
            using(Stream stream = await fileResource.Get())
            {
                if (isEncryptedPDFFile)
                {
                    string password = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, encryptionKeyDefaultValue);
                    using (PdfReader reader = new PdfReader(stream, new System.Text.ASCIIEncoding().GetBytes(password)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
                            {
                                stamper.Close();
                                reader.Close();
                                TemporaryFile = new TemporaryFile(fileName, memoryStream.ToArray());
                            }
                        }
                    }
                }
                else
                {
                    TemporaryFile = new TemporaryFile(fileName, stream);
                }
            }
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(!string.IsNullOrWhiteSpace(e.PropertyName))
            {
                switch(e.PropertyName)
                {
                    case "TemporaryFile":
                        ShowFileInBrowser();
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                string actionText = parameter as string;
                FileExplorerUserControl explorerUserControl = parameter as FileExplorerUserControl;
                if(!string.IsNullOrWhiteSpace(actionText))
                {
                    switch(actionText.ToLower())
                    {
                        case "browse":
                            {
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Multiselect = false;
                                openFileDialog.Title = MessageViewContainerList.GetMessage(ExecutionContext, "Select Image/PDF file");                                
                                openFileDialog.Filter = filterText;
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    TemporaryFile = new TemporaryFile(openFileDialog.FileName);
                                    localFilePath = openFileDialog.FileName;
                                    isChanged = true;
                                    fileName = useRandomFileName? Guid.NewGuid().ToString() + Path.GetExtension(localFilePath) : Path.GetFileName(localFilePath);
                                }
                            }
                            break;
                        case "close":
                        case "clear":
                            {
                                PerformClear();
                            }
                            break;
                    }
                }
                else if(explorerUserControl != null)
                {
                    fileExplorerUserControl = explorerUserControl;
                    if(fileExplorerUserControl.WebBrowserContent != null)
                    {
                        fileExplorerUserControl.WebBrowserContent.Navigated += OnNavigated;
                        fileExplorerUserControl.scvWebBrowser.ScrollChanged += OnScrollChanged;
                        if (!string.IsNullOrWhiteSpace(fileName) && Path.GetExtension(fileName).ToLower().Contains(".html"))
                        {
                            IsHTMLExtenstion = true;
                            mshtml.HTMLDocument Doc = (mshtml.HTMLDocument)fileExplorerUserControl.WebBrowserContent.Document;
                            if (Doc != null && Doc.parentWindow != null)
                            {
                                mshtml.DispHTMLHtmlElement htmlelement = Doc.documentElement as mshtml.DispHTMLHtmlElement;
                                if (htmlelement != null)
                                {
                                    CustomScrollBar customScrollBar = fileExplorerUserControl.scvWebBrowser.Template.FindName("PART_VerticalScrollBar", fileExplorerUserControl.scvWebBrowser) as CustomScrollBar;
                                    if (customScrollBar != null)
                                    {
                                        fileExplorerUserControl.tempBorder.Height = htmlelement.scrollHeight;
                                    }
                                }
                                string elementOverflow = string.Format("document.documentElement.style.overflow='hidden';");
                                Doc.parentWindow.execScript(elementOverflow);
                            }
                        }
                        else
                        {
                            IsHTMLExtenstion = false;
                        }
                    }
                    Window parentWindow = PresentationSource.FromVisual(fileExplorerUserControl).RootVisual as Window;
                    if(parentWindow != null)
                    {
                        parentWindow.Closed += OnParentWindowClosed;
                    }
                    ShowFileInBrowser();
                }
            }
            log.LogMethodExit();
        }
        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrWhiteSpace(fileName) && Path.GetExtension(fileName).ToLower().Contains(".html"))
            {
                IsHTMLExtenstion = true;
                mshtml.HTMLDocument Doc = (mshtml.HTMLDocument)fileExplorerUserControl.WebBrowserContent.Document;
                if (Doc != null && Doc.parentWindow != null)
                {
                    mshtml.DispHTMLHtmlElement htmlelement = Doc.documentElement as mshtml.DispHTMLHtmlElement;
                    if (htmlelement != null)
                    {
                        fileExplorerUserControl.tempBorder.Height = htmlelement.scrollHeight;
                    }
                    string elementOverflow = string.Format("document.documentElement.style.overflow='hidden';");
                    Doc.parentWindow.execScript(elementOverflow);
                }
            }
            else
            {
                IsHTMLExtenstion = false;
            }
            log.LogMethodExit();
        }
        private void OnScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileName) && Path.GetExtension(fileName).ToLower().Contains(".html"))
            {
                IsHTMLExtenstion = true;
                mshtml.HTMLDocument Doc = (mshtml.HTMLDocument)fileExplorerUserControl.WebBrowserContent.Document;
                if (Doc != null && Doc.parentWindow != null)
                {
                    Doc.parentWindow.scroll(0, (int)e.VerticalOffset);
                }
            }
            else
            {
                IsHTMLExtenstion = false;
            }
        }
        private void OnParentWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnActionsClicked("close");
            log.LogMethodExit();
        }
        private void PerformClear()
        {
            log.LogMethodEntry();
            ClearBrowser();
            if (temporaryFile != null)
            {
                temporaryFile.Dispose();
                temporaryFile = null;
            }
            fileName = string.Empty;
            log.LogMethodExit();
        }        
        private void ShowFileInBrowser()
        {
            log.LogMethodEntry();
            if (fileExplorerUserControl != null && fileExplorerUserControl.WebBrowserContent != null)
            {
                Uri navigatingPath = new Uri("about:blank");
                if (temporaryFile != null)
                {
                    navigatingPath = temporaryFile.Uri;
                }
                fileExplorerUserControl.WebBrowserContent.Source = navigatingPath;
            }
            log.LogMethodExit();
        }
        private void ClearBrowser()
        {
            log.LogMethodEntry();
            if (fileExplorerUserControl != null && fileExplorerUserControl.WebBrowserContent != null)
            {
                Uri navigatingPath = new Uri("about:blank");
                fileExplorerUserControl.WebBrowserContent.Source = navigatingPath;
            }   
            log.LogMethodExit();
        }
        public async Task<bool> Save()
        {
            log.LogMethodEntry();
            bool result = false;
            if(isChanged == false || string.IsNullOrWhiteSpace(localFilePath))
            {
                result = true;
                return result;
            }
            try
            {
                if (isEncryptedPDFFile == false)
                {
                    using (Stream stream = File.Open(localFilePath, FileMode.Open))
                    {
                        FileResource fileResource = FileResourceFactory.GetFileResource(ExecutionContext, defaultValueName, fileName, secure);
                        result = await fileResource.Save(stream);
                    }
                }
                else
                {
                    using (Stream stream = File.Open(localFilePath, FileMode.Open))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            using (PdfReader pdfReader = new PdfReader(stream))
                            {
                                string password = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, encryptionKeyDefaultValue);
                                PdfEncryptor.Encrypt(pdfReader, m, true, password, password, PdfWriter.ALLOW_SCREENREADERS);
                            }
                            FileResource fileResource = FileResourceFactory.GetFileResource(ExecutionContext, defaultValueName, fileName, secure);
                            result = await fileResource.Save(m);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.Error("Error occured while saving the file to the server", ex);
            }
            log.LogMethodExit(result);
            return result;
        }
        public void Dispose()
        {
            log.LogMethodEntry();
            try
            {
                PerformClear();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
