/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLoc xmlns:vm="clr-namespace:Hfr"
                           x:Key="Loc" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Loc}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Hfr.Services.Classes;
using Microsoft.Practices.ServiceLocation;

namespace Hfr.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class Loc
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLoc class.
        /// </summary>
        public Loc()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<NavigationService>();
        }

        public static MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static NavigationService NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<NavigationService>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}