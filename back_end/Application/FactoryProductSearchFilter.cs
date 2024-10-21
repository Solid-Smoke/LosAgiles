using Dapper;

namespace back_end.Application
{
    public interface IProductSearchFilter
    {
        public bool filterIsValid(string filter);
        public object getFilterInput(string filter);
        public void appendParametersValues(string filter,ref DynamicParameters parametersValues);
        public string getQuery();
        public string parseSearchText(string searchText);
    }

    public interface IFactoryProductSearchFilter
    {
        bool filterIsValid(string filter, string filterTypeString);
        IProductSearchFilter getFilterType(string filterTypeString);
        bool isFilterType(string filterTypeString);
    }

    public class FactoryProductSearchFilter : IFactoryProductSearchFilter
    {

        public IProductSearchFilter getFilterType(string filterTypeString)
        {
            switch (filterTypeString)
            {
                case "No filter":
                    return new ProductNoFilterSearchFilter();
                case "Business name":
                    return new ProductBusinessNameSearchFilter();
                case "Category":
                    return new ProductCategorySearchFilter();
                default:
                    throw new Exception("Invalid filterType");
            }
        }

        public bool isFilterType(string filterTypeString)
        {
            try
            {
                getFilterType(filterTypeString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool filterIsValid(string filterTypeString, string filter)
        {
            if (isFilterType(filterTypeString))
                return getFilterType(filterTypeString).filterIsValid(filter);
            else
                return false;
        }
    }

    internal class ProductCategorySearchFilter : IProductSearchFilter
    {
        private readonly List<string> _categories;
        public ProductCategorySearchFilter()
        {
            _categories = new List<string>(["Not perishables", "Perishables"]);
        }
        public void appendParametersValues(string filter,
            ref DynamicParameters parametersValues)
        {
            parametersValues.Add("@isPerishable", getFilterInput(filter));
        }
        public string getQuery()
        {
            return "and IsPerishable = @isPerishable";
        }
        public object getFilterInput(string filter)
        {
            return _categories.IndexOf(filter);
        }
        public bool filterIsValid(string filter) {
            return _categories.Contains(filter);
        }
        public string parseSearchText(string searchText)
        {
            return searchText;
        }
    }

    internal class ProductBusinessNameSearchFilter : IProductSearchFilter
    {
        public string getQuery()
        {
            return "and Businesses.Name like '%' + @filter + '%'";
        }
        public object getFilterInput(string filter)
        {
            return filter;
        }
        public void appendParametersValues(string filter,
            ref DynamicParameters parametersValues)
        {
            parametersValues.Add("@filter", getFilterInput(filter));
        }
        public bool filterIsValid(string filter) {
            return filter != "";
        }
        public string parseSearchText(string searchText)
        {
            return searchText;
        }
    }

    internal class ProductNoFilterSearchFilter : IProductSearchFilter
    {
        public void appendParametersValues(string filter,
            ref DynamicParameters parametersValues)
        {

        }
        public string getQuery()
        {
            return "";
        }
        public object getFilterInput(string filter)
        {
            return "";
        }
        public bool filterIsValid(string filter) {
            return true;
        }
        public string parseSearchText(string searchText)
        {
            return "";
        }
    }
}
