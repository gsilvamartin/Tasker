using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Tasker.Dashboard.Model;
using Tasker.Dashboard.Model.Job;
using Tasker.Logging;
using Tasker.Logging.Interfaces;

namespace Tasker.Dashboard.Pages;

public partial class Jobs
{
    private Form<CreateJobRequest> _createJobForm;

    private bool _isJobNameUnique = true;
    private bool _createJobModalVisible = false;
    private bool _createJobModalLoadingBtn = false;

    private List<JobData> _jobData = new();
    private readonly HttpClient _httpClient;
    private readonly ITaskerLogger<Jobs> _logger;
    private readonly CreateJobRequest _createJobRequest = new();

    public Jobs()
    {
        _httpClient = new HttpClient();
        _logger = new TaskerLogger<Jobs>();
    }

    protected override async Task OnInitializedAsync()
    {
        _jobData = await LoadData();
    }

    private bool IsCreateJobFormValid()
    {
        return _isJobNameUnique && _createJobForm.Validate();
    }

    private async Task CreateJob()
    {
        if (!IsCreateJobFormValid())
            return;

        StateHasChanged();
        _createJobModalLoadingBtn = true;

        try
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5040/api/jobs", _createJobForm.Model);
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<JobData>>(result);

            if (apiResponse.Success)
            {
                _createJobModalVisible = false;
                await LoadData();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error creating job", e);
        }
        finally
        {
            _createJobModalLoadingBtn = false;
        }
    }

    private async Task<List<JobData>> LoadData()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:5040/api/jobs");
            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<JobData>>>(result);

            return apiResponse.Data;
        }
        catch (Exception e)
        {
            _logger.LogError("Error loading jobs", e);
            return new List<JobData>();
        }
    }

    private async Task CheckJobNameUnique(FocusEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(_createJobForm.Model.Name))
            {
                _isJobNameUnique = true;
                _createJobForm.Validate();
                StateHasChanged();
                return;
            }

            var response =
                await HttpClient.GetAsync(
                    $"http://localhost:5040/api/Jobs/IsJobNameUnique/{_createJobForm.Model.Name}");

            var result = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<bool>>(result);

            _isJobNameUnique = apiResponse.Data;

            _createJobForm.Validate();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to check job name uniqueness. Error: {ex.Message}");
        }
    }

    private void OnFinishCreateJob(EditContext editContext) => _createJobModalVisible = false;
    private void OnFinishCreateJobFailed(EditContext editContext) => _createJobModalVisible = false;
    private void ShowCreateJobModal() => _createJobModalVisible = true;
    private void HandleCancelCreateJob(MouseEventArgs e) => _createJobModalVisible = false;
    private async Task HandleOkCreateJob(MouseEventArgs e) => await CreateJob();
}