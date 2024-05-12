using AutoMapper;
using Decor_Business.Repository.IRepository;
using Decor_DataAccess;
using Decor_DataAccess.Data;
using Decor_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Business.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _im;
        public ProductRepository(ApplicationDBContext db,IMapper im) 
        {
            _db = db;
            _im = im;
        }
        public async Task<ProductDTO> Create(ProductDTO objDTO)
        {
            var obj = _im.Map<ProductDTO, Product>(objDTO);
            var addedobj = _db.Products.Add(obj);
            await _db.SaveChangesAsync();
            return _im.Map<Product, ProductDTO>(addedobj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(o => o.ID == id);
            if (obj != null)
            {
                _db.Products.Remove(obj);
                await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var obj = await _db.Products.Include(u =>u.Category).Include(u => u.ProductPrices).FirstOrDefaultAsync(o => o.ID == id);
            if (obj != null)
            {
                return _im.Map<Product, ProductDTO>(obj);
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            return _im.Map<IEnumerable<Product>,IEnumerable<ProductDTO>>(_db.Products.Include(u=>u.Category).Include(u => u.ProductPrices));
        }

        public async Task<ProductDTO> Update(ProductDTO objDTO)
        {
            var objFormDB = await _db.Products.FirstOrDefaultAsync(c => c.ID == objDTO.ID);
            if (objFormDB != null)
            {
                objFormDB.Name = objDTO.Name;
                objFormDB.Description = objDTO.Description;
                objFormDB.ShopFavorites = objDTO.ShopFavorites;
                objFormDB.CustommerFavorites= objDTO.CustommerFavorites;
                objFormDB.Color = objDTO.Color;
                objFormDB.ImageUrl = objDTO.ImageUrl;
                objFormDB.CategoryId = objDTO.CategoryId;

                await _db.SaveChangesAsync();
                return _im.Map<Product, ProductDTO>(objFormDB);
            }
            return objDTO;
        }
    }
}
