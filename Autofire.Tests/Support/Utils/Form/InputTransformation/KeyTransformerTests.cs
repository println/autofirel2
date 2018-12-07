using System.Windows.Forms;
using System.Windows.Input;
using Autofire.Support.Utils.Form.InputTransformation;
using NUnit.Framework;

namespace Autofire.Tests.Support.Utils.Form.InputTransformation
{
    [TestFixture()]
    public class KeyTransformerTests
    {
        public class KeyToStringTest
        {
            [Test()]
            public void SimpleKeyIn()
            {
                var kt = new KeyTransformer();

                string expected = "Ctrl + Alt + A";
                string result = kt.KeyToString(Key.A, ModifierKeys.Control | ModifierKeys.Alt);

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void CedilKeyIn()
            {
                var kt = new KeyTransformer();

                string expected = "Ctrl + Alt + Ç";
                string result = kt.KeyToString(Key.Oem1, ModifierKeys.Control | ModifierKeys.Alt);

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void SlashKeyIn()
            {
                var kt = new KeyTransformer();

                string expected = "Ctrl + Alt + LButton, Oemtilde";
                string result = kt.KeyToString(Key.AbntC1, ModifierKeys.Control | ModifierKeys.Alt);

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void DigitIn()
            {
                var kt = new KeyTransformer();

                string expected = "1";
                string result = kt.KeyToString(Key.D1, ModifierKeys.None);

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void NumPadDigitIn()
            {
                var kt = new KeyTransformer();

                string expected = "NumPad1";
                string result = kt.KeyToString(Key.NumPad1, ModifierKeys.None);

                Assert.AreEqual(expected, result);
            }
            
        }

        public class KeysFromStringTest
        {
            [Test()]
            public void SimpleKeyOut()
            {
                var kt = new KeyTransformer();

                Keys expected = Keys.Control | Keys.Alt | Keys.A;
                Keys result = kt.KeysFromString("Ctrl + Alt + A");

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void CedilKeyOut()
            {
                var kt = new KeyTransformer();

                Keys expected = Keys.Control | Keys.Alt | Keys.Oem1;
                Keys result = kt.KeysFromString("Ctrl + Alt + Ç");

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void SlashKeyOut()
            {
                var kt = new KeyTransformer();

                Keys expected = Keys.Control | Keys.Alt | Keys.LButton | Keys.Oemtilde;
                Keys result = kt.KeysFromString("Ctrl + Alt + LButton, Oemtilde");

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void DigitOut()
            {
                var kt = new KeyTransformer();

                Keys expected = Keys.D1;
                Keys result = kt.KeysFromString("1");

                Assert.AreEqual(expected, result);
            }

            [Test()]
            public void NumPadDigitOut()
            {
                var kt = new KeyTransformer();

                Keys expected = Keys.NumPad1;
                Keys result = kt.KeysFromString("NumPad1");

                Assert.AreEqual(expected, result);
            }
        }
    }
}