using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace MainAplication {

    
    // Source: http://stackoverflow.com/questions/661561/how-to-update-the-gui-from-another-thread-in-c
    // myLabel.SetPropertyThreadSafe(() => myLabel.Text, status); // status has to be a string or this will fail to compile
    public static class ControlHelper {

        private delegate void SetPropertyThreadSafeDelegate<TResult>(
            Control @this,
            System.Linq.Expressions.Expression<Func<TResult>> property,
            TResult value
        );

        public static void SetPropertyThreadSafe<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value) {
            var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            if (propertyInfo == null || !@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) || @this.GetType().GetProperty(propertyInfo.Name, propertyInfo.PropertyType) == null) {
                throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
            }

            if (@this.InvokeRequired) {
                @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>(SetPropertyThreadSafe), new object[] { @this, property, value });
            } else {
                @this.GetType().InvokeMember(
                    propertyInfo.Name,
                    BindingFlags.SetProperty,
                    null,
                    @this,
                    new object[] { value });
            }
        }
    }
}
