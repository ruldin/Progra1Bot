using BaseDeDatos.Clases.BaseDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Progra1Bot.Clases.modelos
{
    class mdZonas
    {
        public string nombres { get; set; }
        public string carnet { get; set; }
        public string correo { get; set; }
        public int zona { get; set; }



        public List<mdZonas> cargaZonas(string tabla, string seccion = "no")
        {
            //[zonasProgra1]
            //[zonasP3]

            List<mdZonas> zonas = new List<mdZonas>();
            String Sqlseccion = "";
            if (!seccion.Equals("no")) Sqlseccion = " where seccion = '"+ seccion+"'";
            String cuerito = $"SELECT  CONCAT([Nombre], [Apellidos]) as nombres ,[correo] ,[carnet] ,[Zona] FROM {tabla} {Sqlseccion}";

            foreach(DataRow dr in  new ClsConexion().consultaTablaDirecta(cuerito).Rows)
            {
                var l = new mdZonas();
                l.nombres = dr["nombres"].ToString();
                l.carnet =  dr["carnet"].ToString();
                l.correo =  dr["correo"].ToString();
                l.zona = Convert.ToInt32(dr["zona"]);
                zonas.Add(l);
            }
            return zonas;
        }

    }
}
