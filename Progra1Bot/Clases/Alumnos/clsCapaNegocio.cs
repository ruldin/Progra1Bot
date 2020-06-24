using BaseDeDatos.Clases.BaseDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class clsCapaNegocio
    {

        public clsCapaNegocio()
        {
                
        }


        public mdAlumnos LocalizaAlumnoPorMail(String correo,String IDUsuarioTelegram)
        {
            mdAlumnos AlumnoEncontrado = new mdAlumnos();
            AlumnoEncontrado.nombre = "no encontrado";
            int linea = 1;
            int linea_encontrada = 0;

            var TodosLosAlunnos = new mdAlumnos().cargaDatos("c:\\tmp\\alumnos.csv","T");
            ClsManejoArchivos ClaseArchivos = new ClsManejoArchivos();
            foreach (mdAlumnos item in TodosLosAlunnos)
            {
                if (correo.ToLower().Equals(item.correo))
                {
                    linea_encontrada = linea;
                    AlumnoEncontrado = item;
                   // string nuevaLinea = item.apellido + ";" + item.nombre + ";" + item.correo + ";" + item.carnet + ";" + item.seccion + ";" + IDUsuarioTelegram;
                 //   ClaseArchivos.CambioLinea(nuevaLinea, "c:\\tmp\\alumnos.csv", linea_encontrada);


                }
                linea++;
            }
            return AlumnoEncontrado;
        }

        public List<mdAlumnos> CargarAlumnosBaseDatos()
        {
            ClsConexion cn = new ClsConexion();
            mdAlumnos Alumno = new mdAlumnos();
            List<mdAlumnos> TodosLosAlumnos = new List<mdAlumnos>();

            DataTable dt = cn.consultaTablaDirecta("SELECT *  FROM [tb_alumnos]");

            foreach (DataRow dr in dt.Rows)
            {
                Alumno.nombre = dr["nombre"].ToString();
                Alumno.apellido = dr["apellido"].ToString();
                Alumno.correo = dr["correo"].ToString();
                Alumno.carnet = dr["carnet"].ToString();
                Alumno.seccion = dr["seccion"].ToString();
                Alumno.idbot = dr["idbot"].ToString();
                TodosLosAlumnos.Add(Alumno);
                Alumno = new mdAlumnos();

            }
            return TodosLosAlumnos;
        }


        public mdAlumnos EncontrarAlumnoPorMail(String correo)
        {
            ClsConexion cn = new ClsConexion();
            mdAlumnos Alumno = new mdAlumnos();
            Alumno.idbot = "NO HAY";
            DataTable dt = cn.consultaTablaDirecta("SELECT *  FROM [tb_alumnos] where correo='"+correo+"'");

            foreach (DataRow dr in dt.Rows)
            {
                Alumno.nombre = dr["nombre"].ToString();
                Alumno.apellido = dr["apellido"].ToString();
                Alumno.correo = dr["correo"].ToString();
                Alumno.carnet = dr["carnet"].ToString();
                Alumno.seccion = dr["seccion"].ToString();
                Alumno.idbot = dr["idbot"].ToString();
            }
            return Alumno;
        }


    }
}
