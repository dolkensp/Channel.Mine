using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RazorEngine;
using System.Threading;

namespace Channel.Mine.Web
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String template = @"<tr><td> @Model.Count </td>
@for (int x=0;x<100;x++) {
  <td>space</td>
}
</tr>";
            ThreadPool.SetMaxThreads(100, 100);

            Int32 i = 0, j = 1;
            String results = String.Empty;
            Object lockObj = new Object();

            Razor.Parse(template, new { Count = 0 }, "Fast");

            while (i++ < 1000)
            {
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    String result = Razor.Parse(template, new { Count = x }, "Fast");

                    lock (lockObj)
                    {
                        results += result;
                        j++;
                    }
                }, i);
            }
            // RazorEngine.Templating.TemplateService a = new RazorEngine.Templating.TemplateService(
            while (j < i) ;

            template = @"<!DOCTYPE html>
<html>
  <body>
    <table>
      @Model.Results
    </table>
  </body>
</html>";
            Response.Write(Razor.Parse(template, new { Results = results }));
        }
    }
}