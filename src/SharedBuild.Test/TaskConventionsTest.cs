using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Frosting;
using Grynwald.SharedBuild.Tasks;
using Xunit;

namespace Grynwald.SharedBuild.Test;

/// <summary>
/// Tests verifying all tasks follow the task conventions
/// </summary>
public class TaskConventionsTest
{
    public static IEnumerable<object[]> TaskTypes()
    {
        return typeof(DefaultTask).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IFrostingTask)))
            .Select(t => new object[] { t });
    }



    [Theory]
    [MemberData(nameof(TaskTypes))]
    public void Tasks_must_be_reference_types(Type type)
    {
        Assert.True(type.IsClass);
    }

    [Theory]
    [MemberData(nameof(TaskTypes))]
    public void Tasks_have_a_TaskName_attribute(Type type)
    {
        var taskNameAttribute = type.GetCustomAttribute<TaskNameAttribute>();

        Assert.NotNull(taskNameAttribute);
    }

    [Theory]
    [MemberData(nameof(TaskTypes))]
    public void Task_type_names_have_a_task_suffix(Type type)
    {
        Assert.EndsWith("Task", type.Name);
    }

    [Theory]
    [MemberData(nameof(TaskTypes))]
    public void Task_name_matches_type_name(Type type)
    {
        var actualTaskName = type.GetCustomAttribute<TaskNameAttribute>()?.Name;
        var typeName = type.Name;

        var expectedTaskName = typeName.EndsWith("Task")
            ? typeName.Substring(0, typeName.Length - "Task".Length)
            : typeName;


        Assert.Equal(expectedTaskName, actualTaskName);
    }

    [Theory]
    [MemberData(nameof(TaskTypes))]
    public void Task_name_exists_in_TaskNames_class(Type type)
    {
        var fields = typeof(TaskNames).GetFields();

        var taskName = type.GetCustomAttribute<TaskNameAttribute>()?.Name;


        Assert.Contains(taskName, fields.Select(x => x.Name));
        var field = fields.Single(x => x.Name == taskName);

        Assert.Equal(field.GetValue(null), taskName);
    }

    public static IEnumerable<object[]> TaskNames()
    {
        return typeof(TaskNames).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(field => field.GetValue(null)!)
            .Select(name => new object[] { name });
    }

    [Theory]
    [MemberData(nameof(TaskNames))]
    public void Task_defined_in_TaskNames_exists(string taskName)
    {
        var taskNames = typeof(DefaultTask).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IFrostingTask)))
            .Select(t => t.GetCustomAttribute<TaskNameAttribute>()?.Name)
            .Where(name => !String.IsNullOrEmpty(name))
            .ToArray();

        Assert.Contains(taskName, taskNames);
    }
}
