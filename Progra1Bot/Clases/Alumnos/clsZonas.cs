using PrimerBot.Clases.correo;
using Progra1Bot.Clases.modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class clsZonas
    {

        public void envia_zonas(string tabla, string seccion = "no") {
            clsCorreo corr = new clsCorreo();
            foreach (mdZonas z in new mdZonas().cargaZonas(tabla, seccion))
            {
                mdCorreoParametro p = new mdCorreoParametro();
                p.CORREODESTINO = z.correo;
                p.AsuntoCorreo = "Zona de la Clase de Programación";
                p.Cuerpo = $"Hola {z.nombres}, su zona de la clase de progra es de {z.zona}";
                corr.enviarCorreoHotMail(p);
                

            }
        }

    }
}
