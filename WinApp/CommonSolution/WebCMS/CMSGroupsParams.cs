using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.WebCMS
{
    public class CMSGroupsParams
    {
        private int groupId;
        private int parentGroupId;
        private string name;
        private bool active;
        private int siteId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSGroupsParams()
        {
            this.groupId = -1;
            this.parentGroupId = -1;
            this.name = "";
            this.siteId = -1;
            this.active = false;
        }


        /// <summary>
        /// Get/Set method of the GroupId field
        /// </summary>
        [DisplayName("GroupId")]
        [DefaultValue(-1)]
        public int GroupId { get { return groupId; } set { groupId = value; } }

        /// <summary>
        /// Get/Set method of the ParentGroupId field
        /// </summary>
        [DisplayName("ParentGroupId")]
        [DefaultValue(-1)]
        public int ParentGroupId { get { return parentGroupId; } set { parentGroupId = value; } }


        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; } }


        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; } }



        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
    }
}
