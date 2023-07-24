using System;
using System.Text;
using System.Windows;
using System.Security.Cryptography;

namespace RSA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly UnicodeEncoding ByteConverter = new UnicodeEncoding();
        readonly RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        private byte[] plaintext;
        private byte[] encryptedtext;

        public MainWindow()
        {
            InitializeComponent();
        }

        public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                errorStatusBar.Content = e.Message;
                return null;
            }
        }

        public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                errorStatusBar.Content = e.ToString();
                return null;
            }
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            if (qTextBox.Text.Length > 0 && pTextBox.Text.Length > 0 && long.TryParse(qTextBox.Text, out long q) && long.TryParse(pTextBox.Text, out long p))
            {
                if (IsTheNumberSimple(Convert.ToInt64(qTextBox.Text)) && IsTheNumberSimple(Convert.ToInt64(pTextBox.Text)))
                {
                    plaintext = ByteConverter.GetBytes(originalTextBox.Text);
                    encryptedtext = Encryption(plaintext, RSA.ExportParameters(true), false);
                    encodingTextBox.Text = ByteConverter.GetString(encryptedtext);
                    byte[] decryptedtex = Decryption(encryptedtext,
                    RSA.ExportParameters(true), false);
                    decodingTextBox.Text = ByteConverter.GetString(decryptedtex);
                }
                else
                {
                    errorStatusBar.Content = "Числа p и q не простые!";
                }
            }
            else
            {
                errorStatusBar.Content = "Введите p и q!";
            }
        }

        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
    }
}
