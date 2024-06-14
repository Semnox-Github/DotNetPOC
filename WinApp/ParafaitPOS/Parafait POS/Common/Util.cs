/*
 -------------------------------------------------------------------------------
 GrFinger Sample
 (c) 2005 Griaule Tecnologia Ltda.
 http://www.griaule.com
 -------------------------------------------------------------------------------

 This sample is provided with "GrFinger Fingerprint Recognition Library" and
 can't run without it. It's provided just as an example of using GrFinger
 Fingerprint Recognition Library and should not be used as basis for any
 commercial product.

 Griaule Tecnologia makes no representations concerning either the merchantability
 of this software or the suitability of this sample for any particular purpose.

 THIS SAMPLE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR
 IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 IN NO EVENT SHALL GRIAULE BE LIABLE FOR ANY DIRECT, INDIRECT,
 INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 You can download the free version of GrFinger directly from Griaule website.
                                                                   
 These notices must be retained in any copies of any part of this
 documentation and/or sample.

 * 
 *  Modifications by Shawn Weisfeld http://www.shawnweisfeld.com
 * 
 * 
 -------------------------------------------------------------------------------
*/

// -----------------------------------------------------------------------------------
// Support and fingerprint management routines
// -----------------------------------------------------------------------------------

using GrFingerXLib;
using System;
using System.Drawing;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Parafait_POS
{
    /// <summary>
    /// Raw image data type.
    /// </summary>
    public struct TRawImage
    {
        // Image data.
        public object img;
        // Image width.
        public int width;
        // Image height.
        public int height;
        // Image resolution.
        public int Res;
    };

    /// <summary>
    /// the template class
    /// </summary>
    public class TTemplate
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Template data.
        public System.Array _tpt;
        // Template size
        public int _size;
        public TTemplate()
        {
            log.LogMethodEntry();
            // Create a byte buffer for the template
            _tpt = new byte[(int)GRConstants.GR_MAX_SIZE_TEMPLATE];
            _size = 0;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Helper class for dealing with the GrFinger SDK
    /// </summary>
    public class Util
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Some constants to make our code cleaner
        public const int ERR_CANT_OPEN_BD = -999;
        public const int ERR_INVALID_ID = -998;
        public const int ERR_INVALID_TEMPLATE = -997; 

        // -----------------------------------------------------------------------------------
        // Support functions
        // -----------------------------------------------------------------------------------

        // This class creates an Util class with some functions
        // to help us to develop our GrFinger Application
        public Util(PictureBox pbPic)
        {
            log.LogMethodEntry(pbPic);
            _pbPic = pbPic;
            _tpt = null;
            log.LogMethodExit();
        }

        ~Util()
        {
        }

        // Check if we have a valid template
        private bool TemplateIsValid()
        {
            log.LogMethodEntry();
            bool ret = ((_tpt._size > 0) && (_tpt._tpt != null));
            log.LogMethodExit(ret);
            // Check the template size and data
            return ret;
        }

        // -----------------------------------------------------------------------------------
        // Main functions for fingerprint recognition management
        // -----------------------------------------------------------------------------------

        // Initializes GrFinger ActiveX and all necessary utilities.
        public int InitializeGrFinger(AxGrFingerXLib.AxGrFingerXCtrl grfingerx)
        {
            log.LogMethodEntry(grfingerx);
            GRConstants result;

            _grfingerx = grfingerx;

            //Create a new Template
            if (_tpt == null)
                _tpt = new TTemplate();

            //Create a new raw image
            _raw = new TRawImage();

            //Initialize library
            result = (GRConstants)_grfingerx.Initialize();
            if (result < 0)
            {
                log.LogMethodExit(result);
                return (int)result;
            }
            int ret = (int)_grfingerx.CapInitialize();
            log.LogMethodExit(ret);
            return ret;
        }

        //  Finalizes library and close DB.
        public void FinalizeUtil()
        {
            log.LogMethodEntry();
            // finalize library
            _grfingerx.Finalize();
            _grfingerx.CapFinalize();
            _raw.img = null;
            _tpt = null;
            log.LogMethodExit();
        }

        // Display fingerprint image on screen
        public void PrintBiometricDisplay(bool isBiometric, GrFingerXLib.GRConstants contextId)
        {
            log.LogMethodEntry(isBiometric, contextId);
            // handle to finger image
            System.Drawing.Image handle = null;
            // screen HDC
            IntPtr hdc = GetDC(System.IntPtr.Zero);

            if (isBiometric)
            {
                // get image with biometric info
                _grfingerx.BiometricDisplay(ref _tpt._tpt,
                    ref _raw.img, _raw.width, _raw.height, _raw.Res, hdc.ToInt32(),
                    ref handle, (int)contextId);
            }
            else
            {
                // get raw image
                _grfingerx.CapRawImageToHandle(ref _raw.img, _raw.width,
                    _raw.height, hdc.ToInt32(), ref handle);
            }

            // draw image on picture box
            if (handle != null)
            {
                _pbPic.Image = handle;
                _pbPic.Update();
            }

            // release screen HDC
            ReleaseDC(System.IntPtr.Zero, hdc);
            log.LogMethodExit();
        }

        // Extract a fingerprint template from current image
        public int ExtractTemplate()
        {
            log.LogMethodEntry();
            int result;

            // set current buffer size for the extract template
            _tpt._size = (int)GRConstants.GR_MAX_SIZE_TEMPLATE;
            result = (int)_grfingerx.Extract(
                ref _raw.img, _raw.width, _raw.height, _raw.Res,
                ref _tpt._tpt, ref _tpt._size,
                (int)GRConstants.GR_DEFAULT_CONTEXT);
            // if error, set template size to 0
            if (result < 0)
            {
                // Result < 0 => extraction problem
                _tpt._size = 0;
            }
            log.LogMethodExit(result);
            return result;
        }

        // Identify current fingerprint on our database
        public int Identify(ref int score)
        {
            log.LogMethodEntry(score);
            TTemplate tptRef;

            // Checking if template is valid.
            if (!TemplateIsValid())
            {
                log.LogMethodExit(ERR_INVALID_TEMPLATE);
                return ERR_INVALID_TEMPLATE;
            }
            
            // Starting identification process and supplying query template.
            GRConstants result = (GRConstants)_grfingerx.IdentifyPrepare(ref _tpt._tpt,
                (int)GRConstants.GR_DEFAULT_CONTEXT);
            
            // error?
            if (result < 0)
            {
                log.LogMethodExit(result);
                return (int)result;
            }
            
            // Getting enrolled templates from database.
            SqlCommand cmd = POSStatic.Utilities.getCommand();

            cmd.CommandText = "SELECT user_ID, fp_template FROM users where active_flag = 'Y'";
            SqlDataReader rs = cmd.ExecuteReader();
            while (rs.Read())
            {
                // Getting current template from recordset.

                if (rs[1] == DBNull.Value)
                {
                    continue;
                }
                tptRef = getTemplate(rs);

                // Comparing current template.
                result = (GRConstants)_grfingerx.Identify(ref tptRef._tpt, ref score, (int)GRConstants.GR_DEFAULT_CONTEXT);

                // Checking if query template and the reference template match.
                if (result == GRConstants.GR_MATCH)
                {
                    int userId = int.Parse(rs[0].ToString());
                    rs.Close();
                    cmd.Dispose();
                    return userId;
                }
                else if (result < 0)
                {
                    rs.Close();
                    cmd.Dispose();
                    return (int)result;
                }
            }

            // Closing recordset.
            rs.Close();
            cmd.Dispose();
            int ret = (int)GRConstants.GR_NOT_MATCH;
            log.LogMethodExit(ret);
            return ret;
        }

        // Return template data from an OleDbDataReader
        public TTemplate getTemplate(SqlDataReader rs)
        {
            log.LogMethodEntry(rs);
            TTemplate tptBlob = new TTemplate();
            tptBlob._size = 0;
            
            // alloc space
            System.Byte[] temp = new System.Byte[
                (int)GRConstants.GR_MAX_SIZE_TEMPLATE];

            // get bytes
            long readedBytes = rs.GetBytes(1, 0, temp, 0, temp.Length);
            
            // copy to structure
            System.Array.Copy(temp, 0, tptBlob._tpt, 0, (int)readedBytes);
            
            // set real size
            tptBlob._size = (int)readedBytes;

            log.LogMethodExit(tptBlob);
            return tptBlob;
        }

        //Importing necessary HDC functions
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        // The last acquired image.
        public TRawImage _raw;

        // Reference to main form Image.
        public PictureBox _pbPic;

        // The template extracted from last acquired image.
        public TTemplate _tpt;

        // GrFingerX component
        AxGrFingerXLib.AxGrFingerXCtrl _grfingerx;
    };
}