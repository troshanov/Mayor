namespace Mayor.Services
{
    using System.Collections.Generic;

    public interface IStringOperationsService
    {
        public ICollection<string> SplitByEmptySpace(string text);
    }
}
