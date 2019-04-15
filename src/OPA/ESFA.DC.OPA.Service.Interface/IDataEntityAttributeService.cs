using System;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Service.Interface
{
    public interface IDataEntityAttributeService
    {
        object GetAttributeValue(IDataEntity dataEntity, string attributeName);

        string GetStringAttributeValue(IDataEntity dataEntity, string attributeName);

        int? GetIntAttributeValue(IDataEntity dataEntity, string attributeName);

        long? GetLongAttributeValue(IDataEntity dataEntity, string attributeName);

        decimal? GetDecimalAttributeValue(IDataEntity dataEntity, string attributeName);

        decimal GetDecimalAttributeValue(object attribueValue);

        bool? GetBoolAttributeValue(IDataEntity dataEntity, string attributeName);

        DateTime? GetDateTimeAttributeValue(IDataEntity dataEntity, string attributeName);

        decimal GetDecimalAttributeValueForPeriod(IAttributeData attributes, DateTime periodDate);
    }
}
