using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStoreScraper.Models
{
    public class ProductRepository
    {
        public ProductContext _dataBase;

        public ProductRepository(ProductContext productContext)
        {
            _dataBase = productContext;
        }

        public void Create(ProductModel product)
        {
            _dataBase.Products.Add(product);
            _dataBase.SaveChanges();
        }
        public ProductModel Read(int id)
        {
           return _dataBase.Products.Find(id);
        }

        public void Delete(int id)
        {
            _dataBase.Products.Remove(Read(id));
            _dataBase.SaveChanges();
        }

        public void Update(ProductModel product)
        {
            _dataBase.Update(product);
            _dataBase.SaveChanges();
        }

        public void Create(List<ProductModel> products)
        {
            foreach(ProductModel product in products)
            {
                Create(product);
            }
        }

        public List<ProductModel> ReadAll()
        {
            List<ProductModel> products;
            products = _dataBase.Products.ToList();
            return products;
        }
    }
}
