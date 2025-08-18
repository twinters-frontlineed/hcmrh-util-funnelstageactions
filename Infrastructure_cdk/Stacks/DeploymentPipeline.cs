using Amazon.CDK;
using Amazon.CDK.AWS.Chatbot;
using Amazon.CDK.AWS.CodeStarNotifications;
using Amazon.CDK.Pipelines;
using Infrastructure_cdk.Constants;

namespace Infrastructure_cdk.Stacks
{
    internal class DeploymentPipeline : Stack
    {
        internal DeploymentPipeline()
        {
            var source = CodePipelineSource.GitHub("FrontlineEducation/Infrastructure-cdk", "main", new GitHubSourceOptions
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

            pipeline.AddStage(new AppFunnelStageAction(this, Environments.DEVELOPMENT, new AppHireFileTransferStageProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = "671735518696",
                    Region = "us-east-1"
                },
                CreatedBy = createdBy,
                Environment = "aws_dev",
                RecruitApiUri = "http://localhost:5000/",
                ApiServiceAccountArn = "arn:aws:iam::756759416234:role/k8-qa-api-recruit-sa-role"
            }));


            pipeline.AddStage(new AppHireFileTransfer(this, Environments.STAGING, new AppHireFileTransferStageProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = "658713976729",
                    Region = "us-east-1"
                },
                CreatedBy = createdBy,
                Environment = "aws_stage",
                RecruitApiUri = "https://internal-api-proxy.ss.frontlineeducation.com/recruit-qa/api/",     // Proxy to api-recruit-qa.ss.frontlineeducation.com/{proxy}
                ApiServiceAccountArn = "arn:aws:iam::756759416234:role/k8-qa-api-recruit-sa-role"
            }),

            new AddStageOpts
            {
                Pre = new[] {
                    new ManualApprovalStep("Promote to Stage")
                }
            });

            pipeline.AddStage(new AppHireFileTransfer(this, Environments.PRODUCTION, new AppHireFileTransferStageProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = "729029490000",
                    Region = "us-east-1"
                },
                CreatedBy = createdBy,
                Environment = "aws_prod",
                RecruitApiUri = "https://internal-api-proxy.use1.frontlineeducation.com/recruit-prod/api/",     // Proxy to api-recruit.ss.frontlineeducation.com/{proxy}
                ApiServiceAccountArn = "arn:aws:iam::381203884180:role/k8-prod-api-recruit-sa-role"
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
