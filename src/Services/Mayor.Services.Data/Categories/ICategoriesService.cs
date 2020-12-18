namespace Mayor.Services.Data.Categories
{
    using System.Collections.Generic;

    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
