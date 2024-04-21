using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace DotnetLambdaCdkPipeline
{
    class SampleLambdaStack : Stack
    {
        public SampleLambdaStack(Construct scope, string id, StackProps props = null) : base(scope, id, props)
        {
            // Commands executed in a AWS CDK pipeline to build, package, and extract a .NET function.
            var buildCommands = new[]
            {
                "cd /asset-input",
                "export DOTNET_CLI_HOME=\"/tmp/DOTNET_CLI_HOME\"",
                "export PATH=\"$PATH:/tmp/DOTNET_CLI_HOME/.dotnet/tools\"",
                "dotnet build",
                "dotnet tool install -g Amazon.Lambda.Tools",
                "dotnet lambda package -o output.zip",
                "unzip -o -d /asset-output output.zip"
            };

            new Function(this, "LambdaFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_6,
                Handler = "SampleLambda::SampleLambda.Function::FunctionHandler",

                // Asset path should point to the folder where .csproj file is present.
                // Also, this path should be relative to cdk.json file.
                Code = Code.FromAsset("./src/SampleLambda/src/SampleLambda", new Amazon.CDK.AWS.S3.Assets.AssetOptions
                {
                    Bundling = new BundlingOptions
                    {
                        Image = Runtime.DOTNET_6.BundlingImage,
                        Command = new[]
                        {
                            "bash", "-c", string.Join(" && ", buildCommands)
                        }
                    }
                })
            });
        }
    }
}
