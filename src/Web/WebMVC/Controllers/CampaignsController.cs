namespace Microsoft.eShopOnContainers.WebMVC.Controllers
{
    using AspNetCore.Authorization;
    using AspNetCore.Mvc;
    using Services;
    using ViewModels;
    using System.Threading.Tasks;
    using System;
    using ViewModels.Pagination;
    using global::WebMVC.ViewModels;

    [Authorize]
    public class CampaignsController : Controller
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService) =>
            _campaignService = campaignService;

        public async Task<IActionResult> Index(int page = 0, int pageSize = 10)
        {
            var campaignList = await _campaignService.GetCampaigns(pageSize, page);

            var vm = new CampaignViewModel()
            {
                CampaignItems = campaignList.Data,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = campaignList.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)campaignList.Count / pageSize))
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var campaignDto = await _campaignService.GetCampaignById(id);

            if (campaignDto is null)
            {
                return NotFound();
            }

            var campaign = new CampaignItem
            {
                Id = campaignDto.Id,
                Name = campaignDto.Name,
                Description = campaignDto.Description,
                From = campaignDto.From,
                To = campaignDto.To,
                PictureUri = campaignDto.PictureUri
            };

            return View(campaign);
        }
    }
}