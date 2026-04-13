using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CalendarApp.Models;
using CalendarApp.Services;

namespace CalendarApp.Ui;

public sealed class MainForm : Form
{
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

    public MainForm()
    {
        Text = "Kalendář - semestrální práce";
        Width = 980;
        Height = 650;
        MinimumSize = new System.Drawing.Size(860, 560);

        _dayPicker.Value = DateTime.Today;
        _dayPicker.Format = DateTimePickerFormat.Short;
        _dayPicker.ValueChanged += (_, _) => RefreshAll();

        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(10),
        };

        topPanel.Controls.Add(new Label { Text = "Den:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
        topPanel.Controls.Add(_dayPicker);

        _tabControl.Dock = DockStyle.Fill;

        BuildEventsTab();
        BuildNotesTab();
        BuildRemindersTab();

        Controls.Add(_tabControl);
        Controls.Add(topPanel);

        HookEvents();

        RefreshAll();
    }

    private void BuildEventsTab()
    {
        var page = new TabPage("Události");
        _eventsGrid.Dock = DockStyle.Fill;
        _eventsGrid.ReadOnly = true;
        _eventsGrid.AllowUserToAddRows = false;
        _eventsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _eventsGrid.MultiSelect = false;
        _eventsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _eventsGrid.RowHeadersVisible = false;

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(10),
        };

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
        _notesGrid.Dock = DockStyle.Fill;
        _notesGrid.ReadOnly = true;
        _notesGrid.AllowUserToAddRows = false;
        _notesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _notesGrid.MultiSelect = false;
        _notesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _notesGrid.RowHeadersVisible = false;

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(10),
        };

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
        _remindersGrid.Dock = DockStyle.Fill;
        _remindersGrid.ReadOnly = true;
        _remindersGrid.AllowUserToAddRows = false;
        _remindersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _remindersGrid.MultiSelect = false;
        _remindersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _remindersGrid.RowHeadersVisible = false;

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(10),
        };

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
}

