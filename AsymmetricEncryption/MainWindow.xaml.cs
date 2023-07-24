using System;
using System.Numerics;
using System.Windows;

namespace AsymmetricEncryption
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly AssymetricEncryption encryptor = new AssymetricEncryption();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckSignatureButton_Click(object sender, RoutedEventArgs e)
        {
            statusBar.Text = string.Empty;
            statusBar.Text = encryptor.P != BigInteger.Zero
                ? encryptor.VerificateSignatureSucsess ? "Подпись является подлинной!" : "Подпись не является подлинной!"
                : "Ключ не сгенерирован!";
        }

        private void KeyGeneratorButton_Click(object sender, RoutedEventArgs e)
        {
            statusBar.Text = string.Empty;
            if (messageTextBox.Text.Length != 0)
            {
                encryptor.M = messageTextBox.Text;
                encryptor.GenerateKeys();
                encryptor.GenerateSignature();
                pText.Text = encryptor.P.ToString();
                gText.Text = encryptor.G.ToString();
                yText.Text = encryptor.Y.ToString();
                xText.Text = encryptor.X.ToString();
                kText.Text = encryptor.K.ToString();
                aText.Text = encryptor.A.ToString();
                bText.Text = encryptor.B.ToString();
            }
            else
            {
                statusBar.Text = "Введите сообщение!";
            }
        }
    }
}
