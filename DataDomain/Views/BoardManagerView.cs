using System.Collections.Generic;
using System.Linq;
using WebGarten2.Html;
using DataDomainEntities;
using DataDomain;

namespace DataDomain.Views
{
    class BoardManagerView : HtmlDoc
    {
        public BoardManagerView(IEnumerable<Board> t)
            :base("Boards Manager",
                H1(Text("Boards list:")),

                Ul(
                    t.Select(td => Li(A(ResolveUri.ForBoard(td), td.Name))).ToArray()
                ),
                H2(Text("Create a new board:")),
                Form("post","/boards",
                    Label("name","Name: "),InputText("name")
                    )
                
            ){}
    }
}
