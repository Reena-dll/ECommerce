namespace ECommerce.Application.Core.Results;

public sealed record Error(string Code, string Description)
{
    public static readonly Error UserNotExist = new("UserNotExist", "The specified user does not exist.");
    public static readonly Error UserExist = new("UserExist", "The specified user does exist.");
    public static readonly Error RoleNotExist = new("RoleNotExist", "The specified role does not exist.");
    public static readonly Error PermissionNotExist = new("PermissionNotExist", "The specified permission does not exist.");
    public static readonly Error ProductNotExist = new("ProductNotExist", "The specified product does not exist.");
    public static readonly Error UserNotFound = new("UserNotFound", "The specified user does not exist Email or Old password is not match");
    public static readonly Error InvalidCode = new("InvalidCode", "The provided code is invalid.");
    public static readonly Error InvalidToken = new("InvalidToken", "The provided token is invalid.");
    public static readonly Error InvalidParams = new("InvalidParam", "The provided params are invalid.");
    public static readonly Error HasSamePermission = new("HasSamePermission", "Same Permission available.");
    public static readonly Error HasSameProduct = new("HasSameProduct", "Same Product available.");
    public static readonly Error HasSameRole = new("HasSameRole", "Same role available.");
    public static readonly Error EmailAlreadyInUse = new("EmailAlreadyInUse", "This email address is being used by another user.");

}

