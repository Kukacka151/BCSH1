using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CalendarApp.Models;
using CalendarApp.Services;

namespace CalendarApp.Ui;

public sealed class MainForm : Form
{
    private static readonly Color BackgroundColor = Color.FromArgb(244, 248, 255);
    private static readonly Color PanelColor = Color.FromArgb(230, 238, 251);
    private static readonly Color AccentColor = Color.FromArgb(46, 117, 182);
    private static readonly Color DangerColor = Color.FromArgb(192, 57, 43);

    private readonly CalendarService _service = new();

    private readonly DateTimePicker _dayPicker = new();
    private readonly TabControl _tabControl = new();

    private readonly DataGridView _eventsGrid = new();
    private readonly DataGridView _notesGrid = new();
    private readonly DataGridView _remindersGrid = new();

    private readonly Button _eventsAddBtn = new() { Text = "Přidat" };
    private readonly Button _eventsEditBtn = new() { Text = "Upravit" };
    private readonly Button _eventsDeleteBtn = new() { Text = "Smazat" };

    private readonly Button _notesAddBtn = new() { Text = "Přidat" };
    private readonly Button _notesEditBtn = new() { Text = "Upravit" };
    private readonly Button _notesDeleteBtn = new() { Text = "Smazat" };

    private readonly Button _remindersAddBtn = new() { Text = "Přidat" };
    private readonly Button _remindersEditBtn = new() { Text = "Upravit" };
    private readonly Button _remindersDeleteBtn = new() { Text = "Smazat" };

    private List<CalendarEvent> _events = new();
    private List<NoteItem> _notes = new();
    private List<ReminderItem> _reminders = new();
    private readonly System.Windows.Forms.Timer _reminderTimer = new() { Interval = 1000 };
    private DateTime _lastReminderCheck = DateTime.Now;

    public MainForm()
    {
        Text = "Kalendář - semestrální práce";
        Width = 980;
        Height = 650;
        MinimumSize = new System.Drawing.Size(860, 560);
        BackColor = BackgroundColor;

        _dayPicker.Value = DateTime.Today;
        _dayPicker.Format = DateTimePickerFormat.Short;
        _dayPicker.ValueChanged += (_, _) => RefreshAll();

        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(10),
            BackColor = PanelColor,
        };

        topPanel.Controls.Add(new Label { Text = "Den:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
        topPanel.Controls.Add(_dayPicker);

        _tabControl.Dock = DockStyle.Fill;
        _tabControl.Padding = new Point(18, 6);

        BuildEventsTab();
        BuildNotesTab();
        BuildRemindersTab();

        Controls.Add(_tabControl);
        Controls.Add(topPanel);

        HookEvents();
        HookReminderNotifications();

        RefreshAll();
    }

    private void BuildEventsTab()
    {
        var page = new TabPage("Události");
        page.BackColor = BackgroundColor;
        _eventsGrid.Dock = DockStyle.Fill;
        _eventsGrid.ReadOnly = true;
        _eventsGrid.AllowUserToAddRows = false;
        _eventsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _eventsGrid.MultiSelect = false;
        _eventsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _eventsGrid.RowHeadersVisible = false;
        _eventsGrid.BackgroundColor = Color.White;
        _eventsGrid.BorderStyle = BorderStyle.None;
        _eventsGrid.GridColor = Color.FromArgb(214, 224, 239);

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(10),
            BackColor = PanelColor,
        };

        StyleButton(_eventsAddBtn, AccentColor);
        StyleButton(_eventsEditBtn, AccentColor);
        StyleButton(_eventsDeleteBtn, DangerColor);
        btnRow.Controls.Add(_eventsAddBtn);
        btnRow.Controls.Add(_eventsEditBtn);
        btnRow.Controls.Add(_eventsDeleteBtn);

        page.Controls.Add(_eventsGrid);
        page.Controls.Add(btnRow);
        _tabControl.TabPages.Add(page);
    }

    private void BuildNotesTab()
    {
        var page = new TabPage("Poznámky");
        page.BackColor = BackgroundColor;
        _notesGrid.Dock = DockStyle.Fill;
        _notesGrid.ReadOnly = true;
        _notesGrid.AllowUserToAddRows = false;
        _notesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _notesGrid.MultiSelect = false;
        _notesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _notesGrid.RowHeadersVisible = false;
        _notesGrid.BackgroundColor = Color.White;
        _notesGrid.BorderStyle = BorderStyle.None;
        _notesGrid.GridColor = Color.FromArgb(214, 224, 239);

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(10),
            BackColor = PanelColor,
        };

