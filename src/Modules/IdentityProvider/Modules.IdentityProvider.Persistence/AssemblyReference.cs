using System.Reflection;

namespace Modules.IdentityProvider.Persistence;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
