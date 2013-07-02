using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomainEntities;


namespace DataDomain
{
    static class ResolveUri
    {

        // mostra todos os boards
        public static string ForBoards()
        {
            return "http://localhost:8080/boards";
        }
        
        // mostra um board especifico
        public static string ForBoard(Board td)
        {
            return string.Format("http://localhost:8080/boards/{0}", td.Id);
        }
        /*
        // mostra todas as lista de um board
        public static string ForLists(Boards td)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists", td.Id);
        }*/

        // mostra uma lista de um determinado board        
        public static string ForList(BList l)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}",l.IdBoard, l.Id);
        }

        // iv - 2012.10.18
        // novo método
        // mostra um card de uma lista de um board
        public static string ForCard(Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/cards/{2}", c.IdBoard, c.IdList, c.Id);
        }

        // iv - 2012.10.18
        // novo método
        // remove um board especifico
        public static string RemoveBoard(Board td)
        {
            return string.Format("http://localhost:8080/boards/{0}/delete", td.Id);
        }

        // iv - 2012.10.18
        // novo método
        // remove uma lista de um determinado board        
        public static string RemoveList(BList l)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/delete", l.IdBoard, l.Id);
        }


        // iv - 2012.10.18
        // novo método
        // remove um card de uma lista de um board
        public static string RemoveCard(Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/cards/{2}/delete", c.IdBoard, c.IdList, c.Id);
        }

        // iv - 2012.10.20
        // novo método
        // permite mostrar todos os itens arquivados de um board (pode mostrar listas e/ ou cards)
        public static string ForArchive(Board b)
        {
            return string.Format("http://localhost:8080/boards/{0}/archive", b.Id);
        }

        // iv - 2012.10.20
        // novo método
        // permite mostrar o cartão arquivado 
        public static string ForArchiveCard(int IdBoard, Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/archive/{1}", IdBoard, c.IdArchive);
        }

        // iv - 2012.10.20
        // novo método
        // permite mostrar a lista arquivada
        public static string ForArchiveList(Board b, BList l)
        {
            return string.Format("http://localhost:8080/boards/{0}/archive/{1}", b.Id, l.IdArchive);
        }

        // iv - 2012.10.20
        // novo método
        // permite arquivar o cartão 
        public static string ArchiveCard(Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/cards/{2}/archive", c.IdBoard, c.IdList, c.Id);
        }

        // dm - 2012.10.20
        //novo metodo
        //permite mover a lista uma posição para cima
        public static string MoveUp(BList l)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/up", l.IdBoard, l.Id);
        }
        // dm - 2012.10.20
        //novo metodo
        //permite mover a lista uma posição para baixo
        public static string MoveDown(BList l)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/down", l.IdBoard, l.Id);
        }
        // dm - 2012.10.21
        //novo metodo
        //permite mover a card uma posição para cima
        public static string MoveCardUp(Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/cards/{2}/up", c.IdBoard, c.IdList, c.Id);
        }
        // dm - 2012.10.21
        //novo metodo
        //permite mover a card uma posição para baixo
        public static string MoveCardDown(Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/cards/{2}/down", c.IdBoard, c.IdList, c.Id);
        }
        // dm - 2012.10.22
        //novo metodo
        //permite mover a card para a lista indicada
        public static string ChangeList(Card c)
        {
            return string.Format("http://localhost:8080/boards/{0}/lists/{1}/cards/{2}/changeList", c.IdBoard, c.IdList, c.Id);
        }


    }
}
