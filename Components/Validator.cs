using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoFire
{
    class Validator
    {
        List<object> keys = new List<object>();
        List<int> values = new List<int>();
        private bool loop;
        private Keys activation;

        public Validator(GroupBox groupbox)
        {
            foreach (Control c in groupbox.Controls)
            {
                if (c is Panel)
                    ValidateKeyValue(c);

                if (c is CheckBox)
                    ValidateLoop(c);

                if (c is TextBoxEX)
                    ValidateActivator(c);
            }
        }

        public bool Ready()
        {
            bool result = (keys.Count > 0 && activation != Keys.None);
            return result;
        }

        public DataProfile GetProfile()
        {
            DataProfile profile = new DataProfile();

            profile.keys = keys.ToArray();
            profile.values = values.ToArray();
            profile.loop = loop;
            profile.activation = activation;

            return profile;
        }

        private void ValidateKeyValue(object obj)
        {
            string key = "";
            int value = 0;

            foreach (Control field in ((Panel)obj).Controls)
            {
                if (field is ComboBox)
                    key = field.Text;

                if (field is NumericUpDown)
                    value = (int)((NumericUpDown)field).Value;
            }

            if (!key.Equals("") && value > 0)
            {
                keys.Add(CreateKey(key));
                values.Add(value);
            }

        }

        private void ValidateLoop(object obj)
        {
            loop = ((CheckBox)obj).Checked;
        }

        private void ValidateActivator(object obj)
        {
            activation = ((TextBoxEX)obj).BoxKeys;
        }

        private Keys CreateKey(String txt)
        {
            return (Keys)Enum.Parse(typeof(Keys), txt, true);
        }
    }

    struct DataProfile
    {
        public object[] keys;
        public int[] values;
        public bool loop;
        public object activation;
    }

}
