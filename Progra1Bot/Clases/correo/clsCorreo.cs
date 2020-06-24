﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;


namespace PrimerBot.Clases.correo
{
    public class clsCorreo
    {
      

        /// <summary>
        /// Metodo final para envio de correo con SMTP Y VALIDACION CON CERTIFICADO
        /// </summary>
        /// <param name="mailP"> ESTRUCTURA DE PARAMETRIZACION</param>
        /// <returns></returns> 
        public String enviarCorreoHotMail(mdCorreoParametro mailP)
        {
            var retorno = "OK";
            string SMTP = "smtp.live.com";
            string SMTPPORT = "587";
            string usuarioSMTP = "algun@hotmail.com";
            string claveSMPT = "NO TE LA DIGO";
         
            try
            {

                MailMessage o = new MailMessage(usuarioSMTP, mailP.CORREODESTINO, mailP.AsuntoCorreo, mailP.Cuerpo);
                NetworkCredential netCred = new NetworkCredential(usuarioSMTP, claveSMPT);
                SmtpClient smtpobj = new SmtpClient(SMTP, Convert.ToInt32(SMTPPORT));
                smtpobj.EnableSsl = true;
                smtpobj.Credentials = netCred;

               
                smtpobj.Send(o);
                return retorno;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Enviar el Correo:" + ex.Message);
                return "Error al Enviar el Correo:" + ex.Message;

            }
        }// end function

    }
}
