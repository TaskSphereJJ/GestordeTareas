

using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;

namespace GestordeTareas.DAL
{
    public class CommentDAL
    {
        public static async Task<int> CreateComment(Comment comment)
        {
            int result = 0;

            // Entorno de ejecucion
            using(var dbContext = new ContextoBD()) 
            {
                await dbContext.Comment.AddAsync(comment);
                result = dbContext.SaveChanges();
            }

            return result;
        }

        public static async Task<List<Comment>> GetCommentByProject(int idProject)
        {
            int result = 0;

            // Entorno de ejecucion
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.Comment

                .Where(c => c.IdProyecto == idProject)
                .Include(c => c.Usuario)
                .OrderBy(c => c.FechaComentario)
                .ToListAsync();
            }

        }
    }
}