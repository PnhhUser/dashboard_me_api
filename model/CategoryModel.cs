using System.ComponentModel.DataAnnotations;

public class CategoryModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public static CategoryModel ToModel(CategoryEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CategoryModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}

public class CreateCategoryDTO
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 255 characters")]
    public string Name { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = null!;
}

public class EditCategoryDTO
{
    [Required(ErrorMessage = "Category ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be valid")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 255 characters")]
    public string Name { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = null!;
}