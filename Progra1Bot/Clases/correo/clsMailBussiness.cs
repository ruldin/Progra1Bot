
using Progra1Bot.Clases.Alumnos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimerBot.Clases.correo
{
    class clsMailBussiness
    {

        /// <summary>
        /// envia correo para verificar a la persona
 
        /// </summary>
        /// <param name="alumnoObj"></param>
        public String CorreoVerificacionInicial(mdAlumnos alumnoObj )
        {

            //String ret = "OK";
            Random rnd = new Random();
            clsCorreo cor = new clsCorreo();
            mdCorreoParametro par = new mdCorreoParametro();
            int clave = rnd.Next(10000, 50000);
           
            par.CORREODESTINO = alumnoObj.correo;
            par.AsuntoCorreo = "Verificacion de identidad";
            par.Cuerpo = "Hola, este correo es para verificar tu identidad para usar Telegram en la SUPER CLASE de PROGRA!!!\n";
            par.Cuerpo += "usando telegram manda la palabra verificar seguido de " + clave;
            par.Cuerpo += " \nEjemplo:\n verificar 12345";
            par.Cuerpo += " \nEres un RockStar de la Progra!!!";

            alumnoObj.actualizaInicio(alumnoObj, clave, false);
            
            return cor.enviarCorreoHotMail(par);
        }




    }
}
