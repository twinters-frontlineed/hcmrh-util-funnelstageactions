using Amazon.CDK;
using Infrastructure_cdk.Constants;
using Infrastructure_cdk.Stacks;

var app = new App();


if (app.Node.TryGetContext("env")?.ToString()?.Equals("dev", StringComparison.CurrentCultureIgnoreCase) ?? false)
{
    Console.WriteLine("Dev environment synth");
    new FunnelStageActionStack(app, new StackProps
    {
        Env = new Amazon.CDK.Environment
        {
            Account = AwsAccounts.DEVELOPMENT,
            Region = AwsConstants.REGION
        }
    });
}
else
{
    Console.WriteLine("Non-dev environment synth");
    new DeploymentPipelineStack(app, AwsConstants.DEPLOYMENT_PIPELINE_ID, new StackProps
    {
        Env = new Amazon.CDK.Environment
        {
            Account = AwsAccounts.CICD,
            Region = AwsConstants.REGION
        }
    });
}

app.Synth();