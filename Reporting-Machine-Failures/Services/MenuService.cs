using ReportingMachineFailures.Models;

namespace ReportingMachineFailures.Services
{
    public class MenuService
    {
        public List<Menu> Menus { get; set; }

        public MenuService()
        {
            Menus = new List<Menu>();
        }

        public int AddNewMenu(string name, string menuName)
        {
            var menu = new Menu()
            {
                Name = name,
                MenuName = menuName
            };

            Menus.Add(menu);

            return menu.Id;
        }

        public List<Menu> GetMenusByMenuName(string menuName)
        {
            var menuList = new List<Menu>();

            foreach (var menu in Menus)
            {
                if (menu.MenuName == menuName)
                {
                    menuList.Add(menu);
                }
            }

            return menuList;
        }
    }
}