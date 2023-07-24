using CBRE.Common.Translations;
using CBRE.Providers.GameData;
using CBRE.Providers.Texture;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.BspEditor.Environment.Blitz
{
    public partial class BlitzEnvironmentEditor : UserControl, IEnvironmentEditor
    {
        public event EventHandler EnvironmentChanged;
        public Control Control => this;

        private readonly IGameDataProvider _fgdProvider = Common.Container.Get<IGameDataProvider>("Fgd");
        private readonly ITexturePackageProvider _wadProvider = Common.Container.Get<ITexturePackageProvider>("Generic");

        public IEnvironment Environment
        {
            get => GetEnvironment();
            set => SetEnvironment(value as BlitzEnvironment);
        }

        public BlitzEnvironmentEditor()
        {
            InitializeComponent();

            cmbDefaultPointEntity.SelectedIndexChanged += OnEnvironmentChanged;
            cmbDefaultBrushEntity.SelectedIndexChanged += OnEnvironmentChanged;

            texturesGrid.CellEndEdit += TexturesGrid_CellEndEdit;
            texturesGrid.EditingControlShowing += TexturesGrid_EditingControlShowing;
            texturesGrid.CellClick += TexturesGrid_CellClick;

            modelsGrid.CellEndEdit += ModelsGrid_CellEndEdit;
            modelsGrid.EditingControlShowing += ModelsGrid_EditingControlShowing;
            modelsGrid.CellClick += ModelsGrid_CellClick;

            nudDefaultTextureScale.ValueChanged += OnEnvironmentChanged;
        }

        private void ModelsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                OpenBrowseDirDialog((dir) =>
                {
                    modelsGrid.Rows[e.RowIndex].Cells[1].Value = dir;

                    DirsChanged(modelsGrid);
                });
            }
            else if (e.ColumnIndex == 0)
            {
                modelsGrid.Rows[e.RowIndex].Cells[1].Value = string.Empty;

                DirsChanged(modelsGrid);
            }
        }

        private void ModelsGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl textBox)
            {
                textBox.PreviewKeyDown -= ModelsGrid_KeyDown;
                textBox.PreviewKeyDown += ModelsGrid_KeyDown;
            }
        }

        private void ModelsGrid_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BeginInvoke(new MethodInvoker(() => DirsChanged(modelsGrid)));
            }
        }

        private void ModelsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() => DirsChanged(modelsGrid)));
        }

        private void TexturesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                OpenBrowseDirDialog((dir) =>
                {
                    texturesGrid.Rows[e.RowIndex].Cells[1].Value = dir;

                    DirsChanged(texturesGrid);
                });
            }
            else if (e.ColumnIndex == 0)
            {
                texturesGrid.Rows[e.RowIndex].Cells[1].Value = string.Empty;

                DirsChanged(texturesGrid);
            }
        }

        private void TexturesGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl textBox)
            {
                textBox.PreviewKeyDown -= TexturesGrid_KeyDown;
                textBox.PreviewKeyDown += TexturesGrid_KeyDown;
            }
        }

        private void TexturesGrid_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BeginInvoke(new MethodInvoker(() => DirsChanged(texturesGrid)));
            }
        }

        private void TexturesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() => DirsChanged(texturesGrid)));
        }

        public void Translate(ITranslationStringProvider strings)
        {
            CreateHandle();
            var prefix = GetType().FullName;

            grpDirectories.Text = strings.GetString(prefix, "Directories");
            grpFgds.Text = strings.GetString(prefix, "Settings");

            lblDefaultPointEntity.Text = strings.GetString(prefix, "DefaultPointEntity");
            lblDefaultBrushEntity.Text = strings.GetString(prefix, "DefaultBrushEntity");

            lblDefaultTextureScale.Text = strings.GetString(prefix, "DefaultTextureScale");
        }

        private void OnEnvironmentChanged(object sender, EventArgs e)
        {
            EnvironmentChanged?.Invoke(this, e);
        }

        public void SetEnvironment(BlitzEnvironment env)
        {
            if (env == null) env = new BlitzEnvironment();

            texturesGrid.Rows.Clear();
            modelsGrid.Rows.Clear();

            foreach (string textureDir in env.TextureDirectories)
            {
                if (string.IsNullOrEmpty(textureDir)) continue;

                AddDirsRow(texturesGrid, textureDir);
            }

            AddDirsRow(texturesGrid, string.Empty);

            foreach (string modelDir in env.ModelDirectories)
            {
                if (string.IsNullOrEmpty(modelDir)) continue;

                AddDirsRow(modelsGrid, modelDir);
            }

            AddDirsRow(modelsGrid, string.Empty);

            cmbDefaultPointEntity.SelectedItem = env.DefaultPointEntity;
            cmbDefaultBrushEntity.SelectedItem = env.DefaultBrushEntity;

            nudDefaultTextureScale.Value = env.DefaultTextureScale;
        }

        public BlitzEnvironment GetEnvironment()
        {
            return new BlitzEnvironment()
            {
                TextureDirectories = GetDirs(texturesGrid),
                ModelDirectories = GetDirs(modelsGrid),

                DefaultPointEntity = Convert.ToString(cmbDefaultPointEntity.SelectedItem, CultureInfo.InvariantCulture),
                DefaultBrushEntity = Convert.ToString(cmbDefaultBrushEntity.SelectedItem, CultureInfo.InvariantCulture),

                DefaultTextureScale = nudDefaultTextureScale.Value
            };
        }

        private List<string> GetDirs(DataGridView dataGridView)
        {
            List<string> result = new List<string>();

            foreach(DataGridViewRow row in dataGridView.Rows)
            {
                string dir = row.Cells[1].Value as string;

                if (string.IsNullOrEmpty(dir)) continue;

                result.Add(dir);
            }

            return result;
        }

        private void DirsChanged(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView.Rows[i];
                string dir = row.Cells[1].Value as string;

                if (!string.IsNullOrEmpty(dir))
                {
                    dir = dir.Replace('\\', '/');

                    if (dir.Last() != '/')
                    {
                        dir += "/";
                    }
                }
                if (Directory.Exists(dir))
                {
                    row.Cells[1].Value = dir;

                    if (i >= dataGridView.Rows.Count - 1)
                    {
                        int newRowInd = AddDirsRow(dataGridView, "");

                        if (dataGridView.CurrentRow.Index == i)
                        {
                            dataGridView.CurrentCell = dataGridView.Rows[newRowInd].Cells[1];
                        }
                    }
                }
                else if (i < dataGridView.Rows.Count - 1)
                {
                    dataGridView.Rows.RemoveAt(i);

                    i--;
                }
                else
                {
                    row.Cells[1].Value = "";
                }
            }

            OnEnvironmentChanged(this, EventArgs.Empty);
        }

        private int AddDirsRow(DataGridView dataGridView, string dir)
        {
            int r = dataGridView.Rows.Add("X", dir, "...");

            return r;
        }

        private void OpenBrowseDirDialog(Action<string> callback)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                BeginInvoke(new MethodInvoker(() => callback?.Invoke(dialog.FileName)));
            }
        }
    }
}
