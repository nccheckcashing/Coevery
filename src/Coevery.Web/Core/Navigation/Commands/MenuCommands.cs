﻿using Coevery.Commands;
using Coevery.ContentManagement;
using Coevery.Core.Navigation.Models;
using Coevery.Core.Navigation.Services;

namespace Coevery.Core.Navigation.Commands {
    public class MenuCommands : DefaultCoeveryCommandHandler {
        private readonly IContentManager _contentManager;
        private readonly IMenuService _menuService;

        public MenuCommands(IContentManager contentManager, IMenuService menuService) {
            _contentManager = contentManager;
            _menuService = menuService;
        }

        [CoeverySwitch]
        public string MenuPosition { get; set; }

        [CoeverySwitch]
        public string MenuText { get; set; }

        [CoeverySwitch]
        public string Url { get; set; }

        [CoeverySwitch]
        public string MenuName { get; set; }

        [CommandName("menuitem create")]
        [CommandHelp("menuitem create /MenuPosition:<position> /MenuText:<text> /Url:<url> /MenuName:<name>\r\n\t" + "Creates a new menu item")]
        [CoeverySwitches("MenuPosition,MenuText,Url,MenuName")]
        public void Create() {
            // flushes before doing a query in case a previous command created the menu

            var menu = _menuService.GetMenu(MenuName);

            if(menu == null) {
                Context.Output.WriteLine(T("Menu not found.").Text);
                return;
            }

            var menuItem = _contentManager.Create("MenuItem");
            menuItem.As<MenuPart>().MenuPosition = MenuPosition;
            menuItem.As<MenuPart>().MenuText = T(MenuText).ToString();
            menuItem.As<MenuPart>().Menu = menu.ContentItem;
            menuItem.As<MenuItemPart>().Url = Url;

            Context.Output.WriteLine(T("Menu item created successfully.").Text);
        }

        [CommandName("menu create")]
        [CommandHelp("menu create /MenuName:<name>\r\n\t" + "Creates a new menu")]
        [CoeverySwitches("MenuName")]
        public void CreateMenu() {
            if (string.IsNullOrWhiteSpace(MenuName)) {
                Context.Output.WriteLine(T("Menu name can't be empty.").Text);
                return;
            }

            _menuService.Create(MenuName);

            Context.Output.WriteLine(T("Menu created successfully.").Text);
        }
    }
}