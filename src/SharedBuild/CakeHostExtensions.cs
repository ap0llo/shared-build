using System;
using System.Linq;
using System.Reflection;
using Cake.Frosting;
using Microsoft.Extensions.DependencyInjection;

namespace Grynwald.SharedBuild;

public static class CakeHostExtensions
{
    /// <summary>
    /// Adds the shared build tasks and modules to the Cake host
    /// </summary>
    /// <typeparam name="TContext">The concrete build context type to use. Must implement <see cref="IBuildContext"/></typeparam>
    /// <param name="host">The Cake host to use.</param>
    /// <param name="taskFilter">Optional filter for the build tasks to add. When no filter is specified, all tasks are added.</param>
    public static CakeHost UseSharedBuild<TContext>(this CakeHost host, Func<Type, bool>? taskFilter = null) where TContext : class, IBuildContext
    {
        return host
            .UseContext<TContext>()
            .AddAssembly(typeof(CakeHostExtensions).Assembly, taskFilter);
    }

    /// <summary>
    /// Adds tasks from the specified assembly to the Cake host.
    /// </summary>
    /// <param name="host">The Cake host to add the tasks to.</param>
    /// <param name="assembly">The assembly to load tasks from.</param>
    /// <param name="taskFilter">Optional filter for the build tasks to add. When no filter is specified, all tasks are added.</param>
    public static CakeHost AddAssembly(this CakeHost host, Assembly assembly, Func<Type, bool>? taskFilter = null)
    {
        taskFilter ??= (t => true);

        var taskTypes = assembly.GetExportedTypes()
            .Where(type => type.IsAssignableTo(typeof(IFrostingTask)) && type.IsClass && type.IsPublic)
            .Where(taskFilter);

        foreach (var task in taskTypes)
        {
            host.ConfigureServices(services => services.AddSingleton(typeof(IFrostingTask), task));
        }

        return host;
    }
}
