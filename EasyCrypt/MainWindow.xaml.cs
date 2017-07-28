using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EasyCrypt {
    /// <summary>
    /// By:   Ryan Moise
    /// Date: July 6th 2017
    /// Goal: Easily Encrypt/Decrypt Multiple File Types
    /// </summary>
    public partial class MainWindow : Window {

        string openFileName = "";
        string saveFileName = "";

        public MainWindow(){
            InitializeComponent();
        }

        private void openBTTN_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog fileDialog = new OpenFileDialog();
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            if (dialogOK == true) {

                openFileName = fileDialog.FileName;
                originalTB.Text = openFileName;
            }
        }//End OpenBTTN Click

        private void saveBTTN_Click(object sender, RoutedEventArgs e) {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            Nullable<bool> dialogOK = saveFileDialog.ShowDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|Database File (*.db)|*.db|SQLite file (*.sqlite)|*.sqlite";

            if (dialogOK == true) {
                saveFileName = saveFileDialog.FileName;
                newFileTB.Text = saveFileName;
            }
        }//End SaveBTTN Click

        private void encryptBTTN_Click(object sender, RoutedEventArgs e) {
            EncryptFile(openFileName, saveFileName);
        }//End EncryptBTTN Click

        private void decryptBTTN_Click(object sender, RoutedEventArgs e)
        {
            DecryptFile(openFileName, saveFileName);
        }//End DecryptBTTN Click

        private void EncryptFile(string inputFile, string outputFile) {

            if (passwordTB.Text.Length <= 7)
            {
                MessageBox.Show("Enter a password with a length of 8 characters or more");
            }
            else
            {
                try
                {
                    string password = passwordTB.Text;
                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);

                    string cryptFile = outputFile;
                    FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                    FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                    int data;
                    while ((data = fsIn.ReadByte()) != -1)
                        cs.WriteByte((byte)data);

                    fsIn.Close();
                    cs.Close();
                    fsCrypt.Close();
                    usermssgLBL.Foreground = System.Windows.Media.Brushes.Green;
                    usermssgLBL.Content = "Encryption Successful!";
                }
                catch
                {
                    usermssgLBL.Foreground = System.Windows.Media.Brushes.Red;
                    usermssgLBL.Content = "Failed!";
                }
            }
        }//End Encrypt

        private void DecryptFile(string inputFile, string outputFile) {
            {
                if (passwordTB.Text.Length <= 7)
                {
                    MessageBox.Show("Enter a password with a length of 8 characters or more");
                }
                else
                {
                    try
                    {
                        string password = passwordTB.Text; // Your Key Here
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                        RijndaelManaged RMCrypto = new RijndaelManaged();

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                        int data;
                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
                        usermssgLBL.Foreground = System.Windows.Media.Brushes.Green;
                        usermssgLBL.Content = "Decryption Successful!";
                    }
                    catch
                    {
                        usermssgLBL.Foreground = System.Windows.Media.Brushes.Red;
                        usermssgLBL.Content = "Failed!";
                    }
                }
            }
        }//End Decrpyt
    }//End Partial Class MainWindow
}//End NameSpace
