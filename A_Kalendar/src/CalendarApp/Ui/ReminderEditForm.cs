using System;
using System.Drawing;
using System.Windows.Forms;
using CalendarApp.Models;

namespace CalendarApp.Ui;

public sealed class ReminderEditForm : Form
{
    public ReminderItem CreatedOrEditedItem { get; private set; } = new();

    private readonly DateTimePicker _dateTimePicker = new();
    private readonly TextBox _textBox = new();
    private readonly CheckBox _doneCheckbox = new();

    private readonly Button _saveBtn = new() { Text = "Uložit", DialogResult = DialogResult.OK };
    private readonly Button _cancelBtn = new() { Text = "Zrušit", DialogResult = DialogResult.Cancel };

    public ReminderEditForm(DateTime date)
    {
        CreatedOrEditedItem = new ReminderItem { DateTime = date.Date.AddHours(9) };
        InitializeUi();
    }

    public ReminderEditForm(ReminderItem existing)
    {
        CreatedOrEditedItem = new ReminderItem
        {
            Id = existing.Id,
            DateTime = existing.DateTime,
            Text = existing.Text,
            IsDone = existing.IsDone,
        };
        InitializeUi();

        _dateTimePicker.Value = existing.DateTime;
        _textBox.Text = existing.Text;
        _doneCheckbox.Checked = existing.IsDone;
    }

    private void InitializeUi()
    {
        Text = "Upomínka - editace";
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
            RowCount = 4,
            AutoSize = true,
            BackColor = Color.FromArgb(244, 248, 255),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        layout.Controls.Add(new Label { Text = "Kdy:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _dateTimePicker.Format = DateTimePickerFormat.Custom;
        _dateTimePicker.CustomFormat = "dd.MM.yyyy HH:mm";
        _dateTimePicker.Anchor = AnchorStyles.Left;
        _dateTimePicker.Value = CreatedOrEditedItem.DateTime;
        layout.Controls.Add(_dateTimePicker, 1, 0);

        layout.Controls.Add(new Label { Text = "Text:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _textBox.Multiline = true;
        _textBox.Height = 120;
        layout.Controls.Add(_textBox, 1, 1);

        layout.Controls.Add(new Label { Text = "Hotovo:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _doneCheckbox.Anchor = AnchorStyles.Left;
        _doneCheckbox.Checked = CreatedOrEditedItem.IsDone;
        layout.Controls.Add(_doneCheckbox, 1, 2);

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

        layout.Controls.Add(btnRow, 0, 3);
        layout.SetColumnSpan(btnRow, 2);

        AcceptButton = _saveBtn;
        CancelButton = _cancelBtn;

        _saveBtn.Click += (_, _) =>
        {
            var text = _textBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Text nesmí být prázdný.", "Validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            CreatedOrEditedItem.Text = text;
            CreatedOrEditedItem.DateTime = _dateTimePicker.Value;
            CreatedOrEditedItem.IsDone = _doneCheckbox.Checked;
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

