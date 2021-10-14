using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName("Demo")]
    public class DemoTask : FrostingTask<IBuildContext>
    {
        public override void Run(IBuildContext context)
        {
            context.Log.Information(context.GetType().FullName);
            base.Run(context);
        }


    }
}
