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

        public bool registrarUsuario(string lastName, string firstName, string status, string rol, string email, string passw) {

            // Ejecuta el procedimiento almacenado y obtiene el resultado (0 o 1)
            int result = (int)User.RegistrarUsuario(lastName, firstName, status, rol, email, passw);

            return result == 1;

        }




    }
}
