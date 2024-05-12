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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _im;
        public CategoryRepository(ApplicationDBContext db,IMapper im)
        {
            _db = db;
            _im = im;
        }
        public async Task<CategoryDTO> Create(CategoryDTO objDTO)
        {

            var obj =  _im.Map<CategoryDTO,Category>(objDTO);
            obj.CreatedDate = DateTime.Now;
            var addedobj = _db.Categories.Add(obj);
            await _db.SaveChangesAsync();
            return _im.Map<Category, CategoryDTO>(addedobj.Entity);
        }
        public async Task<int> Delete(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(o => o.ID == id);
            if (obj != null)
            {
                _db.Categories.Remove(obj);
                await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CategoryDTO> Get(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(o => o.ID == id);
            if (obj != null)
            {
                return _im.Map<Category, CategoryDTO>(obj);
            }
            return new CategoryDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            return  _im.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_db.Categories);
        }

        public async Task<CategoryDTO> Update(CategoryDTO objDTO)
        {
            var objFormDB = await _db.Categories.FirstOrDefaultAsync(c => c.ID == objDTO.ID);
            if (objFormDB != null)
            {
                objFormDB.Name = objDTO.Name;
                await _db.SaveChangesAsync();
                return _im.Map<Category, CategoryDTO>(objFormDB);
            }
            return objDTO;
        }
    }
}
