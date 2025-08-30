namespace ParoquiaSLG.API.Modules.ParishManagement.NewsFolder.Contracts;

public record UpdateNewsRequest(
    string Title,
    string Content,
    bool Highlight,
    DateTime? HighlightUntil,
    string Summary,
    IFormFileCollection? FilesToAdd = null,
    List<Guid>? FilesToRemove = null);
