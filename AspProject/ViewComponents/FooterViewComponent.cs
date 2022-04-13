using AspProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspProject.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly LayoutService _layoutService;
        public FooterViewComponent(LayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = _layoutService.GetSettings();

            return (await Task.FromResult(View(settings)));
        }
    }
}
