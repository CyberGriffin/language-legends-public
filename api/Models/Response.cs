namespace API.Models;

public class Response {
    public string Message { get; set; } = "";
    public int Rating { get; set; } = 0;
    public string Mood { get; set; } = "";
    public string Memory { get; set; } = "";
    public bool IsTaskComplete { get; set; } = false;
    public string RatingResponse { get; set; } = "";
}
