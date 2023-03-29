using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Common;

public static class Calulate
{
    public static string ExpiredDateTime(string KeepAliveData, DateTimeOffset purposeCreateDate)
    {
        try
        {
            string strExpired;
            string keepAliveDataStr = KeepAliveData.ToUpper();
           
            char keepAliveUnit = keepAliveDataStr[keepAliveDataStr.Length - 1];
            string keepAliveDataSplite = keepAliveDataStr.Substring(0, keepAliveDataStr.Length - 1);

            int keepAliveDataToInt = int.Parse(keepAliveDataSplite);

            switch (keepAliveUnit)
            {
                case 'D':
                    DateTimeOffset expireDays = purposeCreateDate.AddDays(keepAliveDataToInt);
                    strExpired = expireDays.ToLocalTime().ToString();
                    break;
                case 'M':
                    DateTimeOffset expireMonths = purposeCreateDate.AddMonths(keepAliveDataToInt);
                    strExpired = expireMonths.ToLocalTime().ToString();
                    break;
                case 'Y':
                    DateTimeOffset expireYears = purposeCreateDate.AddYears(keepAliveDataToInt);
                    strExpired = expireYears.ToLocalTime().ToString();
                    break;
                default:
                    throw new Exception("KeepAliveData not available");
            }

            return strExpired;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}