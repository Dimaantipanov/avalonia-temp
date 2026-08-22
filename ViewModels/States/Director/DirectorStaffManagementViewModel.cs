using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Services.Models.Responses;
using FenceFactory.Desktop.Services.Network;
using FenceFactory.Desktop.ViewModels.States.Director.Parts;

namespace FenceFactory.Desktop.ViewModels.States.Director;

public partial class DirectorStaffManagementViewModel : StateViewModelBase
{
    private readonly StaffApiService _staffService;

    public override ApplicationState State => ApplicationState.DirectorStaffManagement;

    [ObservableProperty] private ObservableCollection<EmployeeCardViewModel> _employees = new();
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _hasError;
    [ObservableProperty] private EmployeeCardViewModel? _selectedEmployee;

    // Конструктор принимает напрямую готовый сервис
    // Конструктор принимает исходные зависимости и сам собирает сервис на месте
      // Чистый каноничный конструктор
      public DirectorStaffManagementViewModel(StaffApiService staffService)
      {
          _staffService = staffService ?? throw new ArgumentNullException(nameof(staffService));
      }



    public override async void Activate()
    {
        base.Activate();
        await LoadEmployeesAsync();
    }

    [RelayCommand]
    private async Task LoadEmployeesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var result = await _staffService.GetStaffAsync();

            foreach (var emp in Employees)
            {
                emp.DeleteRequested -= OnEmployeeDeleteRequested;
            }
            Employees.Clear();

            foreach (var empModel in result)
            {
                var cardVm = new EmployeeCardViewModel(empModel);
                cardVm.DeleteRequested += OnEmployeeDeleteRequested;
                Employees.Add(cardVm);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Не удалось загрузить штат: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnEmployeeDeleteRequested(object? sender, EmployeeCardViewModel cardVm)
    {
        await FireEmployeeAsync(cardVm);
    }

    private async Task FireEmployeeAsync(EmployeeCardViewModel? cardVm)
    {
        if (cardVm == null || IsBusy) return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            await _staffService.DismissStaffAsync(cardVm.Email);
            
            cardVm.DeleteRequested -= OnEmployeeDeleteRequested;
            Employees.Remove(cardVm);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Ошибка удаления: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public override void Deactivate()
    {
        foreach (var emp in Employees)
        {
            emp.DeleteRequested -= OnEmployeeDeleteRequested;
        }
        Employees.Clear();
        base.Deactivate();
    }
}
