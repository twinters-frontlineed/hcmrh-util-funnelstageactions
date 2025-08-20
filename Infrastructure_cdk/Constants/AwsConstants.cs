namespace Infrastructure_cdk.Constants
{
    public static class AwsConstants
    {
        public static readonly string DEPLOYMENT_PIPELINE_ID = "DeploymentPipeline";
        public static readonly string CODE_PIPELINE_ID = "FunnelStageAction";
        public static readonly string CODE_PIPELINE_NAME = "Funnel_Stage_Action";
        public static readonly string FUNNEL_STAGE_ACTION_QUEUE_ID = "FunnelStageActionQueue";
        public static readonly string FUNNEL_STAGE_ACTION_DEAD_LETTER_QUEUE_ID = "FunnelStageActionQueue_DeadLetter";
        public static readonly string REGION = "us-east-1";
    }

    public static class AwsAccounts
    {
        public static readonly string DEVELOPMENT = "671735518696";
        public static readonly string STAGING = "658713976729";
        public static readonly string PRODUCTION = "729029490000";
        public static readonly string CICD = "614626420141";
    }
}
