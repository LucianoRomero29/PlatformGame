using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Analytics;

public class InitAnalytics : MonoBehaviour
{
    async void Start(){
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
