using UploadToolGui.Dto.Enums;
using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Common.UploadTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace UploadToolGui
{
    public class MainTab
    {
        private enum TextStyle
        {
            Italic,
            Bold,
            Strikeout,
            Underline,
            Header,
            Bullet
        }

        private readonly MainForm _mainForm;
        private readonly List<EnumDetail<DescriptionPageHeader>> _pageHeaders;
        private readonly List<EnumDetail<CollectibleRarity>> _rarity;

        private IAppMode _baseStatic;
        private IMainDto _item;
        private int _prevTextIndex;
        private bool _keyHandled, _textChanged;

        public MainTab(MainForm mainForm)
        {
            _mainForm = mainForm;

            _pageHeaders = Enum.GetValues(typeof(DescriptionPageHeader))
                .Cast<DescriptionPageHeader>()
                .Select((value, index) =>
                    new EnumDetail<DescriptionPageHeader>(value, value.Description()))
                .ToList();
            _rarity = Enum.GetValues(typeof(CollectibleRarity))
                .Cast<CollectibleRarity>()
                .Select((value, index) =>
                    new EnumDetail<CollectibleRarity>(value, value.Description()))
                .ToList();

            _mainForm.PointsSelector.Maximum = int.MaxValue;
            _mainForm.RaritySelector.Items.AddRange(_rarity.ToArray());

            _mainForm.NameInput.TextChanged += NameInput_TextChanged;
            _mainForm.RaritySelector.SelectedIndexChanged += RaritySelection_SelectedIndexChanged;
            _mainForm.PointsSelector.ValueChanged += PointsSelector_ValueChanged;
            _mainForm.PageSelector.SelectedIndexChanged += PageSelector_SelectedIndexChanged;
            _mainForm.TextBox.KeyDown += TextBox_KeyDown;
            _mainForm.TextBox.KeyPress += TextBox_KeyPress;
            _mainForm.ApplyBold.Click += ApplyBold_Click;
            _mainForm.ApplyItalic.Click += ApplyItalic_Click;
            _mainForm.ApplyStrikeOut.Click += ApplyStrikeOut_Click;
            _mainForm.ApplyHeader.Click += ApplyHeader_Click;
            _mainForm.ApplyNumList.Click += ApplyNumList_Click;
            _mainForm.ApplyUnsortedList.Click += ApplyUnsortedList_Click;
            _mainForm.AddLink.Click += AddLink_Click;
            _mainForm.PasteWord.Click += PasteWord_Click;
            _mainForm.PasteHtml.Click += PasteHtml_Click;
        }

        public void SetupMainTab(IAppMode baseStatic)
        {
            _baseStatic = baseStatic;
            _prevTextIndex = 0;

            _mainForm.Tabs.Controls.Add(_mainForm.TabMain);

            bool points = _baseStatic.ControlOptions.HasFlag(ControlOption.Points),
                rarity = _baseStatic.ControlOptions.HasFlag(ControlOption.Rarity),
                texts = _baseStatic.ControlOptions.HasFlag(ControlOption.MultipleTexts);

            SetRowVisibility((int)MainTabRows.Points, points);
            _mainForm.PointsSelector.Visible = points;
            SetRowVisibility((int)MainTabRows.Rarity, rarity);
            _mainForm.RaritySelector.Visible = rarity;
            SetRowVisibility((int)MainTabRows.PageHeader, texts);
            _mainForm.NameInput.MaxLength = _baseStatic.NameMaxLength;
            _mainForm.PageSelector.Visible = texts;
        }

        public void LoadMainTab(IMainDto item)
        {
            _item = item;

            _mainForm.NameInput.Text = _item.Name;
            _mainForm.RaritySelector.SelectedIndex =
                _rarity.FindIndex(x => x.EnumValue == _item.Rarity);
            _mainForm.PointsSelector.Value = _item.Points;

            SetPageSelector();

            foreach (DescriptionText text in _item.Texts)
            {
                int index = _pageHeaders.FindIndex(x => x.EnumValue == text.TextId);
                var page = (EnumDetail<DescriptionPageHeader>)_mainForm.PageSelector.Items[index];

                _mainForm.PageSelector.Items[index] =
                    new EnumDetail<DescriptionPageHeader>(page, page.BaseDescription + " *");
            }

            LoadPageText();
        }

        public async Task<Dictionary<string, DataConsistency.ItemStatus>> ValidateData()
        {
            if (_textChanged)
            {
                SaveText(_mainForm.PageSelector.SelectedIndex);
                _textChanged = false;
            }

            var items = new Dictionary<string, DataConsistency.ItemStatus>();
            bool hasName = !string.IsNullOrWhiteSpace(_item.Name);
            int nameLength = _baseStatic.NameMaxLength;

            if (!hasName)
            {
                items.Add("Naam", DataConsistency.ItemStatus.Critical);
            }
            else if (_item.Name.Length > nameLength)
            {
                items.Add($"Naam lengte ({_item.Name.Length} > {nameLength})", DataConsistency.ItemStatus.Critical);
            }
            else if (await _item.NameExistOutsideId())
            {
                items.Add("Unieke naam", DataConsistency.ItemStatus.Critical);
            }
            if (_baseStatic.ControlOptions.HasFlag(ControlOption.Points) && _item.Points <= 0)
            {
                items.Add("Punten", DataConsistency.ItemStatus.Optional);
            }

            if (_baseStatic.ControlOptions.HasFlag(ControlOption.MultipleTexts))
            {
                if (!_item.Texts.Any())
                {
                    items.Add("Minstens 1 tekst", DataConsistency.ItemStatus.Critical);
                }
            }
            else if (string.IsNullOrWhiteSpace(_item.Texts[0].Text))
            {
                items.Add("Tekst", DataConsistency.ItemStatus.Critical);
            }

            return items;
        }

        public void NameInput_TextChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                _mainForm.ValueChanged();
                _item.Name = _mainForm.NameInput.Text;
            }
        }

        public void RaritySelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                _mainForm.ValueChanged();
                _item.Rarity =
                    ((EnumDetail<CollectibleRarity>)_mainForm.RaritySelector.SelectedItem).EnumValue;
            }
        }

        public void PointsSelector_ValueChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                _mainForm.ValueChanged();
                _item.Points = (int)_mainForm.PointsSelector.Value;
            }
        }

        public void PageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_mainForm.IsLoading)
            {
                return;
            }

            if (_textChanged)
            {
                _mainForm.IsLoading = true;
                SaveText(_prevTextIndex);
                _mainForm.IsLoading = false;

                _mainForm.ValueChanged();
            }

            _prevTextIndex = _mainForm.PageSelector.SelectedIndex;
            LoadPageText();
        }
        
        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            _keyHandled = true;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (e.Shift)
                    {
                        InsertAtCaret(MarkDown.NewLine);
                    }
                    else
                    {
                        string list = MarkDown.ContinueList(_mainForm.TextBox.Text.Substring(0, _mainForm.TextBox.SelectionStart));

                        InsertAtCaret(MarkDown.NewParagraph + list);
                    }

                    RenderStyling();

                    e.Handled = true;
                    break;
                case Keys.I:
                    if (e.Control)
                    {
                        InsertAtCaret(MarkDown.ItalicBold.ToString());
                        RenderStyling();
                    }
                    else
                    {
                        _keyHandled = false;
                    }
                    break;
                case Keys.B:
                    if (e.Control)
                    {
                        InsertAtCaret(new string(MarkDown.ItalicBold, 2));
                        RenderStyling();
                    }
                    else
                    {
                        _keyHandled = false;
                    }
                    break;
                default:
                    _keyHandled = false;
                    break;
            }
        }

        public void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!_keyHandled && MarkDown.EscapeChars.Contains(e.KeyChar))
            {
                _keyHandled = true;
                InsertAtCaret("\\" + e.KeyChar);
                RenderStyling();
            }

            _textChanged = true;
            _mainForm.ValueChanged();
            e.Handled = _keyHandled;
        }

        public void ApplyBold_Click(object sender, EventArgs e)
        {
            SetStyling(new string(MarkDown.ItalicBold, 2));
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }
        
        public void ApplyItalic_Click(object sender, EventArgs e)
        {
            SetStyling(MarkDown.ItalicBold.ToString());
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }

        public void ApplyStrikeOut_Click(object sender, EventArgs e)
        {
            SetStyling(new string(MarkDown.Strikeout, 2));
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }

        public void ApplyHeader_Click(object sender, EventArgs e)
        {
            AddHeader();
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }

        public void ApplyNumList_Click(object sender, EventArgs e)
        {
            ToggleList(MarkDown.ListType.Numeric);
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }
       
        public void ApplyUnsortedList_Click(object sender, EventArgs e)
        {
            ToggleList(MarkDown.ListType.Unsorted);
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }

        public void AddLink_Click(object sender, EventArgs e)
        {
            InsertAtCaret(string.Format(MarkDown.Link, "Tekst", "URL"));
            RenderStyling();
            _mainForm.ValueChanged();
            _mainForm.TextBox.Focus();
        }

        private void SetRowVisibility(int index, bool visible)
        {
            if (visible)
            {
                _mainForm.MainPanel.RowStyles[index].SizeType = SizeType.AutoSize;
            }
            else
            {
                _mainForm.MainPanel.RowStyles[index].SizeType = SizeType.Absolute;
                _mainForm.MainPanel.RowStyles[index].Height = 0f;
            }
        }

        private void PasteWord_Click(object sender, EventArgs e)
        {
            using RichTextBox rtf = new RichTextBox();

            try
            {
                rtf.Paste();
            }
            catch
            {
                MessageBox.Show("Geen tekst gevonden.", "Geen tekst", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder builder = new StringBuilder(rtf.Text);
            List<TextStyle> applied = new List<TextStyle>();
            bool prevNewLine = false;

            foreach (char esc in MarkDown.EscapeChars)
            {
                builder.Replace(esc.ToString(), "\\" + esc);
            }

            rtf.SelectionStart = 0;
            rtf.SelectionLength = 1;

            for (int i = 0; rtf.SelectionStart < rtf.Text.Length; i++, rtf.SelectionStart++)
            {
                bool isHeader = rtf.SelectionFont.Size > 11;
                int correctedI = prevNewLine ? i - 2 : i;

                if (rtf.SelectionLength > 1)
                {
                    MessageBox.Show("Er is iets fout gegaan bij plakken. Zorg er voor dat de tekst geen hyperlinks bevat.",
                        "Fout bij plakken", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (rtf.SelectedText == "\n" && !applied.Contains(TextStyle.Bullet))
                {
                    builder.Insert(i, '\n');
                    i++;
                }

                if (rtf.SelectionBullet)
                {
                    if (!applied.Contains(TextStyle.Bullet))
                    {
                        builder.Insert(i, MarkDown.UnsortedList);
                        i += MarkDown.UnsortedList.Length;
                        applied.Add(TextStyle.Bullet);
                    }

                    if (rtf.SelectedText == "\n")
                    {
                        applied.Remove(TextStyle.Bullet);
                    }
                }

                if (isHeader && rtf.SelectedText != "\n")
                {
                    if (!applied.Contains(TextStyle.Header))
                    {
                        builder.Insert(i, MarkDown.Header);
                        i += MarkDown.Header.Length;
                        applied.Add(TextStyle.Header);
                    }
                }
                else if (applied.Contains(TextStyle.Header))
                {
                    applied.Remove(TextStyle.Header);
                }

                if (!isHeader)
                {
                    if (applied.Contains(TextStyle.Italic) ^ rtf.SelectionFont.Italic)
                    {
                        if (!applied.Contains(TextStyle.Italic))
                        {
                            applied.Add(TextStyle.Italic);
                            builder.Insert(i, MarkDown.ItalicBold);
                        }
                        else
                        {
                            applied.Remove(TextStyle.Italic);
                            builder.Insert(correctedI, MarkDown.ItalicBold);
                        }

                        i++;
                    }

                    if (applied.Contains(TextStyle.Bold) ^ rtf.SelectionFont.Bold)
                    {
                        if (!applied.Contains(TextStyle.Bold))
                        {
                            applied.Add(TextStyle.Bold);
                            builder.Insert(i, new string(MarkDown.ItalicBold, 2));
                        }
                        else
                        {
                            applied.Remove(TextStyle.Bold);
                            builder.Insert(correctedI, new string(MarkDown.ItalicBold, 2));
                        }

                        i += 2;
                    }
                }

                if (applied.Contains(TextStyle.Underline) ^ rtf.SelectionFont.Underline)
                {
                    if (!applied.Contains(TextStyle.Underline))
                    {
                        applied.Add(TextStyle.Underline);
                        builder.Insert(i, new string(MarkDown.ItalicBold, 2));
                    }
                    else
                    {
                        applied.Remove(TextStyle.Underline);
                        builder.Insert(correctedI, new string(MarkDown.ItalicBold, 2));
                    }

                    i += 2;
                }

                if (applied.Contains(TextStyle.Strikeout) ^ rtf.SelectionFont.Strikeout)
                {
                    if (!applied.Contains(TextStyle.Strikeout))
                    {
                        applied.Add(TextStyle.Strikeout);
                        builder.Insert(i, new string(MarkDown.Strikeout, 2));
                    }
                    else
                    {
                        applied.Remove(TextStyle.Strikeout);
                        builder.Insert(correctedI, new string(MarkDown.Strikeout, 2));
                    }

                    i += 2;
                }

                prevNewLine = rtf.SelectedText == "\n";
            }

            int r = 0;
            for (; builder[builder.Length - r - 1] == '\n' && r < builder.Length; r++)
            { }

            if (r > 0)
            {
                builder.Remove(builder.Length - r, r);
            }

            for (int i = applied.Count - 1; i >= 0 ; i--)
            {
                switch (applied[i])
                {
                    case TextStyle.Italic:
                        builder.Append(MarkDown.ItalicBold);
                        break;
                    case TextStyle.Bold:
                    case TextStyle.Underline:
                        builder.Append(new string(MarkDown.ItalicBold, 2));
                        break;
                    case TextStyle.Strikeout:
                        builder.Append(new string(MarkDown.Strikeout, 2));
                        break;
                }
            }

            _mainForm.TextBox.Text = builder.ToString().Trim();
            RenderStyling();
            _mainForm.ValueChanged();
        }

        private void PasteHtml_Click(object sender, EventArgs e)
        {
            StringBuilder builder;

            try
            {
                builder = new StringBuilder(Clipboard.GetText());
            }
            catch
            {
                MessageBox.Show("Geen tekst gevonden.", "Geen tekst", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (char esc in MarkDown.EscapeChars)
            {
                builder.Replace(esc.ToString(), "\\" + esc);
            }

            builder.Replace("\n", string.Empty);
            builder.Replace("\r", string.Empty);

            builder.Replace("<br>", MarkDown.NewLine);
            builder.Replace("<br/>", MarkDown.NewLine);

            builder.Replace("<p>", string.Empty);
            builder.Replace("</p>", MarkDown.NewParagraph);

            builder.Replace("<b>", new string(MarkDown.ItalicBold, 2));
            builder.Replace("</b>", new string(MarkDown.ItalicBold, 2));

            builder.Replace("<i>", MarkDown.ItalicBold.ToString());
            builder.Replace("</i>", MarkDown.ItalicBold.ToString());

            builder.Replace("<ol>", string.Empty);
            builder.Replace("</ol>", string.Empty);
            builder.Replace("<ul>", string.Empty);
            builder.Replace("</ul>", string.Empty);
            builder.Replace("</li>", string.Empty);
            builder.Replace("<li>", MarkDown.UnsortedList);

            for (int i = 1; i <= 5; i++)
            {
                builder.Replace($"<h{i}>", MarkDown.Header);
                builder.Replace($"</h{i}>", "\n");
            }

            MatchCollection matches = Regex.Matches(builder.ToString(), 
                @"<a +href *= *""(?<link>.+?)"" *>(?<text>.+?)</a>", RegexOptions.RightToLeft);

            foreach (Match match in matches)
            {
                builder.Remove(match.Index, match.Length);
                builder.Insert(match.Index, $"[{match.Groups["text"]}]({match.Groups["link"]})");
            }

            _mainForm.TextBox.Text = builder.ToString().Trim();
            RenderStyling();
            _mainForm.ValueChanged();
        }

        private void SetPageSelector()
        {
            _mainForm.PageSelector.Items.Clear();
            _mainForm.PageSelector.Items.AddRange(_pageHeaders.ToArray());
            _mainForm.PageSelector.SelectedIndex = _prevTextIndex;
        }

        private void LoadPageText()
        {
            DescriptionPageHeader id = _pageHeaders[_prevTextIndex].EnumValue;

            _textChanged = false;
            _mainForm.TextBox.Text = _item.Texts.FirstOrDefault(ct => ct.TextId == id)?.Text ?? string.Empty;

            RenderStyling();
        }

        private void SaveText(int index)
        {
            if (!_baseStatic.ControlOptions.HasFlag(ControlOption.MultipleTexts))
            {
                _item.Texts[0].Text = _mainForm.TextBox.Text;
            }
            else
            {
                SaveTexts(index);
            }
        }

        private void SaveTexts(int index)
        {
            bool empty = string.IsNullOrWhiteSpace(_mainForm.TextBox.Text);
            var item = (EnumDetail<DescriptionPageHeader>)_mainForm.PageSelector.Items[index];

            if (!empty)
            {
                _mainForm.PageSelector.Items[index] =
                    new EnumDetail<DescriptionPageHeader>(item, item.BaseDescription + " *");
            }
            else
            {
                _mainForm.PageSelector.Items[index] =
                    new EnumDetail<DescriptionPageHeader>(item, item.BaseDescription);
            }

            DescriptionPageHeader pageHeader = _pageHeaders[index].EnumValue;
            DescriptionText text = _item.Texts.FirstOrDefault(x => x.TextId == pageHeader);

            if (!empty)
            {
                if (text != null)
                {
                    text.Text = _mainForm.TextBox.Text;
                }
                else
                {
                    _item.Texts.Add(new DescriptionText
                    {
                        TextId = pageHeader,
                        Text = _mainForm.TextBox.Text
                    });
                }
            }
            else if (text != null)
            {
                _item.Texts.Remove(_item.Texts.First(x => x.Id == text.Id));
            }
        }

        private void SetStyling(string text)
        {
            if (_mainForm.TextBox.SelectionLength > 0)
            {
                SurroundSelection(text);
            }
            else
            {
                InsertAtCaret(text);
            }

            RenderStyling();
        }

        private void RenderStyling()
        {
            int caret = _mainForm.TextBox.SelectionStart;

            RenderStyle(1, MarkDown.ItalicBold, FontStyle.Italic);
            RenderStyle(2, MarkDown.ItalicBold, FontStyle.Bold);
            RenderStyle(3, MarkDown.ItalicBold, FontStyle.Italic | FontStyle.Bold);
            RenderStyle(2, MarkDown.Strikeout, FontStyle.Strikeout);

            MatchCollection links = Regex.Matches(_mainForm.TextBox.Text,
                MarkDown.LinkPattern, RegexOptions.Compiled);

            foreach (Match link in links)
            {
                Group group = link.Groups["text"];
                RenderStyle(group.Index, group.Length, FontStyle.Underline);

                group = link.Groups["link"];
                RenderStyle(group.Index, group.Length, FontStyle.Underline);
            }

            string esc = Regex.Escape(MarkDown.Header);
            MatchCollection headers = Regex.Matches(_mainForm.TextBox.Text,
                @$"^(?<header>{esc}[^\n]+)$", RegexOptions.Multiline);

            foreach (Match header in headers)
            {
                Group group = header.Groups["header"];
                RenderStyle(group.Index, group.Length, FontStyle.Bold, 1);
            }

            _mainForm.TextBox.SelectionStart = caret;
            _mainForm.TextBox.SelectionLength = 0;
        }

        private void RenderStyle(int start, int length, FontStyle style, float sizeModifier = 0)
        {
            _mainForm.TextBox.SelectionStart = start;
            _mainForm.TextBox.SelectionLength = length;
            AddFontStyle(style, sizeModifier);
            _mainForm.TextBox.SelectionStart = start + length;
            _mainForm.TextBox.SelectionLength = 0;
            RemoveFontStyle(style);
        }

        private void RenderStyle(int length, char format, FontStyle style, float sizeModifier = 0)
        {
            string esc = Regex.Escape(format.ToString()),
                pattern = @$"(?<!\\|{esc}){esc}{{{length}}}(?!{esc})";
            MatchCollection matches = Regex.Matches(_mainForm.TextBox.Text, pattern);

            for (int i = 0; i < matches.Count; i += 2)
            {
                _mainForm.TextBox.SelectionStart = matches[i].Index;

                if (i + 1 < matches.Count)
                {
                    _mainForm.TextBox.SelectionLength = matches[i + 1].Index - matches[i].Index + length;
                }
                else
                {
                    _mainForm.TextBox.SelectionLength = _mainForm.TextBox.Text.Length - 1;
                }

                AddFontStyle(style, sizeModifier);

                if (i + 1 < matches.Count)
                {
                    _mainForm.TextBox.SelectionStart = matches[i + 1].Index + length;
                    _mainForm.TextBox.SelectionLength = 0;
                    RemoveFontStyle(style);
                }
            }
        }

        private void AddFontStyle(FontStyle style, float sizeModifier)
        {
            _mainForm.TextBox.SelectionFont = new Font(
                _mainForm.TextBox.SelectionFont.FontFamily,
                _mainForm.TextBox.SelectionFont.Size + sizeModifier,
                _mainForm.TextBox.SelectionFont.Style | style
            );
        }

        private void RemoveFontStyle(FontStyle style)
        {
            _mainForm.TextBox.SelectionFont = new Font(
                _mainForm.TextBox.SelectionFont.FontFamily,
                _mainForm.TextBox.SelectionFont.Size,
                _mainForm.TextBox.SelectionFont.Style & ~style
            );
        }

        private void ToggleList(MarkDown.ListType type)
        {
            int caret = _mainForm.TextBox.SelectionStart;
            string start = _mainForm.TextBox.Text.Substring(0, caret);

            _mainForm.TextBox.Text = MarkDown.ToggleList(start, type, out int changed) + _mainForm.TextBox.Text.Substring(caret);
            _mainForm.TextBox.SelectionStart = caret + changed;
            RenderStyling();
        }

        private void InsertAtCaret(string text)
        {
            int caret = _mainForm.TextBox.SelectionStart;
            _mainForm.TextBox.Text = _mainForm.TextBox.Text.Substring(0, caret) + text + _mainForm.TextBox.Text.Substring(caret);
            _mainForm.TextBox.SelectionStart = caret + text.Length;
        }

        private void SurroundSelection(string text)
        {
            int caret = _mainForm.TextBox.SelectionStart;
            _mainForm.TextBox.Text = _mainForm.TextBox.Text.Substring(0, caret) + text + _mainForm.TextBox.SelectedText +
                text + _mainForm.TextBox.Text.Substring(caret + _mainForm.TextBox.SelectionLength);
            _mainForm.TextBox.SelectionStart = caret + text.Length;
        }

        private void AddHeader()
        {
            int caret = _mainForm.TextBox.SelectionStart - 1, i;

            for (i = caret; i > 0 && _mainForm.TextBox.Text[i] != '\n'; i--)
            { }

            if (i > 0)
            {
                i++;
            }

            string divide = _mainForm.TextBox.Text.Substring(i);

            if (divide.StartsWith(MarkDown.Header))
            {
                _mainForm.TextBox.Text = _mainForm.TextBox.Text.Substring(0, i) + divide.Substring(MarkDown.Header.Length);
                _mainForm.TextBox.SelectionStart = caret - MarkDown.Header.Length + 1;
            }
            else
            {
                _mainForm.TextBox.Text = _mainForm.TextBox.Text.Substring(0, i) + MarkDown.Header + divide;
                _mainForm.TextBox.SelectionStart = caret + MarkDown.Header.Length + 1;
            }

            RenderStyling();
        }
    }
}
