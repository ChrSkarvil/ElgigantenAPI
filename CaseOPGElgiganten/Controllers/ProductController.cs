
using AutoMapper;
using CaseOPGElgiganten.Data;
using CaseOPGElgiganten.Data.Entities;
using CaseOPGElgiganten.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseOPGElgiganten.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IElgigantenRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ProductController(IElgigantenRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<ProductModel[]>> Get()
        {
            try
            {
                var results = await _repository.GetAllProductsAsync();

                return _mapper.Map<ProductModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpGet("{ean}/{gtin}")]
        public async Task<ActionResult<ProductModel>> Get(long? ean, long? gtin)
        {
            try
            {

                var result = await _repository.GetProductAsync(ean, gtin);

                if (result == null) return NotFound();

                return _mapper.Map<ProductModel>(result);

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        public async Task<ActionResult<ProductModel>> Post(ProductModel model)
        {
            try
            {
                if (model.EAN != null)
                {
                    var existing = await _repository.GetProductAsync(model.EAN, null);
                    if (existing != null)
                    {
                        return BadRequest("EAN in Use");
                    }
                }
                else if (model.GTIN != null)
                {
                    var existing = await _repository.GetProductAsync(model.GTIN, null);
                    if (existing != null)
                    {
                        return BadRequest("GTIN in Use");
                    }
                }

                if (model.EAN != null)
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Product", new { ean = model.EAN });

                    if (string.IsNullOrWhiteSpace(location))
                    {
                        return BadRequest("Could not use current EAN");
                    }

                    //Create a new ProductInfo
                    var product = _mapper.Map<Product>(model);
                    _repository.Add(product);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/Product/{product.EAN}", _mapper.Map<ProductModel>(product));
                    }

                }
                else if (model.GTIN != null)
                {
                    var location = _linkGenerator.GetPathByAction("Get", "Product", new { gtin = model.GTIN });

                    if (string.IsNullOrWhiteSpace(location))
                    {
                        return BadRequest("Could not use current GTIN");
                    }

                    //Create a new ProductInfo
                    var product = _mapper.Map<Product>(model);
                    _repository.Add(product);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/ProductInfo/{product.GTIN}", _mapper.Map<ProductModel>(product));
                    }
                }

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpPut("{ean}/{gtin}")]
        public async Task<ActionResult<ProductModel>> Put(long? ean, long? gtin, ProductModel model)
        {
            try
            {
                var oldProduct = await _repository.GetProductAsync(ean, gtin);
                if (oldProduct == null) return NotFound($"Could not find product{ean} {gtin}");

                _mapper.Map(model, oldProduct);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductModel>(oldProduct);
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpDelete("{ean}/{gtin}")]
        public async Task<IActionResult> Delete(long? ean, long? gtin)
        {
            try
            {
                var oldProduct = await _repository.GetProductAsync(ean, gtin);
                if (oldProduct == null) return NotFound();

                _repository.Delete(oldProduct);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok("Successfully deleted Product Info");
                }

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest("Failed To Delete The Product Info");
        }

    }
}
