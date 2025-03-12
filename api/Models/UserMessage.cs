// using API.Characters;

namespace API.Models;

public class UserMessage {
    public string Message { get; set; } = "";
    public string ConversationHistory { get; set; } = "";
    public string CharacterJson { get; set; } = "";
    public string? Character { get; set; } = null;

    // public Character getCharacterModel() {
    //     return Character switch
    //     {
    //         "Rachel" => new Rachel(),
    //         "Birdman" => new Birdman(),
    //         "Stein" => new Stein(),
    //         _ => throw new NotImplementedException()
    //     };
    // }
}
