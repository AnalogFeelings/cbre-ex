using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Components;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Mutation;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.Modification
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Tools", "", "Transform", "D")]
    [CommandID("BspEditor:Tools:Transform")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Transform))]
    [DefaultHotkey("Ctrl+M")]
    public class Transform : BaseCommand
    {
        [Import] private Lazy<ITranslationStringProvider> _translator;

        public override string Name { get; set; } = "Transform";
        public override string Details { get; set; } = "Transform the current selection";

        public string ErrorCannotScaleByZeroTitle { get; set; } = "Cannot scale by zero";
        public string ErrorCannotScaleByZeroMessage { get; set; } = "Please enter a non-zero value for all axes when scaling.";

        protected override bool IsInContext(IContext context, MapDocument document)
        {
            return base.IsInContext(context, document) && !document.Selection.IsEmpty;
        }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            System.Collections.Generic.List<IMapObject> objects = document.Selection.GetSelectedParents().ToList();
            DataStructures.Geometric.Box box = document.Selection.GetSelectionBoundingBox();

            using (TransformDialog dialog = new TransformDialog(box))
            {
                _translator.Value.Translate(dialog);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Transaction transaction = new Transaction();

                        // Add the operation
                        System.Numerics.Matrix4x4 transform = dialog.GetTransformation(box);
                        BspEditor.Modification.Operations.Mutation.Transform transformOperation = new BspEditor.Modification.Operations.Mutation.Transform(transform, objects);
                        transaction.Add(transformOperation);

                        // Check for texture transform
                        TransformationFlags tl = document.Map.Data.GetOne<TransformationFlags>() ?? new TransformationFlags();
                        if (dialog.Type == TransformDialog.TransformType.Rotate || dialog.Type == TransformDialog.TransformType.Translate)
                        {
                            if (tl.TextureLock) transaction.Add(new TransformTexturesUniform(transform, objects.SelectMany(x => x.FindAll())));
                        }
                        else if (dialog.Type == TransformDialog.TransformType.Scale)
                        {
                            if (tl.TextureScaleLock) transaction.Add(new TransformTexturesScale(transform, objects.SelectMany(x => x.FindAll())));
                        }

                        await MapDocumentOperation.Perform(document, transaction);
                    }
                    catch (TransformDialog.CannotScaleByZeroException)
                    {
                        MessageBox.Show(ErrorCannotScaleByZeroMessage, ErrorCannotScaleByZeroTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}