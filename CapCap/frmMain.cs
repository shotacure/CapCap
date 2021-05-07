using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace CapCap
{
    public partial class frmMain : Form
    {
        private Bitmap captionImage;
        private Bitmap captureImage;
        private Bitmap combinedImage;
        private Graphics combinedGraphics;

        private string captionPath;
        private string watchPath;
        private string savePath;

        /// <summary>
        /// 
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            // キャプション画像ディレクトリ
            captionPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // キャプチャ画像監視ディレクトリ
            watchPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // 出力先ディレクトリ
            savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), @"capture");
            // 存在しない場合はフォルダ作成
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // ダイアログ設定
            openFileDialog1.Filter = "PNGファイル (*.png)|*.*";
            openFileDialog1.FileName = "*.png";
            openFileDialog1.InitialDirectory = captionPath;
            openFileDialog1.Title = "合成用ファイル";

            // ピクチャ設定
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            // ファイルチェッカ設定
            fileSystemWatcher1.Path = watchPath;
            fileSystemWatcher1.Filter = "*.jpg";
        }

        /// <summary>
        /// フォーム開始時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            // ダイアログ表示
            openFileDialog1.ShowDialog();
        }

        /// <summary>
        /// 出典表記ファイル読込時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            captionImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = captionImage;
        }

        /// <summary>
        /// ファイル検知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            // 検知したファイルがJPGなら指定ファイルを合成して上書き
            if (e.ChangeType == WatcherChangeTypes.Changed && e.FullPath.EndsWith(".jpg"))
            {
                try 
                {
                    System.Threading.Thread.Sleep(40);

                    // キャプチャ画像読み込み
                    captureImage = new Bitmap(e.FullPath);
                    combinedImage = new Bitmap(captureImage);
                    captureImage.Dispose();
                    File.Delete(e.FullPath);

                    combinedGraphics = Graphics.FromImage(combinedImage);
                    
                    combinedGraphics.DrawImage(captionImage, 0, 0, captionImage.Width, captionImage.Height);
                    
                    pictureBox1.Image = combinedImage;
                    combinedImage.Save(Path.Combine(savePath, Path.GetFileName(e.FullPath)), ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// フォームリサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Resize(object sender, EventArgs e)
        {
            // ウインドウ比率
            this.Height = (this.Width - 16) / 16 * 9 + 39;

            // ピクチャボックス
            pictureBox1.Width = this.Width - 16;
            pictureBox1.Height = this.Height - 39;
        }

        /// <summary>
        /// ピクチャボックスクリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // ダイアログ表示
            openFileDialog1.ShowDialog();
        }
    }
}
