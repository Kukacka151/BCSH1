using System;
using System.ComponentModel;
using CalendarApp.Storage;

namespace CalendarApp.Models;

public sealed class ReminderItem : IHasGuidId
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Full date+time to allow "upomínky" at a moment in the day.
    [DisplayName("Kdy")]
    public DateTime DateTime { get; set; } = DateTime.Now;

    [DisplayName("Text")]
    public string Text { get; set; } = string.Empty;
    [DisplayName("Hotovo")]
    public bool IsDone { get; set; }
}

