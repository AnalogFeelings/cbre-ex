using CBRE.Common.Native;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Translations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CBRE.Shell.Forms
{
    public partial class SaveChangesForm : Form
    {
        public SaveChangesForm(List<IDocument> unsaved)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            foreach (IDocument document in unsaved)
            {
                DocumentList.Items.Add(document.Name + " *");
            }

            StockIconInfo stockIconInfo = new StockIconInfo();
            stockIconInfo.cbSize = (uint)Marshal.SizeOf(typeof(StockIconInfo));
            StockIcon.SHGetStockIconInfo(StockIconId.SIID_HELP, StockIconFlags.SHGSI_ICON | StockIconFlags.SHGSI_SHELLICONSIZE, ref stockIconInfo);

            systemBitmap.Image = Icon.FromHandle(stockIconInfo.hIcon).ToBitmap();

            CreateHandle();
        }

        private void SaveAllClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void DiscardAllClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void Translate(ITranslationStringProvider translation)
        {
            this.InvokeLater(() =>
            {
                SaveAllButton.Text = translation.GetString(typeof(SaveChangesForm).FullName + ".SaveAll");
                DiscardButton.Text = translation.GetString(typeof(SaveChangesForm).FullName + ".DiscardAll");
                CancelButton.Text = translation.GetString(typeof(SaveChangesForm).FullName + ".Cancel");
                UnsavedChangesLabel.Text = translation.GetString(typeof(SaveChangesForm).FullName + ".UnsavedChangesMessage");
                Text = translation.GetString(typeof(SaveChangesForm).FullName + ".Title");
            });
        }
    }
}
