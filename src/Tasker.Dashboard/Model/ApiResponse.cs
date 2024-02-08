using System.Text.Json.Serialization;

namespace Tasker.Dashboard.Model;

public class ApiResponse<T>
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("data")] public T Data { get; set; }
}