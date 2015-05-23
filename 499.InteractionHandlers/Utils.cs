using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _499.InteractionHandlers {
    public class Utils {

        /// <summary>
        /// Obtiene todos los controles hijos, del tipo 'type', del control 'control'.
        /// http://stackoverflow.com/questions/3419159/how-to-get-all-child-controls-of-a-windows-forms-form-of-a-specific-type-button
        /// </summary>
        public static IEnumerable<System.Windows.Forms.Control> GetAll(System.Windows.Forms.Control control, Type type) {
            var controls = control.Controls.Cast<System.Windows.Forms.Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
        public static void DisableAllButMe(System.Windows.Forms.Control me, Type type) {
            foreach (var control in GetAll(me.Parent, type)) {
                if (control != me)
                    control.Enabled = false;
            }
        }

        public static void EnableAllButMe(System.Windows.Forms.Control me, Type type) {
            foreach (var control in GetAll(me.Parent, type)) {
                if (control != me)
                    control.Enabled = true;
            }
        }
    }
}
