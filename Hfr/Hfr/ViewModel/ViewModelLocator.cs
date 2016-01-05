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

        }

        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EditorViewModel>();
            SimpleIoc.Default.Register<NavigationService>();
            SimpleIoc.Default.Register<SubCategoryViewModel>();
            SimpleIoc.Default.Register<TopicViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        public static MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static EditorViewModel Editor
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditorViewModel>();
            }
        }

        public static SubCategoryViewModel SubCategory => ServiceLocator.Current.GetInstance<SubCategoryViewModel>();

        public static TopicViewModel Topic => ServiceLocator.Current.GetInstance<TopicViewModel>();

        public static SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

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