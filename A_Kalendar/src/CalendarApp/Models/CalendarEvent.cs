using System;
using System.ComponentModel;
using CalendarApp.Storage;

namespace CalendarApp.Models;

public sealed class CalendarEvent : IHasGuidId
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Store the day (date part only) separately to simplify filtering in UI.
    [DisplayName("Datum")]
    public DateTime Date { get; set; } = DateTime.Today;

    [DisplayName("Začátek")]
    public TimeSpan Start { get; set; } = TimeSpan.Zero;
    [DisplayName("Konec")]
    public TimeSpan End { get; set; } = new TimeSpan(1, 0, 0);

    [DisplayName("Název")]
    public string Title { get; set; } = string.Empty;
    [DisplayName("Popis")]
    public string Description { get; set; } = string.Empty;
}

