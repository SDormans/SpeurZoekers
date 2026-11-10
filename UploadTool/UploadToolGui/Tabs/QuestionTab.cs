using UploadToolGui.Dto.Interfaces;
using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.UploadTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadToolGui
{
    public class QuestionTab
    {
        private readonly MainForm _mainForm;

        private IQuestionDto _item;

        public QuestionTab(MainForm mainForm)
        {
            _mainForm = mainForm;

            mainForm.QuestionPointsSelector.Maximum = int.MaxValue;

            _mainForm.QuestionInput.TextChanged += QuestionInput_TextChanged;
            _mainForm.QuestionPointsSelector.ValueChanged += QuestionPointsSelector_ValueChanged;

            for (int i = 0; i < 5; i++)
            {
                ((TextBox)_mainForm.QuestionPanel.Controls.Find("Choice" + i, false)[0])
                    .TextChanged += Choice_TextChanged;
                ((CheckBox)_mainForm.QuestionPanel.Controls.Find("Answer" + i, false)[0])
                    .CheckedChanged += Answer_CheckedChanged;
            }
        }

        public void SetupQuestionTab()
        {
            _mainForm.Tabs.Controls.Add(_mainForm.TabQuestion);
        }

        public void LoadQuestionTab(IQuestionDto item)
        {
            _item = item;

            _mainForm.QuestionInput.Text = _item.QuestionText;
            _mainForm.QuestionPointsSelector.Value = _item.QuestionPoints;

            var textBoxes = _mainForm.QuestionPanel.Controls
                .Cast<Control>()
                .Where(x => x.Name.StartsWith("Choice"))
                .Cast<TextBox>()
                .OrderBy(x => x.Name);
            var checks = _mainForm.QuestionPanel.Controls
                .Cast<Control>()
                .Where(x => x.Name.StartsWith("Answer"))
                .Cast<CheckBox>()
                .OrderBy(x => x.Name);

            int i;

            for (i = 0; i < _item.QuestionChoices.Count && i < 5; i++)
            {
                QuestionChoice quest = _item.QuestionChoices.ElementAt(i);
                textBoxes.ElementAt(i).Text = quest.Text;
                checks.ElementAt(i).Checked = quest.IsAnswer;
            }

            for (; i < 5; i++)
            {
                textBoxes.ElementAt(i).Text = string.Empty;
                checks.ElementAt(i).Checked = false;
            }
        }

        public Task<Dictionary<string, DataConsistency.ItemStatus>> ValidateData()
        {
            var items = new Dictionary<string, DataConsistency.ItemStatus>();

            if (string.IsNullOrWhiteSpace(_item.QuestionText)
                || !_item.QuestionChoices.Any())
            {
                items.Add("Vraag(keuzes)", DataConsistency.ItemStatus.Optional);
            }
            else if (_item.QuestionChoices.Count(x => x.IsAnswer) != 1)
            {
                items.Add("Uniek correct antwoord", DataConsistency.ItemStatus.Optional);
            }

            if (_item.QuestionPoints == 0)
            {
                items.Add("Punten vraag", DataConsistency.ItemStatus.Optional);
            }

            return Task.FromResult(items);
        }

        public void QuestionInput_TextChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                _item.QuestionText = _mainForm.QuestionInput.Text;
                _mainForm.ValueChanged();
            }
        }

        public void QuestionPointsSelector_ValueChanged(object sender, EventArgs e)
        {
            if (!_mainForm.IsLoading)
            {
                _item.QuestionPoints = (int)_mainForm.QuestionPointsSelector.Value;
                _mainForm.ValueChanged();
            }
        }

        public void Choice_TextChanged(object sender, EventArgs e)
        {
            ChoiceChanged(sender as Control);
        }

        public void Answer_CheckedChanged(object sender, EventArgs e)
        {
            ChoiceChanged(sender as Control);
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(_item.QuestionText))
            {
                _item.QuestionChoices.Clear();
                _item.QuestionPoints = 0;
            }
        }

        private void ChoiceChanged(Control control)
        {
            if (_mainForm.IsLoading)
            {
                return;
            }

            int index = int.Parse(control.Name.Substring(6));
            TextBox textBox = _mainForm.QuestionPanel.Controls.Find("Choice" + index, false).First() as TextBox;
            CheckBox check = _mainForm.QuestionPanel.Controls.Find("Answer" + index, false).First() as CheckBox;
            bool empty = string.IsNullOrWhiteSpace(textBox.Text);
            QuestionChoice choice = _item.QuestionChoices.ElementAtOrDefault(index);

            if (!empty)
            {
                if (choice != null)
                {
                    choice.Text = textBox.Text;
                    choice.IsAnswer = check.Checked;
                }
                else
                {
                    _item.QuestionChoices.Add(new QuestionChoice
                    {
                        Text = textBox.Text,
                        IsAnswer = check.Checked
                    });
                }
            }
            else if (choice != null)
            {
                _item.QuestionChoices.Remove(choice);
            }

            _mainForm.ValueChanged();
        }
    }
}
