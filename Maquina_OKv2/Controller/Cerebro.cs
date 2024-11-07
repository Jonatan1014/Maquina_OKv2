using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maquina_OKv2.maquina_bdDataSetTableAdapters;                              

namespace Maquina_OKv2.Controller
{                                                   
    internal class Cerebro
    {

        UsuariosTableAdapter User = new UsuariosTableAdapter();

        public bool verificarUsuario(string email, string passw)
        {
            return (bool)User.VerificarUsuario(email, passw);

        }


    }
}
