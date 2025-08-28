using Amazon.CDK;
using Constructs;
using Infrastructure_cdk.Stacks;

namespace Infrastructure_cdk.Stages;

public class FunnelStageActionProps: StageProps
{
    public string CreatedBy;
    public string Environment;
}

public class FunnelStageAction : Stage
{
    public FunnelStageAction(Construct scope, string id, FunnelStageActionProps props) : base(scope, id, props)
    {
        new FunnelStageActionStack(this, new StackProps
        {
            Env = props.Env
        });

        // Tagging
        Tags.Of(this).Add("Environment", props.Environment);
        Tags.Of(this).Add("Owner", "DIST_Technology_SaaSIO_DevOps@frontlineed.com");
        Tags.Of(this).Add("Created_by", props.CreatedBy);
        Tags.Of(this).Add("FrontlineProduct", "recruit");
    }
}