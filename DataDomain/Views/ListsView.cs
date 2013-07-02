using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGarten2.Html;
using DataDomain;
using DataDomainEntities;

namespace DataDomain.Views
{
    class ListsView : HtmlDoc
    {
        public ListsView(IEnumerable<BList> l, int id)
            : base("Lists",
                H1(Text("Lists View")),
                Ul(
                    l.Select(lst => Li(A(ResolveUri.ForList(lst), lst.Description))).ToArray()
                    ),
                H2(Text("Create a new list:")),
                Form("post", "/boards/" + id + "/lists",
                    Label("desc", "Description: "), InputText("desc")
                    )
                ) { }
    }
}
