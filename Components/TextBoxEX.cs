using System.Windows.Forms;

namespace AutoFire
{
    class TextBoxEX : TextBox
    {
        private Keys key = Keys.None;
        private KeyEventArgs args;

        public KeyEventArgs Args
        {
            get { return args; }
            set { args = value; }
        }

        public Keys BoxKeys
        {
            get { return key; }
            set { key = value; }
        }

        
    }
}