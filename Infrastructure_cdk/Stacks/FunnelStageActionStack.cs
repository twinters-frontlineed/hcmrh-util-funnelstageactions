using Amazon.CDK;
using Amazon.CDK.AWS.SQS;
using Constructs;
using Infrastructure_cdk.Constants;

namespace Infrastructure_cdk.Stacks
{
    internal class FunnelStageActionStack: Stack
    {
        internal FunnelStageActionStack(Construct scope, StackProps props) : base(scope, AwsConstants.FUNNEL_STAGE_ACTION_STACK_ID, props)
        {
            var deadLetterQueue = new Queue(this, AwsConstants.FUNNEL_STAGE_ACTION_DEAD_LETTER_QUEUE_ID, new QueueProps
            {
                QueueName = AwsConstants.FUNNEL_STAGE_ACTION_DEAD_LETTER_QUEUE_ID,
                RetentionPeriod = Duration.Days(14)
            });

            var funnelStageActionQueue = new Queue(this, AwsConstants.FUNNEL_STAGE_ACTION_QUEUE_ID, new QueueProps
            {
                QueueName = AwsConstants.FUNNEL_STAGE_ACTION_QUEUE_ID,
                DeadLetterQueue = new DeadLetterQueue() { Queue = deadLetterQueue, MaxReceiveCount = AwsConstants.DLQ_MAX_RECEIVE_COUNT }
            });
        }
    }
}
