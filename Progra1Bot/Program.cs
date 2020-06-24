
using Progra1Bot.Clases;
using Progra1Bot.Clases.Alumnos;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Progra1Bot
{
    class Program
    {
        // ejemplo de bot
        // hecho por Ruldin Ayala
        

        public static async Task Main()
        {
            // con fines netamente didácticos
            // esta aplicación no es apta para ponerla en producción
            
            await new clsBotAlumnos().IniciarTelegram();
            //new clsZonas().envia_zonas("zonasP3");
          // new clsZonas().envia_zonas("zonasProgra1","B");
            // new clsZonas().envia_zonas("zonasProgra1","A");

            Console.WriteLine("Fin del Proceso!!!");
            Console.ReadLine();

        }
    } // fin de la clase
  
}


































































