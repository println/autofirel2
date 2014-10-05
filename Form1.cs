using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AutoFire
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeFormControls();
            InitializeBasicComponents();
        }

        #region Seeker

        private WindowSeeker Seeker;
        private int SelectedWindowID = 0;

        private const string TEXT_VALUE = "Enter a Key";

        private void InitializeBasicComponents()
        {
            Seeker = new WindowSeeker();
            Seeker.Name = "Lineage II";
            Seeker.WindowEvent += new WindowSeeker.WindowEventHandler(ltbx_windows_Update);//alteracao na lista de janelas
            Seeker.ActivityWinEvent += new WindowSeeker.ActivityWinEventHandler(ActivityWindow_running);//rodando
            Seeker.WindowClosedEvent += new WindowSeeker.WindowClosedEventHandler(Window_Closed);//janela fechou
            Seeker.Start();
        }

        private delegate void AddListBoxItemDelegate(object names, object ids);
        private void ltbx_windows_Update(object names, object ids)
        {

            if (ltbx_windows.InvokeRequired)
                ltbx_windows.Invoke(new AddListBoxItemDelegate(ltbx_windows_Update), names, ids);
            else
            {
                ltbx_windows.BeginUpdate();//Paint mode - begin

                ltbx_windows.Items.Clear();
                ltbx_windows_Update_apply((string[])names, (int[])ids);

                ltbx_windows.EndUpdate();//end
            }
        }

        private void ltbx_windows_Update_apply(string[] names, int[] ids)
        {
            for (int i = 0; i < names.Length; i++)
            {
                ltbx_windows.Items.Add(ids[i] + "\t" + names[i]);

                if (SelectedWindowID > 0 && SelectedWindowID == ids[i])
                    ltbx_windows.SelectedIndex = i;

                if (SelectedWindowID == 0 && i == 0)
                {
                    SelectedWindowID = ids[i];
                    ltbx_windows.SelectedIndex = 0;
                }
            }
        }

        private void ltbx_windows_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newSelectedWindowID = Seeker.Set(((ListBox)sender).SelectedIndex);
            if (SelectedWindowID != newSelectedWindowID)
            {
                DisableAll();
                SelectedWindowID = newSelectedWindowID;
            }
        }

        private void ActivityWindow_running(bool active)
        {
            if (active)
                pnl_activity.BackColor = System.Drawing.Color.GreenYellow;
            else
                pnl_activity.BackColor = System.Drawing.Color.Empty;
        }

        private void Window_Closed(bool critical, string info)
        {
            SelectedWindowID = -1;

            if (critical)
                MessageBox.Show(info);

            DisableAll();
        }

        private void DisableAll()
        {
            List<Button> newButtonList = new List<Button>(ButtonList);

            foreach (Button button in newButtonList)
                Disable(button);
        }

        private delegate void DisableDelegate(Button button);
        private void Disable(Button button)
        {
            if (button.InvokeRequired)
                button.Invoke(new DisableDelegate(Disable), button);
            else
                button.PerformClick();
        }

        #endregion

        #region FormControls

        private List<ComboBox> ComboList = new List<ComboBox>();
        private List<Button> ButtonList = new List<Button>();
        private List<Macro> MacroList = new List<Macro>();

        private void InitializeFormControls()// Sets default values for all fields
        {

            string[] comboValues = GenerateComboValues();

            foreach (Control groupbox in this.Controls)
            {
                if (groupbox is GroupBox)
                {
                    foreach (Control element in groupbox.Controls)
                    {

                        if (element is Panel)
                        {

                            foreach (Control field in element.Controls)
                            {
                                if (field is ComboBox)
                                    SetComboBox((ComboBox)field, comboValues);

                                if (field is NumericUpDown)
                                    SetNumericUpDown((NumericUpDown)field);
                            }

                        }

                        if (element is Button)
                            SetButton((Button)element);

                        if (element is TextBoxEX)
                            SetTextBox((TextBoxEX)element);
                    }
                }
            }

        }

        private void SetNumericUpDown(NumericUpDown numericUpDown)//initial
        {
            numericUpDown.Maximum = 300000;//5 minutes - 1000(ms) * 60(s) * 5(min)
            numericUpDown.Minimum = 9;
            numericUpDown.Value = 340;
        }

        private void SetComboBox(ComboBox combobox, string[] comboboxValues)//initial
        {
            BindingSource bsr = new BindingSource();
            bsr.DataSource = comboboxValues;
            combobox.DataSource = new BindingSource(bsr.DataSource, bsr.DataMember);
            combobox.SelectedIndexChanged += new System.EventHandler(GenericComboBox_SelectedIndexChanged);
            ComboList.Add(combobox);
        }

        private void SetButton(Button button)
        {
            SetButtonText(button);
            button.Click += new System.EventHandler(GenericButton_Click);
        }

        private void SetTextBox(TextBoxEX textboxEX)
        {
            SetTextBoxText(textboxEX);
            textboxEX.Enter += new System.EventHandler(GenericTextBox_Focus);
            textboxEX.Leave += new System.EventHandler(GenericTextBox_Leave);
            textboxEX.KeyDown += new System.Windows.Forms.KeyEventHandler(GenericTextBox_KeyDown);
        }

        private void SetTextBoxText(TextBoxEX textboxEx)
        {
            textboxEx.Text = TEXT_VALUE;
        }

        private void SetButtonText(Button button)
        {
            button.Text = "Activate";
        }

        private string[] GenerateComboValues()//inicial
        {
            List<string> texts = new List<string>();

            for (int i = 0; i <= 12; i++)
            {
                if (i == 0)
                    texts.Add("");
                else
                    texts.Add("F" + i.ToString());
            }

            return texts.ToArray();
        }

        private void GenericComboBox_SelectedIndexChanged(object sender, EventArgs e)//Event - combobox controller
        {

            //ComboBox selected_combobox = (ComboBox)sender;

            //if (selected_combobox.SelectedIndex > 0)
            //{
            //    foreach (ComboBox combobox in ComboList)
            //    {

            //        if (combobox.SelectedIndex == selected_combobox.SelectedIndex && combobox != selected_combobox)
            //        {
            //            if (!combobox.Enabled)
            //            {
            //                DialogResult question = MessageBox.Show("Would you like to change previously selected key?", "Change key", MessageBoxButtons.OKCancel);
            //                if (question == DialogResult.Cancel)
            //                {
            //                    selected_combobox.ResetText();
            //                    break;
            //                }
            //                else
            //                {
            //                    Break((ComboBox)combobox);
            //                }
            //            }
            //            combobox.ResetText();
            //        }
            //    }
            //}
        }

        private void GenericTextBox_Focus(object sender, EventArgs e)//Event - textbox controller
        {
            ((TextBoxEX)sender).Text = "";
        }

        private void GenericTextBox_Leave(object sender, EventArgs e)//Event
        {
            TextBoxEX textboxEx = (TextBoxEX)sender;

            if (textboxEx.BoxKeys == Keys.None)
                SetTextBoxText(textboxEx);

            if (textboxEx.BoxKeys != Keys.None && textboxEx.Text.Equals(""))
                textboxEx.Text = ASCII.Converter(textboxEx.Args);
        }

        private void GenericTextBox_KeyDown(object sender, KeyEventArgs e)//Event
        {
            TextBoxEX textboxEx = (TextBoxEX)sender;

            string text = ASCII.Converter(e);

            if (!text.Equals(""))
            {
                textboxEx.BoxKeys = e.KeyData;
                textboxEx.Args = e;
                textboxEx.Text = ASCII.Converter(textboxEx.Args);
            }
        }

        private void GenericButton_Click(object sender, EventArgs e)//Event - button controller
        {
            Button button = ((Button)sender);

            GroupBox groupbox = (GroupBox)button.Parent;

            Validator validator = new Validator(groupbox);

            if (validator.Ready() && !ButtonList.Contains(button) && SelectedWindowID > 0)
            {
                Macro macro = new Macro(validator.GetProfile(), chk_mode.Checked);

                ButtonList.Add(button);
                MacroList.Add(macro);

                button.Text = "Deactivate";

                Lock(true, groupbox);
            }
            else if (ButtonList.Contains(button))
            {
                int index = ButtonList.IndexOf(button);

                Macro macro = MacroList[index];
                macro.Destroy();

                MacroList.Remove(macro);
                ButtonList.Remove(button);

                SetButtonText(button);

                Lock(false, groupbox);
            }

            if (ButtonList.Count > 0)
                chk_mode.Enabled = false;
            else
                chk_mode.Enabled = true;

        }

        //private void KeySender(object sender) { }
        #endregion

        #region FormManagers

        private void Break(ComboBox combobox)
        {
            GroupBox groupbox = (GroupBox)((Panel)combobox.Parent).Parent;
            foreach (Control element in groupbox.Controls)
            {
                if (element is Button)
                {
                    ((Button)element).PerformClick();
                    break;
                }
            }
        }

        private void Lock(bool locked, GroupBox groupbox)
        {
            locked = !locked;

            foreach (Control element in groupbox.Controls)
            {

                if (element is Panel)
                    foreach (Control field in element.Controls)
                        field.Enabled = locked;

                element.Enabled = locked;

                if (element is Button)
                    element.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//Event
        {
            Seeker.Destroy();
        }
        #endregion

        #region Storage
        private void btn_save_Click(object sender, EventArgs e)
        {
            Storage storage = new Storage(this);
            storage.Save();
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            if (ButtonList.Count == 0)
            {
                Storage storage = new Storage(this);
                storage.Load();
            }
            else
                MessageBox.Show("You need deactivate all panels!");
        }
        #endregion

        #region NewForm_about
        private void btn_about_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog(this);
        }
        #endregion

        #region Skill Label
        private void Label_edit_Click(object sender, EventArgs e)
        {

            var label = sender as Label;

            TextBoxEXLabel text = new TextBoxEXLabel();

            text.Label = label;

            text.Leave += new System.EventHandler(this.TextBox_edit_Save);
            text.KeyDown += new KeyEventHandler(this.TextBox_KeyDown);

            text.Text = label.Text;

            text.Parent = label.Parent;

            text.Location = new System.Drawing.Point(0, 3);
            text.Name = "textedit";
            text.Size = new System.Drawing.Size(83, 20);
            text.TabIndex = 3;

            text.BringToFront();

            text.Focus();
        }

        private void TextBox_edit_Save(object sender, EventArgs e)
        {
            TextBoxEXLabel text = sender as TextBoxEXLabel;

            Label label = text.Label;

            label.Text = text.Text;

            text.Dispose();

        }

        private void TextBox_edit_Cancel(object sender, EventArgs e)
        {
            TextBoxEXLabel text = sender as TextBoxEXLabel;
            text.Dispose();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            TextBoxEXLabel text = sender as TextBoxEXLabel;
            switch (e.KeyData)
            {
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    text.Leave -= new System.EventHandler(this.TextBox_edit_Save);
                    text.KeyDown -= new KeyEventHandler(this.TextBox_KeyDown);
                    TextBox_edit_Cancel(sender, null);
                    break;
                case Keys.Return:
                    DialogResult = DialogResult.OK;
                    text.KeyDown -= new KeyEventHandler(this.TextBox_KeyDown);
                    TextBox_edit_Save(sender, null);
                    break;
            }

        }
        #endregion
    }
}
