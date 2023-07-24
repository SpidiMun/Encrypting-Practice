using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace Compression
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string WrongFileExtensionError => "Выберите текстовый файл!";
        private string EmptyFileError => "Используемый вами файл пуст!";
        private string NotHuffmanFileNameError => "Данный файл не был сжат методом Хаффмана!";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "originText",
                DefaultExt = ".txt",
                Filter = $"Text files|*.txt"
            };
            if (openDialog.ShowDialog() == true)
            {
                originTextBox.Text = openDialog.FileName;
            }
        }

        private void ButtonCheck1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = Process.Start(originTextBox.Text);
            }
            catch
            {
                _ = MessageBox.Show(WrongFileExtensionError);
            }
        }

        private void ButtonCheck2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = Process.Start(huffmanTextBox.Text);
            }
            catch
            {
                _ = MessageBox.Show(WrongFileExtensionError);
            }
        }

        private void ButtonCheck3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = Process.Start(arithmeticTextBox.Text);
            }
            catch
            {
                _ = MessageBox.Show(WrongFileExtensionError);
            }
        }

        private void ButtonHuffman_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(originTextBox.Text, "txt"))
            {
                _ = MessageBox.Show(WrongFileExtensionError);
                return;
            }
            if (originTextBox.Text.Length > 0)
            {
                string message = string.Empty;
                string fileName = originTextBox.Text.Split('/')[originTextBox.Text.Split('/').Length - 1];
                StreamReader sr = new StreamReader(fileName);
                while (!sr.EndOfStream)
                {
                    message += sr.ReadLine();
                }
                sr.Close();

                HuffmanTree huffmanTree = new HuffmanTree();
                huffmanTree.Build(message);
                BitArray encoded = huffmanTree.Encode(message);
                message = string.Empty;
                for (int i = 0; i < encoded.Length; i++)
                {
                    message += Convert.ToInt32(encoded[i]).ToString();
                }
                fileName = originTextBox.Text.Split('.')[0] + "huffman." + originTextBox.Text.Split('.')[1];
                huffmanTextBox.Text = fileName;
                StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(message);
                sw.Close();
            }
            else
            {
                _ = MessageBox.Show(EmptyFileError);
            }
        }

        private void ButtonArithmetic_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(huffmanTextBox.Text, "txt"))
            {
                _ = MessageBox.Show(WrongFileExtensionError);
                return;
            }
            if (!Regex.IsMatch(huffmanTextBox.Text, "huffman"))
            {
                _ = MessageBox.Show(NotHuffmanFileNameError);
                return;
            }
            if (huffmanTextBox.Text.Length > 0)
            {
                string message = string.Empty;
                string fileName = originTextBox.Text.Split('/')[originTextBox.Text.Split('/').Length - 1];
                StreamReader sr = new StreamReader(fileName);
                while (!sr.EndOfStream)
                {
                    message += sr.ReadLine();
                }
                sr.Close();

                ArithmeticCoding arithmeticCoding = new ArithmeticCoding();
                message = arithmeticCoding.Algorithm(message).ToString();

                fileName = originTextBox.Text.Split('.')[0] + "arithmetic." + originTextBox.Text.Split('.')[1];
                arithmeticTextBox.Text = fileName;
                StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(message);
                sw.Close();
            }
            else
            {
                _ = MessageBox.Show(EmptyFileError);
            }
        }
    }
}
