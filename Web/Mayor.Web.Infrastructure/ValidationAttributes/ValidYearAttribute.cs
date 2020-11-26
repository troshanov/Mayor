namespace Mayor.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ValidYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                var year = date.Year;

                if (1900 <= year && year <= DateTime.UtcNow.Year)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
