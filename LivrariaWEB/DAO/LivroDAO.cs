﻿using LivrariaWEB.Data;
using LivrariaWEB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaWEB.DAO
{
    public class LivroDAO : ILivroDAO
    {
        private readonly ApplicationDbContext _context;

        public LivroDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Livro livro)
        {
            _context.Add(livro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Livro livro)
        {
            _context.Update(livro);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Livro livro)
        {
            _context.Remove(livro);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Livro>> Listar()
        {
            return await _context.Livro
                .ToListAsync();
        }

        public IQueryable<Livro> ListFiltroAll(string filter)
        {
            IQueryable<Livro> livros = _context.Livro
                        .AsNoTracking()
                        .Where(p => (filter == null
                                || p.Autor == filter
                                || p.Nome == filter))
                        .AsQueryable();
            return livros;
        }


        public IQueryable<Livro>ListFiltroCustom(int? Isbn, string autor, string nome, decimal? preco, DateTime? data_Publicacao)
        {
            IQueryable<Livro> livros = _context.Livro
                 .AsNoTracking()
                .Where(p => (Isbn == null || p.ISBN.Equals(Isbn))
                    && (autor == null || p.Autor.Contains(autor))
                    && (nome == null || p.Nome.Contains(nome))
                    && (preco == null || p.Preco.Equals(preco))
                    && ((!data_Publicacao.HasValue) || (p.Data_Publicacao >= data_Publicacao && p.Data_Publicacao <= data_Publicacao)))
                .OrderByDescending(p => p.Data_Publicacao)
                .AsQueryable();
            return livros;
        }

        public async Task<ICollection<Livro>> FiltroLivro(int? Isbn, string autor, string nome, decimal? preco, DateTime? data_Publicacao)
        {
            return await _context.Livro
                 .AsNoTracking()
                .Where(p => (Isbn == null || p.ISBN.Equals(Isbn))
                    && (autor == null || p.Autor.Contains(autor))
                    && (nome == null || p.Nome.Contains(nome))
                    && (preco == null || p.Preco.Equals(preco))
                    && ((!data_Publicacao.HasValue) || (p.Data_Publicacao >= data_Publicacao && p.Data_Publicacao <= data_Publicacao)))
                .OrderByDescending(p => p.Data_Publicacao)
                .ToListAsync();
        }

        public async Task<Livro> GetByLivroId(int? Id)
        {
            return await _context.Livro
                .AsNoTracking()
                .Where(p => p.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Livro> GetISBN(int? Isbn)
        {
            return await _context.Livro
                .AsNoTracking()
                .Where(p => p.ISBN == Isbn)
                .FirstOrDefaultAsync();
        }
    }
}
