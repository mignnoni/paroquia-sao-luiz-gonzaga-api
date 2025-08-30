namespace ParoquiaSLG.API.Modules.ParishManagement.NewsFolder.Contracts;

public record CreateNewsRequest(
    string Title,
    string Content,
    string Summary,
    bool Highlight,
    DateTime? HighlightUntil,
    IFormFileCollection? Files = null);
