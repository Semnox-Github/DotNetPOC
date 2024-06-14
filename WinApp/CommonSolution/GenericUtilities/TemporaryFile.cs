/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TemporaryFile class to interact with FileExplorerVM.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140    25-Sep-2020   Lakshmi Narayan           Created for POS UI Redesign 
 *2.140    25-Sep-2020   Uthanda Raja              Modified for return BitmapImage
 ********************************************************************************************/
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Semnox.Core.GenericUtilities
{
    public class TemporaryFile : IDisposable
    {
        #region Members
        private string path;

        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public Uri Uri
        {
            get
            {
                if (path == null)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }
                return new Uri(path);
            }
        }
        public BitmapImage BitmapImage
        {
            get
            {
                log.LogMethodEntry();
                BitmapImage bitmapImage = null;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    string extension = Path.GetExtension(path).ToLower();
                    if (extension.Contains(".png") || extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".jpe")
                                || extension.Contains(".jfif") || extension.Contains(".gif"))
                    {
                        bitmapImage = new BitmapImage(new Uri(path));
                    }
                }
                log.LogMethodExit(bitmapImage);
                return bitmapImage;
            }
        }
        #endregion

        #region Constructors & Finalizers
        public TemporaryFile(string fileName, Stream stream)
        {
            log.LogMethodEntry( fileName, "stream");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }   
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            path = GetTemporaryFilePath(Path.GetExtension(fileName));
            using(FileStream fs = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(fs);
            }
            log.LogMethodExit();
        }
        public TemporaryFile(string fileName, byte[] bytes)
        {
            log.LogMethodEntry(fileName, "bytes");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }   
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            path = GetTemporaryFilePath(Path.GetExtension(fileName));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            log.LogMethodExit();
        }
        public TemporaryFile(string localFilePath)
        {
            log.LogMethodEntry(localFilePath);
            if (string.IsNullOrWhiteSpace(localFilePath))
            {
                throw new ArgumentNullException("localFilePath");
            }   
            if (File.Exists(localFilePath) == false)
            {
                throw new ArgumentException("localFilePath");
            }   
            path = GetTemporaryFilePath(Path.GetExtension(localFilePath));
            using (Stream stream = File.Open(localFilePath, FileMode.Open))
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    stream.CopyTo(fs);
                }
            }
            log.LogMethodExit();
        }
        ~TemporaryFile()
        {
            Dispose(false);
        }
        #endregion

        #region Methods
        private static string GetTemporaryFilePath(string extension)
        {
            log.LogMethodEntry(extension);
            string tempDirectory = Path.Combine(Path.GetTempPath(), "Semnox");
            string result = Path.Combine(tempDirectory, Guid.NewGuid().ToString() + extension);
            if(Directory.Exists(tempDirectory) == false)
            {
                Directory.CreateDirectory(tempDirectory);
            }
            log.LogMethodExit(result);
            return result;
        }        
        public void Dispose()
        {
            log.LogMethodEntry();
            Dispose(true);
            log.LogMethodExit();
        }
        private void Dispose(bool disposing)
        {
            log.LogMethodEntry(disposing);
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            if (path != null)
            {
                try
                {
                    File.Delete(path);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                } // best effort
                path = null;
            }
            log.LogMethodExit();
        }
        #endregion

    }
}
