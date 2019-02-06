using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog {
                Filter = "Файли зображень (*.bmp,*.jpg,*.jpeg,*.gif,*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png|Усі файли (*.*) | *.* "
            };
            if (ofd.ShowDialog() == DialogResult.OK) {

                sourceImagePB.Load(ofd.FileName);

                Bitmap sourceBitmap = (Bitmap)sourceImagePB.Image;
                BasePicturePropertys.Text = "H:" + sourceBitmap.Height + " W:" + sourceBitmap.Width + "\r\n"
                    + "Total pixels: " + sourceBitmap.Height * sourceBitmap.Width;
                Application.DoEvents();

                int numberOfColors = 256;

                FPTNPicture.Image = QBitmap.LinearQuantization((Bitmap)sourceImagePB.Image, numberOfColors);
                FPTNPictureESTLabel.Text = "EST=" + QBitmap.BitmapEST((Bitmap)sourceImagePB.Image, (Bitmap)FPTNPicture.Image).ToString();
                Application.DoEvents();

                FPTYPicture.Image = QBitmap.Tonning((Bitmap)sourceImagePB.Image, (Bitmap)FPTNPicture.Image);
                FPTYPictureESTLabel.Text = "EST=" + QBitmap.BitmapEST((Bitmap)sourceImagePB.Image, (Bitmap)FPTNPicture.Image).ToString();
                Application.DoEvents();

                OPTNPicture.Image = QBitmap.PopularQuantization((Bitmap)sourceImagePB.Image, numberOfColors);
                OPTNPictureESTLabel.Text = "EST=" + QBitmap.BitmapEST((Bitmap)sourceImagePB.Image, (Bitmap)OPTNPicture.Image).ToString();
                Application.DoEvents();

                OPTYPicture.Image = QBitmap.Tonning((Bitmap)sourceImagePB.Image, (Bitmap)OPTNPicture.Image);
                OPTYPictureESTLabel.Text = "EST=" + QBitmap.BitmapEST((Bitmap)sourceImagePB.Image, (Bitmap)OPTYPicture.Image).ToString();
                Application.DoEvents();
            }
        }

        private void SaveOnPictureBox_Click(object sender, EventArgs e)
        {
            PictureBox thisPictureBox = (PictureBox)sender;

            if (thisPictureBox.Image != null)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Файли зображень (*.bmp,*.jpg,*.jpeg,*.gif,*.png)|*.bmp;*.jpg;*.jpeg;*.gif;*.png|Усі файли (*.*) | *.* "
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ((PictureBox)sender).Image.Save(sfd.FileName);
                }
            }
        }
    }
}
