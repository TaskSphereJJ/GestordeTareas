﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestordeTaras.EN
{
    public class ImagenesPrueba
    {


        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Ruta de la Imagen es Requerida")]
        [MaxLength(4000, ErrorMessage = "Máximo 4000 Carácteres")]
        [Display(Name = "RutaImagen")]
        public string Imagen { get; set; } = string.Empty;  // Ruta de la imagen 

        [ForeignKey("TareaFinalizada")]
        [Required(ErrorMessage = "Tarea finalizada es Requerida")]
        [Display(Name = "Estado de la Tarea")]
        public int IdTareaFinalizada { get; set; }

        [NotMapped]
        public int Top_Aux { get; set; }

        public TareaFinalizada TareaFinalizada { get; set; } = new TareaFinalizada(); // Relación con TareaFinalizada

    }
}