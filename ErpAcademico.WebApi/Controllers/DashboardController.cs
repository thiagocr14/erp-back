using ErpAcademico.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ErpAcademico.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterDashboard()
    {
        var dashboard = await _dashboardService.ObterDashboard();
        return Ok(dashboard);
    }

    [HttpGet("detalhado")]
    public async Task<IActionResult> ObterDashboardDetalhado()
    {
        var dashboard = await _dashboardService.ObterDashboardDetalhado();
        return Ok(dashboard);
    }
}