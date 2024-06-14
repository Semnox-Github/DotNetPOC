using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public class ThemeUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static System.Drawing.Color SkinColor = System.Drawing.Color.White;
        public static int UIProfile = 1;

        public static void SetupVisuals(Control c)
        {
            log.LogMethodEntry();
            string type = c.GetType().ToString().ToLower();

            if (c.HasChildren)
            {
                c.BackColor = SkinColor;
                foreach (Control cc in c.Controls)
                {
                    SetupVisuals(cc);
                }
            }
            if (type.Contains("radiobutton"))
            {
                ;
            }
            else if (type.Contains("forms.button"))
            {
                setupButtonVisuals((Button)c);
            }
            else if (type.Contains("tabpage"))
            {
                TabPage tp = (TabPage)c;
                tp.BackColor = SkinColor;
            }
            else if (type == "system.windows.forms.datagridview")
            {
                DataGridView dg = (DataGridView)c;
                dg.BackgroundColor = SkinColor;
            }
            log.LogMethodExit();
        }

        public static void setupButtonVisuals(Button b)
        {
            log.LogMethodEntry();
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseDownBackColor =
            b.FlatAppearance.MouseOverBackColor =
            b.BackColor = System.Drawing.Color.Transparent;
            b.Font = new System.Drawing.Font("arial", 8.5f);
            b.ForeColor = System.Drawing.Color.Black;
            if (b.Width < 100)
                b.Width = 90;
            b.Height = 25;
            b.BackgroundImageLayout = ImageLayout.Stretch;
            if (UIProfile == 2)
                b.BackgroundImage = Properties.Resources.ButtonNav2Normal;
            else
                b.BackgroundImage = Properties.Resources.normal3;

            b.MouseDown += b_MouseDown;
            b.MouseUp += b_MouseUp;
            log.LogMethodExit();
        }

        static void b_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            if (UIProfile == 2)
                b.BackgroundImage = Properties.Resources.ButtonNav2Normal;
            else
                b.BackgroundImage = Properties.Resources.normal3;
            b.ForeColor = System.Drawing.Color.Black;
            log.LogMethodExit();
        }

        static void b_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            if (UIProfile == 2)
                b.BackgroundImage = Properties.Resources.ButtonNav2Pressed;
            else
                b.BackgroundImage = Properties.Resources.pressed3;
            b.ForeColor = System.Drawing.Color.White;
            log.LogMethodExit();
        }
    }
}
