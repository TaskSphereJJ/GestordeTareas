﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class Prioridad
    {
        [Key]
        public int Id { get; set; }

        //anotaciones de validacion
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50, ErrorMessage = "Maximo de caracteres 50")]
        public string Nombre { get; set; } = string.Empty; //inicializo qeu es un string de logitud cero
    }
}
