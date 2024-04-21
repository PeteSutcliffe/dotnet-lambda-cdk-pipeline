using Amazon.CDK;
using Constructs;

namespace DotnetLambdaCdkPipeline
{
    public class DotnetLambdaCdkPipelineStage : Stage
    {
        internal DotnetLambdaCdkPipelineStage(Construct scope, string id, IStageProps props = null) : base(scope, id, props)
        {
            Stack lambdaStack = new SampleLambdaStack(this, "LambdaStack");
        }
    }
}