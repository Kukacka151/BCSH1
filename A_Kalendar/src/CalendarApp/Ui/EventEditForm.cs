using System;
using System.Drawing;
using System.Windows.Forms;
using CalendarApp.Models;

namespace CalendarApp.Ui;

public sealed class EventEditForm : Form
{
    public CalendarEvent CreatedOrEditedItem { get; private set; } = new();

    private readonly TextBox _titleBox = new();
    private readonly TextBox _descBox = new();
    private readonly DateTimePicker _datePicker = new();
    private readonly DateTimePicker _startPicker = new();
    private readonly DateTimePicker _endPicker = new();

    private readonly Button _saveBtn = new() { Text = "Uložit", DialogResult = DialogResult.OK };
    private readonly Button _cancelBtn = new() { Text = "Zrušit", DialogResult = DialogResult.Cancel };

    public EventEditForm(DateTime date)
    {
        CreatedOrEditedItem = new CalendarEvent { Date = date.Date };
        InitializeUi();
    }

    public EventEditForm(CalendarEvent existing)
    {
        CreatedOrEditedItem = new CalendarEvent
        {
            Id = existing.Id,
            Date = existing.Date.Date,
            Start = existing.Start,
            End = existing.End,
            Title = existing.Title,
            Description = existing.Description,
        };
        InitializeUi();
        _titleBox.Text = existing.Title;
        _descBox.Text = existing.Description;
        _datePicker.Value = existing.Date.Date;
        _startPicker.Value = DateTime.Today.Add(existing.Start);
        _endPicker.Value = DateTime.Today.Add(existing.End);
    }

    private void InitializeUi()
    {
        Text = "Událost - editace";
        Width = 520;
        Height = 360;
        BackColor = Color.FromArgb(244, 248, 255);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 2,
            RowCount = 6,
            AutoSize = true,
            BackColor = Color.FromArgb(244, 248, 255),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        layout.Controls.Add(new Label { Text = "Datum:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _datePicker.Format = DateTimePickerFormat.Short;
        _datePicker.Value = CreatedOrEditedItem.Date;
        _datePicker.Anchor = AnchorStyles.Left;
        layout.Controls.Add(_datePicker, 1, 0);

        layout.Controls.Add(new Label { Text = "Začátek:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _startPicker.Format = DateTimePickerFormat.Time;
        _startPicker.ShowUpDown = true;
        _startPicker.Value = DateTime.Today.Add(CreatedOrEditedItem.Start);
        _startPicker.Anchor = AnchorStyles.Left;
        layout.Controls.Add(_startPicker, 1, 1);

        layout.Controls.Add(new Label { Text = "Konec:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _endPicker.Format = DateTimePickerFormat.Time;
        _endPicker.ShowUpDown = true;
        _endPicker.Value = DateTime.Today.Add(CreatedOrEditedItem.End);
        _endPicker.Anchor = AnchorStyles.Left;
        layout.Controls.Add(_endPicker, 1, 2);

        layout.Controls.Add(new Label { Text = "Název:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _titleBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _titleBox.Width = 280;
        layout.Controls.Add(_titleBox, 1, 3);

        layout.Controls.Add(new Label { Text = "Popis:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 4);
        _descBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _descBox.Multiline = true;
        _descBox.Height = 80;
        layout.Controls.Add(_descBox, 1, 4);

        var btnRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(8),
        };
        btnRow.BackColor = Color.FromArgb(230, 238, 251);
        StyleDialogButton(_saveBtn);
        StyleDialogButton(_cancelBtn);
        _saveBtn.BackColor = Color.FromArgb(46, 117, 182);
        _saveBtn.ForeColor = Color.White;
        btnRow.Controls.Add(_cancelBtn);
        btnRow.Controls.Add(_saveBtn);

        layout.Controls.Add(btnRow, 0, 5);
        layout.SetColumnSpan(btnRow, 2);

        AcceptButton = _saveBtn;
        CancelButton = _cancelBtn;

        _saveBtn.Click += (_, _) =>
        {
            var title = _titleBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Název nesmí být prázdný.", "Validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            var start = _startPicker.Value.TimeOfDay;
            var end = _endPicker.Value.TimeOfDay;
            if (end <= start)
            {
                MessageBox.Show("Konec musí být po začátku.", "Validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            CreatedOrEditedItem.Title = title;
            CreatedOrEditedItem.Description = _descBox.Text.Trim();
            CreatedOrEditedItem.Date = _datePicker.Value.Date;
            CreatedOrEditedItem.Start = start;
            CreatedOrEditedItem.End = end;
        };

        Controls.Add(layout);
    }

    private static void StyleDialogButton(Button button)
    {
        button.AutoSize = false;
        button.Width = 96;
        button.Height = 34;
        button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        button.TextAlign = ContentAlignment.MiddleCenter;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Margin = new Padding(6, 2, 0, 2);
    }
}

