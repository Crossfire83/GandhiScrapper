using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace gandhiScrapper
{
    public partial class Form1 : Form
    {
        List<Book> BookList;
        Scrapper sc;
        List<string> files;

        public Form1()
        {
            InitializeComponent();
            files = new List<string>();
            webBrowser.ScriptErrorsSuppressed = true;
        }

        private void SearchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BookList = new List<Book>();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        PictureBox CoverImage = (PictureBox)tableLayoutPanel1.Controls.Find("pictureBox" + i.ToString() + j.ToString(), false)[0];
                        LinkLabel TitleLabel = (LinkLabel)tableLayoutPanel1.Controls.Find("BookTitleLabel" + i.ToString() + j.ToString(), false)[0];
                        Label PriceLabel = (Label)tableLayoutPanel1.Controls.Find("PriceLabel" + i.ToString() + j.ToString(), false)[0];
                        Label AuthorLabel = (Label)tableLayoutPanel1.Controls.Find("AuthorLabel" + i.ToString() + j.ToString(), false)[0];
                        Book myBook = new Book(CoverImage, TitleLabel, PriceLabel, AuthorLabel, this);
                        BookList.Add(myBook);
                    }
                }
                sc = new Scrapper(SearchTextbox.Text, BookList);
                for (int i = 0; i < 16; i++)
                {
                    if (BookList[i].coverPath != "")
                    {
                        files.Add(BookList[i].coverPath);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BookList != null)
            {
                foreach (Book b in BookList)
                {
                    b.Dispose();
                }
                foreach (string s in files)
                {
                    File.Delete(s);
                }
            }
        }
    }
}
