/********************************************************************************************
 * Project Name - Customer
 * Description  - ProfilePhotoUI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Profile photo management ui
    /// </summary>
    public partial class ProfilePhotoUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProfileBL profileBL;
        private Image image;
        private MessageBoxDelegate messageBoxDelegate;
        private string photoDirectory;
        //Create webcam object
        VideoCaptureDevice videoSource;
        private Utilities utilities;
        private bool _selecting;
        private Rectangle _selection;
        int prevX, prevY;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="profileDTO"></param>
        /// <param name="messageBoxDelegate"></param>
        public ProfilePhotoUI(Utilities utilities, ProfileDTO profileDTO, MessageBoxDelegate messageBoxDelegate)
        {
            log.LogMethodEntry(utilities, profileDTO);
            InitializeComponent();
            profileBL = new ProfileBL(utilities.ExecutionContext, profileDTO);
            this.utilities = utilities;
            this.messageBoxDelegate = messageBoxDelegate;
            photoDirectory = GetImageDirectory();
            if (string.IsNullOrWhiteSpace(profileDTO.PhotoURL) == false)
            {
                try
                {
                    pbCurrentImage.Image = profileBL.GetProfileImage();
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    pbCurrentImage.Image = null;
                }
            }
            StartCamera();

            string CropRegionSize = ParafaitDefaultContainerList.GetParafaitDefault(this.utilities.ExecutionContext,"PHOTO_CAPTURE_CROP_REGION_SIZE");
            Size cropSize = new Size(280, 280);
            if (string.IsNullOrEmpty(CropRegionSize) == false)
            {
                int size;
                if (!Int32.TryParse(CropRegionSize, out size))
                    size = 280;
                cropSize.Width = cropSize.Height = Math.Min(pbPhoto.Height - 20, Math.Max(180, size));
            }

            _selection = new Rectangle(new Point(0, 0), cropSize);
            _selection.X = (pbPhoto.Width - _selection.Width) / 2;
            _selection.Y = (pbPhoto.Height - _selection.Height) / 2;
            log.LogMethodExit();
        }

        private string GetImageDirectory()
        {
            log.LogMethodEntry();
            string path = ParafaitDefaultContainerList.GetParafaitDefault(this.utilities.ExecutionContext, "IMAGE_DIRECTORY");
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while creating directory: "+ path, ex);
                    messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 16, ex.Message), MessageContainerList.GetMessage(utilities.ExecutionContext, "IO Error"));
                }
            }
            log.LogMethodExit(path);
            return path;
        }

        /// <summary>
        /// Selected profile image
        /// </summary>
        public Image SelectedImage
        {
            get
            {
                return image;
            }
        }

        void StartCamera()
        {
            log.LogMethodEntry();
            if(videoSource == null)
            {
                try
                {
                    //List all available video sources. (That can be webcams as well as tv cards, etc)
                    FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    //Check if atleast one video source is available
                    if (videosources != null)
                    {
                        foreach (FilterInfo source in videosources)
                        {
                            //For example use first video device. You may check if this is your webcam.
                            videoSource = new VideoCaptureDevice(source.MonikerString);
                            if (videoSource.VideoCapabilities.Length > 0)
                            {
                                break;
                            }
                        }

                        try
                        {
                            //Check if the video device provides a list of supported resolutions
                            if (videoSource.VideoCapabilities.Length > 0)
                            {
                                string highestSolution = "0;0";
                                //Search for the highest resolution
                                for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                                {
                                    if (videoSource.VideoCapabilities[i].FrameSize.Width > Convert.ToInt32(highestSolution.Split(';')[0]))
                                        highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                                }
                                //Set the highest resolution as active
                                videoSource.DesiredFrameSize = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])].FrameSize;
                                pbPhoto.Height = (int)((decimal)pbPhoto.Width / ((decimal)videoSource.DesiredFrameSize.Width / (decimal)videoSource.DesiredFrameSize.Height));
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred due to video source", ex);
                        }

                        if (videoSource != null)
                        {
                            //Create NewFrame event handler
                            //(This one triggers every time a new frame/image is captured
                            videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);

                            //Start recording
                            videoSource.Start();
                            pbCapture.Image = Properties.Resources.Stop_Red;
                        }
                    }
                    else
                        pbCapture.Enabled = false;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while starting the camera", ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(6), "Web Camera");
                    MessageBox.Show(ex.Message);
                    pbCapture.Enabled = false;
                }
            }
            log.LogMethodExit();
        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            log.LogMethodEntry(sender, eventArgs);
            //Cast the frame as Bitmap object and don't forget to use ".Clone()" otherwise
            //you'll probably get access violation exceptions
            pbPhoto.Image = (Bitmap)eventArgs.Frame.Clone();
            log.LogMethodExit();
        }

        private void CustomerPhoto_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Stop and free the webcam object if application is closing
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
            }
            log.LogMethodExit();
        }

        private void pbCapture_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (pbCapture.Tag != null)
            {
                pbCapture.Image = Properties.Resources.Stop_Red;
                pbCapture.Tag = null;
                videoSource.Start();
                log.LogMethodExit();
                return;
            }

            if (videoSource == null)
            {
                StartCamera();
            }
            else
            {
                try
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    pbCapture.Tag = "X";
                    playSimpleSound(); 
                    
                    capturePhoto();

                    pbCapture.Image = Properties.Resources.Player_Green;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while capturing photo", ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(13, ex.Message), "Save Image");
                }
            }
            log.LogMethodExit();
        }

        void capturePhoto()
        {
            log.LogMethodEntry();
            try
            {
                Image currentImage = pbPhoto.Image.Clone() as Image;

                // Create cropped image:
                currentImage = Crop(currentImage, ScaleRectangle());

                // Fit image to the picturebox:
                pbCurrentImage.Image = currentImage;
                image = currentImage;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while capturing photo", ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(13, ex.Message), "Capture Image");
            }
            log.LogMethodExit();
        }

        private void playSimpleSound()
        {
            log.LogMethodEntry();
            try
            {
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(Properties.Resources.camera_click);
                simpleSound.Play();
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while playing simple sound", ex);
            }
            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(image != null)
            {
                string photoFile = Guid.NewGuid().ToString();
                photoFile += ".jpg"; //added file extension

                profileBL.SaveProfilePhoto(photoFile, utilities.ConvertToByteArray(image));
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 17), MessageContainerList.GetMessage(utilities.ExecutionContext, "Capture Photo"));
            }
            log.LogMethodExit();
        }

        private void pbPhoto_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // Starting point of the selection:
            if (e.Button == MouseButtons.Left)
            {
                _selecting = true;

                prevX = e.X;
                prevY = e.Y;
            }
            log.LogMethodExit();
        }

        private void pbPhoto_MouseMove(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // Update the actual size of the selection:
            if (_selecting)
            {
                _selection.X += e.X - prevX;
                _selection.Y += e.Y - prevY;

                prevX = e.X;
                prevY = e.Y;
                // Redraw the picturebox:
                pbPhoto.Refresh();
            }
            log.LogMethodExit();
        }

        private void pbPhoto_Paint(object sender, PaintEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Pen pen = Pens.GreenYellow;
            e.Graphics.DrawRectangle(pen, _selection);
            log.LogMethodExit();
        }

        private void pbPhoto_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.Button == MouseButtons.Left
                    && _selecting
                    && _selection.Size != new Size())
                {
                    Image _orig = pbPhoto.Image.Clone() as Image;

                    capturePhoto();

                    pbPhoto.Image = _orig;
                }
            }
            catch (Exception ex)
            {
                log.Error("error occurred in capturePhoto", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _selecting = false;
            }
            log.LogMethodExit();
        }

        Rectangle ScaleRectangle()
        {
            log.LogMethodEntry();
            Rectangle scaled = new Rectangle();

            scaled.X = _selection.X * pbPhoto.Image.Width / pbPhoto.Width;
            scaled.Y = _selection.Y * pbPhoto.Image.Height / pbPhoto.Height;
            scaled.Width = _selection.Width * pbPhoto.Image.Width / pbPhoto.Width;
            scaled.Height = _selection.Height * pbPhoto.Image.Height / pbPhoto.Height;
            log.LogMethodExit(scaled);
            return scaled;

        }

        private Image Crop(Image image, Rectangle selection)
        {
            log.LogMethodEntry(image, selection);

            if (selection.X < 0 || selection.Y < 0 || selection.Right > image.Width || selection.Bottom > image.Height)
            {
                log.LogMethodExit(null, "Throwing Application exception-Invalid Crop Area");
                throw new ApplicationException("Invalid Crop Area");
            }


            Bitmap bmp = image as Bitmap;

            // Check if it is a bitmap:
            if (bmp == null)
            {
                log.LogMethodExit(null, "Throwing Application exception-No valid bitmap");
                throw new ArgumentException("No valid bitmap");

            }
            // Crop the image:
            Bitmap cropBmp = bmp.Clone(selection, bmp.PixelFormat);

            // Release the resources:
            image.Dispose();
            log.LogMethodExit(cropBmp);
            return cropBmp;
        }
    }
}
