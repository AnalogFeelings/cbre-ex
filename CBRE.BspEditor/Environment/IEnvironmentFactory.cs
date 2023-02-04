using System;

namespace CBRE.BspEditor.Environment
{
    public interface IEnvironmentFactory
    {
        Type Type { get; }
        string TypeName { get; }
        string Description { get; }

        IEnvironment Deserialise(SerialisedEnvironment environment);
        SerialisedEnvironment Serialise(IEnvironment environment);

        IEnvironment CreateEnvironment();
        IEnvironmentEditor CreateEditor();
    }
}