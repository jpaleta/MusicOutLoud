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
    class CardArchive : HtmlDoc
    {
        public CardArchive(Card c)
            : base("Card Archive - " + c.IdArchive.ToString(),
                H1(Text("Card: " + c.Name)),
                H2(Text("Description: ")),
                P(Text(c.Description)),
                P(Text("Creation date: " + c.CreationDate)),
                P(Text("Conclusion Date: " + c.ConclusionLimitDate)),
                P(),
                A("http://localhost:8080/boards/" + c.IdBoard, "Back to Board Viewer"),
                P(),
                A(ResolveUri.ForBoards(), "Back to Boards Manager")
           ){ }

    }
}
