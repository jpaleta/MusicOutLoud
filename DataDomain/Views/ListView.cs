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
    class ListView : HtmlDoc
    {
        public ListView(BList l, IEnumerable<Card> c)
            : base("LIST - " + l.Id.ToString() + "do Board - " + l.IdBoard.ToString(),
                H1(Text("List: " + l.Name)),
                H2(Text("Description: ")),
                P (Text(l.Description)),
                Ul(
                    c.Select(lst => Li(A(ResolveUri.ForCard(lst), lst.Name))).ToArray()
                ),
                P(),
                A("http://localhost:8080/boards/" + l.IdBoard, "Back to Board Viewer"),
                P(),
                A(ResolveUri.ForBoards(), "Back to Board Manager"),
                 H2(Text("Change Description")),
                    Form("post", "/boards/" + l.Id + "/lists/" + l.Id,
                        Label("changeDesc", "Description: "), InputText("changeDesc")
                        ),
                H2(Text("Create a new card")),
                    Form("post", "/boards/" + l.IdBoard + "/lists/" + l.Id + "/cards",
                        Label("name", "Name: "), InputText("name")
                        ),
                H2(Form("post", "/boards/" + l.IdBoard + "/lists/" + l.Id + "/delete",
                             InputSubmit("Remove List")
                            )),
                H2(Form("post", "/boards/" + l.IdBoard + "/lists/" + l.Id + "/up",
                             InputSubmit("Move Up")
                            )),
                H2(Form("post", "/boards/" + l.IdBoard + "/lists/" + l.Id + "/down",
                             InputSubmit("Mode Down")
                            ))
                ){ }
    }
}
