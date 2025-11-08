using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class CommentDAL
    {
        // Crear un nuevo comentario
        public static async Task<int> CreateCommentAsync(Comment comment)
        {
            using var dbContext = new ContextoBD();

            if (comment.FechaComentario == default)
                comment.FechaComentario = DateTime.Now;

            await dbContext.Comment.AddAsync(comment);
            return await dbContext.SaveChangesAsync();
        }

        // Obtener comentarios por ID de proyecto
        public static async Task<List<Comment>> ObtenerCommentPorProyectoAsync(int idProyecto)
        {
            using var dbContext = new ContextoBD();

            return await dbContext.Comment
                .Where(c => c.IdProyecto == idProyecto)
                .Include(c => c.Usuario)
                .OrderBy(c => c.FechaComentario)
                .ToListAsync();
        }

        // Eliminar comentario por ID
        public static async Task<int> EliminarCommentAsync(int idComment)
        {
            using var dbContext = new ContextoBD();

            var comment = await dbContext.Comment.FirstOrDefaultAsync(c => c.Id == idComment);
            if (comment == null) return 0;

            dbContext.Comment.Remove(comment);
            return await dbContext.SaveChangesAsync();
        }

        // Obtener comentario por ID
        public static async Task<Comment> ObtenerComentarioPorIdAsync(int idComment)
        {
            using var dbContext = new ContextoBD();

            return await dbContext.Comment
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == idComment);
        }
    }
}

















//using GestordeTaras.EN;
//using Microsoft.EntityFrameworkCore;

//namespace GestordeTareas.DAL
//{
//    public class CommentDAL
//    {
//        public static async Task<int> CreateCommentAsync(Comment comment)
//        {
//            int result = 0;

//            // Entorno de ejecucion
//            using (var dbContext = new ContextoBD())
//            {
//                if (comment.FechaComentario == default)
//                    comment.FechaComentario = DateTime.Now;

//                await dbContext.Comment.AddAsync(comment);
//                result = await dbContext.SaveChangesAsync();
//            }

//            return result;
//        }

//        public static async Task<List<Comment>> ObtenerCommentPorProyectoAsync(int idProjecto)
//        {
//            // Entorno de ejecucion
//            using (var dbContext = new ContextoBD())
//            {
//                return await dbContext.Comment
//                .Where(c => c.IdProyecto == idProjecto)
//                .Include(c => c.Usuario)
//                .OrderBy(c => c.FechaComentario)
//                .ToListAsync();
//            }
//        }

//        // Método para eliminar un comentario
//        public static async Task<int> EliminarCommentAsync(int idComment)
//        {
//            int result = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                // Buscar el comentario por su ID
//                var comment = await dbContext.Comment
//                    .FirstOrDefaultAsync(c => c.Id == idComment);

//                if (comment != null)
//                {
//                    // Eliminar el comentario si se encuentra
//                    dbContext.Comment.Remove(comment);
//                    result = await dbContext.SaveChangesAsync(); // Guardar cambios
//                }
//            }

//            return result;
//        }

//        // Método para obtener un comentario por su ID
//        public static async Task<Comment> ObtenerComentarioPorIdAsync(int idComment)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                return await dbContext.Comment
//                    .Include(c => c.Usuario) // Incluir la información del usuario (si es necesario)
//                    .FirstOrDefaultAsync(c => c.Id == idComment);
//            }
//        }

//    }
//}