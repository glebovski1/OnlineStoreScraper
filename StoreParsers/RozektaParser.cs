using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OnlineStoreScraper.Models;

namespace OnlineStoreScraper.StoreParsers
{
    public class RozektaParser : IParser
    {
        public async Task<List<ProductModel>> ParseWebPage(string url)
        {
            List<ProductModel> productList = new List<ProductModel>();
            var httpClient = new HttpClient();
            var html =  await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var ProductsHtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("name", "")
                .Equals("goods_list")).ToList();
            List<HtmlNode> ProductsList = ProductsHtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("g-i-tile g-i-tile-catalog")).ToList();


            foreach (var product in ProductsList)
            {
                string id = product.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("g-id")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');


                

                string imageUrl = product.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "null")
                    .Equals("clearfix pos-fix")).FirstOrDefault().Descendants("img").FirstOrDefault().GetAttributeValue("src", "");

                string productUrl = product.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "null")
                    .Equals("clearfix pos-fix")).FirstOrDefault().Descendants("a").FirstOrDefault().GetAttributeValue("href", "");

                string name = product.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "null")
                    .Equals("g-i-tile-i-title clearfix")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');

                var localHtml = await httpClient.GetStringAsync(productUrl);

                var localHtmlDocument = new HtmlDocument();
                localHtmlDocument.LoadHtml(localHtml);

                decimal price;
                try
                {
                    string _price = localHtmlDocument.DocumentNode.Descendants("span").
                        Where(node => node.GetAttributeValue("class", "")
                        .Equals("detail-price-uah")).FirstOrDefault().InnerText;

                    int value;
                    int.TryParse(string.Join("", _price.Where(c => char.IsDigit(c))), out value);
                     price = Convert.ToDecimal(value);
                }
                catch
                {
                     price = 0;
                   
                }

                string description;

                try
                {
                    description = localHtmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "null")
                    .Equals("b-rich-text goods-description-content")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t');
                }
                catch
                {
                    description = "";
                }

              
                

                productList.Add(new ProductModel {
                    ImageUrl = imageUrl,
                    ProductUrl = productUrl,
                    Price = price,
                    Name = name,
                    Description = description
                });
            }
            return productList;
        }
    }
}
