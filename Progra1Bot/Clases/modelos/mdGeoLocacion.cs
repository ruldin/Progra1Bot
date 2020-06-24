using BaseDeDatos.Clases.BaseDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Progra1Bot.Clases.modelos
{
    class mdGeoLocacion
    {
        public int id_localidad { get; set; }
        public string botid { get; set; }
        public float longitud { get; set; }
        public float latitud { get; set; }
        public DateTime fecha { get; set; }



        public void guardaUbicacion(mdGeoLocacion localizacion)
        {
            String sql_ = @"insert into tb_localizacion ([botid],[longitud],[latitud])
            values (" + localizacion.botid + "," + localizacion.longitud + "," + localizacion.latitud + ")";
            new ClsConexion().EjecutaSQLDirecto(sql_);
        }
    }
}
