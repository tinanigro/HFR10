using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using Hfr.Commands;
using Hfr.Commands.Topics;
using Hfr.Helpers;
using Hfr.Model;
using Hfr.Models;

namespace Hfr.ViewModel
{
    public class SubCategoryViewModel : ViewModelBase
    {
        #region private properties
        private bool _categoriesLoading;
        private SubCategory _currentSubCategory;
        private uint _topicsPage = 1;
        #endregion
        #region private fields
        private List<SubCategory> _categories;
        private IEnumerable<IGrouping<string, SubCategory>> _catsGrouped;

        private List<Topic> _topics;

        #endregion
        #region public properties
        public bool IsCategoriesLoading
        {
            get { return _categoriesLoading; }
            set
            {
                Set(ref _categoriesLoading, value);
                RaisePropertyChanged(nameof(LoadingCategoriesList));
            }
        }

        public Visibility LoadingCategoriesList => IsCategoriesLoading ? Visibility.Visible : Visibility.Collapsed;
        public SubCategory CurrentSubCategory
        {
            get { return _currentSubCategory; }
            set
            {
                Set(ref _currentSubCategory, value);
                TopicsPage = 1;
            }
        }

        public uint TopicsPage
        {
            get { return _topicsPage;}
            set { Set(ref _topicsPage, value); }
        }
        #endregion
        #region public fields

        public List<SubCategory> Categories
        {
            get { return _categories; }
            set
            {
                Set(ref _categories, value);
                IsCategoriesLoading = false;
            }
        }

        public IEnumerable<IGrouping<string, SubCategory>> CategoriesGrouped
        {
            get
            {
                if (_catsGrouped == null)
                {
                    RefreshCats();
                }
                return _catsGrouped;
            }
            set
            {
                Set(ref _catsGrouped, value);
            }
        }

        public List<Topic> Topics
        {
            get { return _topics; }
            set { Set(ref _topics, value); }
        }
        #endregion

        #region commands
        public OpenSubCatCommand OpenSubCatCommand { get; } = new OpenSubCatCommand();

        public ChangeTopicsListPageInSubCatCommand ChangeTopicsListPageInSubCatCommand { get; } =
            new ChangeTopicsListPageInSubCatCommand();
        #endregion

        #region methods

        void RefreshCats()
        {
            if (!_categoriesLoading)
            {
                IsCategoriesLoading = true;
                Task.Run(async () => await CatFetcher.GetCats());
            }
        }
        #endregion

    }
}
