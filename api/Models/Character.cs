using System.Text.Json.Serialization;

namespace API.Models;

public record Character(
    [property: JsonPropertyName("id")] int Id = 0,
    [property: JsonPropertyName("actorDatabaseId")] int ActorDatabaseId = 0,
    [property: JsonPropertyName("firstName")] string FirstName = "",
    [property: JsonPropertyName("lastName")] string LastName = "",
    [property: JsonPropertyName("age")] int Age = 0,
    [property: JsonPropertyName("gender")] string Gender = "",
    [property: JsonPropertyName("appearance")] string Appearance = "",
    [property: JsonPropertyName("occupation")] string Occupation = "",
    [property: JsonPropertyName("traits")] string Traits = "",
    [property: JsonPropertyName("moralAlignment")] string MoralAlignment = "",
    [property: JsonPropertyName("motivations")] string Motivations = "",
    [property: JsonPropertyName("fears")] string Fears = "",
    [property: JsonPropertyName("backstory")] string Backstory = "",
    [property: JsonPropertyName("pastEvents")] string PastEvents = "",
    [property: JsonPropertyName("role")] string Role = "",
    [property: JsonPropertyName("mood")] string Mood = "",
    [property: JsonPropertyName("location")] string Location = "",
    [property: JsonPropertyName("relationships")] Relationship[]? Relationships = null,
    [property: JsonPropertyName("relationshipsActorData")] Character[]? RelationshipsActorData = null,
    [property: JsonPropertyName("memory")] string[]? Memory = null,
    [property: JsonPropertyName("conversationHistory")] Conversation[]? ConversationHistory = null,
    [property: JsonPropertyName("taskInstructions")] string TaskInstructions = "",
    [property: JsonPropertyName("taskSuccessCondition")] string TaskSuccessCondition = "",
    [property: JsonPropertyName("characterInstructions")] string CharacterInstructions = ""
);

public class Conversation {
    [JsonPropertyName("userMessage")] public string UserMessage { get; set; } = "";
    [JsonPropertyName("responseMessage")] public string ResponseMessage { get; set; } = "";
}

public class Relationship {
    [JsonPropertyName("actorDatabaseId")]
    public int ActorDatabaseId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("appearance")]
    public string Appearance { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("opinion")]
    public string Opinion { get; set; } = "";

    [JsonPropertyName("trust")]
    public int Trust { get; set; }

    [JsonPropertyName("hasMet")]
    public bool HasMet { get; set; }

    [JsonPropertyName("knowledge")]
    public List<string> Knowledge { get; set; } = [];

    public override string ToString() {
        return $"ActorDatabaseId: {ActorDatabaseId}, Name: {Name}, Status: {Status}, Opinion: {Opinion}, Trust: {Trust}, HasMet: {HasMet}, Knowledge: '{string.Join(", ", Knowledge)}'";
    }

    public string GetActor() {
        return $"Name: {Name}, Id({ActorDatabaseId})";
    }

    public string GetRelationship(string[] actors = null) {
        if (actors == null || actors.Length == 0 || actors.Contains(Name)) {
            return $"Your relationship with {Name} is '{Status}', " +
            $"your opinion of them is '{Opinion}', and '{GetTrustAsDescription()} ({Trust}% trust)'." +
            $" You have {(HasMet ? "met" : "not met")} them{(HasMet ? $" and know that '{Appearance}'" : "")}." +
            $" You know the following about them: {string.Join(", ", Knowledge)}.";
        }
        return "";
    }

    public string GetTrustAsDescription() {
        if (Trust <= 0) return "You don't trust them at all.";
        if (Trust <= 25) return "You don't trust them very much.";
        if (Trust <= 50) return "You trust them a little.";
        if (Trust <= 75) return "You trust them a fair amount.";
        if (Trust < 100) return "You trust them a lot.";
        return "You trust them completely.";
    }
}
// Response: {"actors":[{"name":"Rachel Howard","id":22}]}
public record RelationshipMapping(
    [property: JsonPropertyName("actors")] RelationshipMappingData[] Actors
);

public class RelationshipMappingData {
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("id")]
    public int Id { get; set; }
};
