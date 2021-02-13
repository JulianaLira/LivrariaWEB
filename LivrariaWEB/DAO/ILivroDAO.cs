using LivrariaWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaWEB.DAO
{
    public interface ILivroDAO
    {
        Task CreateAsync(Livro livro);
        Task Delete(Livro livro);
        Task<ICollection<Livro>> FiltroLivro(int? Isbn, string autor, string nome, decimal? preco, DateTime? data_Publicacao);
        Task<Livro> GetByLivroId(int? Id);
        Task<Livro> GetISBN(int? Isbn);
        Task<ICollection<Livro>> Listar();
        IQueryable<Livro> ListFiltroAll(string filter);

        IQueryable<Livro> ListFiltroCustom(int? Isbn, string autor, string nome, decimal? preco, DateTime? data_Publicacao);
        Task UpdateAsync(Livro livro);
    }
}