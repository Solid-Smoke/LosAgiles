namespace back_end.Application
{
    public interface IProductSearchHttpRequestParameterValidator
    {
        void checkSearchProductsParameters(string searchText, int startIndex, int maxResults, string? filterTypeString, string? filter);
    }

    public class ProductSearchHttpRequestParameterValidator : IProductSearchHttpRequestParameterValidator
    {

        private readonly IFactoryProductSearchFilter factoryProductSearchFilter;

        public ProductSearchHttpRequestParameterValidator(IFactoryProductSearchFilter factoryProductSearchFilter)
        {
            this.factoryProductSearchFilter = factoryProductSearchFilter;
        }


        public void checkSearchProductsParameters(string searchText, int startIndex,
            int maxResults, string? filterTypeString, string? filter)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(
                    $"Invalid parameter startIndex:" +
                    $"{startIndex}, should be greather than 1");

            else if (maxResults < 1)
                throw new ArgumentOutOfRangeException(
                    $"Invalid parameter maxResults: {maxResults}," +
                    $"should be greather than 1");

            else if (filter != null && filterTypeString == null)
                throw new ArgumentNullException("Filter type not specified");

            else if (filterTypeString != null && filter == null)
                throw new ArgumentNullException("Filter not specified");

            else if (filterTypeString != null &&
                    factoryProductSearchFilter.isFilterType(filterTypeString) == false)
                throw new ArgumentException($"Unexisting filter type:" +
                    $"{filterTypeString}");

            else if (filterTypeString != null && factoryProductSearchFilter
                .filterIsValid(filterTypeString, filter) == false)
                throw new ArgumentException(
                    $"Invalid filter for filter type {filterTypeString}:" +
                    $"{filter}");
        }
    }
}
