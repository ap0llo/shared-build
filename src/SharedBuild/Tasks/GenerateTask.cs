using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.Generate)]
[TaskDescription("Updates files under source control")]
public class GenerateTask : FrostingTask;
