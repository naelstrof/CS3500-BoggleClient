using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class BoggleApplicationContext : ApplicationContext
    {
        private int windowCount = 0;
        private static BoggleApplicationContext context;
        public static BoggleApplicationContext GetContext()
        {
            if (context == null)
            {
                context = new BoggleApplicationContext();
            }
            return context;
        }
        public void RunNew()
        {
            Boggle window = new Boggle();
            new Controller(window);
            windowCount++;
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };
            window.Show();
        }
    }
}