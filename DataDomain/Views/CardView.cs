using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain;
using DataDomainEntities;
using WebGarten2.Html;

namespace DataDomain.Views
{
    class CardView : HtmlDoc
    {
         public CardView(Card c)
            : base("CARD - " + c.Id.ToString() + " da lista - " + c.IdList.ToString() + " do board - " + c.IdBoard.ToString(),
                H1(Text("Card: " + c.Name)),
                H2(Text("Description: ")),
                P(Text(c.Description)),
                P(Text("Creation date: " + c.CreationDate)),
                P(Text("Conclusion Date: " + c.ConclusionLimitDate)),
                P(),
                A("http://localhost:8080/boards/" + c.IdBoard + "/lists/"+ c.IdList, "Back to List Viewer"),
                P(),
                A("http://localhost:8080/boards/" + c.IdBoard, "Back to Board Viewer"),
                P(),
                A(ResolveUri.ForBoards(), "Back to Boards Manager"),
                H2(Text("Add/Change Description")),
                    Form("post", "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" +c.Id,
                        Label("changeDesc", "Description: "),InputText("changeDesc")
                        ),
                H2(Form("post", 
                         "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" + c.Id +"/setlimit",
                         Label("cDate", "Conclusion Date Limit (dd-mm-aaaa hh:mm:ss): "),InputText ("cDate")
                    )),
                H2(Form("post", "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" + c.Id +"/delete",
                             InputSubmit("Remove Card")
                            )),
                H2(
                    Form("post", 
                         "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" + c.Id +"/archive",
                          InputSubmit("Arquivar"))),
                H2(
                    Form("post", "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" + c.Id + "/up",
                            InputSubmit("Move Up")
                            )),
                H2(
                    Form("post", "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" + c.Id + "/down",
                            InputSubmit("Mode Down")
                            )),
                H2(Form("post",
                    "/boards/" + c.IdBoard + "/lists/" + c.IdList + "/cards/" + c.Id + "/changeList",
                    Label("cDate", "Write the name of destination List: "), InputText("dListName")
                    ))
           )
                
                
       { }
    }
}
