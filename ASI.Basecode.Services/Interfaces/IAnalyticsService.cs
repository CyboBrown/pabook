using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAnalyticsService
    {
        AnalyticsViewModel GetAnalyticsDashboard();
        int GetTotalBookings();
        int GetCancelledBookings();
        int GetTotalUsers();
        int GetDeletedUsers();
        string GetMostBookedRoom();
        string GetPeakTime();
        Dictionary<string, MonthlyDataViewModel> GetMonthlyData();
        Dictionary<string, MonthlyDataViewModel> GetMonthlyData(int year, int month); 
        Dictionary<string, MonthlyDataViewModel> GetYearlyData(int year); 
    }
}


   
  
  