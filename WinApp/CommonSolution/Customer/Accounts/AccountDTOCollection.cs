using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the collection of Account data object class. This acts as data holder for the list of Account business object. and used as a return value in controller
    /// </summary>
    public class AccountDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AccountDTO> _data;
        private int _currentPageNo;
        private int _totalCount;
        private string _barCode;
        private string _token;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor Required data fields
        /// </summary>
        public AccountDTOCollection(List<AccountDTO> _data, int _currentPageNo, int _totalCount, string _barCode, string _token)
        {
            log.LogMethodEntry(_data, _currentPageNo, _totalCount, _barCode, _token);
            this._data = _data;
            this._currentPageNo = _currentPageNo;
            this._totalCount = _totalCount;
            this._barCode = _barCode;
            this._token = _token;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the data field
        /// </summary>
        public List<AccountDTO> data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        /// <summary>
        /// Get/Set method of the currentPageNo field
        /// </summary>
        public int currentPageNo
        {
            get
            {
                return _currentPageNo;
            }
            set
            {
                _currentPageNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the totalCount field
        /// </summary>
        public int totalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the barCode field
        /// </summary>
        public string barCode
        {
            get
            {
                return _barCode;
            }
            set
            {
                _barCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the token field
        /// </summary>
        public string token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
            }
        }
    }
}
