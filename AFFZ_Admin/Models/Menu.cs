namespace AFFZ_Admin.Models;

public partial class Menu
{
    public int? menuId { get; set; }

    public string? menuName { get; set; } = null!;

    public string? menuUrl { get; set; } = null!;

    public string? description { get; set; }

    public string? menuIcon { get; set; }

    public virtual List<Permission>? permissions { get; set; } = new List<Permission>();

    public virtual List<SubMenu>? subMenus { get; set; } = new List<SubMenu> { };
}
