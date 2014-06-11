using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EAP.Logic
{
    public class MyDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var md = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            DataTypeAttribute dataTypeAttribute = attributes.OfType<DataTypeAttribute>().FirstOrDefault();
            DisplayFormatAttribute displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
            if (displayFormatAttribute == null && dataTypeAttribute != null)
            {
                displayFormatAttribute = dataTypeAttribute.DisplayFormat;
            }
            if (displayFormatAttribute == null)
            {
                md.ConvertEmptyStringToNull = false;
            }

            return md;
        }
    }
}
