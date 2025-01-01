using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Common
{
    public static class ObjectExtensionMethods
    {
        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();
            var notMapProperty = new string[] { "Code", "CreatedDate", "CreatedBy", "UpdatedBy", "UpdatedDate" };

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    try
                    {
                        if (fromProperty.Name == toProperty.Name && !notMapProperty.Contains(fromProperty.Name))
                        {
                            toProperty.SetValue(self, fromProperty.GetValue(parent));
                            break;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}
