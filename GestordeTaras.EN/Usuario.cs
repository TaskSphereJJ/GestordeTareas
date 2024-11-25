using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestordeTaras.EN
{
    public class Usuario
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "Foto")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string FotoPerfil { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Correo Electrónico")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; } = string.Empty;
        [NotMapped] // Esto es importante para que no se mapee en la base de datos
        public string ConfirmarPass { get; set; }

        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Telefono { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public byte Status { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        [ForeignKey("Cargo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Cargo")]
        public int IdCargo { get; set; }


        [NotMapped]
        public int Top_Aux { get; set; } // propiedad auxiliar

        [NotMapped]
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "La contraseña debe tener entre 6 y 32 caracteres", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar la contraseña")]
        public string ConfirmPassword_Aux { get; set; } = string.Empty; // propiedad auxiliar

        public Cargo Cargo { get; set; } // propiedad de navegación
        public virtual ICollection<ProyectoUsuario> ProyectoUsuario { get; set; }
        public ICollection<PasswordResetCode> PasswordResetCode { get; set; }
        public ICollection<Comment> Comment { get; set; }
    }
    public enum User_Status
    {
        ACTIVO = 1, INACTIVO = 2
    }

}

