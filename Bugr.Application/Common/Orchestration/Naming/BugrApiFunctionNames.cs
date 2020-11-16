namespace Bugr.Application.Common.Orchestration.Naming
{
	public static class BugrReminderApiFunctionNames
	{
		public const string ProcessSmsResponseWebHookOrchestrationTrigger = "ProcessSmsResponseWebHookOrchestrationTrigger";
		public const string BugrReminderOrchestrationTrigger = "BugrReminderOrchestrationTrigger";

		public const string GenerateBugrMessageActivityFunction = "GenerateBugrMessageActivityFunction";
		public const string AuthenticateWebHookRequestActivityFunction = "AuthenticateWebHookRequestActivityFunction";
		public const string SendBugrMessageActivityFunction = "SendBugrMessageActivityFunction";
		public const string ProcessSmsWebhookResponseActivityFunction = "ProcessSmsWebhookResponseActivityFunction";
		public const string PurgePreviousOrchestrationHistory = "PurgePreviousOrchestrationHistory";

		public const string BugrReminderTimerTriggerFunctionOrchestrator = "BugrReminderTimerTriggerFunctionOrchestrator";
		public const string ProcessSmsResponseWebHookHttpTriggerOrchestrator = "ProcessSmsResponseWebHookHttpTriggerOrchestrator";
	}
}
