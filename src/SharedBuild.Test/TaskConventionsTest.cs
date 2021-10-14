using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Frosting;
using Grynwald.SharedBuild.Tasks;
using Xunit;

namespace Grynwald.SharedBuild.Test
{
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
    }
}
