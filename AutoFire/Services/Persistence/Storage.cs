using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;

namespace AutoFire.Service.Persistence
{
    public class Storage
    {
        Form form;
        public Storage(Form f)
        {
            form = f;
        }
        public void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "config files(*.config)|*.config";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                WriteXML(saveFileDialog.FileName);

        }
        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "congif files(*.config)|*.config";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
                ReadXML(openFileDialog.FileName);
        }

        private void ReadXML(string file)
        {

            XElement root = XElement.Load(file);

            int g = 0;

            foreach (Control groupbox in form.Controls)
            {
                if (groupbox is GroupBox)
                {
                    XElement groupboxXML = root.Elements("macro").ElementAt(g);

                    int p = 0;

                    foreach (Control element in groupbox.Controls)
                    {
                        if (element is Panel)
                        {
                            XElement panelXML = groupboxXML.Elements("key_interval").ElementAt(p);

                            foreach (Control field in element.Controls)
                            {
                                if (field is Label)
                                {
                                    if (panelXML.Element("name") != null)
                                        ((Label)field).Text = panelXML.Element("name").Value;
                                }

                                if (field is ComboBox)
                                    ((ComboBox)field).SelectedIndex = (int)Convert.ToInt32(panelXML.Element("key").Value);

                                if (field is NumericUpDown)
                                    ((NumericUpDown)field).Value = (int)Convert.ToInt32(panelXML.Element("interval").Value);
                            }
                            p++;
                        }

                        if (element is CheckBox)
                            ((CheckBox)element).Checked = Convert.ToBoolean(groupboxXML.Element("loop").Value);

                        if (element is TextBoxEX)
                        {
                            XElement key = groupboxXML.Element("activation-key");
                            if (key != null)
                            {
                                TextBoxEX textbox = element as TextBoxEX;
                                KeysConverter kc = new KeysConverter();

                                textbox.BoxKeys = (Keys)kc.ConvertFromInvariantString(key.Element("winkeys").Value);
                                textbox.Text = key.Element("label").Value;
                                textbox.Args = new KeyEventArgs(textbox.BoxKeys);
                            }

                        }
                    }
                    g++;
                }
            }
        }

        private void WriteXML(string file)
        {
            XElement root = new XElement("AutoFire");

            foreach (Control groupbox in form.Controls)
            {
                if (groupbox is GroupBox)
                {
                    XElement groupboxXML = new XElement("macro");

                    foreach (Control element in groupbox.Controls)
                    {

                        if (element is Panel)
                        {
                            XElement panelXML = new XElement("key_interval");

                            foreach (Control field in element.Controls)
                            {
                                if (field is Label)
                                    panelXML.Add(new XElement("name", ((Label)field).Text));

                                if (field is ComboBox)
                                    panelXML.Add(new XElement("key", ((ComboBox)field).SelectedIndex));

                                if (field is NumericUpDown)
                                    panelXML.Add(new XElement("interval", ((NumericUpDown)field).Value));
                            }

                            groupboxXML.Add(panelXML);
                        }

                        if (element is CheckBox)
                            groupboxXML.Add(new XElement("loop", ((CheckBox)element).Checked.ToString()));

                        if (element is TextBoxEX)
                        {
                            TextBoxEX textbox = element as TextBoxEX;

                            if (!textbox.BoxKeys.Equals(Keys.None))
                            {
                                XElement key = new XElement("activation-key");
                                KeysConverter kc = new KeysConverter();

                                key.Add(new XElement("label", textbox.Text));
                                key.Add(new XElement("winkeys", kc.ConvertToInvariantString(textbox.BoxKeys)));

                                groupboxXML.Add(key);
                            }
                        }
                    }
                    root.Add(groupboxXML);
                }
            }
            root.Save(file);
        }
    }
}
