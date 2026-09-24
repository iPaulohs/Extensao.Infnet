namespace Geekhub.EventHandler.Domain.Models;

public record EmailMessage(string To, string Subject, string Body, bool IsHtml = false);
