using ECommerce.API.Infrastructure;
using ECommerce.Application.DTOs.Common;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class CategoryController(ICategoryService _categoryService) : BaseController
{
    [HttpGet("GetAll")]
    [Permission("DataDefinition.CategoryManagement.View")]
    public async Task<IActionResult> GetAll([FromQuery] GetCategoryDto getCategoryDto)
    {
        var result = await _categoryService.GetAllAsync(getCategoryDto);
        return Ok(result);
    }

    [HttpGet("GetById")]
    [Permission("DataDefinition.CategoryManagement.View")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("Add")]
    [Permission("DataDefinition.CategoryManagement.Add")]
    public async Task<IActionResult> Add([FromBody] AddCategoryDto addCategoryDto)
    {
        var result = await _categoryService.AddAsync(addCategoryDto);
        return Ok(result);
    }

    [HttpPost("Update")]
    [Permission("DataDefinition.CategoryManagement.Edit")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var result = await _categoryService.UpdateAsync(updateCategoryDto);
        return Ok(result);
    }

    [HttpPost("Delete")]
    [Permission("DataDefinition.CategoryManagement.Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
    {
        var result = await _categoryService.DeleteAsync(deleteDto);
        return Ok(result);
    }
}

