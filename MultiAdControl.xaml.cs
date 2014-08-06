using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Marketplace;
using System.Diagnostics;

namespace WPAdKit
{
  public partial class MultiAdControl
  {
    public MultiAdControl()
    {
      TrialOnly = true;

      InitializeComponent();

      Loaded += HandleLoaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs args)
    {
      var msad = new Microsoft.Advertising.Mobile.UI.AdControl()
      {
        Width = 480,
        Height = 80
      };
      grid.Children.Add(msad);
      msad.ApplicationId = ApplicationId;
      msad.AdUnitId = AdUnitId;
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
      Loaded -= HandleLoaded;
    }

    public bool TrialOnly { get; set; }


    public string AdDuplexId { get; set; }

    public string AdUnitId { get; set; }

    public string ApplicationId { get; set; }
  }
}