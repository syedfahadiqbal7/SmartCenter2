namespace temporary.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public string MenuUrl { get; set; } = null!;

    public string? Description { get; set; }

    public string? MenuIcon { get; set; }

    public virtual ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<SubMenu> SubMenus { get; set; } = new List<SubMenu>();
}
