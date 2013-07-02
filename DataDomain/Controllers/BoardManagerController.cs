using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WebGarten2;
using WebGarten2.Html;
using DataAccessLayer;
using DataDomain.Views;
using DataDomainEntities;

namespace DataDomain.Controllers
{
    class BoardManagerController
    {
        private BoardManager bm;
        private readonly IBoardsRepository _repo;
        private readonly IListsRepository _repoList;
        private readonly ICardsRepository _repoCard;
        private readonly ICardsArchiveRepository _repoCardsArchive;
        public BoardManagerController()
        {
            bm = new BoardManager();
            _repo = BoardsRepositoryLocator.Get();
            _repoList = ListsRepositoryLocator.Get();
            _repoCard = CardsRepositoryLocator.Get();
            _repoCardsArchive = CardsArchiveRepositoryLocator.Get();
        }

        
        // mostra todos os boards no Board Manager
        [HttpMethod("GET", "/boards")]
        public HttpResponseMessage GetBoardManager()
        {
            return new HttpResponseMessage
            {
                Content = new BoardManagerView(_repo.GetAll()).AsHtmlContent()
            };
        }

        // cria um novo Board
        [HttpMethod("POST", "/boards")]
        public HttpResponseMessage CreateBoard(NameValueCollection content)
        {
            var name = content["name"];
            if (name == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            foreach (Board td in _repo.GetAll())
            {
                if (td.Name == name)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
                    
            var tdAdd = new Board { Name = name };
            _repo.Add(tdAdd);
            //bm.boards.Add(td); // adiciona a board ao contentor de boards no board manager
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForBoard(tdAdd));
            return resp;
        }

        // mostra o novo board criado, ou mostra o board seleccionado na lista do Board Manager        
        [HttpMethod("GET", "/boards/{id}")]
        public HttpResponseMessage GetBoard(int id)
        {
            var td = _repo.GetById(id);
            return td == null ? new HttpResponseMessage(HttpStatusCode.NotFound) : 
                new HttpResponseMessage{
                    // 2012.10.12 - Idálio
                    // chamada ao novo método criado que devolve só as lista do board especifico
                    //Content = new BoardView(td, _repoList.GetAll()).AsHtmlContent()
                    Content = new BoardView(td, _repoList.GetAllByBoardId(id)).AsHtmlContent()
                };
        }
        //Altera a Description da Board em que se encontra
         [HttpMethod("POST", "/boards/{id}")]
        public HttpResponseMessage ChangeBoardDesc(NameValueCollection content, int id)
        {
            var td = _repo.GetById(id);
            var desc = content["changeDesc"];
            if (desc == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            td.Description = desc;

            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForBoard(td));
            return resp;
        }


         //Remove o board caso esteja vazio
         [HttpMethod("POST", "/boards/{id}/delete")]
         public HttpResponseMessage RemoveBoard(NameValueCollection content, int id)
         {
             var td = _repo.GetById(id);
             var lists = _repoList.GetAllByBoardId(id);
             //var desc = content["changeDesc"];
             if (lists.FirstOrDefault() != null)
             {
                 return new HttpResponseMessage(HttpStatusCode.BadRequest);
             }
             
             _repo.Remove(td);

             var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
             resp.Headers.Location = new Uri(ResolveUri.ForBoards());
             return resp;
         }


        [HttpMethod("POST", "/boards/{id}/lists")]  
        // cria uma nova lista
        public HttpResponseMessage CreateList(NameValueCollection content, int id)
        {
            Board td = _repo.GetById(id);
            if (td == null) {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var name = content["name"];
            if (name == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            foreach (BList lst in _repoList.GetAllByBoardId(id))
            {
                if (lst.Name == name)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest); 
            }

            var l = new BList { Name = name };
            _repoList.Add(l, td.Id);
            //td.lists.Add(l);  // adiciona a lista ao contentor de listas da board
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForList(l));
            return resp;
        }

        // mostra a nova lista criada, ou seleccionada
        [HttpMethod("GET", "/boards/{id}/lists")]
        public HttpResponseMessage GetLists(int id)
        {
            return new HttpResponseMessage
            {
                // 2012.10.12 - Idálio
                // chamada ao novo método criado que devolve só as lista do board especifico
                // Content = new ListsView(_repoList.GetAll(), id).AsHtmlContent()
                Content = new ListsView(_repoList.GetAllByBoardId(id), id).AsHtmlContent()
               
            };
        }
        // actualiza a description da lista
        [HttpMethod("Post", "/boards/{id}/lists/{id2}")]
        public HttpResponseMessage ChangeListDesc(NameValueCollection content, int id, int id2)
        {
            Board td = _repo.GetById(id);
            BList lst = _repoList.GetById(id2);
            var changeD = content["changeDesc"];

            if (changeD == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            lst.Description = changeD;
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForList(lst));
            return resp;
        }

        //Remove a lista caso esteja vazia
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/delete")]
        public HttpResponseMessage RemoveList(NameValueCollection content, int id2, int id)
        {
            var td = _repo.GetById(id);
            var lists = _repoList.GetAllByBoardId(id);
            
            
            var cards = _repoCard.GetAllByListAndBoardId(id2, id);
            //var desc = content["changeDesc"];
            if (cards.FirstOrDefault() != null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            //_repoList.Remove(lists,td.Id);
            //O que adicionei (David)
            //td.lists.Remove(_repoList.GetById(id2));
            _repoList.Remove(lists, id2);
            //
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForBoard(td));
            return resp;
        }

        // mostra o conteúdo de uma lista
        [HttpMethod("GET", "/boards/{id}/lists/{id2}")]
        public HttpResponseMessage GetList(int id, int id2)
        {
            var lst = _repoList.GetById(id2);
            return lst == null ? new HttpResponseMessage(HttpStatusCode.NotFound) : 
                new HttpResponseMessage{
                    Content = new ListView(lst, _repoCard.GetAllByListAndBoardId(id2, id)).AsHtmlContent()
                };
        }
                
        // cria um novo card
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards")]
        public HttpResponseMessage CreateCard(NameValueCollection content, int id, int id2)
        {
            Board td = _repo.GetById(id);
            BList lst = _repoList.GetById(id2);
            var name = content["name"];

            if (td == null || lst == null){
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (name == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
            var c = new Card { Name = name };
            c.CreationDate = DateTime.Now;
            _repoCard.Add(c, id2, id);
            //lst.cards.Add(c);  // adiciona a lista ao contentor de listas da board
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);

            resp.Headers.Location = new Uri(ResolveUri.ForCard(c));
            return resp;
        }

        //Remove o card
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards/{id3}/delete")]
        public HttpResponseMessage RemoveCard(NameValueCollection content, int id, int id2, int id3)
        {
            var td = _repo.GetById(id);
            var lists = _repoList.GetAllByBoardId(id);


            var cards = _repoCard.GetAllByListAndBoardId(id2, id);

            var card = _repoCard.GetById(id3);

            //2012.10.20 - João
            //remove do boardmanager
            /*foreach (BList b in td.lists)
            {
                if (b.Id == id2) 
                {
                    b.cards.Remove(card);
                }
            }*/

            //remove o card
            _repoCard.Remove(card);

            var lst = _repoList.GetById(id2);
            return lst == null ? new HttpResponseMessage(HttpStatusCode.NotFound) :
                new HttpResponseMessage
                {
                    Content = new ListView(lst, _repoCard.GetAllByListAndBoardId(id2, id)).AsHtmlContent()
                };

        }

        
        // mostra o card criado ou seleccionado
        [HttpMethod("GET", "/boards/{id}/lists/{id2}/cards/{id3}")]
        public HttpResponseMessage GetCard(int id, int id2, int id3)
        {
            return new HttpResponseMessage
            {
                Content = new CardView(_repoCard.GetById(id3)).AsHtmlContent()

            };
        }


        //Idálio - 2012.10.20 Arquiva um cartão
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards/{id3}/archive")]
        public HttpResponseMessage ArchiveCard(NameValueCollection content, int id, int id2, int id3)
        {
            var card = _repoCard.GetById(id3);

            // adicionar o card ao arquivo
            _repoCardsArchive.Add(card);  
            //remove o card da lista actual
            _repoCard.Remove(card);

            var lst = _repoList.GetById(id2);
            return lst == null ? new HttpResponseMessage(HttpStatusCode.NotFound) :
                new HttpResponseMessage
                {
                    Content = new ListView(lst, _repoCard.GetAllByListAndBoardId(id2, id)).AsHtmlContent()
                };
        }

        //Idálio - 2012.10.20  - mostra items arquivados do board seleccionado
        [HttpMethod("GET", "/boards/{id}/archive")]
        public HttpResponseMessage ArchiveCards(NameValueCollection content, int id)
        {
            /*return new HttpResponseMessage
            {
                Content = new CardsArchive(_repoCardsArchive.GetAllByBoardId(id), id).AsHtmlContent()
            };*/
            var cardList = _repoCardsArchive.GetAllByBoardId(id);
            return cardList == null ? new HttpResponseMessage(HttpStatusCode.NotFound) :
                new HttpResponseMessage
                {
                    Content = new CardsArchive(_repoCardsArchive.GetAllByBoardId(id), id).AsHtmlContent()
                };
        }

        //Idálio - 2012.10.20  - mostra items arquivados do board seleccionado
        [HttpMethod("GET", "/boards/{id}/archive/{id2}")]
        public HttpResponseMessage ArchiveCard(NameValueCollection content, int id, int id2)
        {
            /*return new HttpResponseMessage
            {
                Content = new CardsArchive(_repoCardsArchive.GetAllByBoardId(id), id).AsHtmlContent()
            };*/
            var cardList = _repoCardsArchive.GetAllByBoardId(id);
            return cardList == null ? new HttpResponseMessage(HttpStatusCode.NotFound) :
                new HttpResponseMessage
                {
                    Content = new CardArchive(_repoCardsArchive.GetById(id)).AsHtmlContent()
                };
        }

        //David - 2012.10.20 - actualiza a description do card
        [HttpMethod("Post", "/boards/{id}/lists/{id2}/cards/{id3}")]
        public HttpResponseMessage ChangeListDesc(NameValueCollection content, int id, int id2, int id3)
        {
            var card = _repoCard.GetById(id3);
            var changeD = content["changeDesc"];

            if (changeD == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            card.Description = changeD;
            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForCard(card));
            return resp;
        }
        //David - 2012.10.20 - Insere data de conclusão ao card
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards/{id3}/setlimit")]
        public HttpResponseMessage ConcludeCard(NameValueCollection content, int id, int id2, int id3)
        {
            var card = _repoCard.GetById(id3);

            var cDate = content["cDate"];

            card.ConclusionLimitDate = DateTime.Parse(cDate);

            var lst = _repoList.GetById(id2);
            return lst == null ? new HttpResponseMessage(HttpStatusCode.NotFound) :
                new HttpResponseMessage
                {
                    Content = new CardView(_repoCard.GetById(id3)).AsHtmlContent()
                };
        }
        //David - 2012.10.20 - Move a lista uma posição para cima
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/up")]
        public HttpResponseMessage MoveUp(NameValueCollection content, int id, int id2)
        {
            var lst = _repoList.GetAllByBoardId(id);
            var lstUp = _repoList.GetById(id2 - 1);
 
            if (lstUp == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _repoList.Swap(id2-1, id2);
            //altera os idList's nos cards das listas alteradas
            var aux = _repoCard.GetAllByListAndBoardId(id2, id);
            var aux2 = _repoCard.GetAllByListAndBoardId(id2 - 1, id);
            foreach (Card c in aux)
            {
                c.IdList = id2-1;
            }
            foreach (Card c in aux2)
            {
                c.IdList = id2;
            }            

            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForBoard(_repo.GetById(id)));
            return resp;
        }
        //David - 2012.10.21 - Move a lista uma posição para baixo
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/down")]
        public HttpResponseMessage MoveDown(NameValueCollection content, int id, int id2)
        {
            var lst = _repoList.GetAllByBoardId(id);
            var lstDown = _repoList.GetById(id2 + 1);

            if (lstDown == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _repoList.Swap(id2, id2 + 1);
            //altera os idList's nos cards das listas alteradas
            var aux = _repoCard.GetAllByListAndBoardId(id2, id);
            var aux2 = _repoCard.GetAllByListAndBoardId(id2 + 1, id);
            foreach (Card c in aux)
            {
                c.IdList = id2 + 1;
            }
            foreach (Card c in aux2)
            {
                c.IdList = id2;
            }

            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForBoard(_repo.GetById(id)));
            return resp;
        }
        //David - 2012.10.22 - Move a card uma posição para cima
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards/{id3}/up")]
        public HttpResponseMessage MoveCardUp(NameValueCollection content, int id, int id2, int id3)
        {
            var card = _repoCard.GetById(id3);
            var cardUp = _repoCard.GetById(id3 - 1);

            if (cardUp == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _repoCard.Swap(id3, id3 - 1);

            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForList(_repoList.GetById(id2)));
            return resp;
        }

        //David - 2012.10.22 - Move a card uma posição para baixo
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards/{id3}/down")]
        public HttpResponseMessage MoveCardDown(NameValueCollection content, int id, int id2, int id3)
        {
            var card = _repoCard.GetById(id3);
            var cardDown = _repoCard.GetById(id3 + 1);

            if (cardDown == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _repoCard.Swap(id3, id3 + 1);

            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForList(_repoList.GetById(id2)));
            return resp;
        }
        
        //David - 2012.10.22 - Move a Card entre listas        
        [HttpMethod("POST", "/boards/{id}/lists/{id2}/cards/{id3}/changeList")]
        public HttpResponseMessage ChangeList(NameValueCollection content, int id, int id2, int id3)
        {
            var nameListDest = content ["dListName"];
             if (nameListDest == null)
             {
                 return new HttpResponseMessage(HttpStatusCode.BadRequest);
             }

            var card = _repoCard.GetById(id3);
            var aux = _repoList.GetAll();
            int tmp = -1;
            foreach (BList l in aux)
            {
                if (l.Name == nameListDest)
                {
                    tmp = l.Id;
                    card.IdList = tmp;
                }

            }
            if (tmp == -1)
                 return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var resp = new HttpResponseMessage(HttpStatusCode.SeeOther);
            resp.Headers.Location = new Uri(ResolveUri.ForList(_repoList.GetById(tmp)));
            return resp;
                   
        }

 
    }
}
