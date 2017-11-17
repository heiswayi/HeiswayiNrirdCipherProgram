using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HeiswayiNrirdCipherProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string inputData, outputData, hashCode;
        Dictionary<string, string> encryptionDictionary, decryptionDictionary;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Heiswayi Nrird Cipher Program " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lblDataSizeInput.Content = SizeSuffix(Encoding.UTF8.GetByteCount(tbInput.Text));
            lblDataSizeOutput.Content = SizeSuffix(Encoding.UTF8.GetByteCount(tbOutput.Text));
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                MessageBox.Show("Input is empty.");
            }
            else if (string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Password is required for encryption.");
            }
            else
            {
                pbStatus.Value = 0;
                //pbStatus.IsIndeterminate = true;
                tbOutput.Text = "";
                inputData = tbInput.Text;
                lblStatus.Content = "Encrypting...";
                btnEncrypt.IsEnabled = false;
                btnDecrypt.IsEnabled = false;

                string password = tbPassword.Text;
                hashCode = String.Format("{0:X}", password.GetHashCode());

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync("encrypt");
            }
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                MessageBox.Show("Input is empty.");
            }
            else if (string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Password is required for decryption.");
            }
            else
            {
                pbStatus.Value = 0;
                //pbStatus.IsIndeterminate = true;
                tbOutput.Text = "";
                inputData = tbInput.Text;

                string password = tbPassword.Text;
                hashCode = String.Format("{0:X}", password.GetHashCode());
                string[] splitInput = inputData.Split('$');
                if (splitInput[2] != "HN")
                {
                    MessageBox.Show("Invalid signature. Please make sure you use the encrypted string is encrypted by this program.");
                }
                else
                {
                    if (hashCode != splitInput[0])
                    {
                        MessageBox.Show("Invalid decryption password!");
                    }
                    else
                    {
                        inputData = splitInput[1];
                        lblStatus.Content = "Decrypting...";
                        btnDecrypt.IsEnabled = false;
                        btnEncrypt.IsEnabled = false;

                        BackgroundWorker worker = new BackgroundWorker();
                        worker.WorkerReportsProgress = true;
                        worker.DoWork += worker_DoWork;
                        worker.ProgressChanged += worker_ProgressChanged;
                        worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                        worker.RunWorkerAsync("decrypt");
                    }
                }
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string mode = e.Argument.ToString();

            encryptionDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "a", "h01" },
                { "b", "e02" },
                { "c", "i03" },
                { "d", "s04" },
                { "e", "w05" },
                { "f", "a06" },
                { "g", "y07" },
                { "h", "i08" },
                { "i", "n09" },
                { "j", "r10" },
                { "k", "i11" },
                { "l", "r12" },
                { "m", "d13" },
                { "n", "h14" },
                { "o", "e15" },
                { "p", "i16" },
                { "q", "s17" },
                { "r", "w18" },
                { "s", "a19" },
                { "t", "y20" },
                { "u", "i21" },
                { "v", "n22" },
                { "w", "r23" },
                { "x", "i24" },
                { "y", "r25" },
                { "z", "d26" },
                { "0", "h27" },
                { "1", "e28" },
                { "2", "i29" },
                { "3", "s30" },
                { "4", "w31" },
                { "5", "a32" },
                { "6", "y33" },
                { "7", "i34" },
                { "8", "n35" },
                { "9", "r36" },
                { "~", "i37" },
                { "`", "r38" },
                { "!", "d39" },
                { "@", "h40" },
                { "#", "e41" },
                { "$", "i42" },
                { "%", "s43" },
                { "^", "w44" },
                { "&", "a45" },
                { "*", "y46" },
                { "(", "i47" },
                { ")", "n48" },
                { "-", "r49" },
                { "_", "i50" },
                { "=", "r51" },
                { "+", "d52" },
                { "[", "h53" },
                { "{", "e54" },
                { "]", "i55" },
                { "}", "s56" },
                { @"\", "w57" },
                { "|", "a58" },
                { ";", "y59" },
                { ":", "i60" },
                { "'", "n61" },
                { "\"", "r62" },
                { ",", "i63" },
                { "<", "r64" },
                { ".", "d65" },
                { ">", "h66" },
                { "/", "e67" },
                { "?", "i68" },
                { " ", "s69" },
                { "\n", "w70" },
            };

            decryptionDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "h01", "a" },
                { "e02", "b" },
                { "i03", "c" },
                { "s04", "d" },
                { "w05", "e" },
                { "a06", "f" },
                { "y07", "g" },
                { "i08", "h" },
                { "n09", "i" },
                { "r10", "j" },
                { "i11", "k" },
                { "r12", "l" },
                { "d13", "m" },
                { "h14", "n" },
                { "e15", "o" },
                { "i16", "p" },
                { "s17", "q" },
                { "w18", "r" },
                { "a19", "s" },
                { "y20", "t" },
                { "i21", "u" },
                { "n22", "v" },
                { "r23", "w" },
                { "i24", "x" },
                { "r25", "y" },
                { "d26", "z" },
                { "h27", "0" },
                { "e28", "1" },
                { "i29", "2" },
                { "s30", "3" },
                { "w31", "4" },
                { "a32", "5" },
                { "y33", "6" },
                { "i34", "7" },
                { "n35", "8" },
                { "r36", "9" },
                { "i37", "~" },
                { "r38", "`" },
                { "d39", "!" },
                { "h40", "@" },
                { "e41", "#" },
                { "i42", "$" },
                { "s43", "%" },
                { "w44", "^" },
                { "a45", "&" },
                { "y46", "*" },
                { "i47", "(" },
                { "n48", ")" },
                { "r49", "-" },
                { "i50", "_" },
                { "r51", "=" },
                { "d52", "+" },
                { "h53", "[" },
                { "e54", "{" },
                { "i55", "]" },
                { "s56", "}" },
                { "w57", @"\" },
                { "a58", "|" },
                { "y59", ";" },
                { "i60", ":" },
                { "n61", "'" },
                { "r62", "\"" },
                { "i63", "," },
                { "r64", "<" },
                { "d65", "." },
                { "h66", ">" },
                { "e67", "/" },
                { "i68", "?" },
                { "s69", " " },
                { "w70", "\n" },
            };

            // Everything to lowercase
            string data1 = inputData.ToLower();

            // Reverse everything
            char[] charArrayReverse = data1.ToCharArray();
            Array.Reverse(charArrayReverse);
            string data2 = new string(charArrayReverse);

            string data3 = "";
            if (mode == "encrypt")
            {
                // Encode everything, one by one, update the progress
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data2.Length; i++)
                {
                    foreach (KeyValuePair<string, string> kvp in encryptionDictionary)
                    {
                        if (data2[i].ToString() == kvp.Key)
                        {
                            sb.Append(kvp.Value);
                        }
                    }
                    int progressPercentage = Convert.ToInt32(((double)i / data2.Length) * 100);
                    (sender as BackgroundWorker).ReportProgress(progressPercentage);
                }
                data3 = sb.ToString();
            }
            else
            {
                // Decode everything, one by one, update the progress
                StringBuilder sb = new StringBuilder(data2);
                int max = data2.Length / 3;
                int count = 0;
                foreach (KeyValuePair<string, string> kvp in decryptionDictionary)
                {
                    sb.Replace(kvp.Key, kvp.Value);
                    int progressPercentage = Convert.ToInt32(((double)count++ / max) * 100);
                    (sender as BackgroundWorker).ReportProgress(progressPercentage);
                }
                data3 = sb.ToString();
            }

            // Reverse everything again
            charArrayReverse = data3.ToCharArray();
            Array.Reverse(charArrayReverse);
            string data4 = new string(charArrayReverse);

            // Everything to uppercase for uniformity, signed the output
            if (mode == "encrypt")
            {
                outputData = hashCode.ToUpper() + "$" + data4.ToUpper() + "$HN";
            }
            else
            {
                outputData = data4.ToUpper();
            }

            // Flush data*
            data1 = data2 = data3 = data4 = "";

            (sender as BackgroundWorker).ReportProgress(100);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
            if (e.ProgressPercentage == 100)
            {
                tbOutput.Text = outputData;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus.Content = "Finished";
            btnEncrypt.IsEnabled = true;
            btnDecrypt.IsEnabled = true;
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        private void tbOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblDataSizeOutput.Content = SizeSuffix(Encoding.UTF8.GetByteCount(tbOutput.Text));
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblDataSizeInput.Content = SizeSuffix(Encoding.UTF8.GetByteCount(tbInput.Text));
        }

        private void btnLoadInputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open HNC File";
            openFileDialog.Filter = "HNC Files (*.hnc)|*.hnc";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                tbInputFile.Text = openFileDialog.FileName;
                byte[] raw = File.ReadAllBytes(tbInputFile.Text);
                tbInput.Text = Unzip(raw);
                Mouse.OverrideCursor = null;
            }
        }

        static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        private void btnExportOutputFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".hnc";
            saveFileDialog.Filter = "HNC Files (*.hnc)|*.hnc";
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                byte[] output = Zip(tbOutput.Text);
                File.WriteAllBytes(saveFileDialog.FileName, output);
                MessageBox.Show("Output file has been successfully exported: " + saveFileDialog.FileName);
            }
        }
    }
}
