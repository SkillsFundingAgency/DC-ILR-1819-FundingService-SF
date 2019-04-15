using System;
using System.Globalization;
using System.Linq;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using Oracle.Determinations.Masquerade.Util;

namespace ESFA.DC.OPA.Service
{
    public class DataEntityAttributeService : IDataEntityAttributeService
    {
        private const string Uncertain = "uncertain";
        private const string True = "true";
        private const string False = "false";

        public object GetAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return attribute;
                }
            }

            return null;
        }

        public string GetStringAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return attributeString;
                }
            }

            return null;
        }

        public int? GetIntAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return int.Parse(attributeString, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                }
            }

            return null;
        }

        public long? GetLongAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return long.Parse(attributeString, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                }
            }

            return null;
        }

        public decimal? GetDecimalAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return decimal.Parse(attributeString, NumberStyles.Float);
                }
            }

            return 0;
        }

        public decimal GetDecimalAttributeValue(object attributeValue)
        {
            if (attributeValue != null && attributeValue.ToString() != Uncertain)
            {
                var stringValue = attributeValue.ToString();

                if (stringValue == True || stringValue == False)
                {
                    return stringValue == True ? 1.0m : 0.0m;
                }

                decimal decValue;

                decimal.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out decValue);

                return decValue;
            }

            return 0;
        }

        public bool? GetBoolAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return attributeString == True;
                }
            }

            return null;
        }

        public DateTime? GetDateTimeAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null && attribute.ToString() != Uncertain)
            {
                return ((Date)attribute).GetDateTime();
            }

            return null;
        }

        public decimal GetDecimalAttributeValueForPeriod(IAttributeData attributes, DateTime periodDate)
        {
            return GetDecimalAttributeValue(attributes.Changepoints.Where(cp => cp.ChangePoint == periodDate).Select(v => v.Value).SingleOrDefault());
        }
    }
}
