using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class CommentBL
    {
        // Método para obtener los comentarios de un proyecto específico
        public async Task<List<Comment>> ObtenerComentariosPorProyectoAsync(int idProyecto)
        {
            return await CommentDAL.ObtenerCommentPorProyectoAsync(idProyecto);
        }
        // Método para crear un comentario
        public async Task<int> CrearComentarioAsync(int idProyecto, int idUsuario, string contenido)
        {
            var comentario = new Comment
            {
                IdUsuario = idUsuario,
                IdProyecto = idProyecto,
                Content = contenido,
                FechaComentario = DateTime.Now
            };

            return await CommentDAL.CreateCommentAsync(comentario);
        }

        // Método para obtener un comentario por su ID
        public async Task<Comment> ObtenerComentarioPorIdAsync(int idComentario)
        {
            return await CommentDAL.ObtenerComentarioPorIdAsync(idComentario);
        }

        // Método para eliminar un comentario
        public async Task<int> EliminarComentarioAsync(int idComentario, int idUsuario)
        {
            // Obtener el comentario
            var comentario = await ObtenerComentarioPorIdAsync(idComentario);
            if (comentario == null)
            {
                return 0; // Comentario no encontrado
            }

            // Verificar si el comentario pertenece al usuario logueado
            if (comentario.IdUsuario != idUsuario)
            {
                return -1; // El comentario no pertenece al usuario
            }

            // Verificar si han pasado más de 15 minutos desde que se creó el comentario
            if ((DateTime.Now - comentario.FechaComentario).TotalMinutes > 15)
            {
                return -2; // El comentario ha pasado más de 15 minutos
            }

            return await CommentDAL.EliminarCommentAsync(idComentario);
        }


    }
}