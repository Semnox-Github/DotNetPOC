using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Ui control to display the criteria
    /// </summary>
    public class CriteriaUI : FlowLayoutPanel
    {
        private Criteria criteria;

        private Label label;

        private List<CriteriaUI> criteriaUIList;

        private CheckBox checkBox;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaUI(Criteria criteria)
        {
            this.criteria = criteria;
            FlowDirection = FlowDirection.LeftToRight;
            this.AutoSize = true;
            BorderStyle = BorderStyle.FixedSingle;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            checkBox = new CheckBox();
            checkBox.Width = 15;
            Controls.Add(checkBox);
            label = new Label();
            Controls.Add(label);
            label.AutoSize = true;
            label.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            if (criteria is ComplexCriteria)
            {
                ComplexCriteria complexCriteria = criteria as ComplexCriteria;
                label.Text = complexCriteria.GetLogialOperator();
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.AutoSize = true;
                flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Controls.Add(flowLayoutPanel);
                criteriaUIList = new List<CriteriaUI>();
                foreach (var criteriaItem in complexCriteria.GetCriteriaList())
                {
                    CriteriaUI creteriaUI = new CriteriaUI(criteriaItem is SearchCriteria? (criteriaItem as SearchCriteria).GetCriteria() : criteriaItem);
                    criteriaUIList.Add(creteriaUI);
                    flowLayoutPanel.Controls.Add(creteriaUI);
                }
            }
            else if (criteria is NotCriteria)
            {
                label.Text = "NOT";
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.AutoSize = true;
                flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Controls.Add(flowLayoutPanel);
                criteriaUIList = new List<CriteriaUI>();
                Criteria childCriteria = (criteria as NotCriteria).GetCriteria();
                CriteriaUI creteriaUI = new CriteriaUI(childCriteria is SearchCriteria ? (childCriteria as SearchCriteria).GetCriteria() : childCriteria);
                criteriaUIList.Add(creteriaUI);
                flowLayoutPanel.Controls.Add(creteriaUI);
            }
            else if (criteria is SearchCriteria)
            {
                label.Text = "";
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.AutoSize = true;
                flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Controls.Add(flowLayoutPanel);
                criteriaUIList = new List<CriteriaUI>();
                CriteriaUI creteriaUI = new CriteriaUI((criteria as SearchCriteria).GetCriteria());
                criteriaUIList.Add(creteriaUI);
                flowLayoutPanel.Controls.Add(creteriaUI);
            }
            else
            {
                label.Text = criteria.GetDisplayClause();
            }
        }
        
        /// <summary>
        /// returns the selected criteria
        /// </summary>
        /// <returns></returns>
        public Criteria GetSelectedCriteria()
        {
            Criteria selectedCriteria = null;
            if(checkBox.Checked)
            {
                selectedCriteria = criteria;
            }
            else
            {
                if(criteriaUIList != null)
                {
                    foreach (var criteriaUI in criteriaUIList)
                    {
                        selectedCriteria = criteriaUI.GetSelectedCriteria();
                        if(selectedCriteria != null)
                        {
                            break;
                        }
                    }
                }
            }
            return selectedCriteria;
        }
    }
}
