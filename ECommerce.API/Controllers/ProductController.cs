using ECommerce.API.Infrastructure;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductController(IProductService _productService) : BaseController
{
    [HttpGet("GetAll")]
    [Permission("DataDefinition.ProductManagement.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetProductDto getProductDto)
    {
        var result = await _productService.GetAllAsync(getProductDto);
        return Ok(result);
    }

    [HttpGet("GetById")]
    [Permission("DataDefinition.ProductManagement.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("Add")]
    [Permission("DataDefinition.ProductManagement.Add")]
    public async Task<IActionResult> Add([FromBody] AddProductDto ProductDto)
    {
        var result = await _productService.AddAsync(ProductDto);
        return Ok(result);
    }

    [HttpPost("Update")]
    [Permission("DataDefinition.ProductManagement.Edit")]
    public async Task<IActionResult> Update([FromBody] UpdateProductDto ProductDto)
    {
        var result = await _productService.UpdateAsync(ProductDto);
        return Ok(result);
    }

    [HttpPost("Delete")]
    [Permission("DataDefinition.ProductManagement.Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
    {
        var result = await _productService.DeleteAsync(deleteDto);
        return Ok(result);
    }
}

