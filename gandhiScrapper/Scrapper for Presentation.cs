using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace gandhiScrapper
{
    public partial class Scrapper
    {
        public bool isLoggedIn { get; private set; }
        private string currentPageHTML;
        private HttpWebRequest req;
        private CookieContainer myContainer;
        private HttpWebResponse res;
        private List<Book> bookList;
        HtmlDocument doc;
        

        public Scrapper(string searchWords, List<Book> books)
        {
            currentPageHTML = string.Empty;
            myContainer = new CookieContainer();
            bookList = books;
            string url = @"http://busqueda.gandhi.com.mx/busca?q=" + WebUtility.UrlEncode(searchWords);
            DoInitialGetRequest(url);
            AssignListValues(url);
        }
                
        private void DoInitialGetRequest(string stringURL)
        {
            req = (HttpWebRequest)WebRequest.Create(stringURL);
            var sp = req.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour",
                                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);

            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.Headers.Add(HttpRequestHeader.AcceptLanguage, "es-419,es;q=0.8");
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            
            req.Headers.Add("Upgrade-Insecure-Requests", "1");
            req.Host = "busqueda.gandhi.com.mx";
            req.KeepAlive = true;
            req.CookieContainer = new CookieContainer();
            req.CookieContainer = myContainer;
            res = (HttpWebResponse)req.GetResponse();
            currentPageHTML = new StreamReader(res.GetResponseStream()).ReadToEnd();
            doc = new HtmlDocument();
            doc.LoadHtml(currentPageHTML);
        }

        private void AssignListValues(string referer) {
            var list = doc.GetElementbyId("neemu-products-container");
            List<HtmlNode> auxList = new List<HtmlNode>();
            for (int i = 0; i < list.ChildNodes.Count; i++) {
                if (list.ChildNodes[i].Name == "li") {
                    auxList.Add(list.ChildNodes[i]);
                }
            }
            for (int i = 0; i < 16 && i < auxList.Count; i++)
            {
                for (int j = 0; j < auxList[i].ChildNodes[1].ChildNodes.Count; j++)
                {
                    if (auxList[i].ChildNodes[1].ChildNodes[j].Name == "img")
                    {
                        bookList[i].coverUrl = auxList[i].ChildNodes[1].ChildNodes[j].Attributes[2].Value.TrimStart('/');
                        bookList[i].coverUrl = "http://" + bookList[i].coverUrl;
                        //DoInitialGetImageRequest(referer, bookList[i]);
                        break;
                    }
                }
                bookList[i].TitleLabel.Text = auxList[i].ChildNodes[3].ChildNodes[1].Attributes[0].Value.ToUpper();
                bookList[i].TitleLabel.Links.Clear();
                bookList[i].TitleLabel.Links.Add(0, bookList[i].TitleLabel.Text.Length, auxList[i].ChildNodes[3].ChildNodes[1].Attributes[1].Value.TrimStart('/'));
                bookList[i].AuthorLabel.Text = "AUTOR: " + auxList[i].ChildNodes[5].ChildNodes[1].InnerText.Trim().ToUpper();
                if (auxList[i].ChildNodes[11].Name == "div")
                {
                    if (auxList[i].ChildNodes[11].ChildNodes[3].ChildNodes.Count > 3)
                    {
                        bookList[i].PriceLabel.Text = "PRECIO: " + auxList[i].ChildNodes[11].ChildNodes[3].ChildNodes[3].InnerText;
                    }else { bookList[i].PriceLabel.Text = "PRECIO: $ "; }
                }
                else if (auxList[i].ChildNodes[13].Name == "div")
                {
                    bookList[i].PriceLabel.Text = "PRECIO: " + auxList[i].ChildNodes[13].ChildNodes[3].ChildNodes[3].InnerText;
                }
                else {
                    bookList[i].PriceLabel.Text = "PRECIO: " + auxList[i].ChildNodes[15].ChildNodes[3].ChildNodes[3].InnerText;
                }
            }
        }

        private void DoInitialGetImageRequest(string referer, Book mybook)
        {
            //Create request Headers
            









            Guid guid = Guid.NewGuid();
            mybook.coverPath = string.Format(@"{0}\{1}.jpeg", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), guid.ToString());
            //Write stream into file
            



            
            
            
            
            
            
            mybook.AssignImageToPictureBox();
        }
    }
}
