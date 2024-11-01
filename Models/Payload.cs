namespace Example.Console.Models;

public record Payload(
    int TotalCount,
    bool IncompleteResults,
    IList<SearchResult> Items
){}