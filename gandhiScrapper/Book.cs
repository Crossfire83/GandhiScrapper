using System.Windows.Forms;
using System.Drawing;
using System;

namespace gandhiScrapper
{
    public class Book : IDisposable
    {
        public string title { get; set; }
        public string author { get; set; }
        public float price { get; set; }
        public Image cover { get; set; }
        public string coverUrl { get; set; }
        public string coverPath { get; set; }
        public PictureBox CoverImage { get; }
        public LinkLabel TitleLabel { get; }
        public Label PriceLabel { get; }
        public Label AuthorLabel { get; }
        public string BookUrl { get; set; }
        private Form1 form;
        private bool isDisposed;


        public Book(PictureBox p, LinkLabel l, Label priceLbl, Label a, Form1 f)
        {
            CoverImage = p;
            TitleLabel = l;
            PriceLabel = priceLbl;
            AuthorLabel = a;
            TitleLabel.Text = title = "";
            AuthorLabel.Text = author = "";
            coverUrl = "";
            BookUrl = "";
            coverPath = "";
            cover = null;
            price = 0;
            PriceLabel.Text = "";
            CoverImage.Image = null;
            TitleLabel.LinkClicked += TitleLabel_LinkClicked;
            TitleLabel.TextChanged += TitleLabel_TextChanged;
            AuthorLabel.TextChanged += AuthorLabel_TextChanged;
            PriceLabel.TextChanged += PriceLabel_TextChanged;
            form = f;
            isDisposed = false;
        }

        private void PriceLabel_TextChanged(object sender, EventArgs e)
        {
            if (PriceLabel.Text.Replace("PRECIO: $ ", "") != "")
            {
                price = Single.Parse(PriceLabel.Text.Replace("PRECIO: $ ", ""));
            }
            else {
                price = 0;
            }
        }

        private void AuthorLabel_TextChanged(object sender, EventArgs e)
        {
            author = AuthorLabel.Text.Replace("AUTOR: ", "");
        }

        private void TitleLabel_TextChanged(object sender, EventArgs e)
        {
            TitleLabel.LinkVisited = false;
            title = TitleLabel.Text;
        }

        private void TitleLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            form.webBrowser.Navigate(e.Link.LinkData.ToString());
            TitleLabel.LinkVisited = true;
            form.tabControl1.SelectedTab = form.DetailTab;
        }

        public void AssignImageToPictureBox() {
            cover = Image.FromFile(coverPath);
            CoverImage.Image = cover;
        }

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            if (!isDisposed) {
                if (disposing) {
                    if (cover != null)
                    {
                        cover.Dispose();
                        CoverImage.Image.Dispose();
                    }
                }
            }
        }

    }
}
