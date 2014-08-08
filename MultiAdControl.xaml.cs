using System.Windows;
using Microsoft.Phone.Marketplace;
using System.Diagnostics;

namespace WPAdKit
{
  public partial class MultiAdControl
  {
    public MultiAdControl()
    {
      InitializeComponent();

      Loaded += HandleLoaded;
    }

    public static bool CheckRemoveAdPurchase(string key)
    {
      var ret = false;
      if (!string.IsNullOrEmpty(key))
      {
        var allLicenses = Windows.ApplicationModel.Store.CurrentApp.LicenseInformation.ProductLicenses;
        if (allLicenses.ContainsKey(key))
        {
          var license = allLicenses[key];
          if (license.IsActive)
          {
            ret = true;
          }
        }
      }
      return ret;
    }


    private void HandleLoaded(object sender, RoutedEventArgs args)
    {
      if (CheckRemoveAdPurchase(RemoveAdKey))
      {
        grid.Visibility = Visibility.Collapsed;
      }
      else
      {
        var msad = new Microsoft.Advertising.Mobile.UI.AdControl
        {
          Width = 480,
          Height = 80,
          ApplicationId = ApplicationId,
          AdUnitId = AdUnitId
        };
        // TODO
        //msad.ApplicationId = "Test_client";
        //msad.AdUnitId = "Image480_80";
        grid.Children.Add(msad);
        var adduplex = new AdDuplex.AdControl()
        {
          Visibility = Visibility.Collapsed
        };
        grid.Children.Add(adduplex);
        adduplex.AppId = AdDuplexId;
        msad.ErrorOccurred += (_, e) =>
        {
          Debug.WriteLine(e.Error.Message);
          msad.Visibility = Visibility.Collapsed;
          adduplex.Visibility = Visibility.Visible;
        };
        if (TrialOnly)
        {
          if (!new LicenseInformation().IsTrial())
          {
            grid.Visibility = Visibility.Collapsed;
          }
        }
      }
      Loaded -= HandleLoaded;
    }

    public bool TrialOnly { get; set; }

    public string AdDuplexId { get; set; }

    public string AdUnitId { get; set; }

    public string ApplicationId { get; set; }

    public string RemoveAdKey { get; set; }
  }
}