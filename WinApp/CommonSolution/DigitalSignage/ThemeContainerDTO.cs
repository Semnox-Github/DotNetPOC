using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the Theme data object class. This acts as data holder for the Theme container business object
    /// </summary>
    public class ThemeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;
        private int typeId;
        private string description;
        private int initialScreenId;
        private int themeNumber;
        private int id;

        public ThemeContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ThemeContainerDTO(int id, string name, int typeId, string description, int initialScreenId, int themeNumber)
            : this()
        {
            log.LogMethodEntry(id, name, typeId, description, initialScreenId, themeNumber);
            this.name = name;
            this.typeId = typeId;
            this.description = description;
            this.initialScreenId = initialScreenId;
            this.themeNumber = themeNumber;
            this.id = id;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        public String ThemeDetails { get { return name + " [" + themeNumber + "]"; } }

        /// <summary>
        /// Get/Set method of the TypeId field
        /// </summary>
        public int TypeId { get { return typeId; } set { typeId = value; } }

        /// <summary>
        /// Get/Set method of the InitialScreenId field
        /// </summary>
        public int InitialScreenId { get { return initialScreenId; } set { initialScreenId = value; } }

        /// <summary>
        /// Get/Set method of the themeNumber field
        /// </summary>
        public int ThemeNumber { get { return themeNumber; } set { themeNumber = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the ThemeNameWithThemeNumber field
        /// </summary>
        public string ThemeNameWithThemeNumber
        {
            get
            {
                return ((string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name)) ? " " : Name + "[" + ThemeNumber + "]");
            }
            set { }
        }



    }
}