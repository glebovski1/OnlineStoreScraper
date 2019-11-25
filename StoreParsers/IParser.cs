using OnlineStoreScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStoreScraper.StoreParsers
{
    public interface IParser
    {
        Task<List<ProductModel>> ParseWebPage(string url);
    }
}
