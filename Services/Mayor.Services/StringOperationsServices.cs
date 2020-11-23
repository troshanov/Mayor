namespace Mayor.Services
{
    using System.Collections.Generic;

    public class StringOperationsServices : IStringOperationsService
    {

        public ICollection<string> SplitByEmptySpace(string text)
        {
            return text
                .Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
