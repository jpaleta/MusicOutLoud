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
    class CardsArchive : HtmlDoc 
    {
        public CardsArchive(IEnumerable<Card> c, int IdBoard)
            : base("Board - " + IdBoard.ToString() + " Archive Items",
            H1(Text("Cards: ")),
            Ul(
                    c.Select(lst => Li(A(ResolveUri.ForArchiveCard(IdBoard, lst), lst.Name))).ToArray()
            ),
            P(),
            A("http://localhost:8080/boards/" + IdBoard.ToString(), "Back to Board Viewer"),
            P(),
            A(ResolveUri.ForBoards(), "Back to Board Manager")
            ){ }
    }
}