        StyleButton(_notesAddBtn, AccentColor);
        StyleButton(_notesEditBtn, AccentColor);
        StyleButton(_notesDeleteBtn, DangerColor);
        btnRow.Controls.Add(_notesAddBtn);
        btnRow.Controls.Add(_notesEditBtn);
        btnRow.Controls.Add(_notesDeleteBtn);

        page.Controls.Add(_notesGrid);
        page.Controls.Add(btnRow);
        _tabControl.TabPages.Add(page);
    }

    private void BuildRemindersTab()
    {
        var page = new TabPage("Upomínky");
        page.BackColor = BackgroundColor;
        _remindersGrid.Dock = DockStyle.Fill;
        _remindersGrid.ReadOnly = false;
        _remindersGrid.AllowUserToAddRows = false;
        _remindersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _remindersGrid.MultiSelect = false;
        _remindersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _remindersGrid.RowHeadersVisible = false;
        _remindersGrid.BackgroundColor = Color.White;
        _remindersGrid.BorderStyle = BorderStyle.None;
        _remindersGrid.GridColor = Color.FromArgb(214, 224, 239);
        _remindersGrid.EditMode = DataGridViewEditMode.EditOnEnter;

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(10),
            BackColor = PanelColor,
        };

        StyleButton(_remindersAddBtn, AccentColor);
        StyleButton(_remindersEditBtn, AccentColor);
        StyleButton(_remindersDeleteBtn, DangerColor);
        btnRow.Controls.Add(_remindersAddBtn);
        btnRow.Controls.Add(_remindersEditBtn);
        btnRow.Controls.Add(_remindersDeleteBtn);

        page.Controls.Add(_remindersGrid);
        page.Controls.Add(btnRow);
        _tabControl.TabPages.Add(page);
    }

    private void HookEvents()
    {
        _eventsAddBtn.Click += (_, _) =>
        {
            using var dlg = new EventEditForm(_dayPicker.Value.Date);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _service.AddOrUpdateEvent(dlg.CreatedOrEditedItem);
            RefreshAll();
        };

        _eventsEditBtn.Click += (_, _) =>
        {
            if (_eventsGrid.CurrentRow?.DataBoundItem is not CalendarEvent selected)
                return;

            using var dlg = new EventEditForm(selected);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _service.AddOrUpdateEvent(dlg.CreatedOrEditedItem);
            RefreshAll();
        };

        _eventsDeleteBtn.Click += (_, _) =>
        {
            if (_eventsGrid.CurrentRow?.DataBoundItem is not CalendarEvent selected)
                return;

            if (MessageBox.Show($"Opravdu smazat událost '{selected.Title}'?", "Potvrzení", MessageBoxButtons.YesNo)
                != DialogResult.Yes)
                return;

            _service.DeleteEvent(selected.Id);
            RefreshAll();
        };

        _notesAddBtn.Click += (_, _) =>
        {
            using var dlg = new NoteEditForm(_dayPicker.Value.Date);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _service.AddOrUpdateNote(dlg.CreatedOrEditedItem);
            RefreshAll();
        };

        _notesEditBtn.Click += (_, _) =>
        {
            if (_notesGrid.CurrentRow?.DataBoundItem is not NoteItem selected)
                return;

            using var dlg = new NoteEditForm(selected);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _service.AddOrUpdateNote(dlg.CreatedOrEditedItem);
            RefreshAll();
        };

        _notesDeleteBtn.Click += (_, _) =>
        {
            if (_notesGrid.CurrentRow?.DataBoundItem is not NoteItem selected)
                return;

            if (MessageBox.Show("Opravdu smazat tuto poznámku?", "Potvrzení", MessageBoxButtons.YesNo)
                != DialogResult.Yes)
                return;

            _service.DeleteNote(selected.Id);
            RefreshAll();
        };

        _remindersAddBtn.Click += (_, _) =>
        {
            using var dlg = new ReminderEditForm(_dayPicker.Value.Date);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _service.AddOrUpdateReminder(dlg.CreatedOrEditedItem);
            RefreshAll();
        };

        _remindersEditBtn.Click += (_, _) =>
        {
            if (_remindersGrid.CurrentRow?.DataBoundItem is not ReminderItem selected)
                return;

            using var dlg = new ReminderEditForm(selected);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                return;

            _service.AddOrUpdateReminder(dlg.CreatedOrEditedItem);
            RefreshAll();
        };

        _remindersDeleteBtn.Click += (_, _) =>
        {
            if (_remindersGrid.CurrentRow?.DataBoundItem is not ReminderItem selected)
                return;

            if (MessageBox.Show("Opravdu smazat tuto upomínku?", "Potvrzení", MessageBoxButtons.YesNo)
                != DialogResult.Yes)
                return;

            _service.DeleteReminder(selected.Id);
            RefreshAll();
        };

        _remindersGrid.CurrentCellDirtyStateChanged += (_, _) =>
        {
            if (_remindersGrid.IsCurrentCellDirty)
                _remindersGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };

        _remindersGrid.CellValueChanged += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var changedColumn = _remindersGrid.Columns[e.ColumnIndex];
            if (!string.Equals(changedColumn?.DataPropertyName, nameof(ReminderItem.IsDone), StringComparison.Ordinal))
                return;

            if (_remindersGrid.Rows[e.RowIndex].DataBoundItem is not ReminderItem changedReminder)
                return;

            _service.AddOrUpdateReminder(changedReminder);
            RefreshAll();
        };
    }

    private void HookReminderNotifications()
    {
        _reminderTimer.Tick += (_, _) => CheckDueReminders();
        _reminderTimer.Start();
        FormClosed += (_, _) => _reminderTimer.Stop();
    }

    private void CheckDueReminders()
    {
        var now = DateTime.Now;
        var dueReminders = _service.GetPendingRemindersDueBetween(_lastReminderCheck, now);
        _lastReminderCheck = now;

        if (dueReminders.Count == 0)
            return;

        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine("Máš upomínku:");
        messageBuilder.AppendLine();

        foreach (var reminder in dueReminders)
            messageBuilder.AppendLine($"• {reminder.DateTime:HH:mm} - {reminder.Text}");

        MessageBox.Show(
            this,
            messageBuilder.ToString().TrimEnd(),
            "Upomínka",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        RefreshAll();
    }

    private void RefreshAll()
    {
        var date = _dayPicker.Value.Date;
        _events = _service.GetEventsForDate(date).ToList();
        _notes = _service.GetNotesForDate(date).ToList();
        _reminders = _service.GetRemindersForDate(date).ToList();

        _eventsGrid.DataSource = null;
        _notesGrid.DataSource = null;
        _remindersGrid.DataSource = null;

        _eventsGrid.DataSource = _events;
        _notesGrid.DataSource = _notes;
        _remindersGrid.DataSource = _reminders;

        ConfigureColumns(_eventsGrid, columnsToHide: new[] { "Id", "Date" });
        ConfigureColumns(_notesGrid, columnsToHide: new[] { "Id", "Date" });
        ConfigureColumns(_remindersGrid, columnsToHide: new[] { "Id" });
        ConfigureRemindersEditability();

        if (_eventsGrid.Columns.Contains("Start"))
            _eventsGrid.Columns["Start"].DefaultCellStyle.Format = @"hh\:mm";
        if (_eventsGrid.Columns.Contains("End"))
            _eventsGrid.Columns["End"].DefaultCellStyle.Format = @"hh\:mm";

        if (_remindersGrid.Columns.Contains("DateTime"))
            _remindersGrid.Columns["DateTime"].DefaultCellStyle.Format = "g";
    }

    private static void ConfigureColumns(DataGridView grid, IEnumerable<string> columnsToHide)
    {
        foreach (var col in columnsToHide)
        {
            if (grid.Columns.Contains(col))
                grid.Columns[col].Visible = false;
        }
    }

    private void ConfigureRemindersEditability()
    {
        foreach (DataGridViewColumn column in _remindersGrid.Columns)
            column.ReadOnly = true;

        if (_remindersGrid.Columns.Contains(nameof(ReminderItem.IsDone)))
            _remindersGrid.Columns[nameof(ReminderItem.IsDone)].ReadOnly = false;
    }

    private static void StyleButton(Button button, Color backColor)
    {
        button.AutoSize = false;
        button.Width = 100;
        button.Height = 34;
        button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        button.BackColor = backColor;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Margin = new Padding(6, 0, 0, 0);
    }
}

