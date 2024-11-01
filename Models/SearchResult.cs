namespace Example.Console.Models;

public record SearchResult(
    string Title,
    int Number,
    User User,
    string State,
    IList<Label> Labels,
    Uri HtmlUrl,    
    string Body
){}

public record User(
    string Login,
    Uri HtmlUrl
){}

public record Label(
    string Name,
    string? Description
){}