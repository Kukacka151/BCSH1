using System;
using System.Windows.Forms;
using CalendarApp.Models;

namespace CalendarApp.Ui;

public sealed class NoteEditForm : Form
{
    public NoteItem CreatedOrEditedItem { get; private set; } = new();

    private readonly TextBox _labelBox = new();
    private readonly TextBox _textBox = new();
    private readonly DateTimePicker _datePicker = new();

    private readonly Button _saveBtn = new() { Text = "Uložit", DialogResult = DialogResult.OK };
    private readonly Button _cancelBtn = new() { Text = "Zrušit", DialogResult = DialogResult.Cancel };

    public NoteEditForm(DateTime date)
    {
        CreatedOrEditedItem = new NoteItem { Date = date.Date };
        InitializeUi();
    }

    public NoteEditForm(NoteItem existing)
    {
        CreatedOrEditedItem = new NoteItem
        {
            Id = existing.Id,
            Date = existing.Date.Date,
            Label = existing.Label,
            Text = existing.Text,
        };
        InitializeUi();

        _datePicker.Value = existing.Date.Date;
        _labelBox.Text = existing.Label;
        _textBox.Text = existing.Text;
    }

    private void InitializeUi()
    {
        Text = "Poznámka - editace";
        Width = 520;
        Height = 360;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 2,
            RowCount = 5,
            AutoSize = true,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        layout.Controls.Add(new Label { Text = "Datum:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _datePicker.Format = DateTimePickerFormat.Short;
        _datePicker.Anchor = AnchorStyles.Left;
        _datePicker.Value = CreatedOrEditedItem.Date;
        layout.Controls.Add(_datePicker, 1, 0);

        layout.Controls.Add(new Label { Text = "Popis:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _labelBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        layout.Controls.Add(_labelBox, 1, 1);

        layout.Controls.Add(new Label { Text = "Text:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _textBox.Multiline = true;
        _textBox.Height = 120;
        layout.Controls.Add(_textBox, 1, 2);

        var btnRow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        btnRow.Controls.Add(_cancelBtn);
        btnRow.Controls.Add(_saveBtn);

        layout.Controls.Add(btnRow, 0, 3);
        layout.SetColumnSpan(btnRow, 2);

        AcceptButton = _saveBtn;
        CancelButton = _cancelBtn;

        _saveBtn.Click += (_, _) =>
        {
            var label = _labelBox.Text.Trim();
            var text = _textBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(label))
            {
                MessageBox.Show("Popis nesmí být prázdný.", "Validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Text nesmí být prázdný.", "Validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            CreatedOrEditedItem.Label = label;
            CreatedOrEditedItem.Text = text;
            CreatedOrEditedItem.Date = _datePicker.Value.Date;
        };

        Controls.Add(layout);
    }
}

