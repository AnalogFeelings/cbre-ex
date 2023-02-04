using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;
using CBRE.DataStructures.GameData;
using CBRE.Shell;

namespace CBRE.BspEditor.Tools.Entity
{
    [AutoTranslate]
    [Export(typeof(ISidebarComponent))]
    [OrderHint("F")]
    public partial class EntitySidebarPanel : UserControl, ISidebarComponent
    {
        public string Title { get; set; } = "Entities";
        public object Control => this;

        public string EntityTypeLabelText
        {
            get => EntityTypeLabel.Text;
            set { EntityTypeLabel.InvokeLater(() => { EntityTypeLabel.Text = value; }); }
        }

        public EntitySidebarPanel()
        {
            InitializeComponent();
            CreateHandle();

            Oy.Subscribe<MapDocument>("Document:Activated", d => { this.InvokeLater(() => RefreshEntities(d)); });
            Oy.Subscribe<EntityTool>("EntityTool:ResetEntityType", t => { this.InvokeLater(() => ResetEntityType(t)); });
        }

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveTool", out EntityTool _);
        }

        private async Task RefreshEntities(MapDocument doc)
        {
            if (doc == null) return;

            var sel = GetSelectedEntity()?.Name;
            var gameData = await doc.Environment.GetGameData();
            var defaultName = doc.Environment.DefaultPointEntity ?? "";
            
            EntityTypeLabel.InvokeLater(() =>
            {
                EntityTypeList.BeginUpdate();
                EntityTypeList.Items.Clear();

                var def = doc.Environment.DefaultPointEntity;
                GameDataObject reselect = null, redef = null;
                foreach (var gdo in gameData.Classes.Where(x => x.ClassType == ClassType.Point).OrderBy(x => x.Name.ToLowerInvariant()))
                {
                    EntityTypeList.Items.Add(gdo);
                    if (String.Equals(sel, gdo.Name, StringComparison.InvariantCultureIgnoreCase)) reselect = gdo;
                    if (String.Equals(def, gdo.Name, StringComparison.InvariantCultureIgnoreCase)) redef = gdo;
                }

                if (reselect == null && redef == null)
                {
                    redef = gameData.Classes
                        .Where(x => x.ClassType == ClassType.Point)
                        .OrderBy(x => x.Name == defaultName ? 0 : (x.Name.StartsWith("info_player_start") ? 1 : 2))
                        .FirstOrDefault();
                }

                EntityTypeList.SelectedItem = reselect ?? redef;
                EntityTypeList.EndUpdate();
            });
            EntityTypeList_SelectedIndexChanged(null, null);
        }

        private async Task ResetEntityType(EntityTool tool)
        {
            var doc = tool.GetDocument();
            if (doc == null) return;

            var gameData = await doc.Environment.GetGameData();
            var defaultName = doc.Environment.DefaultPointEntity ?? "";

            var redef = gameData.Classes
                .Where(x => x.ClassType == ClassType.Point)
                .OrderBy(x => x.Name == defaultName ? 0 : (x.Name.StartsWith("info_player_start") ? 1 : 2))
                .FirstOrDefault();
            if (redef != null)
            {
                EntityTypeList.SelectedItem = redef;
            }
        }

        private GameDataObject GetSelectedEntity()
        {
            return EntityTypeList.SelectedItem as GameDataObject;
        }

        private void EntityTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Oy.Publish("Context:Add", new ContextInfo("EntityTool:ActiveEntity", GetSelectedEntity()?.Name));
        }
    }
}
