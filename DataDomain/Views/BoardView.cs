using WebGarten2.Html;
using System.Collections.Generic;
using System.Linq;
using DataDomain;
using DataDomainEntities;

namespace DataDomain.Views
{
    
    class BoardView : HtmlDoc
    {
        public BoardView(Board t, IEnumerable<BList> l)
            :base("BOARD - " + t.Name.ToString(),
                H1(Text("Board: " + t.Name)),
                H2(Text("Description: ")),
                P(Text(t.Description)),
                Ul(
                    l.Select(lst => Li(A(ResolveUri.ForList(lst), lst.Name))).ToArray()
                    ),
                P(),
                A(ResolveUri.ForBoards(),"Back to Board Manager"),
                H2(Text("Add/Change Description")),
                    Form("post", "/boards/" + t.Id,
                        Label("changeDesc", "Description: "),InputText("changeDesc")
                        ),
                H2(Text("Create a new list:")),
                    Form("post", "/boards/" + t.Id + "/lists",
                        Label("name", "Name: "), InputText("name")
                        ),
                H2(Form("post", "/boards/" + t.Id + "/delete",
                         InputSubmit("Remove Board")
                        )),
                H2(
                    Form("get", 
                         "/boards/" + t.Id.ToString() + "/archive",
                          InputSubmit("Itens Arquivados")
                    )
                )
         ){}
    }
}