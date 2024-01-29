using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPA.UI.Controls
{
    public partial class ModelSettingViewer : DevExpress.XtraEditors.XtraUserControl
    {
        public ModelSettingViewer()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.Bind모델정보.DataSource = Global.모델자료;
            this.Refresh();
        }
    }
}
