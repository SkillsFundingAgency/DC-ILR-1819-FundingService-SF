using System;

namespace ESFA.DC.ILR.FundingService.Data.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveEquals(this string source, string data)
        {
            return source.Equals(data, StringComparison.OrdinalIgnoreCase);
        }
    }
}