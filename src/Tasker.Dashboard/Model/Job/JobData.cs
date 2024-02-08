using System;
using System.ComponentModel;

namespace Tasker.Dashboard.Model.Job;

public class JobData
{
    [DisplayName("Id")] public int Id { get; set; }
    [DisplayName("Name")] public string Name { get; set; }
    [DisplayName("Maximum Retries")] public int MaximumRetries { get; set; }
    [DisplayName("Currently Retries")] public int CurrentlyRetries { get; set; }
    [DisplayName("Status")] public int StatusId { get; set; }
    [DisplayName("Creation Date")] public DateTime CreationDate { get; set; }
    [DisplayName("Last Update")] public DateTime? LastUpdate { get; set; }
    [DisplayName("Last Execution")] public DateTime? LastExecution { get; set; }
    [DisplayName("Executed By")] public string ExecutedBy { get; set; }
    [DisplayName("Cron Expression")] public string CronExpression { get; set; }
    [DisplayName("Interval In Minutes")] public int IntervalInMinutes { get; set; }
}