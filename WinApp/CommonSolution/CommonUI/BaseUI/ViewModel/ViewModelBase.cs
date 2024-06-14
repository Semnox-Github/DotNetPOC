/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - View Model Base
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy            Created for POS UI Redesign 
 *2.130.0     12-Jul-2021   Lakshminarayana       Modified to add date time converter class
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI.BaseUI.ViewModel;

namespace Semnox.Parafait.CommonUI
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private bool isLoadingVisible;
        private DateTimeValueConverter dateTimeValueConverter;
        private DateTimeValueConverter dateValueCoverter;
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public bool IsLoadingVisible
        {
            get
            {
                log.LogMethodEntry();
                return isLoadingVisible;
            }
            set
            {
                log.LogMethodExit(value);
                SetProperty(ref isLoadingVisible, value);
            }
        }

        public DateTimeValueConverter DateValueConverter
        {
            get
            {
                if (dateValueCoverter == null)
                {
                    if (ExecutionContext != null)
                    {
                        dateValueCoverter = new DateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT", "dd-MM-yyyy"));
                    }
                    else
                    {
                        dateValueCoverter = new DateTimeValueConverter("dd-MM-yyyy");
                    }
                }
                return dateValueCoverter;
            }
        }
        public DateTimeValueConverter DateTimeValueConverter
        {
            get
            {
                if (dateTimeValueConverter == null)
                {
                    if (ExecutionContext != null)
                    {
                        dateTimeValueConverter = new DateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT", "dd-MMM-yyyy h:mm tt"));
                    }
                    else
                    {
                        dateTimeValueConverter = new DateTimeValueConverter("dd-MMM-yyyy h:mm tt");
                    }
                }
                return dateValueCoverter;
            }
        }

        public ExecutionContext ExecutionContext
        {
            get
            {
                return executionContext;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref executionContext, value);
                log.LogMethodExit(executionContext);
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            log.LogMethodEntry();
            if (object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            this.OnPropertyChanged(propertyName);
            log.LogMethodExit();
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            log.LogMethodEntry();
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
            log.LogMethodExit();
        }

        internal void ExecuteAction(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }
        protected virtual bool ButtonEnable(object state)
        {
            return !IsLoadingVisible;
        }
        #endregion
    }
}
