using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseOPGElgiganten.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CaseOPGElgiganten.Data
{
    public class ElgigantenRepository : IElgigantenRepository
    {
        private readonly ElgigantenContext _context;
        private readonly ILogger<ElgigantenRepository> _logger;

        public ElgigantenRepository(ElgigantenContext context, ILogger<ElgigantenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            _logger.LogInformation($"Getting all Product informations");

            //IQueryable<ProductInfo> query = _context.ProductInfo;

            var query = _context.Product;

            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductAsync(long? ean, long? gtin)
        {
            if (ean != 0)
            {
                _logger.LogInformation($"Getting Product info for {ean}");

                IQueryable<Product> query = _context.Product;
                // Query It
                query = query.Where(c => c.EAN == ean);
            return await query.FirstOrDefaultAsync();
            }
            else
            {
                _logger.LogInformation($"Getting Product info for {gtin}");

                IQueryable<Product> query = _context.Product;
                // Query It
                query = query.Where(c => c.GTIN == gtin);
                return await query.FirstOrDefaultAsync();
            }

           }
        }
    }


