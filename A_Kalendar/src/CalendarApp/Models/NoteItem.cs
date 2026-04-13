using System;
using System.ComponentModel;
using CalendarApp.Storage;

namespace CalendarApp.Models;

public sealed class NoteItem : IHasGuidId
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Day (date part only).
    [DisplayName("Datum")]
    public DateTime Date { get; set; } = DateTime.Today;

    [DisplayName("Text")]
    public string Text { get; set; } = string.Empty;
    [DisplayName("Štítek")]
    public string Label { get; set; } = string.Empty;
}

