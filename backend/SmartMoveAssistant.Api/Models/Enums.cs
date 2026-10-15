using System.Text.Json.Serialization;

namespace SmartMoveAssistant.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionIntent
{
    Start,
    Stop,
    Move
}
