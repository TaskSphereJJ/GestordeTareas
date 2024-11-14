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
        public async Task<int> CreateComment(Comment comment)
        {
            return await CommentDAL.CreateComment(comment);
        }

        public async Task<List<Comment>> GetCommentsByProject(int idProyecto)
        {
            return await CommentDAL.GetCommentByProject(idProyecto);
        }
    }
}
