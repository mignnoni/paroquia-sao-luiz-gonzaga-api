using System;
using System.Reflection;

namespace Modules.ParishManagement.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
