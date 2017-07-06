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

        private void EncryptFile(string inputFile, string outputFile) {

            try {
                string password = @"myKey123"; // Your Key Here
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
            }
            catch {
                MessageBox.Show("Encryption failed!", "Error");
            }
        }//End Encrypt

        private void DecryptFile(string inputFile, string outputFile) {
            {
                string password = @"myKey123"; // Your Key Here

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
            }
        }//End Decrpyt
    }//End Partial Class MainWindow
}//End NameSpace
