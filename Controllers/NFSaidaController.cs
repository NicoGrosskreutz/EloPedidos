using Android.App;
using Android.Content;
using EloPedidos.Models;
using EloPedidos.Persistence;
using System.Collections.Generic;

namespace EloPedidos.Controllers
{
    public class NFSaidaController
    {
        private Context Context;
        private NFSaidaDAO DAO;

        public NFSaidaController()
        {
            Context = Application.Context;
            DAO = new NFSaidaDAO();
        }

        public bool Save(NFSaida nf) => DAO.Save(nf);

        public List<NFSaida> FindAll() => (List<NFSaida>)DAO.FindAll();

        public NFSaida FindById(long id) => DAO.FindById(id);

        public NFSaida FindByNRONF(int pNRONF) => DAO.FindByNRONF(pNRONF);

        public int? GetNRONF() => DAO.GetNRONF();

        public List<NFSaida> FindAllNotSinc() => (List<NFSaida>)DAO.FindAllNotSinc();

        public bool Delete(long pFT_NFSAIDA_ID) => DAO.Delete(pFT_NFSAIDA_ID);
    }
}