namespace Mayor.Services
{
    using System.Collections.Generic;

    public class StringOperationsServices : IStringOperationsService
    {

        public ICollection<string> SplitByEmptySpace(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text
                .Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
