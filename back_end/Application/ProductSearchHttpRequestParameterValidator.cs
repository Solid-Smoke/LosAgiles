namespace back_end.Application
{
    public interface IProductSearchHttpRequestParameterValidator
    {
        void checkSearchProductsParameters(string searchText, int startIndex, int maxResults, string filterTypeString, string filter);
    }

    public class ProductSearchHttpRequestParameterValidator : IProductSearchHttpRequestParameterValidator
    {

        private readonly IFactoryProductSearchFilter factoryProductSearchFilter;

        public ProductSearchHttpRequestParameterValidator(IFactoryProductSearchFilter factoryProductSearchFilter)
        {
            this.factoryProductSearchFilter = factoryProductSearchFilter;
        }


        public void checkSearchProductsParameters(string searchText, int startIndex,
            int maxResults, string filterTypeString, string filter)
        {
            if (searchText == null)
                throw new ArgumentNullException("seachText parameter is null");

            else if (startIndex < 1)
                throw new ArgumentOutOfRangeException(
                    $"Invalid parameter startIndex:" +
                    $"{startIndex}, should be greather than 1");

            else if (maxResults < 1)
                throw new ArgumentOutOfRangeException(
                    $"Invalid parameter maxResults: {maxResults}," +
                    $"should be greather than 1");

            else if (filterTypeString == null)
                throw new ArgumentNullException("filterTypeString is null");

            else if (filter == null)
                throw new ArgumentNullException("filter is null");

            else if (factoryProductSearchFilter.isFilterType(filterTypeString) == false)
                throw new ArgumentException($"Invalid filter type:" +
                    $"{filterTypeString}");

            else if (factoryProductSearchFilter
                .filterIsValid(filterTypeString, filter) == false)
                throw new ArgumentException(
                    $"Invalid filter for filter type {filterTypeString}:" +
                    $"{filter}");
        }
    }
}
