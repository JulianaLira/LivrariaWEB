using LivrariaWEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivrariaAPI.DAO
{
    public interface ILivroDAO
    {
        Task CreateAsync(Livro livro);
        Task Delete(Livro livro);
        Task<ICollection<Livro>> FiltroLivro(int? Isbn, string autor, string nome, decimal? preco, DateTime? data_Publicacao);
        Task<Livro> GetByLivroId(int? Id);
        Task<Livro> GetISBN(int? Isbn);
        Task<List<Livro>> ListAllAsync();
        Task<ICollection<Livro>> Listar();
        Task UpdateAsync(Livro livro);
    }
}