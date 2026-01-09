using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class HomeViewModel : PageViewModel
{
    public HomeViewModel(BaseServices baseServices) : base(baseServices)
    {
    }

    [UnoCommand]
    private async Task NavigateToCvAsync()
    {
        await Navigator.NavigateViewModelAsync<CvViewModel>(this);
    }

    [UnoCommand]
    private async Task NavigateToSkillsAsync()
    {
        await Navigator.NavigateViewModelAsync<SkillsViewModel>(this);
    }

    [UnoCommand]
    private async Task NavigateToProjectsAsync()
    {
        await Navigator.NavigateViewModelAsync<ProjectsViewModel>(this);
    }
}
