using Amazon.CDK;
using Amazon.CDK.Pipelines;
using Constructs;
using Infrastructure_cdk.Constants;
using Infrastructure_cdk.Stages;

namespace Infrastructure_cdk.Stacks
{
    internal class DeploymentPipelineStack : Stack
    {
        internal DeploymentPipelineStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var source = CodePipelineSource.GitHub("FrontlineEducation/hcmrh-util-funnelstageactions", "main", new GitHubSourceOptions
            {
                Authentication = SecretValue.SecretsManager("github-token")
            });

            var pipeline = new CodePipeline(this, AwsConstants.CODE_PIPELINE_ID, new CodePipelineProps
            {
                PipelineName = AwsConstants.CODE_PIPELINE_NAME,
                CrossAccountKeys = true,
                SelfMutation = true,
                Synth = new ShellStep("Synth", new ShellStepProps
                {
                    Input = source,
                    InstallCommands = new[]
                    {
                        "npm install -g aws-cdk",
                    },
                    Commands = new[] {
                        "dotnet build -c Release",
                        "dotnet publish -c Release -r linux-musl-x64",
                        "cdk synth"
                    }
                })
            });

            var createdBy = source.ToString().Replace("(", "").Replace(")", "");

            pipeline.AddStage(new FunnelStageAction(this, Environments.STAGING, new FunnelStageActionProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = AwsAccounts.STAGING,
                    Region = AwsConstants.REGION
                },
                CreatedBy = createdBy,
                Environment = "aws_stage"
            }),

            new AddStageOpts
            {
                Pre = new[] {
                    new ManualApprovalStep("Promote to Stage")
                }
            });

            pipeline.AddStage(new FunnelStageAction(this, Environments.PRODUCTION, new FunnelStageActionProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = AwsAccounts.PRODUCTION,
                    Region = AwsConstants.REGION
                },
                CreatedBy = createdBy,
                Environment = "aws_prod"
            }),

            new AddStageOpts
            {
                Pre = new[] {
                    new ManualApprovalStep("Promote to Production")
                }
            });
        }
    }
}
