using System.Reflection;

namespace Modules.IdentityProvider.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
