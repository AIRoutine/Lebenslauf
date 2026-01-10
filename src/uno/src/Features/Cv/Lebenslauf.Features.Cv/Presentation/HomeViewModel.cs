using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using Lebenslauf.Features.Cv.Contracts.Models;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class HomeViewModel : PageViewModel
{
    public HomeViewModel(BaseServices baseServices) : base(baseServices)
    {
        // Load data immediately
        _ = LoadPersonalDataAsync();
    }

    [ObservableProperty]
    private string _firstName = "";

    [ObservableProperty]
    private string _lastName = "";

    [ObservableProperty]
    private string _title = "";

    [ObservableProperty]
    private string _email = "";

    [ObservableProperty]
    private string _phone = "";

    [ObservableProperty]
    private string _location = "";

    private async Task LoadPersonalDataAsync()
    {
        try
        {
            var (_, result) = await Mediator.Request(new GetCvHttpRequest());

            if (result?.PersonalData is not null)
            {
                var pd = result.PersonalData;

                // Split name into first and last name
                var nameParts = pd.Name.Split(' ', 2);
                FirstName = nameParts.Length > 0 ? nameParts[0].ToUpperInvariant() : "";
                LastName = nameParts.Length > 1 ? nameParts[1].ToUpperInvariant() : "";

                Title = pd.Title.ToUpperInvariant();
                Email = pd.Email;
                Phone = pd.Phone.Replace("-", " ");
                Location = $"{pd.City}, {pd.Country}";
                return;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load personal data: {ex.Message}");
        }

        // Fallback to default values
        FirstName = "DANIEL";
        LastName = "HUFNAGL";
        Title = "SENIOR CROSS-PLATFORM DEVELOPER";
        Email = "d.hufnagl@codelisk.com";
        Phone = "+43 664 73221804";
        Location = "Laakirchen, Oesterreich";
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
