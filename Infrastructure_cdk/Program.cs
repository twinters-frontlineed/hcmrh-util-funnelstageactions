using Amazon.CDK;
using Infrastructure_cdk.Constants;
using Infrastructure_cdk.Stacks;

var app = new App();

new DeploymentPipelineStack(app, AwsConstants.DEPLOYMENT_PIPELINE_ID, new StackProps
{
    Env = new Amazon.CDK.Environment
    {
        Account = AwsAccounts.CICD,
        Region = AwsConstants.REGION
    }
});

app.Synth();