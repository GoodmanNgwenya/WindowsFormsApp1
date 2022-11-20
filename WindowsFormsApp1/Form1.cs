using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _ = GetDataFromWebPage();
        }

        public async Task GetDataFromWebPage()
        {
            string fullURl = "https://api.slack.com/methods";
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullURl);

            var linkList = ParseHtml(response);

            WriteToCsv(linkList);

        }
        private List<string> ParseHtml(string html)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);

            var programmerLinks = htmlDoc.DocumentNode.Descendants("li")
              //.Where(node => !node.GetAttributeValue("class", "").Contains("compactFeedListItem"))
                .ToList();


            List<string> webLink = new List<string>();

            foreach (var link in programmerLinks)
            {
                if (link.FirstChild.Attributes.Count > 0)
                    //webLink.Add("https://api.slack.com/methods/" + link.FirstChild.Attributes[0].Value);
                    webLink.Add(link.FirstChild.ChildNodes[0].InnerText);

            }

            return webLink;

        }

        private void WriteToCsv(List<string> links)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var link in links)
            {
                sb.AppendLine(link);
            }

           File.WriteAllText("D:\\links.csv", sb.ToString());
        }

    }
}
