public static class ErrorMessage
{
    // --- ACCOUNT ---
    public const string UsernameIsRequired = "Username is required";
    public const string PasswordIsRequired = "Password is required";
    public const string AccountNotFound = "Account not found";
    public const string UsernameAlreadyExists = "Username already exists";
    public const string UsernameMinLength = "Username must be at least 3 characters";
    public const string PasswordMinLength = "Password must be at least 6 characters";
    public const string RoleInvalid = "Your role is invalid.";

    // --- CATEGORY ---
    public const string CategoryNotFound = "Category not found";
    public const string CategoryAlreadyExists = "Category already exists";
    public const string CategoryIsRequired = "Category is required";

    // --- PRODUCT ---
    public const string ProductNotFound = "Product not found";
    public const string ProductAlreadyExists = "Product already exists";
    public const string ProductIsRequired = "Product name is required";
    public const string ProductCodeAlreadyExists = "Product code already exists";
    public const string PriceInvalid = "Price must be greater than 0";

    // --- STOCK ---
    public const string StockNotFound = "Stock not found";
    public const string QuantityInvalid = "Quantity must be greater than or equal to 0";
    public const string CostInvalid = "Cost must be greater than 0";

    // --- AUTHENTICATION ---
    public const string InvalidCredentials = "Invalid username or password";
}