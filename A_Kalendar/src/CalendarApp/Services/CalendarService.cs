using System;
using System.Collections.Generic;
using System.Linq;
using CalendarApp.Models;
using CalendarApp.Storage;

namespace CalendarApp.Services;

public sealed class CalendarService
{
    private readonly JsonRepository<CalendarEvent> _eventsRepo;
    private readonly JsonRepository<NoteItem> _notesRepo;
    private readonly JsonRepository<ReminderItem> _remindersRepo;

    private readonly List<CalendarEvent> _events;
    private readonly List<NoteItem> _notes;
    private readonly List<ReminderItem> _reminders;

    public CalendarService()
    {
        var dataDir = AppPaths.GetDataDirectory();
        _eventsRepo = new JsonRepository<CalendarEvent>(Path.Combine(dataDir, "events.json"));
        _notesRepo = new JsonRepository<NoteItem>(Path.Combine(dataDir, "notes.json"));
        _remindersRepo = new JsonRepository<ReminderItem>(Path.Combine(dataDir, "reminders.json"));

        _events = _eventsRepo.LoadAll();
        _notes = _notesRepo.LoadAll();
        _reminders = _remindersRepo.LoadAll();
    }

    public DateTime Today => DateTime.Today;

    public IReadOnlyList<CalendarEvent> GetEventsForDate(DateTime date)
    {
        var day = date.Date;
        return _events.Where(e => e.Date.Date == day).OrderBy(e => e.Start).ToList();
    }

    public IReadOnlyList<NoteItem> GetNotesForDate(DateTime date)
    {
        var day = date.Date;
        return _notes.Where(n => n.Date.Date == day).OrderBy(n => n.Label).ToList();
    }

    public IReadOnlyList<ReminderItem> GetRemindersForDate(DateTime date)
    {
        var day = date.Date;
        return _reminders
            .Where(r => r.DateTime.Date == day)
            .OrderBy(r => r.DateTime)
            .ToList();
    }

    public IReadOnlyList<ReminderItem> GetPendingRemindersDueBetween(DateTime fromExclusive, DateTime toInclusive)
    {
        return _reminders
            .Where(r => !r.IsDone && r.DateTime > fromExclusive && r.DateTime <= toInclusive)
            .OrderBy(r => r.DateTime)
            .ToList();
    }

    public void AddOrUpdateEvent(CalendarEvent item)
    {
        _eventsRepo.Upsert(_events, item);
        Save();
    }

    public void DeleteEvent(Guid id)
    {
        _eventsRepo.Delete(_events, id);
        Save();
    }

    public void AddOrUpdateNote(NoteItem item)
    {
        _notesRepo.Upsert(_notes, item);
        Save();
    }

    public void DeleteNote(Guid id)
    {
        _notesRepo.Delete(_notes, id);
        Save();
    }

    public void AddOrUpdateReminder(ReminderItem item)
    {
        _remindersRepo.Upsert(_reminders, item);
        Save();
    }

    public void DeleteReminder(Guid id)
    {
        _remindersRepo.Delete(_reminders, id);
        Save();
    }

    private void Save()
    {
        _eventsRepo.SaveAll(_events);
        _notesRepo.SaveAll(_notes);
        _remindersRepo.SaveAll(_reminders);
    }
}

