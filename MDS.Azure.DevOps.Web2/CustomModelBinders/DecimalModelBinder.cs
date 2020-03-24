using System.Globalization;
using System.Web.Mvc;

namespace MDS.Azure.DevOps.Web2.CustomModelBinders
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result != null && !string.IsNullOrEmpty(result.AttemptedValue))
            {
                if (bindingContext.ModelType == typeof(decimal) || bindingContext.ModelType == typeof(decimal?))
                {
                    decimal temp;
                    var attempted = result.AttemptedValue.Replace(",", ".");
                    if (decimal.TryParse(
                        attempted,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out temp)
                    )
                    {
                        return temp;
                    }
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}