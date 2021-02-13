using LivrariaWEB.DAO;
using LivrariaWEB.Data;
using LivrariaWEB.Models;
using LivrariaWEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaWEB.Controllers
{
    public class LivrosController : Controller
    {

        private readonly ILivroDAO _livroDAO;
        private readonly ApplicationDbContext _context;
        public int PageSize { get; set; } = 15;
        private PagingList<LivroViewModel> _plLivro;

        public LivrosController(ApplicationDbContext context, ILivroDAO livroDAO)
        {
            _context = context;
            _livroDAO = livroDAO;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filter, string nome, string autor, int? isbn, int? preco, string datapublicacao,
            int page = 1, string sortExpression = "Id", bool flagFilter = false)
        {
            if (!flagFilter)
            {
                ViewBag.Filter = filter;

                IQueryable<Livro> listLivro  =  _livroDAO.ListFiltroAll(filter);

                var livrosViewModel = new List<LivroViewModel>();

                foreach (var livro in listLivro)
                {
                    var data = livro.Data_Publicacao != null ? livro.Data_Publicacao.Value.ToString("dd/MM/yyyy") : null;
                    
                    var livroViewModel = new LivroViewModel
                    {
                        Id = livro.Id,
                        ISBN = livro.ISBN,
                        Autor = livro.Autor,
                        Nome = livro.Nome,
                        Preco = livro.Preco,
                        Data_Publicacao = data,
                        Url_Imagem = livro.Url_Imagem
                    };

                    livrosViewModel.Add(livroViewModel);
                }

                _plLivro = PagingList.Create(livrosViewModel, PageSize, page, sortExpression, "Id");

                _plLivro.RouteValue = new RouteValueDictionary {
                    { "filter", filter},
                };

            }
            else
            {

              
                DateTime? dp = datapublicacao != null ? Convert.ToDateTime(datapublicacao) : (DateTime?)null;

                
                IQueryable<Livro> listLivro = _livroDAO.ListFiltroCustom(isbn, autor, nome,  preco, dp);

                var livrosViewModel = new List<LivroViewModel>();

                foreach (var livro in listLivro)
                {
                    var data = livro.Data_Publicacao != null ? livro.Data_Publicacao.Value.ToString("dd/MM/yyyy") : null;

                    var livroViewModel = new LivroViewModel
                    {
                        Id = livro.Id,
                        ISBN = livro.ISBN,
                        Autor = livro.Autor,
                        Nome = livro.Nome,
                        Preco = livro.Preco,
                        Data_Publicacao = data,
                        Url_Imagem = livro.Url_Imagem
                    };

                    livrosViewModel.Add(livroViewModel);
                }

                _plLivro = PagingList.Create(livrosViewModel, PageSize, page, sortExpression, "Id");

                _plLivro.RouteValue = new RouteValueDictionary {
                    { "filter", filter},
                };
            }

            return View(_plLivro);
        }

    }
}
