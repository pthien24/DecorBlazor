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
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _im;
        public ProductPriceRepository(ApplicationDBContext db, IMapper im)
        {
            _db = db;
            _im = im;
        }

        public async Task<ProductPriceDTO> Create(ProductPriceDTO objDTO)
        {
            var obj = _im.Map<ProductPriceDTO, ProductPrice>(objDTO);
            var addedobj = _db.ProductPrices.Add(obj);
            await _db.SaveChangesAsync();
            return _im.Map<ProductPrice, ProductPriceDTO>(addedobj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(o => o.ID == id);
            if (obj != null)
            {
                _db.ProductPrices.Remove(obj);
                await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductPriceDTO> Get(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(o => o.ID == id);
            if (obj != null)
            {
                return _im.Map<ProductPrice, ProductPriceDTO>(obj);
            }
            return new ProductPriceDTO();
        }

        public async Task<IEnumerable<ProductPriceDTO>> GetAll(int? Id = null)
        {
            if (Id != null && Id > 0 )
            {
                return _im.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>
                    (_db.ProductPrices.Where(p =>p.ProductID == Id));

            }
            return _im.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices);
        }

        public async Task<ProductPriceDTO> Update(ProductPriceDTO objDTO)
        {
             var objFormDB = await _db.ProductPrices.FirstOrDefaultAsync(c => c.ID == objDTO.ID);
            if (objFormDB != null)
            {
                objFormDB.Size = objDTO.Size;
                objFormDB.Price = objDTO.Price;
                objFormDB.ProductID = objDTO.ProductID;
                await _db.SaveChangesAsync();
                return _im.Map<ProductPrice, ProductPriceDTO>(objFormDB);
            }
            return objDTO;
        }
    }
}
