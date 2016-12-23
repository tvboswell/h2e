﻿namespace BlogSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.UnitOfWork;
    using Infrastructure.Mapping;
    using ViewModels.Nav;

    public class NavController : BaseController
    {
        private readonly IBlogSystemData data;

        public NavController(IBlogSystemData data)
        {
            this.data = data;
        }

        [ChildActionOnly]
        public PartialViewResult Menu()
        {
            var menu = this.data
                .Pages
                .All()
                .To<MenuItemViewModel>()
                .ToList();

            return this.PartialView(menu);
        }
    }
}