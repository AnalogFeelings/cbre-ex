using System;
using System.Windows.Forms;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Environment
{
    public interface IEnvironmentEditor : IManualTranslate
    {
        event EventHandler EnvironmentChanged;
        Control Control { get; }
        IEnvironment Environment { get; set; }
    }
}