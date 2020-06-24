using Progra1Bot.Clases.Alumnos;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using System.Collections.Generic;
using Progra1Bot.Clases.emojis;
using System.Text.RegularExpressions;
using PrimerBot.Clases.correo;
using Progra1Bot.Clases.modelos;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Progra1Bot.Clases
{
    class clsBotAlumnos
    {
        private static string llave= "Token Telegram";
        private static TelegramBotClient Bot = new TelegramBotClient(llave);
        private List<mdAlumnos> TodosLosAlumnos = new mdAlumnos().cargaTodosAlumnosBaseDatos();

        public async Task IniciarTelegram()
        {
            

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            Bot.OnMessage += BotCuandoRecibeMensajes;
            Bot.OnMessageEdited += BotCuandoRecibeMensajes;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"escuchando solicitudes del BOT @{me.Username}");



            Console.ReadLine();
            Bot.StopReceiving();
        }



        public async System.Threading.Tasks.Task EnviaPDFAsync(string usuario, String archivo, String titulo)
        {
            ITelegramBotClient botClient2 = new TelegramBotClient(llave);
            var me = botClient2.GetMeAsync().Result;
            Console.WriteLine($"Envio de manual   al usuario {usuario}");
    
            try
            {
                await botClient2.SendChatActionAsync(usuario, ChatAction.Typing);
                using (var fileStream = new FileStream(archivo, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    await botClient2.SendDocumentAsync(
                        chatId: usuario,
                        caption: titulo,
                        document: new InputOnlineFile( /* content: */ fileStream, /* fileName: */ "manual.pdf")
                        );
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("error al mandar PDF!" + err.Message);
            }
        }


        /// <summary>
        /// funcion de enviar audios
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="archivo"></param>
        /// <param name="titulo"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task EnviaAudioAsync(string usuario, String archivo, String titulo)
        {
            ITelegramBotClient botClient2 = new TelegramBotClient(llave);
            var me = botClient2.GetMeAsync().Result;
            Console.WriteLine($"Envio de AUDIO   al usuario {usuario}");
           
            try
            {
                await botClient2.SendChatActionAsync(usuario, ChatAction.Typing);
                using (var fileStream = new FileStream(archivo, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    await botClient2.SendAudioAsync(
                        chatId: usuario,
                        caption: titulo,
                        audio: new InputOnlineFile( /* content: */ fileStream, /* fileName: */ Path.GetFileName(archivo))
                        );
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("error al mandar MP3!"+err.Message);
            }
        }



        public async System.Threading.Tasks.Task EnviaFoto(string usuario, String archivo, String titulo)
        {
            var botClient = new TelegramBotClient(llave);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Envío de Foto a: {usuario}");
           

            using (var fileStream = new FileStream(archivo, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await botClient.SendPhotoAsync(
                     chatId: usuario,
                     photo: fileStream,
                     caption: titulo
                     );
            }
        }



        // cuando recibe mensajes
        private  async void BotCuandoRecibeMensajes(object sender, MessageEventArgs messageEventArgumentos)
        {

            mdAlumnos alumno;
            var ObjetoMensajeTelegram = messageEventArgumentos;
            var mensajes = ObjetoMensajeTelegram.Message;
          
            string Telegram_id_manda_mensaje = mensajes.Chat.Id.ToString();
            string respuesta = "No te entiendo";
            string mensajeEntrante;



            if (mensajes.Text == null)
            {
                mensajeEntrante = "Recibiendo objeto...";

            }
            else
            {
                mensajeEntrante = mensajes.Text.ToLower();
            }





            List<mdAlumnos> alu = new mdAlumnos().cargaTodosAlumnosBaseDatos();
            alumno = alu.Find(x => x.idbot.Equals(Telegram_id_manda_mensaje));

            //if (Telegram_id_manda_mensaje.Equals("1133551696"))
            // {
            //     await Bot.SendTextMessageAsync(
            //             chatId: ObjetoMensajeTelegram.Message.Chat,
            //             parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            //             text: "Lo lamento no estas registrado");
            //     return;
            // }



            Console.WriteLine($"Recibiendo Mensaje del ID {Telegram_id_manda_mensaje}.");
           // Console.WriteLine($"Dice {mensajeEntrante}.");
            Console.WriteLine($"Dice {mensajeEntrante}.");
            

            //// si el mensaje viene nulo, lo retorna
            //if (mensajes.Text == null)
            //{
            //    return;
            //}


            //if (alumno == null)
            //{
            //    await Bot.SendTextMessageAsync(
            //       chatId: ObjetoMensajeTelegram.Message.Chat,
            //       parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            //       text: "No estas registrado, lo lamento"
            //       );

            //}




            // busca entre todos los alumnos si existe el idbot que escribe, todos por defecto tienen 0
            mdAlumnos alumnoEscribe = TodosLosAlumnos.Find(x => x.idbot.Equals(Telegram_id_manda_mensaje));

            // SI EL OBJETO ES NULO ES PORQUE NO EXITE EN LA BASE DE DATOS, ES DECIR, TIENE 0 EN IDBOT PORQUE NO LO ENCONTRO
            if (alumnoEscribe == null)
            {
                respuesta = mdEmoji.saludo + "  Hola, mi pequeño padawan, será un gusto de enseñarte el camino a la gloria del aprendizaje del mundo de la progra!!";
                respuesta += " por motivos de seguridad y confidencialidad quiero saber si eres parte de este selecto grupo\n";
                respuesta += "Para que pueda verificar si eres parte de las futuras RockStar de Guatemala, escribe la palabra *CORREO* y tu direccion de correo de la UMG \n";
                respuesta += "Ejemplo:\n *correo* GeekJedy@miumg.edu.gt";
               
            }
           
            //if (mensajes == null || mensajes.Type != MessageType.Text) return;


            if (mensajeEntrante.Contains("papá")  || mensajeEntrante.Contains("padre"))
            {
            //    if (id_manda_mensaje.Equals("821287266"))
            //    {
            //        respuesta = " Ruldin!! tu eres mi Padre!!!";
            //    } else
            //    {
            //        respuesta = "Mi papa se llama Ruldin";
            //    }
            }
   
      

            //verifica  el codigo de registro que se mandó
            if (mensajeEntrante.ToLower().StartsWith("verificar"))
            {
                Match m = Regex.Match(mensajeEntrante, "(\\d+)");
                string num = string.Empty;

                if (m.Success)
                {
                    num = m.Value;
                }
                else { num = "0"; }

                mdAlumnos alumnoVerificar = TodosLosAlumnos.Find(x => x.idbot.Equals("P" + num));
                if (alumnoVerificar != null)
                {
                    alumnoVerificar.actualizaInicio(alumnoVerificar, Convert.ToInt32(Telegram_id_manda_mensaje), true);
                    //resp = mdEmoji.caralentesoscuros+" Bienvenido!!!! ya eres de mi confianza!!!";
                    respuesta = "Ya eres parte de este selecto grupo, ganaste mi confianza, vas a llegar lejos\n Claro, si tu quieres";
                }
                else
                {
                    respuesta = "Lo siento, el códido no es el que te mandé, preguntale al profe!!";
                }
            }




            //si la palabra es correo, se le envia el correo de verificaion
            if (mensajeEntrante.StartsWith("correo"))
            {
                string correoAlumno = mensajeEntrante.Replace("correo", "").Trim();
                mdAlumnos alumnoLocal = TodosLosAlumnos.Find(x => x.correo.Equals(correoAlumno));

                if (alumnoLocal == null)
                {
                    respuesta = "El correo que ingresaste no aparece en la base de datos de padawans";
                }
                else
                {
                    if (!correoAlumno.ToLower().EndsWith("@miumg.edu.gt"))
                    {
                        respuesta = "Lo lamento, el correo:\n *" + correoAlumno + "* que tengo registrado en la base de datos,no pertenece a  *UMG*,\n Qué puede hacer? \n Estudie en la UMG, especificamente en la Facultad de Ingenieria en Sistemas\n Gracias!";
                          
                    }


                    if (new clsMailBussiness().CorreoVerificacionInicial(alumnoLocal).Equals("OK"))
                    {
                        respuesta = "Hola, si tu eres *" + alumnoLocal.nombre + "*, ahorita te mandé un correo  " + mdEmoji.correo + " de verificación a " + alumnoLocal.correo + " \n con instrucciones para registrarte " + mdEmoji.smile;
                    }
                    else
                    {
                        respuesta = "Lo lamento hubo un inconveniente al envíar el correo de verificación";
                    }

                    TodosLosAlumnos = new mdAlumnos().cargaTodosAlumnosBaseDatos(); //recarga la info de alumnos


                }

            }

            if (mensajeEntrante.ToLower().Contains("archivos de ruldin"))
            {
               respuesta = new ClsManejoArchivos().ListaArchivos("C:\\tmp");
            }

            if (mensajeEntrante.ToLower().Contains("audio") && alumno != null)
            {
                await new clsBotAlumnos().EnviaAudioAsync(Telegram_id_manda_mensaje, "c:\\tmp\\some.mp3", "FOR YOU by The Outfield");
                respuesta = "Hola " + alumno.nombre + " " + alumno.apellido + " Te mando una canción super clasica";
            }
            if (mensajeEntrante.ToLower().Contains("libro") && alumno != null)
            {
                await new clsBotAlumnos().EnviaPDFAsync(Telegram_id_manda_mensaje, "c:\\tmp\\JavaEENotesForProfessionals.pdf", "Notas de los Java Ninjas");
                respuesta = "Hola " + alumno.nombre + " " + alumno.apellido + " Te mando un libro, espero que te sirva";
            }
            if (mensajeEntrante.ToLower().Contains("foto") && alumno != null)
            {
                await new clsBotAlumnos().EnviaFoto(Telegram_id_manda_mensaje, "c:\\tmp\\quedateEnCasa.jpg", "Movilidad en guatemala");
                respuesta = "Hola " + alumno.nombre + " " + alumno.apellido + " Movilidad a partir del 15 de mayo";
            }


            if (respuesta.Equals("No te entiendo") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " Lamento decirte que no entiendo lo que me quieres decir "+ mdEmoji.chinoEscritura+ mdEmoji.chinoEscritura + mdEmoji.chinoEscritura + mdEmoji.chinoEscritura + mdEmoji.chinoEscritura;
            }


            if (mensajeEntrante.Contains("hola") && alumno != null)
            {
                respuesta = emojis.mdEmoji.telefono + "Hola me da mucho gusto de Saludarte " + alumno.nombre;
            }




            //obtiene documentos
            if (ObjetoMensajeTelegram.Message.Document != null && alumno != null)
            {
                Console.WriteLine($"Recibiendo documento del chat {ObjetoMensajeTelegram.Message.Chat.Id}.");
                Console.WriteLine($"Tamano {ObjetoMensajeTelegram.Message.Photo.Length}.");

                Console.WriteLine($"hora y fecha {ObjetoMensajeTelegram.Message.Date.ToString() }.");

                String re = "Recibida Su doucmento, muchas Gracias";
                // var file = await Bot.GetFileAsync(message.Document.FileId);
                var file = await Bot.GetFileAsync(ObjetoMensajeTelegram.Message.Document.FileId);
                FileStream fs = new FileStream("c:\\tmp\\ruldin.doc", FileMode.Create);
                await Bot.DownloadFileAsync(file.FilePath, fs);
                fs.Close();
                fs.Dispose();

                if (!re.Equals(""))
                {
                    await Bot.SendTextMessageAsync(
                        chatId: ObjetoMensajeTelegram.Message.Chat,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        text: "Gracias por mandar su archivo"
                );
                    return;
                }
            }


            //obtiene fotos
            if (ObjetoMensajeTelegram.Message.Photo  != null && alumno != null)
            {
                Console.WriteLine($"Recibiendo Foto del chat {ObjetoMensajeTelegram.Message.Chat.Id}.");
                Console.WriteLine($"Alumno:{alumno.nombre + " " + alumno.apellido}");
                Console.WriteLine($"Tamano {ObjetoMensajeTelegram.Message.Photo.Length}.");
               
                Console.WriteLine($"hora y fecha {ObjetoMensajeTelegram.Message.Date.ToString() }.");

                String re = "Recibida Su Foto, muchas Gracias";
                try
                {
                    var file = await Bot.GetFileAsync(ObjetoMensajeTelegram.Message.Photo[ObjetoMensajeTelegram.Message.Photo.Length - 1].FileId);
                    //  var archivo = "c:\\tmp\\foto_"+alumno.apellido+"_"+alumno.nombre+".jpg";
                    var archivo = "c:\\tmp\\" + alumno.idbot + "_"+alumno.nombre+"_"+alumno.apellido+".jpg";
                    FileStream fs = new FileStream(archivo, FileMode.Create);
                    await Bot.DownloadFileAsync(file.FilePath, fs);
                    fs.Close();
                    fs.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error descargando archivo: " + ex.Message);
                }



                if (!re.Equals(""))
                {
                    await Bot.SendTextMessageAsync(
                        chatId: ObjetoMensajeTelegram.Message.Chat,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        text: "Gracias por mandar tu foto"
                );
                    return;
                }
            }




            // recibe localidad
            // nuevo codigo 16_5_2020
            if (ObjetoMensajeTelegram.Message.Location != null && alumno != null)
            {
                // nueva clase
                mdGeoLocacion lo = new mdGeoLocacion();

                //  String id = e.Message.Chat.Id.ToString();
                // Console.WriteLine("atendiendo a:" + id);
                Console.WriteLine($"Recibiendo longitud y latitud del chat {ObjetoMensajeTelegram.Message.Chat.Id}.");
                Console.WriteLine($"Lonmgitud {ObjetoMensajeTelegram.Message.Location.Longitude}.");
                Console.WriteLine($"Latitud {ObjetoMensajeTelegram.Message.Location.Latitude}.");
                Console.WriteLine($"hora y fecha {ObjetoMensajeTelegram.Message.Date.ToString() }.");

                String re = "Recibida Su locación, muchas Gracias";

                // String id = e.Message.Chat.Id.ToString();


                // if (lo.botid == null) return;

                lo.botid = alumno.idbot.ToString();
                lo.longitud = ObjetoMensajeTelegram.Message.Location.Longitude;
                lo.latitud = ObjetoMensajeTelegram.Message.Location.Latitude;
                lo.guardaUbicacion(lo);

                if (!re.Equals(""))
                {
                    await Bot.SendTextMessageAsync(
                        chatId: ObjetoMensajeTelegram.Message.Chat,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        text: "Gracias por mandar tu ubicacion"
                );
                    return;
                }
            }




            // envia la respuesta del diaglo
            if (!String.IsNullOrEmpty(respuesta))//    
            {
                await Bot.SendTextMessageAsync(
                    chatId: ObjetoMensajeTelegram.Message.Chat,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    text: respuesta

            );
            }

        } // fin del metodo de recepcion de mensajes


        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("UPS!!! Recibo un error!!!: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

    }//fin clase
}
